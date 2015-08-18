using System;

namespace IdleLandsRedux.Common
{
	public interface IRandomHelper
	{
		int Next();

		int Next(int max);

		int Next(int min, int max);

		double NextDouble();
	}
}

