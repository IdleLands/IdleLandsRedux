using System;

namespace IdleLandsRedux.Common
{
	//Taken from https://github.com/seiyria/restricted-number/blob/master/RestrictedNumber.coffee
	public class RestrictedNumber : IEquatable<RestrictedNumber>
	{
		public int _maximum { get; private set; }
		public int _minimum { get; private set; }
		public int _current { get; private set; }

		public RestrictedNumber()
		{
			_minimum = _maximum = _current = 0;
		}

		public RestrictedNumber(int minimum, int maximum, int? current)
		{
			_minimum = minimum;
			_maximum = maximum;
			_current = current.HasValue ? current.Value : maximum;
		}

		// Chainable setting functions

		public RestrictedNumber Set(int num)
		{
			num = Math.Min(num, _maximum);
			num = Math.Max(num, _minimum);
			_current = num;
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
			return (int)Math.Floor((double)GetTotal() / _maximum * 100.0f);
		}

		// Equals

		public override bool Equals(object obj)
		{
			var rn = obj as RestrictedNumber;

			if (rn == null)
				return false;

			return rn._current == _current && rn._maximum == _maximum && rn._minimum == _minimum;
		}

		public bool Equals(RestrictedNumber rn)
		{
			if (rn == null)
				return false;

			return rn._current == _current && rn._maximum == _maximum && rn._minimum == _minimum;
		}

		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + _current.GetHashCode();
			hash = (hash * 7) + _maximum.GetHashCode();
			hash = (hash * 7) + _minimum.GetHashCode();
			return hash;
		}
	}
}

