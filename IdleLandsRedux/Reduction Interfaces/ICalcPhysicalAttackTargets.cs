using System;
using System.Collections.Generic;

namespace IdleLandsRedux
{
	public interface ICalcPhysicalAttackTargets
	{
		//allEnemies = every target that you would want to kill
		//allCombatMembers = every target in the entire battle
		List<Tuple<IActor, int>> PhysicalAttackTargets(List<Tuple<IActor, int>> allEnemies);
	}
}

