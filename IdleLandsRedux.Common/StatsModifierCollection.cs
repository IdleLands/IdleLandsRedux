using System;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Common
{
    [Serializable]
    public class DisallowedValueModificationException : Exception
    {
        public DisallowedValueModificationException() : base()
        {
        }

        public DisallowedValueModificationException(string msg) : base(msg)
        {
        }

        public DisallowedValueModificationException(string msg, Exception inner) : base(msg, inner)
        {

        }

        protected DisallowedValueModificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }

    [SuppressMessage("Gendarme.Rules.Smells", "AvoidCodeDuplicatedInSameClassRule", Justification = "This is true and accepted.")]
    // Anyone have a better name?
    public class StatsModifierCollection
    {
        // fields - stats
        private StatsModifierObject _HitPoints = new StatsModifierObject();
        private StatsModifierObject _MagicPoints = new StatsModifierObject();
        private StatsModifierObject _Strength = new StatsModifierObject();
        private StatsModifierObject _Constitution = new StatsModifierObject();
        private StatsModifierObject _Dexterity = new StatsModifierObject();
        private StatsModifierObject _Agility = new StatsModifierObject();
        private StatsModifierObject _Intelligence = new StatsModifierObject();
        private StatsModifierObject _Wisdom = new StatsModifierObject();
        private StatsModifierObject _Luck = new StatsModifierObject();

        private StatsModifierObject _ExperienceGain = new StatsModifierObject();
        private StatsModifierObject _GoldGain = new StatsModifierObject();
        private StatsModifierObject _PhysicalAttackChance = new StatsModifierObject();
        private StatsModifierObject _CriticalChance = new StatsModifierObject();
        private StatsModifierObject _MinimalDamage = new StatsModifierObject();
        private StatsModifierObject _HitPointsRegen = new StatsModifierObject();
        private StatsModifierObject _MagicPointsRegen = new StatsModifierObject();
        private StatsModifierObject _FleePercent = new StatsModifierObject();
        private StatsModifierObject _DamageReduction = new StatsModifierObject();

        // properties - stats
        public StatsModifierObject HitPoints { get { return _HitPoints; } set { IsSetAllowed(); _HitPoints = value; } }
        public StatsModifierObject MagicPoints { get { return _MagicPoints; } set { IsSetAllowed(); _MagicPoints = value; } }
        public StatsModifierObject Strength { get { return _Strength; } set { IsSetAllowed(); _Strength = value; } }
        public StatsModifierObject Constitution { get { return _Constitution; } set { IsSetAllowed(); _Constitution = value; } }
        public StatsModifierObject Dexterity { get { return _Dexterity; } set { IsSetAllowed(); _Dexterity = value; } }
        public StatsModifierObject Agility { get { return _Agility; } set { IsSetAllowed(); _Agility = value; } }
        public StatsModifierObject Intelligence { get { return _Intelligence; } set { IsSetAllowed(); _Intelligence = value; } }
        public StatsModifierObject Wisdom { get { return _Wisdom; } set { IsSetAllowed(); _Wisdom = value; } }
        public StatsModifierObject Luck { get { return _Luck; } set { IsSetAllowed(); _Luck = value; } }

        // various
        public StatsModifierObject ExperienceGain { get { return _ExperienceGain; } set { IsSetAllowed(); _ExperienceGain = value; } }
        public StatsModifierObject GoldGain { get { return _GoldGain; } set { IsSetAllowed(); _GoldGain = value; } }
        public StatsModifierObject PhysicalAttackChance { get { return _PhysicalAttackChance; } set { IsSetAllowed(); _PhysicalAttackChance = value; } }
        public StatsModifierObject CriticalChance { get { return _CriticalChance; } set { IsSetAllowed(); _CriticalChance = value; } }
        public StatsModifierObject MinimalDamage { get { return _MinimalDamage; } set { IsSetAllowed(); _MinimalDamage = value; } }
        public StatsModifierObject HitPointsRegen { get { return _HitPointsRegen; } set { IsSetAllowed(); _HitPointsRegen = value; } }
        public StatsModifierObject MagicPointsRegen { get { return _MagicPointsRegen; } set { IsSetAllowed(); _MagicPointsRegen = value; } }
        public StatsModifierObject FleePercent { get { return _FleePercent; } set { IsSetAllowed(); _FleePercent = value; } }
        public StatsModifierObject DamageReduction { get { return _DamageReduction; } set { IsSetAllowed(); _DamageReduction = value; } }

        public bool AllowValueModification { get; set; } = true;

        [ThreadStatic]
        private static PropertyInfo[] _properties;

        public StatsModifierCollection()
        {
            if (_properties == null)
            {
                _properties = typeof(StatsModifierCollection).GetProperties().Where(x => x.Name != "AllowValueModification").ToArray();
            }
        }

        public PropertyInfo[] GetProperties()
        {
            // TODO performance optimisation: http://stackoverflow.com/questions/724143/how-do-i-create-a-delegate-for-a-net-property
            return _properties;
        }

        private void IsSetAllowed([CallerMemberName] string propertyName = "")
        {
            if (!AllowValueModification)
                throw new DisallowedValueModificationException(propertyName);
        }

        #region operator overloads

        /// <param name="value">Value to set EVERYTHING to</param>
        public static implicit operator StatsModifierCollection(double value)
        {
            StatsModifierCollection smc = new StatsModifierCollection();

            foreach (PropertyInfo info in smc.GetProperties())
            {
                StatsModifierObject smo = new StatsModifierObject();
                smo.Value = value;
                info.SetValue(smc, smo);
            }

            return smc;
        }

        public static StatsModifierCollection operator +(StatsModifierCollection a, StatsModifierCollection b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            StatsModifierCollection smo = new StatsModifierCollection();

            foreach (PropertyInfo info in a.GetProperties())
            {
                var tempA = (StatsModifierObject)info.GetValue(a); // debugging purposes
                var tempB = (StatsModifierObject)info.GetValue(b); // debugging purposes
                info.SetValue(smo, tempA + tempB);
            }

            return smo;
        }

        public static StatsModifierCollection operator -(StatsModifierCollection a, StatsModifierCollection b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            StatsModifierCollection smo = new StatsModifierCollection();

            foreach (PropertyInfo info in a.GetProperties())
            {
                var tempA = (StatsModifierObject)info.GetValue(a); // debugging purposes
                var tempB = (StatsModifierObject)info.GetValue(b); // debugging purposes
                info.SetValue(smo, tempA - tempB);
            }

            return smo;
        }

        public static StatsModifierCollection operator *(StatsModifierCollection smo1, StatsModifierCollection smo2)
        {
            if (smo1 == null)
            {
                throw new ArgumentNullException(nameof(smo1));
            }

            if (smo2 == null)
            {
                throw new ArgumentNullException(nameof(smo2));
            }

            StatsModifierCollection ret = new StatsModifierCollection();

            foreach (PropertyInfo info in smo1.GetProperties())
            {
                var tempA = (StatsModifierObject)info.GetValue(smo1); // debugging purposes
                var tempB = (StatsModifierObject)info.GetValue(smo2); // debugging purposes
                info.SetValue(ret, tempA * tempB);
            }

            return ret;
        }

        public static StatsModifierCollection operator /(StatsModifierCollection smo1, StatsModifierCollection smo2)
        {
            if (smo1 == null)
            {
                throw new ArgumentNullException(nameof(smo1));
            }

            if (smo2 == null)
            {
                throw new ArgumentNullException(nameof(smo2));
            }

            StatsModifierCollection ret = new StatsModifierCollection();

            foreach (PropertyInfo info in smo1.GetProperties())
            {
                var tempA = (StatsModifierObject)info.GetValue(smo1); // debugging purposes
                var tempB = (StatsModifierObject)info.GetValue(smo2); // debugging purposes
                info.SetValue(ret, tempA / tempB);
            }

            return ret;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[StatsModifierCollection: HitPoints={0}, MagicPoints={1}, Strength={2}, Constitution={3}, Dexterity={4}, Agility={5}, Intelligence={6}, Wisdom={7}, Luck={8}, ExperienceGain={9}, GoldGain={10}, PhysicalAttackChance={11}, CriticalChance={12}, MinimalDamage={13}, HpRegen={14}, AllowValueModification={15}]", HitPoints, MagicPoints, Strength, Constitution, Dexterity, Agility, Intelligence, Wisdom, Luck, ExperienceGain, GoldGain, PhysicalAttackChance, CriticalChance, MinimalDamage, HitPointsRegen, AllowValueModification);
        }
    }
}

