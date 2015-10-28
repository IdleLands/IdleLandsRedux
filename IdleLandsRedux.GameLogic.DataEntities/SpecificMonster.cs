using System;
using System.Collections.Generic;
using System.Linq;

namespace IdleLandsRedux.GameLogic.DataEntities
{
	public partial class SpecificMonster : SpecificCharacter
	{
		public override int DamageReduction()
		{
			return -25;
		}
	}
}

