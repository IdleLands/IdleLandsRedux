using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.BattleActions;
using IdleLandsRedux.GameLogic.DataEntities;
using System.Collections.Generic;

namespace IdleLandsRedux.GameLogic.BusinessLogic.BattleActions
{
	public abstract class BattleAction : IBattleAction
	{
		public SpecificCharacter Originator { get; set; }
		public IEnumerable<SpecificCharacter> Targets { get; set; }

		public abstract void ActionExecute();
		public abstract void ActionDone();
	}
}

