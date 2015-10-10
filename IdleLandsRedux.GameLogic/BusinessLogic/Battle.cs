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
using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.Interop;
using IdleLandsRedux.GameLogic.Interfaces.Scripts;
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
		private IBattleInterop BattleInterop { get; set; }
		private IScriptHelper ScriptHelper { get; set; }
		public Dictionary<string, List<SpecificCharacter>> _allCharactersInTeams { get; set; }

		private List<SpecificCharacter> _turnOrder;

		public Battle(List<List<Character>> teams, IBattleInterop battleInterop, IScriptHelper scriptHelper)
		{
			_allCharactersInTeams = new Dictionary<string, List<SpecificCharacter>>();
			BattleInterop = battleInterop;
			ScriptHelper = scriptHelper;
		}

		//Not using _characters here, since some functions don't want to include dead people. Ugh. Zombies.
		public void SetTurnOrder(ref List<SpecificCharacter> characters)
		{
			if (characters == null) {
				throw new ArgumentNullException("characters");
			}

			/*characters.Sort((a, b) => (a.CalculatedStats.StaticAgility * (1 + a.CalculatedStats.AgilityPercent)).CompareTo( 
				b.CalculatedStats.Agility * (1 + b.CalculatedStats.AgilityPercent)));*/
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

		public StatsModifierCollection CalculateStats(SpecificCharacter character)
		{
			StatsModifierCollection summedModifiers = new StatsModifierCollection();
			var engine = BattleInterop.CreateJSEngineWithCommonScripts(character);
			List<string> allScripts = new List<string>();
			List<Tuple<string, string>> allStaticFunctions = new List<Tuple<string, string>>();
			List<Tuple<string, string>> allStaticHookFunctions = new List<Tuple<string, string>>();
			List<Tuple<string, string>> allDependentFunctions = new List<Tuple<string, string>>();
			List<Tuple<string, string>> allDependentHookFunctions = new List<Tuple<string, string>>();
			List<Tuple<string, string>> allOverrulingFunctions = new List<Tuple<string, string>>();

			//init
			allScripts.Add(string.Format("./Classes/{0}.js", character.Class));
			string lastScript = allScripts.Last();
			allStaticFunctions.Add(new Tuple<string, string>(lastScript, OnStaticBonusString(character.Class)));
			allStaticHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyStaticBonusScriptForString(character.Class)));
			allDependentFunctions.Add(new Tuple<string, string>(lastScript, OnDependentBonusString(character.Class)));
			allDependentHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyDependentBonusScriptForString(character.Class)));
			allOverrulingFunctions.Add(new Tuple<string, string>(lastScript, OnOverrulingBonusString(character.Class)));

			if (!string.IsNullOrEmpty(character.Personalities)) {
				foreach (string personality in character.Personalities.Split(';')) {
					allScripts.Add(string.Format("./Personalities/{0}.js", personality));
					lastScript = allScripts.Last();
					allStaticFunctions.Add(new Tuple<string, string>(lastScript, OnStaticBonusString(personality)));
					allStaticHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyStaticBonusScriptForString(personality)));
					allDependentFunctions.Add(new Tuple<string, string>(lastScript, OnDependentBonusString(personality)));
					allDependentHookFunctions.Add(new Tuple<string, string>(lastScript, OnShouldModifyDependentBonusScriptForString(personality)));
					allOverrulingFunctions.Add(new Tuple<string, string>(lastScript, OnOverrulingBonusString(personality)));
				}
			}

			//load all scripts
			foreach (string function in allScripts) {
				ScriptHelper.ExecuteScript(ref engine, function);
			}

			//Execute static functions on scripts
			foreach (var script in allStaticFunctions) {
				var modifiers = BattleInterop.InvokeFunctionWithHooks(engine, script.Item2,
					allStaticHookFunctions.Where(x => x.Item1 != script.Item1).Select(x => x.Item2), character, summedModifiers);

				summedModifiers = BattleInterop.addObjectToStatsModifierObject(summedModifiers, modifiers);
			}

			//dependent functions
			foreach (var script in allDependentFunctions) {
				var modifiers = BattleInterop.InvokeFunctionWithHooks(engine, script.Item2,
					allDependentHookFunctions.Where(x => x.Item1 != script.Item1).Select(x => x.Item2), character, summedModifiers);

				summedModifiers = BattleInterop.addObjectToStatsModifierObject(summedModifiers, modifiers);
			}

			//overruling
			foreach (var script in allOverrulingFunctions) {
				var modifiers = BattleInterop.InvokeFunction(engine, script.Item2, character, summedModifiers);

				summedModifiers = BattleInterop.addObjectToStatsModifierObject(summedModifiers, modifiers);
			}

			return summedModifiers;
		}

		public void TakeTurn()
		{
			List<SpecificCharacter> characters = _allCharactersInTeams.SelectMany(x => x.Value).ToList();

			foreach (var character in characters) {
				character.CalculatedStats = CalculateStats(character);
			}

			SetTurnOrder(ref characters);


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

