using System;

namespace IdleLandsRedux.Common
{
	//Adapted from http://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
	public class RandomHelper
	{
		private static readonly Random _global = new Random();
		[ThreadStatic] private static Random _local;

		public RandomHelper()
		{
			if (_local == null)
			{
				int seed;
				lock (_global)
				{
					seed = _global.Next();
				}
				_local = new Random(seed);
			}
		}

		public int Next()
		{
			return _local.Next();
		}

		public int Next(int max)
		{
			return _local.Next(max);
		}

		public int Next(int min, int max)
		{
			return _local.Next(min, max);
		}

		public double NextDouble()
		{
			return _local.NextDouble();
		}
	}
}

