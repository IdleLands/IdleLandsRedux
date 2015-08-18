using System;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.Core.SpecificMappings;

namespace IdleLandsRedux.Core.BusinessLogic
{
	public class Battle
	{
		public Dictionary<string, List<SpecificCharacter>> _allCharactersInTeams { get; set; }

		public Battle()
		{
			_allCharactersInTeams = new Dictionary<string, List<SpecificCharacter>>();
		}

		public List<SpecificCharacter> GetAllCharacters()
		{
			List<SpecificCharacter> ret = new List<SpecificCharacter>();

			foreach (var entry in _allCharactersInTeams) {
				ret.AddRange(entry.Value);
			}

			return ret;
		}

		//Not using _characters here, since some functions don't want to include dead people. Ugh. Zombies.
		public void SetTurnOrder(ref List<SpecificCharacter> characters)
		{
			characters.Sort((a, b) => (a.GetTotalStats().Agility * (1 + a.GetTotalStats().AgilityPercent)).CompareTo( 
				b.GetTotalStats().Agility * (1 + b.GetTotalStats().AgilityPercent)));
		}

		public List<SpecificCharacter> GetValidTargetsFor(SpecificCharacter character)
		{
			return null;
		}
	}
}

