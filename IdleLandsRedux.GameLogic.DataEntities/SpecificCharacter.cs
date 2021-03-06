﻿using System;
using System.Collections.Generic;
using System.Linq;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.DataEntities.Interfaces.Reductions;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.GameLogic.DataEntities
{
    public partial class SpecificCharacter : Character, ICalcPhysicalAttackTargets, ICalcDamageReduction, IEquatable<SpecificCharacter>
    {
        public int Special { get; set; } //For stuff like rage (barbarian), focus (archer) etc
        public StatsModifierCollection CalculatedStats { get; set; }
        public StatsObject TotalStats { get; set; }
        public TransientStatsObject TransientStats { get; set; } //Accumulated damage, poison etc

        public bool Dead { get; set; }
        public bool Fled { get; set; }
        public bool StillParticipatingInCombat { get { return !Dead && !Fled; } }

        public SpecificCharacter() : base()
        {
        }

        public SpecificCharacter(Character c) : base()
        {
            if(c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }
            
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

        public virtual ICollection<Tuple<Character, int>> PhysicalAttackTargets(ICollection<Tuple<Character, int>> allEnemies)
        {
            var validTargets = allEnemies.Where(x => x.Item1.Id != this.Id).ToList();
            var tuple = validTargets.OrderBy(x => x.Item1.Stats.Dexterity).First();
            validTargets.Remove(tuple);
            validTargets.Add(new Tuple<Character, int>(tuple.Item1, 200));
            return validTargets;
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
            unchecked
            { // Overflow is fine, just wrap
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

