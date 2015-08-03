using System;
using System.Collections.Generic;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux
{
	public interface ICalcPhysicalAttackTargets
	{
		//allEnemies = every target that you would want to kill
		//allCombatMembers = every target in the entire battle
		List<Tuple<Character, int>> PhysicalAttackTargets(List<Tuple<Character, int>> allEnemies);
	}
}

