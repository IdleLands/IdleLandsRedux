using System;
using System.Reflection;

namespace IdleLandsRedux.Common
{
	public class StatsModifierObject
	{
		//stats
		public double StaticHitPoints { get; set; } = 0;
		public double StaticMagicPoints { get; set; } = 0;
		public double StaticStrength { get; set; } = 0;
		public double StaticConstitution { get; set; } = 0;
		public double StaticDexterity { get; set; } = 0;
		public double StaticAgility { get; set; } = 0;
		public double StaticIntelligence { get; set; } = 0;
		public double StaticWisdom { get; set; } = 0;
		public double StaticLuck { get; set; } = 0;
		public double PercentageHitPoints { get; set; }  = 0;
		public double PercentageMagicPoints { get; set; }  = 0;
		public double PercentageStrength { get; set; }  = 0;
		public double PercentageConstitution { get; set; }  = 0;
		public double PercentageDexterity { get; set; }  = 0;
		public double PercentageAgility { get; set; }  = 0;
		public double PercentageIntelligence { get; set; }  = 0;
		public double PercentageWisdom { get; set; }  = 0;
		public double PercentageLuck { get; set; }  = 0;

		//various
		public double StaticExperienceGain { get; set; }  = 0;
		public double StaticGoldGain { get; set; }  = 0;
		public double StaticPhysicalAttackChance { get; set; }  = 0;
		public double StaticCriticalChance { get; set; }  = 0;
		public double StaticMinimalDamage { get; set; }  = 0;

		[ThreadStatic]
		private static PropertyInfo[] _properties;

		public StatsModifierObject()
		{
			if(_properties == null)
				_properties = typeof(StatsModifierObject).GetProperties();
		}

		public PropertyInfo[] GetProperties()
		{
			return _properties;
		}

		public static StatsModifierObject operator+(StatsModifierObject a, StatsModifierObject b)
		{
			if (a == null)
				throw new NullReferenceException("a");

			if (b == null)
				throw new NullReferenceException("b");

			StatsModifierObject smo = new StatsModifierObject();

			foreach (PropertyInfo info in a.GetProperties()) {
				var tempA = (double)info.GetValue(a); //debugging purposes
				info.SetValue(smo, tempA + (double)info.GetValue(b));
			}

			return smo;
		}

		#region operator overloads

		/// <param name="value">Value to set EVERYTHING to</param>
		public static implicit operator StatsModifierObject(double value)
		{
			StatsModifierObject smo = new StatsModifierObject();

			foreach (PropertyInfo info in smo.GetProperties()) {
				info.SetValue(smo, value);
			}

			return smo;
		}

		public static StatsModifierObject operator*(StatsModifierObject smo1, StatsModifierObject smo2)
		{
			foreach (PropertyInfo info in smo1.GetProperties()) {
				var tempSmo1 = (double)info.GetValue(smo1); //debugging purposes
				info.SetValue(smo1, tempSmo1 * (double)info.GetValue(smo2));
			}

			return smo1;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("[StatsModifierObject: StaticHitPoints={0}, StaticMagicPoints={1}, StaticStrength={2}, StaticConstitution={3}, StaticDexterity={4}, StaticAgility={5}, StaticIntelligence={6}, StaticWisdom={7}, StaticLuck={8}, PercentageHitPoints={9}, PercentageMagicPoints={10}, PercentageStrength={11}, PercentageConstitution={12}, PercentageDexterity={13}, PercentageAgility={14}, PercentageIntelligence={15}, PercentageWisdom={16}, PercentageLuck={17}, StaticExperienceGain={18}, StaticGoldGain={19}, StaticPhysicalAttackChance={20}, StaticCriticalChance={21}, StaticMinimalDamage={22}]", StaticHitPoints, StaticMagicPoints, StaticStrength, StaticConstitution, StaticDexterity, StaticAgility, StaticIntelligence, StaticWisdom, StaticLuck, PercentageHitPoints, PercentageMagicPoints, PercentageStrength, PercentageConstitution, PercentageDexterity, PercentageAgility, PercentageIntelligence, PercentageWisdom, PercentageLuck, StaticExperienceGain, StaticGoldGain, StaticPhysicalAttackChance, StaticCriticalChance, StaticMinimalDamage);
		}
	}
}

