using System;
using System.Collections.Generic;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.GameLogic.DataEntities.Interfaces.Reductions
{
	public interface ICalcPhysicalAttackTargets
	{
		//allEnemies = every target that you would want to kill
		//allCombatMembers = every target in the entire battle
		ICollection<Tuple<Character, int>> PhysicalAttackTargets(ICollection<Tuple<Character, int>> allEnemies);
	}
}

