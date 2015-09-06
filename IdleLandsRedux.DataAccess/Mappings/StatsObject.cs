using System;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.DataAccess.Mappings
{
	public class StatsObject
	{
		public virtual int Id { get; set; }
		public virtual RestrictedNumber Strength { get; set; }
		public virtual int StrengthPercent { get; set; }
		public virtual RestrictedNumber Dexterity { get; set; }
		public virtual int DexterityPercent { get; set; }
		public virtual RestrictedNumber Intelligence { get; set; }
		public virtual int IntelligencePercent { get; set; }
		public virtual RestrictedNumber Constitution { get; set; }
		public virtual int ConstitutionPercent { get; set; }
		public virtual RestrictedNumber Wisdom { get; set; }
		public virtual int WisdomPercent { get; set; }
		public virtual RestrictedNumber Agility { get; set; }
		public virtual int AgilityPercent { get; set; }
		public virtual RestrictedNumber Luck { get; set; }
		public virtual int LuckPercent { get; set; }
		public virtual RestrictedNumber HitPoints { get; set; }
		public virtual int HitPointsPercent { get; set; }
		public virtual RestrictedNumber MagicPoints { get; set; }
		public virtual int MagicPointsPercent { get; set; }
		public virtual RestrictedNumber Level { get; set; }
		public virtual RestrictedNumber ExperiencePoints { get; set; }
		public virtual int ExperiencePointsPercent { get; set; }
		public virtual RestrictedNumber Gold { get; set; }
		public virtual int GoldPercent { get; set; }

		public static StatsObject operator+(StatsObject a, StatsObject b)
		{
			if (a == null) {
				throw new ArgumentNullException("a");
			}

			if (b == null) {
				throw new ArgumentNullException("b");
			}

			StatsObject ret = new StatsObject();

			ret.Strength = a.Strength + b.Strength;
			ret.StrengthPercent = a.StrengthPercent + b.StrengthPercent;
			ret.Dexterity = a.Dexterity + b.Dexterity;
			ret.DexterityPercent = a.DexterityPercent + b.DexterityPercent;
			ret.Intelligence = a.Intelligence + b.Intelligence;
			ret.IntelligencePercent = a.IntelligencePercent + b.IntelligencePercent;
			ret.Constitution = a.Constitution + b.Constitution;
			ret.ConstitutionPercent = a.ConstitutionPercent + b.ConstitutionPercent;
			ret.Wisdom = a.Wisdom + b.Wisdom;
			ret.WisdomPercent = a.WisdomPercent + b.WisdomPercent;
			ret.Agility = a.Agility + b.Agility;
			ret.AgilityPercent = a.AgilityPercent + b.AgilityPercent;
			ret.Luck = a.Luck + b.Luck;
			ret.LuckPercent = a.LuckPercent + b.LuckPercent;
			ret.HitPoints = a.HitPoints + b.HitPoints;
			ret.HitPointsPercent = a.HitPointsPercent + b.HitPointsPercent;
			ret.MagicPoints = a.MagicPoints + b.MagicPoints;
			ret.MagicPointsPercent = a.MagicPointsPercent + b.MagicPointsPercent;
			ret.Level = a.Level + b.Level; //As if level is important anyway. Psh.
			ret.ExperiencePoints = a.ExperiencePoints + b.ExperiencePoints;
			ret.ExperiencePointsPercent = a.ExperiencePointsPercent + b.ExperiencePointsPercent;
			ret.Gold = a.Gold + b.Gold;
			ret.GoldPercent = a.GoldPercent + b.GoldPercent;

			return ret;
		}
	}
}

