using System;

namespace IdleLandsRedux
{
	public interface IActor : ICalcPhysicalAttackTargets
	{
		int id { get; set; }
		int str { get; set; }
		int strPercent { get; set; }
		int @int { get; set; }
		int intPercent { get; set; }
		int dex { get; set; }
		int dexPercent { get; set; }
	}
}

