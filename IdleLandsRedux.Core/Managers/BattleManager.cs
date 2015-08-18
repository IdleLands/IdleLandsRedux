using System;
using IdleLandsRedux.Core.Interfaces.Managers;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.Core.Managers
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

