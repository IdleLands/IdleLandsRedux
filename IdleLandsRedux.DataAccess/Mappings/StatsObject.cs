using IdleLandsRedux.Common;

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
		public virtual int Consitution { get; set; }
		public virtual int ConsitutionPercent { get; set; }
		public virtual int Wisdom { get; set; }
		public virtual int WisdomPercent { get; set; }
		public virtual int Agility { get; set; }
		public virtual int AgilityPercent { get; set; }
		public virtual int Luck { get; set; }
		public virtual int LuckPercent { get; set; }
		public virtual RestrictedNumber HitPoints { get; set; }
		public virtual int HitPointsPercent { get; set; }
		public virtual RestrictedNumber MagicPoints { get; set; }
		public virtual int MagicPointsPercent { get; set; }
		public virtual RestrictedNumber Level { get; set; }
		public virtual int ExperiencePoints { get; set; }
		public virtual int ExperiencePointsPercent { get; set; }
		public virtual int Gold { get; set; }
		public virtual int GoldPercent { get; set; }

		public static StatsObject operator+(StatsObject a, StatsObject b)
		{
			StatsObject ret = new StatsObject();

			ret.Strength = a.Strength + b.Strength;
			ret.StrengthPercent = a.StrengthPercent + b.StrengthPercent;
			ret.Dexterity = a.Dexterity + b.Dexterity;
			ret.DexterityPercent = a.DexterityPercent + b.DexterityPercent;
			ret.Intelligence = a.Intelligence + b.Intelligence;
			ret.IntelligencePercent = a.IntelligencePercent + b.IntelligencePercent;
			ret.Consitution = a.Consitution + b.Consitution;
			ret.ConsitutionPercent = a.ConsitutionPercent + b.ConsitutionPercent;
			ret.Wisdom = a.Wisdom + b.Wisdom;
			ret.WisdomPercent = a.WisdomPercent + b.WisdomPercent;
			ret.Agility = a.Agility + b.Agility;
			ret.AgilityPercent = a.AgilityPercent + b.AgilityPercent;
			ret.Luck = a.Luck + b.Luck;
			ret.LuckPercent = a.LuckPercent + b.LuckPercent;
			//ret.HitPoints = a.HitPoints + b.HitPoints; //TODO
			ret.HitPointsPercent = a.HitPointsPercent + b.HitPointsPercent;
			//ret.MagicPoints = a.MagicPoints + b.MagicPoints;
			ret.MagicPointsPercent = a.MagicPointsPercent + b.MagicPointsPercent;
			//ret.Level = a.Level + b.Level; //As if level is important anyway. Psh.
			ret.ExperiencePoints = a.ExperiencePoints + b.ExperiencePoints;
			ret.ExperiencePointsPercent = a.ExperiencePointsPercent + b.ExperiencePointsPercent;
			ret.Gold = a.Gold + b.Gold;
			ret.GoldPercent = a.GoldPercent + b.GoldPercent;

			return ret;
		}
	}
}

