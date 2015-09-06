using System;

namespace IdleLandsRedux.Common
{
	//Taken from https://github.com/seiyria/restricted-number/blob/master/RestrictedNumber.coffee
	public class RestrictedNumber : IEquatable<RestrictedNumber>, IComparable<RestrictedNumber>, IComparable<int>
	{
		public int _maximum { get; private set; }
		public int _minimum { get; private set; }
		public int _current { get; private set; }
		public int _remainder { get; private set; }

		public RestrictedNumber()
		{
			_minimum = _maximum = _current = _remainder = 0;
		}

		public RestrictedNumber(int minimum, int maximum)
		{
			if (minimum > maximum) {
				throw new ArgumentException("Minimum is higher than maximum");
			}

			_minimum = minimum;
			_maximum = _current = maximum;
			_remainder = 0;
		}

		public RestrictedNumber(int minimum, int maximum, int current)
		{
			if (minimum > maximum) {
				throw new ArgumentException("Minimum is higher than maximum");
			}

			_minimum = minimum;
			_maximum = maximum;
			Set(current);
		}

		// Chainable setting functions

		public RestrictedNumber Set(int num)
		{
			int num2 = Math.Min(num, _maximum);
			num2 = Math.Max(num2, _minimum);
			_current = num2;
			_remainder = Math.Abs(num2 - num);
			return this;
		}

		public RestrictedNumber Add(int num)
		{
			return Set(_current + num);
		}

		public RestrictedNumber Sub(int num)
		{
			return Set(_current - num);
		}

		public RestrictedNumber AddAndBound(int num)
		{
			_maximum += num;
			return Add(num);
		}

		public RestrictedNumber SubAndBound(int num)
		{
			_minimum -= num;
			return Sub(num);
		}

		public RestrictedNumber ToMaximum()
		{
			return Set(_maximum);
		}

		public RestrictedNumber ToMinimum()
		{
			return Set(_minimum);
		}

		public RestrictedNumber SetToPercent(int perc)
		{
			return Set(perc * (_maximum - _minimum) / 100);
		}

		public RestrictedNumber AddPercent(int perc)
		{
			return Add(perc * (_maximum - _minimum) / 100);
		}

		public RestrictedNumber SubPercent(int perc)
		{
			return Sub(perc * (_maximum - _minimum) / 100);
		}

		//Non-chainable check functions

		public int GetTotal()
		{
			return _current;
		}

		public bool AtMax()
		{
			return _current == _maximum;
		}

		public bool AtMin()
		{
			return _current == _minimum;
		}

		public int AsPercent()
		{
			return (int)Math.Floor((double)_current / _maximum * 100.0f);
		}

		#region operator overloads

		public static RestrictedNumber operator+(RestrictedNumber a, RestrictedNumber b)
		{
			return new RestrictedNumber(Math.Max(a._minimum, b._minimum), a._maximum + b._maximum, a._current + b._current);
		}

		public static RestrictedNumber operator-(RestrictedNumber a, RestrictedNumber b)
		{
			var newMax = Math.Abs(a._maximum - b._maximum);
			var newMin = Math.Min(a._minimum, b._minimum);

			if (newMin > newMax)
				newMax = newMin;

			return new RestrictedNumber(newMin, newMax, a._current - b._current);
		}

		public static bool operator<(RestrictedNumber a, RestrictedNumber b)
		{
			return a._current < b._current;
		}

		public static bool operator>(RestrictedNumber a, RestrictedNumber b)
		{
			return a._current > b._current;
		}

		public static bool operator==(RestrictedNumber a, RestrictedNumber b)
		{
			if (object.ReferenceEquals(a, null))
				return object.ReferenceEquals(b, null);

			if (object.ReferenceEquals(b, null))
				return object.ReferenceEquals(a, null);

			return a._current == b._current;
		}

		public static bool operator!=(RestrictedNumber a, RestrictedNumber b)
		{
			if (object.ReferenceEquals(a, null))
				return !object.ReferenceEquals(b, null);

			if (object.ReferenceEquals(b, null))
				return !object.ReferenceEquals(a, null);

			return a._current != b._current;
		}

		public static int operator+(int a, RestrictedNumber b)
		{
			return b._current + a;
		}

		public static int operator-(int a, RestrictedNumber b)
		{
			return b._current - a;
		}

		public static int operator*(int a, RestrictedNumber b)
		{
			return b._current * a;
		}

		public static int operator/(int a, RestrictedNumber b)
		{
			return a / b._current;
		}

		public static bool operator<(int a, RestrictedNumber b)
		{
			return a < b._current;
		}

		public static bool operator>(int a, RestrictedNumber b)
		{
			return a > b._current;
		}

		public static bool operator<=(int a, RestrictedNumber b)
		{
			return a <= b._current;
		}

		public static bool operator>=(int a, RestrictedNumber b)
		{
			return a >= b._current;
		}

		public static bool operator==(int a, RestrictedNumber b)
		{
			if (object.ReferenceEquals(b, null))
				return false;
			
			return a == b._current;
		}

		public static bool operator!=(int a, RestrictedNumber b)
		{
			if (object.ReferenceEquals(b, null))
				return true;
			
			return a != b._current;
		}

		public static int operator+(RestrictedNumber a, int b)
		{
			return a._current + b;
		}

		public static int operator-(RestrictedNumber a, int b)
		{
			return a._current - b;
		}

		public static int operator*(RestrictedNumber a, int b)
		{
			return a._current * b;
		}

		public static int operator/(RestrictedNumber a, int b)
		{
			return a._current / b;
		}

		public static bool operator<(RestrictedNumber a, int b)
		{
			return a._current < b;
		}

		public static bool operator>(RestrictedNumber a, int b)
		{
			return a._current > b;
		}

		public static bool operator<=(RestrictedNumber a, int b)
		{
			return a._current <= b;
		}

		public static bool operator>=(RestrictedNumber a, int b)
		{
			return a._current >= b;
		}

		public static bool operator==(RestrictedNumber a, int b)
		{
			if (object.ReferenceEquals(a, null))
				return false;
			
			return a._current == b;
		}

		public static bool operator!=(RestrictedNumber a, int b)
		{
			if (object.ReferenceEquals(a, null))
				return true;
			
			return a._current != b;
		}

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

			return rn._current == _current && rn._maximum == _maximum && rn._minimum == _minimum && rn._remainder == _remainder;
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + _current.GetHashCode();
			hash = (hash * 7) + _maximum.GetHashCode();
			hash = (hash * 7) + _minimum.GetHashCode();
			hash = (hash * 7) + _remainder.GetHashCode();
			return hash;
		}

		#endregion

		#region IComparable members

		public int CompareTo(RestrictedNumber rn)
		{
			if (rn == null)
				throw new ArgumentNullException("rn");

			if (this._current > rn._current)
				return -1;
			if (this._current == rn._current)
				return 0;
			return 1;
		}

		public int CompareTo(int num)
		{
			if (this._current > num)
				return -1;
			if (this._current == num)
				return 0;
			return 1;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("[RestrictedNumber: _maximum={0}, _minimum={1}, _current={2}, _remainder={3}]", _maximum, _minimum, _current, _remainder);
		}
	}
}

