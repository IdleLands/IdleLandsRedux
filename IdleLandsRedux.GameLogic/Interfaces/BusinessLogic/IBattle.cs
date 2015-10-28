using System;
using System.Collections.Generic;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.GameLogic.Interfaces.BusinessLogic
{
	public interface IBattle
	{
		void SetTurnOrder(ref List<SpecificCharacter> characters);
		List<SpecificCharacter> GetValidTargetsFor(SpecificCharacter character);
		bool MoreThanOneTeamAlive();
		List<SpecificCharacter> GetVictoriousTeam();
		StatsModifierCollection CalculateStats(SpecificCharacter character);
		void TakeTurn();
	}
}

