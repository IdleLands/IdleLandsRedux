using System;
using System.Collections.Generic;
using System.Linq;

namespace IdleLandsRedux.SpecificMappings
{
	public partial class SpecificPlayer : SpecificCharacter
	{
		public override int DamageReduction()
		{
			return 50;
		}
	}
}

