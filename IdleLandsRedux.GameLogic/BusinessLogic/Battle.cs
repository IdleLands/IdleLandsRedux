using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IdleLandsRedux.Common;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.BusinessLogic.Interop;
using Jint;

namespace IdleLandsRedux.GameLogic.BusinessLogic
{
	public class BattleException : Exception
	{
		public BattleException() : base()
		{
		}

		public BattleException(string msg) : base(msg)
		{
		}
	}

	public class Battle
	{
		public Dictionary<string, List<SpecificCharacter>> _allCharactersInTeams { get; set; }

		public Battle(List<List<Character>> teams)
		{
			_allCharactersInTeams = new Dictionary<string, List<SpecificCharacter>>();
		}

		//Not using _characters here, since some functions don't want to include dead people. Ugh. Zombies.
		public void SetTurnOrder(ref List<SpecificCharacter> characters)
		{
			if (characters == null) {
				throw new ArgumentNullException("characters");
			}

			characters.Sort((a, b) => (a.GetTotalStats().Agility * (1 + a.GetTotalStats().AgilityPercent)).CompareTo( 
				b.GetTotalStats().Agility * (1 + b.GetTotalStats().AgilityPercent)));
		}

		public List<SpecificCharacter> GetValidTargetsFor(SpecificCharacter character)
		{
			if (character == null) {
				throw new ArgumentNullException("character");
			}

			var ret = new List<SpecificCharacter>();

			foreach (var team in _allCharactersInTeams) {
				if(team.Value.Contains(character)) {
					continue;
				}

				ret.AddRange(team.Value);
			}

			return ret;
		}

		public bool MoreThanOneTeamAlive()
		{
			int teamsAlive = 0;

			foreach (var characters in _allCharactersInTeams) {
				if(characters.Value.Any(c => c.Stats.HitPoints > 0))
					teamsAlive++;
			}

			return teamsAlive > 1;
		}

		public List<SpecificCharacter> GetVictoriousTeam()
		{
			if (MoreThanOneTeamAlive()) {
				throw new BattleException("Battle is not yet finished!");
			}

			foreach (var characters in _allCharactersInTeams) {
				if (characters.Value.Any(c => c.Stats.HitPoints > 0)) {
					return characters.Value;
				}
			}

			//Tie?
			return null;
		}

		public StatsModifierObject CalculateStats(SpecificCharacter character)
		{
			StatsModifierObject summedModifiers = new StatsModifierObject();
			var engine = BattleInterop.CreateJSEngineWithCommonScripts(character);
			List<string> allScripts = new List<string>();
			List<Tuple<string, string>> allStaticFunctions = new List<Tuple<string, string>>();
			List<Tuple<string, string>> allHookFunctions = new List<Tuple<string, string>>();

			//init
			allScripts.Add(string.Format("./Classes/{0}.js", character.Class));
			allStaticFunctions.Add(new Tuple<string, string>(allScripts.Last(), string.Format("{0}_OnStaticBonus", character.Class)));
			allHookFunctions.Add(new Tuple<string, string>(allScripts.Last(), string.Format("{0}_OnShouldModifyStaticBonusScriptFor", character.Class)));
			if (!string.IsNullOrEmpty(character.Personalities)) {
				foreach (string personality in character.Personalities.Split(';')) {
					allScripts.Add(string.Format("./Personalities/{0}.js", personality));
					allStaticFunctions.Add(new Tuple<string, string>(allScripts.Last(), string.Format("{0}_OnStaticBonus", personality)));
					allHookFunctions.Add(new Tuple<string, string>(allScripts.Last(), string.Format("{0}_OnShouldModifyStaticBonusScriptFor", personality)));
				}
			}

			//load all scripts
			foreach (string function in allScripts) {
				ScriptHelper.ExecuteScript(ref engine, function);
			}

			//Execute functions on scripts
			foreach (var script in allStaticFunctions) {
				var modifiers = BattleInterop.InvokeStaticBonusWithHooks(engine, script.Item2,
					allHookFunctions.Where(x => x.Item1 != script.Item1).Select(x => x.Item2), character);

				summedModifiers = BattleInterop.addObjectToStatsModifierObject(summedModifiers, modifiers);
			}

			return summedModifiers;
		}


	}
}

