using System;
using System.Collections.Generic;
using System.Linq;

namespace IdleLandsRedux.GameLogic.SpecificMappings
{
	public partial class SpecificPlayer : SpecificCharacter
	{
		public override int DamageReduction()
		{
			return 50;
		}
	}
}

