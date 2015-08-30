using System;
using IdleLandsRedux.GameLogic.Interfaces.Managers;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.GameLogic.Managers
{
	public class BattleManager : IBattleManager
	{
		private IRandomHelper randomHelper { get; set; }

		public BattleManager(IRandomHelper randomHelper)
		{
			this.randomHelper = randomHelper;
		}


	}
}

