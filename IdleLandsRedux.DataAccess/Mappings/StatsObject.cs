using System;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.DataAccess.Mappings
{
    public class StatsObject
    {
        public virtual int Id { get; set; }
        public virtual RestrictedNumber Strength { get; set; }
        public virtual RestrictedNumber Dexterity { get; set; }
        public virtual RestrictedNumber Intelligence { get; set; }
        public virtual RestrictedNumber Constitution { get; set; }
        public virtual RestrictedNumber Wisdom { get; set; }
        public virtual RestrictedNumber Agility { get; set; }
        public virtual RestrictedNumber Luck { get; set; }
        public virtual RestrictedNumber HitPoints { get; set; }
        public virtual RestrictedNumber MagicPoints { get; set; }
        public virtual RestrictedNumber Level { get; set; }
        public virtual RestrictedNumber ExperiencePoints { get; set; }
        public virtual RestrictedNumber Gold { get; set; }

        public StatsObject()
        {
        }

        public StatsObject(StatsObject baseStats, StatsModifierCollection toApply)
        {
            if (baseStats == null)
            {
				throw new ArgumentNullException(nameof(baseStats));
            }
			
			if (toApply == null)
            {
				throw new ArgumentNullException(nameof(toApply));
            }

            StatsObject retStats = new StatsObject();
            retStats.Strength = new RestrictedNumber(
                (int)(baseStats.HitPoints.Minimum * (1 + toApply.HitPoints.Percent) + toApply.HitPoints.Value),
                (int)(baseStats.HitPoints.Minimum * (1 + toApply.HitPoints.Percent) + toApply.HitPoints.Value),
                (int)(baseStats.HitPoints.Minimum * (1 + toApply.HitPoints.Percent) + toApply.HitPoints.Value));
        }

        public static StatsObject operator +(StatsObject a, StatsObject b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            StatsObject ret = new StatsObject();

            ret.Strength = a.Strength + b.Strength;
            ret.Dexterity = a.Dexterity + b.Dexterity;
            ret.Intelligence = a.Intelligence + b.Intelligence;
            ret.Constitution = a.Constitution + b.Constitution;
            ret.Wisdom = a.Wisdom + b.Wisdom;
            ret.Agility = a.Agility + b.Agility;
            ret.Luck = a.Luck + b.Luck;
            ret.HitPoints = a.HitPoints + b.HitPoints;
            ret.MagicPoints = a.MagicPoints + b.MagicPoints;
            ret.Level = a.Level + b.Level;
            ret.ExperiencePoints = a.ExperiencePoints + b.ExperiencePoints;
            ret.Gold = a.Gold + b.Gold;

            return ret;
        }
        
        public static StatsObject operator -(StatsObject a, StatsObject b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            StatsObject ret = new StatsObject();

            ret.Strength = a.Strength - b.Strength;
            ret.Dexterity = a.Dexterity - b.Dexterity;
            ret.Intelligence = a.Intelligence - b.Intelligence;
            ret.Constitution = a.Constitution - b.Constitution;
            ret.Wisdom = a.Wisdom - b.Wisdom;
            ret.Agility = a.Agility - b.Agility;
            ret.Luck = a.Luck - b.Luck;
            ret.HitPoints = a.HitPoints - b.HitPoints;
            ret.MagicPoints = a.MagicPoints - b.MagicPoints;
            ret.Level = a.Level - b.Level;
            ret.ExperiencePoints = a.ExperiencePoints - b.ExperiencePoints;
            ret.Gold = a.Gold - b.Gold;

            return ret;
        }
    }
}

