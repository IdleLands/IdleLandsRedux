using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Scripts;

namespace IdleLandsRedux.GameLogic.BussinessLogic
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

		public ExpandoObject CalculateStats(SpecificCharacter character)
		{
			var engine = ScriptHelper.CreateScriptEngine();
			ScriptHelper.ExecuteScript(ref engine, "./Classes/BaseClass.js");
			ScriptHelper.ExecuteScript(ref engine, string.Format("./Classes/{0}.js", character.Class));
			return (ExpandoObject)engine.Invoke("OnStaticBonus", character).ToObject();
		}
	}

	public class ReturnStatsTemp
	{
		public double staticHitPoints;
		public double staticMagicPoints;
	}
}

