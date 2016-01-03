using System.Collections.Generic;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.Common;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.GameLogic.Interfaces.BusinessLogic
{
	public interface IBattle
	{
		[SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotExposeGenericListsRule", Justification = "I would have to write my own IList.Sort extension method using comparison stuff. Why?")]
		void SetTurnOrder(List<SpecificCharacter> characters);
		ICollection<SpecificCharacter> GetValidTargetsFor(SpecificCharacter character);
		bool MoreThanOneTeamAlive();
		ICollection<SpecificCharacter> GetVictoriousTeam();
		StatsModifierCollection CalculateStats(SpecificCharacter character);
		void TakeTurn(ICollection<string> battleLog);
	}
}

