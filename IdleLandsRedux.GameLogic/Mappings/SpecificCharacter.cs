using System;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Interfaces.Reductions;

namespace IdleLandsRedux.GameLogic.SpecificMappings
{
	public partial class SpecificCharacter : Character, ICalcPhysicalAttackTargets, ICalcDamageReduction, IEquatable<SpecificCharacter>
	{
		public SpecificCharacter() : base()
		{
		}

		public SpecificCharacter(Character c) : base()
		{
			this.Id = c.Id;
			this.Name = c.Name;
			this.Gender = c.Gender;
			this.Stats = c.Stats;
			this.Equipment = c.Equipment;
		}

		public virtual int DamageReduction()
		{
			return 0;
		}

		public virtual List<Tuple<Character, int>> PhysicalAttackTargets(List<Tuple<Character, int>> allEnemies)
		{
			var validTargets = allEnemies.Where(x => x.Item1.Id != this.Id).ToList();
			var tuple = validTargets.OrderBy(x => x.Item1.Stats.Dexterity).First();
			validTargets.Remove(tuple);
			validTargets.Add(new Tuple<Character, int>(tuple.Item1, 200));
			return validTargets;
		}

		public StatsObject GetTotalStats()
		{
			var ret = this.Stats;

			foreach (var item in this.Equipment) {
				ret += item.Stats;
			}

			return ret;
		}

		#region IEquatable members

		public override bool Equals(object obj)
		{
			var sc = obj as SpecificCharacter;

			return Equals(sc);
		}

		public bool Equals(SpecificCharacter sc)
		{
			if (sc == null)
				return false;

			if (ReferenceEquals(sc, this))
				return true;

			return sc.Id == 0 && this.Id == 0 ? false : sc.Id == this.Id;
		}

		public override int GetHashCode()
		{
			unchecked { // Overflow is fine, just wrap
				int hash = 13;
				hash = (hash * 7) + Id.GetHashCode();
				if (!string.IsNullOrEmpty(Name))
					hash = (hash * 7) + Name.GetHashCode();
				return hash;
			}
		}

		#endregion
	}
}

