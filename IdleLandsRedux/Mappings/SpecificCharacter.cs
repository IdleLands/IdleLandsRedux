using System;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.SpecificMappings
{
	public partial class SpecificCharacter : Character, ICalcPhysicalAttackTargets, ICalcDamageReduction
	{
		public virtual int DamageReduction()
		{
			return 0;
		}

		public virtual List<Tuple<Character, int>> PhysicalAttackTargets(List<Tuple<Character, int>> allEnemies)
		{
			var validTargets = allEnemies.Where(x => x.Item1.Id != this.Id).ToList();
			var tuple = validTargets.OrderBy(x => x.Item1.Stats.Dexterity).First();
			validTargets.Remove(tuple);
			validTargets.Add(new Tuple<Character, int>(tuple.Item1, 200));
			return validTargets;
		}

		public StatsObject GetTotalStats()
		{
			var ret = this.Stats;

			foreach (var item in this.Equipment) {
				ret += item.Stats;
			}

			return ret;
		}
	}
}

