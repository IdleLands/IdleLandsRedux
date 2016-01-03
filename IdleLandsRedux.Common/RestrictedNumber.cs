using System;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Common
{
    [SuppressMessage("Gendarme.Rules.Correctness", "ReviewInconsistentIdentityRule", Justification = "This is actually desired behaviour. Only for the hashcode & equals methods do we do a deep-equals, otherwise the current is enough for easy comparison.")]
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidCodeDuplicatedInSameClassRule", Justification = "This is true and accepted.")]
    //Taken from https://github.com/seiyria/restricted-number/blob/master/RestrictedNumber.coffee
    public class RestrictedNumber : IEquatable<RestrictedNumber>, IComparable<RestrictedNumber>, IComparable<int>
    {
        public int Maximum { get; private set; }
        public int Minimum { get; private set; }
        public int Current { get; private set; }
        public int Remainder { get; private set; }

        public RestrictedNumber()
        {
            Minimum = Maximum = Current = Remainder = 0;
        }

        public RestrictedNumber(int minimum, int maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum is higher than maximum");
            }

            Minimum = minimum;
            Maximum = Current = maximum;
            Remainder = 0;
        }

        public RestrictedNumber(int minimum, int maximum, int current)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("Minimum is higher than maximum");
            }

            Minimum = minimum;
            Maximum = maximum;
            Set(current);
        }

        // Chainable setting functions

        public RestrictedNumber Set(int num)
        {
            int num2 = Math.Min(num, Maximum);
            num2 = Math.Max(num2, Minimum);
            Current = num2;
            Remainder = Math.Abs(num2 - num);
            return this;
        }

        public RestrictedNumber Add(int num)
        {
            return Set(Current + num);
        }

        public RestrictedNumber Sub(int num)
        {
            return Set(Current - num);
        }

        public RestrictedNumber AddAndBound(int num)
        {
            Maximum += num;
            return Add(num);
        }

        public RestrictedNumber SubAndBound(int num)
        {
            Minimum -= num;
            return Sub(num);
        }

        public RestrictedNumber ToMaximum()
        {
            return Set(Maximum);
        }

        public RestrictedNumber ToMinimum()
        {
            return Set(Minimum);
        }

        public RestrictedNumber SetToPercent(int perc)
        {
            return Set(perc * (Maximum - Minimum) / 100);
        }

        public RestrictedNumber AddPercent(int perc)
        {
            return Add(perc * (Maximum - Minimum) / 100);
        }

        public RestrictedNumber SubPercent(int perc)
        {
            return Sub(perc * (Maximum - Minimum) / 100);
        }

        //Non-chainable check functions

        public int GetTotal()
        {
            return Current;
        }

        public bool AtMax()
        {
            return Current == Maximum;
        }

        public bool AtMin()
        {
            return Current == Minimum;
        }

        public int AsPercent()
        {
            return (int)Math.Floor((double)Current / Maximum * 100.0f);
        }

        #region operator overloads

        public static RestrictedNumber operator +(RestrictedNumber a, RestrictedNumber b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new RestrictedNumber(Math.Max(a.Minimum, b.Minimum), a.Maximum + b.Maximum, a.Current + b.Current);
        }

        public static RestrictedNumber operator -(RestrictedNumber a, RestrictedNumber b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var newMax = Math.Abs(a.Maximum - b.Maximum);
            var newMin = Math.Min(a.Minimum, b.Minimum);

            if (newMin > newMax)
                newMax = newMin;

            return new RestrictedNumber(newMin, newMax, a.Current - b.Current);
        }

        public static bool operator <(RestrictedNumber a, RestrictedNumber b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a.Current < b.Current;
        }

        public static bool operator >(RestrictedNumber a, RestrictedNumber b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a.Current > b.Current;
        }

        public static bool operator ==(RestrictedNumber a, RestrictedNumber b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);

            if (object.ReferenceEquals(b, null))
                return object.ReferenceEquals(a, null);

            return a.Current == b.Current;
        }

        public static bool operator !=(RestrictedNumber a, RestrictedNumber b)
        {
            if (object.ReferenceEquals(a, null))
                return !object.ReferenceEquals(b, null);

            if (object.ReferenceEquals(b, null))
                return !object.ReferenceEquals(a, null);

            return a.Current != b.Current;
        }

        public static int operator +(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return b.Current + a;
        }

        public static int operator -(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return b.Current - a;
        }

        public static int operator *(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return b.Current * a;
        }

        public static int operator /(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a / b.Current;
        }

        public static bool operator <(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a < b.Current;
        }

        public static bool operator >(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a > b.Current;
        }

        public static bool operator <=(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a <= b.Current;
        }

        public static bool operator >=(int a, RestrictedNumber b)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return a >= b.Current;
        }

        public static bool operator ==(int a, RestrictedNumber b)
        {
            if (object.ReferenceEquals(b, null))
                return false;

            return a == b.Current;
        }

        public static bool operator !=(int a, RestrictedNumber b)
        {
            if (object.ReferenceEquals(b, null))
                return true;

            return a != b.Current;
        }

        public static int operator +(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current + b;
        }

        public static int operator -(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current - b;
        }

        public static int operator *(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current * b;
        }

        public static int operator /(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current / b;
        }

        public static bool operator <(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current < b;
        }

        public static bool operator >(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current > b;
        }

        public static bool operator <=(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current <= b;
        }

        public static bool operator >=(RestrictedNumber a, int b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return a.Current >= b;
        }

        public static bool operator ==(RestrictedNumber a, int b)
        {
            if (object.ReferenceEquals(a, null))
                return false;

            return a.Current == b;
        }

        public static bool operator !=(RestrictedNumber a, int b)
        {
            if (object.ReferenceEquals(a, null))
                return true;

            return a.Current != b;
        }

        /// <param name="value">Value to set EVERYTHING to</param>
        public static implicit operator RestrictedNumber(int value)
        {
            return new RestrictedNumber(int.MinValue, int.MaxValue, value);
        }

        #endregion

        #region IEquatable members

        public override bool Equals(object obj)
        {
            var rn = obj as RestrictedNumber;

            return Equals(rn);
        }

        public bool Equals(RestrictedNumber rn)
        {
            if (rn == null)
                return false;

            if (ReferenceEquals(rn, this))
                return true;

            return rn.Current == Current && rn.Maximum == Maximum && rn.Minimum == Minimum && rn.Remainder == Remainder;
        }

        public override int GetHashCode()
        {
            unchecked
            { // Overflow is fine, just wrap
                int hash = 13;
                hash = (hash * 7) + Current.GetHashCode();
                hash = (hash * 7) + Maximum.GetHashCode();
                hash = (hash * 7) + Minimum.GetHashCode();
                hash = (hash * 7) + Remainder.GetHashCode();
                return hash;
            }
        }

        #endregion

        #region IComparable members

        public int CompareTo(RestrictedNumber rn)
        {
            if (rn == null)
                throw new ArgumentNullException(nameof(rn));

            if (this.Current > rn.Current)
                return -1;
            if (this.Current == rn.Current)
                return 0;
            return 1;
        }

        public int CompareTo(int num)
        {
            if (this.Current > num)
                return -1;
            if (this.Current == num)
                return 0;
            return 1;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[RestrictedNumber: _maximum={0}, _minimum={1}, _current={2}, _remainder={3}]", Maximum, Minimum, Current, Remainder);
        }
    }
}

