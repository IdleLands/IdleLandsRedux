﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace IdleLands2.NET
{
	public sealed class Person : ICalcDamageReduction, IActor
	{
		public int id { get; set; }
		public int str { get; set; }
		public int strPercent { get; set; }
		public int @int { get; set; }
		public int intPercent { get; set; }
		public int dex { get; set; }
		public int dexPercent { get; set; }

		public Person ()
		{
		}

		public int DamageReduction()
		{
			return 50;
		}

		public List<Tuple<IActor, int>> PhysicalAttackTargets(List<Tuple<IActor, int>> allEnemies)
		{
			var validTargets = allEnemies.Where(x => x.Item1.id != this.id).ToList();
			var tuple = validTargets.OrderBy(x => x.Item1.dex).First();
			validTargets.Remove(tuple);
			validTargets.Add(new Tuple<IActor, int>(tuple.Item1, 200));
			return validTargets;
		}
	}
}

