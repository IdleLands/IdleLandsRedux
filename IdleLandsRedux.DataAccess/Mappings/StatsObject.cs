using System;

namespace IdleLandsRedux.DataAccess.Mappings
{
	public class StatsObject
	{
		public virtual int Id { get; set; }
		public virtual int Strength { get; set; }
		public virtual int StrengthPercent { get; set; }
		public virtual int Dexterity { get; set; }
		public virtual int DexterityPercent { get; set; }
		public virtual int Intelligence { get; set; }
		public virtual int IntelligencePercent { get; set; }
		public virtual int HitPoints { get; set; }
		public virtual int HitPointsPercent { get; set; }
		public virtual int MagicPoints { get; set; }
		public virtual int MagicPointsPercent { get; set; }

		public StatsObject()
		{
		}
	}
}

