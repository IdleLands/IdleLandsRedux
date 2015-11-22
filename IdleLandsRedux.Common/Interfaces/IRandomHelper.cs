using System.Collections.Generic;

namespace IdleLandsRedux.Common
{
	public interface IRandomHelper
	{
		int Next();

		int Next(int max);

		int Next(int min, int max);

		double NextDouble();

		T RandomFromList<T>(IEnumerable<T> list);
	}
}

