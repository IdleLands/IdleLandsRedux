using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IdleLandsRedux.Common
{
	[Serializable]
	public class DisallowedValueModificationException : Exception
	{
		public DisallowedValueModificationException(string message)
			: base(message)
		{
		}
	}

	public class StatsModifierCollection
	{
		//fields - stats
		public StatsModifierObject _HitPoints = new StatsModifierObject();
		public StatsModifierObject _MagicPoints = new StatsModifierObject();
		public StatsModifierObject _Strength = new StatsModifierObject();
		public StatsModifierObject _Constitution = new StatsModifierObject();
		public StatsModifierObject _Dexterity = new StatsModifierObject();
		public StatsModifierObject _Agility = new StatsModifierObject();
		public StatsModifierObject _Intelligence = new StatsModifierObject();
		public StatsModifierObject _Wisdom = new StatsModifierObject();
		public StatsModifierObject _Luck = new StatsModifierObject();

		public StatsModifierObject _ExperienceGain = new StatsModifierObject();
		public StatsModifierObject _GoldGain = new StatsModifierObject();
		public StatsModifierObject _PhysicalAttackChance = new StatsModifierObject();
		public StatsModifierObject _CriticalChance = new StatsModifierObject();
		public StatsModifierObject _MinimalDamage = new StatsModifierObject();
		public StatsModifierObject _HpRegen = new StatsModifierObject();

		//properties - stats
		public StatsModifierObject HitPoints { get { return _HitPoints; } set { IsSetAllowed(); _HitPoints = value;  } }
		public StatsModifierObject MagicPoints { get { return _MagicPoints; } set { IsSetAllowed(); _MagicPoints = value;  } }
		public StatsModifierObject Strength { get { return _Strength; } set { IsSetAllowed(); _Strength = value;  } }
		public StatsModifierObject Constitution { get { return _Constitution; } set { IsSetAllowed(); _Constitution = value;  } }
		public StatsModifierObject Dexterity { get { return _Dexterity; } set { IsSetAllowed(); _Dexterity = value;  } }
		public StatsModifierObject Agility  { get { return _Agility; } set { IsSetAllowed(); _Agility = value;  } }
		public StatsModifierObject Intelligence  { get { return _Intelligence; } set { IsSetAllowed(); _Intelligence = value;  } }
		public StatsModifierObject Wisdom  { get { return _Wisdom; } set { IsSetAllowed(); _Wisdom = value;  } }
		public StatsModifierObject Luck { get { return _Luck; } set { IsSetAllowed(); _Luck = value;  } }

		//various
		public StatsModifierObject ExperienceGain  { get { return _ExperienceGain; } set { IsSetAllowed(); _ExperienceGain = value;  } }
		public StatsModifierObject GoldGain { get { return _GoldGain; } set { IsSetAllowed(); _GoldGain = value;  } }
		public StatsModifierObject PhysicalAttackChance { get { return _PhysicalAttackChance; } set { IsSetAllowed(); _PhysicalAttackChance = value;  } }
		public StatsModifierObject CriticalChance { get { return _CriticalChance; } set { IsSetAllowed(); _CriticalChance = value;  } }
		public StatsModifierObject MinimalDamage  { get { return _MinimalDamage; } set { IsSetAllowed(); _MinimalDamage = value;  } }
		public StatsModifierObject HpRegen  { get { return _HpRegen; } set { IsSetAllowed(); _HpRegen = value;  } }

		public bool AllowValueModification { get; set; } = true;

		[ThreadStatic]
		private static PropertyInfo[] _properties;

		public StatsModifierCollection()
		{
			if (_properties == null) {
				_properties = typeof(StatsModifierCollection).GetProperties().Where(x => x.Name != "AllowValueModification").ToArray();
			}
		}

		public PropertyInfo[] GetProperties()
		{
			//TODO performance optimisation: http://stackoverflow.com/questions/724143/how-do-i-create-a-delegate-for-a-net-property
			return _properties;
		}

		public static StatsModifierCollection operator+(StatsModifierCollection a, StatsModifierCollection b)
		{
			if (a == null)
				throw new NullReferenceException("a");

			if (b == null)
				throw new NullReferenceException("b");

			StatsModifierCollection smo = new StatsModifierCollection();

			foreach (PropertyInfo info in a.GetProperties()) {
				var tempA = (StatsModifierObject)info.GetValue(a); //debugging purposes
				var tempB = (StatsModifierObject)info.GetValue(b); //debugging purposes
				info.SetValue(smo, tempA + tempB);
			}

			return smo;
		}

		public double GetTotalOfProperty(string propertyName)
		{
			return 0;
		}

		private void IsSetAllowed([CallerMemberName] string propertyName = "")
		{
			if(!AllowValueModification)
				throw new DisallowedValueModificationException(propertyName);
		}

		#region operator overloads

		/// <param name="value">Value to set EVERYTHING to</param>
		public static implicit operator StatsModifierCollection(double value)
		{
			StatsModifierCollection smc = new StatsModifierCollection();

			foreach (PropertyInfo info in smc.GetProperties()) {
				StatsModifierObject smo = new StatsModifierObject();
				smo.Value = value;
				info.SetValue(smc, smo);
			}

			return smc;
		}

		public static StatsModifierCollection operator*(StatsModifierCollection smo1, StatsModifierCollection smo2)
		{
			StatsModifierCollection ret = new StatsModifierCollection();

			foreach (PropertyInfo info in smo1.GetProperties()) {
				var tempA = (StatsModifierObject)info.GetValue(smo1); //debugging purposes
				var tempB = (StatsModifierObject)info.GetValue(smo2); //debugging purposes
				info.SetValue(ret, tempA * tempB);
			}

			return ret;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("[StatsModifierCollection: HitPoints={0}, MagicPoints={1}, Strength={2}, Constitution={3}, Dexterity={4}, Agility={5}, Intelligence={6}, Wisdom={7}, Luck={8}, ExperienceGain={9}, GoldGain={10}, PhysicalAttackChance={11}, CriticalChance={12}, MinimalDamage={13}, HpRegen={14}, AllowValueModification={15}]", HitPoints, MagicPoints, Strength, Constitution, Dexterity, Agility, Intelligence, Wisdom, Luck, ExperienceGain, GoldGain, PhysicalAttackChance, CriticalChance, MinimalDamage, HpRegen, AllowValueModification);
		}
	}
}

