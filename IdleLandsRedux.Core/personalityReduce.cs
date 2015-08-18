using System;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.Core.Interfaces.Reductions;

namespace IdleLandsRedux.Core
{
	public class personalityReduce
	{
		public personalityReduce ()
		{
		}

		public static TRet PersonalityReduce<TPar, TRet>(Func<TRet, TPar, TRet> expr, List<TPar> args, TRet defaultVal = default(TRet))
			where TRet : struct
		{
			TRet total = defaultVal;

			foreach (TPar a in args) {
				total = expr(total, a);
			}

			return total;
		}

		public static List<Tuple<TPar2, TRet>> PersonalityReduce<TPar, TPar2, TRetCalc, TRet>(Func<List<TPar>, TPar2, TRetCalc> expr,
			List<TPar> args1, List<TPar2> args2, TRet defaultRetVal = default(TRet))
			where TRetCalc : List<Tuple<TRet, int>>, new()
		{
			List<Tuple<TPar2, TRet>> ret = new List<Tuple<TPar2, TRet>>();

			foreach (TPar2 a in args2) {
				TRetCalc total = new TRetCalc();
				total = expr(args1, a);
				var target = total.OrderBy(x => x.Item2).FirstOrDefault();
				Tuple<TPar2, TRet> retVal = null;
				if (target != null)
					retVal = new Tuple<TPar2, TRet>(a, target.Item1);
				else
					retVal = new Tuple<TPar2, TRet>(a, defaultRetVal);
				ret.Add(retVal);
			}

			return ret;
		}

		public static Func<int, ICalcDamageReduction, int> calcDamageReduction =
		(int total, ICalcDamageReduction arg) => {
			return total + arg.DamageReduction();
		};

		public static Func<List<Tuple<Character, int>>, ICalcPhysicalAttackTargets, List<Tuple<Character, int>>> calcPhysicalAttackTargets = 
			(List<Tuple<Character, int>> allEnemies, ICalcPhysicalAttackTargets actor) => {
			return actor.PhysicalAttackTargets(allEnemies);
		};
	}
}

