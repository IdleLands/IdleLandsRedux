using System;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.Common;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic;
using IdleLandsRedux.InteropPlugins;
using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.BattleActions;
using IdleLandsRedux.GameLogic.BusinessLogic.BattleActions;
using IdleLandsRedux.GameLogic.DataEntities.Interfaces.Effects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace IdleLandsRedux.GameLogic.BusinessLogic
{
    [Serializable]
    public class BattleException : Exception
    {
        public BattleException() : base()
        {
        }

        public BattleException(string msg) : base(msg)
        {
        }
        
        public BattleException(string msg, Exception inner) : base(msg, inner)
        {
            
        }
        
        protected BattleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }

    public class Battle : IBattle
    {
        private IPlugin InteropPlugin { get; set; }

        private IJSScriptHelper ScriptHelper { get; set; }

        private IRandomHelper RandomHelper { get; set; }

        private List<IBattleAction> ActionQueue { get; set; }

        public Dictionary<string, ICollection<SpecificCharacter>> AllCharactersInTeams { get; set; }

        private string WinningTeam { get; set; }

        public Battle(IEnumerable<IEnumerable<Character>> teams, IPlugin interopPlugin, IJSScriptHelper scriptHelper, IRandomHelper randomHelper)
        {
            if(teams == null)
            {
                throw new ArgumentNullException(nameof(teams));
            }
            
            if(interopPlugin == null)
            {
                throw new ArgumentNullException(nameof(interopPlugin));
            }
            
            if(scriptHelper == null)
            {
                throw new ArgumentNullException(nameof(scriptHelper));
            }
            
            if(randomHelper == null)
            {
                throw new ArgumentNullException(nameof(randomHelper));
            }
            
            this.AllCharactersInTeams = new Dictionary<string, ICollection<SpecificCharacter>>();

            foreach (var team in teams)
            {
                var teamCharacters = (ICollection<SpecificCharacter>)team.Select(x => new SpecificCharacter(x));
                this.AllCharactersInTeams.Add("team", teamCharacters);
            }

            this.InteropPlugin = interopPlugin;
            this.ScriptHelper = scriptHelper;
            this.RandomHelper = randomHelper;
        }

        //Not using _characters here, since some functions don't want to include dead people. Ugh. Zombies.
        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotExposeGenericListsRule", Justification = "I would have to write my own IList.Sort extension method using comparison stuff. Why?")]
        public void SetTurnOrder(List<SpecificCharacter> characters)
        {
            if (characters == null)
            {
                throw new ArgumentNullException(nameof(characters));
            }

            characters.Sort((a, b) => (a.CalculatedStats.Agility.Total * (1 + a.CalculatedStats.Agility.Total)).CompareTo(
                b.CalculatedStats.Agility * (1 + b.CalculatedStats.Agility.Total)));
        }

        public ICollection<SpecificCharacter> GetValidTargetsFor(SpecificCharacter character)
        {
            if (character == null)
            {
                throw new ArgumentNullException(nameof(character));
            }

            var ret = new List<SpecificCharacter>();

            foreach (var team in AllCharactersInTeams)
            {
                if (team.Value.Contains(character))
                {
                    continue;
                }

                ret.AddRange(team.Value);
            }

            return ret;
        }

        public bool MoreThanOneTeamAlive()
        {
            int teamsAlive = 0;

            foreach (var characters in AllCharactersInTeams)
            {
                if (characters.Value.Any(c => c.Stats.HitPoints > 0))
                    teamsAlive++;
            }

            return teamsAlive > 1;
        }

        public ICollection<SpecificCharacter> GetVictoriousTeam()
        {
            if (MoreThanOneTeamAlive())
            {
                throw new BattleException("Battle is not yet finished!");
            }

            foreach (var characters in AllCharactersInTeams)
            {
                if (characters.Value.Any(c => c.Stats.HitPoints > 0))
                {
                    WinningTeam = characters.Key;
                    return characters.Value;
                }
            }

            //Tie?
            return null;
        }

        public IEnumerable<SpecificCharacter> GetLosingTeam()
        {
            return AllCharactersInTeams.Where(team => team.Key != WinningTeam).SelectMany(team => team.Value);
        }

        public StatsModifierCollection CalculateStats(SpecificCharacter character)
        {
            StatsModifierCollection summedModifiers = new StatsModifierCollection();
            var engine = InteropPlugin.CreateEngineWithCommonScripts(character);
            List<string> allScripts = new List<string>();
            List<Tuple<string, string>> allStaticFunctions = new List<Tuple<string, string>>();
            List<Tuple<string, string>> allStaticHookFunctions = new List<Tuple<string, string>>();
            List<Tuple<string, string>> allDependentFunctions = new List<Tuple<string, string>>();
            List<Tuple<string, string>> allDependentHookFunctions = new List<Tuple<string, string>>();
            List<Tuple<string, string>> allOverrulingFunctions = new List<Tuple<string, string>>();

            //init
            string lastScript = string.Format("./Classes/{0}.js", character.Class);
            allScripts.Add(lastScript);
            allStaticFunctions.Add(new Tuple<string, string>(lastScript, OnStaticBonusString(character.Class)));
            allStaticHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyStaticBonusScriptForString(character.Class)));
            allDependentFunctions.Add(new Tuple<string, string>(lastScript, OnDependentBonusString(character.Class)));
            allDependentHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyDependentBonusScriptForString(character.Class)));
            allOverrulingFunctions.Add(new Tuple<string, string>(lastScript, OnOverrulingBonusString(character.Class)));

            if (!string.IsNullOrEmpty(character.Personalities))
            {
                foreach (string personality in character.Personalities.Split(';'))
                {
                    lastScript = string.Format("./Personalities/{0}.js", personality);
                    allScripts.Add(lastScript);
                    allStaticFunctions.Add(new Tuple<string, string>(lastScript, OnStaticBonusString(personality)));
                    allStaticHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyStaticBonusScriptForString(personality)));
                    allDependentFunctions.Add(new Tuple<string, string>(lastScript, OnDependentBonusString(personality)));
                    allDependentHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyDependentBonusScriptForString(personality)));
                    allOverrulingFunctions.Add(new Tuple<string, string>(lastScript, OnOverrulingBonusString(personality)));
                }
            }

            //load all scripts
            foreach (string function in allScripts)
            {
                ScriptHelper.ExecuteScript(engine, function);
            }

            //Execute static functions on scripts
            foreach (var script in allStaticFunctions)
            {
                var modifiers = InteropPlugin.InvokeFunctionWithHooks(engine, script.Item2,
                                    allStaticHookFunctions.Where(x => x.Item1 != script.Item1).Select(x => x.Item2), character, summedModifiers);

                summedModifiers = InteropPlugin.AddObjectToStatsModifierObject(summedModifiers, modifiers);
            }

            //dependent functions
            foreach (var script in allDependentFunctions)
            {
                var modifiers = InteropPlugin.InvokeFunctionWithHooks(engine, script.Item2,
                                    allDependentHookFunctions.Where(x => x.Item1 != script.Item1).Select(x => x.Item2), character, summedModifiers);

                summedModifiers = InteropPlugin.AddObjectToStatsModifierObject(summedModifiers, modifiers);
            }

            //overruling
            foreach (var script in allOverrulingFunctions)
            {
                var modifiers = InteropPlugin.InvokeFunction(engine, script.Item2, character, summedModifiers);

                summedModifiers = InteropPlugin.AddObjectToStatsModifierObject(summedModifiers, modifiers);
            }

            return summedModifiers;
        }

        public void TakeTurn(ICollection<string> battleLog)
        {
            if(battleLog == null)
            {
                throw new ArgumentNullException(nameof(battleLog));
            }
            
            List<SpecificCharacter> characters = AllCharactersInTeams.SelectMany(x => x.Value).Where(x => x.StillParticipatingInCombat).ToList();

            foreach (var character in characters)
            {
                character.CalculatedStats = CalculateStats(character);
            }

            SetTurnOrder(characters);

            DetermineActions(characters);

            //Execute all actions

            foreach (var action in ActionQueue)
            {
                string targets = string.Empty;
                if (action.Targets != null)
                {
                    if (action.Targets.Count() == 1)
                    {
                        targets = action.Targets.First().Name;
                    }
                    else
                    {
                        targets = string.Join(", ", action.Targets.SkipLastN(1).Select(x => x.Name)) + " and " + action.Targets.Last().Name;
                    }
                }
                battleLog.Add($"{action.Originator.Name} uses {action.ActionName} on {targets}.");
                action.ActionExecute();
            }

            //Apply all effects such as poison
            foreach (var character in characters)
            {
                foreach (var effect in character.TransientStats.Effects)
                {
                    var specificEffect = effect as ISpecificEffect;

                    if (specificEffect == null)
                    {
                        //TODO log something
                        continue;
                    }

                    specificEffect.ApplyTo(character);
                }
            }

            //Propagate all actions to scripts here?

            foreach (var action in ActionQueue)
            {
                action.ActionDone();
            }

            foreach (var character in characters)
            {
                foreach (var effect in character.TransientStats.Effects)
                {
                    var specificEffect = effect as ISpecificEffect;

                    if (specificEffect == null)
                    {
                        //TODO log something
                        continue;
                    }

                    specificEffect.Cleanup(character);
                }

                character.TransientStats.Effects.RemoveAll(effect => effect.RoundsLeft <= 0);
            }

            ActionQueue.Clear();

            foreach (var deadCharacter in characters.Where(character => character.TransientStats.HitPointsDrained >= character.TotalStats.HitPoints.Maximum))
            {
                deadCharacter.Dead = true;
            }
        }

        private void DetermineActions(ICollection<SpecificCharacter> characters)
        {
            foreach (var character in characters)
            {
                if (this.RandomHelper.Next(100) <= character.CalculatedStats.FleePercent.Total)
                {
                    character.Fled = true;
                    continue;
                }

                //Add standard regen action
                ActionQueue.Add(new RegenAction { Originator = character });

                //TODO make something SRSLY out of this.
                IBattleAction action = null;
                if (this.RandomHelper.Next(100) <= 50)
                {
                    action = new PhysicalAttackAction
                    {
                        Originator = character,
                        Targets = new List<SpecificCharacter> { this.RandomHelper.RandomFromList(GetValidTargetsFor(character)) }
                    };
                }
                else
                {
                    action = new DefendAction()
                    {
                        Originator = character,
                        Targets = new List<SpecificCharacter> { character }
                    };
                }

                ActionQueue.Add(action);
            }
        }

        private static string OnStaticBonusString(string input)
        {
            return string.Format("{0}_OnStaticBonus", input);
        }

        private static string OnShouldModifyStaticBonusScriptForString(string input)
        {
            return string.Format("{0}_OnShouldModifyStaticBonusScriptFor", input);
        }

        private static string OnDependentBonusString(string input)
        {
            return string.Format("{0}_OnDependentBonus", input);
        }

        private static string OnShouldModifyDependentBonusScriptForString(string input)
        {
            return string.Format("{0}_OnShouldModifyDependentBonusScriptFor", input);
        }

        private static string OnOverrulingBonusString(string input)
        {
            return string.Format("{0}_OnOverrulingBonus", input);
        }
    }
}

