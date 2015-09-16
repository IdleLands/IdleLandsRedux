﻿using NUnit.Framework;
using Shouldly;
using System;

namespace IdleLandsRedux.Common.Tests
{
	[TestFixture]
	public class RestrictedNumberTests
	{
		[Test]
		public void RemainderTest()
		{
			RestrictedNumber rn = new RestrictedNumber(5, 12);

			rn.Add(3);

			rn.Current.ShouldBe(rn.Maximum);
			rn.Current.ShouldBe(12);
			rn.Remainder.ShouldBe(3);

			rn.Sub(5);

			rn.Current.ShouldBe(7);
			rn.Remainder.ShouldBe(0);

			rn.Sub(6);

			rn.Current.ShouldBe(5);
			rn.Current.ShouldBe(rn.Minimum);
			rn.Remainder.ShouldBe(4);
		}

		[Test]
		public void ComparisonTest()
		{
			RestrictedNumber rn = new RestrictedNumber(5, 12);
			RestrictedNumber rnNull = null;

			rn.Set(7);

			(rn < 7).ShouldBe(false);
			(rn > 7).ShouldBe(false);
			(rn <= 7).ShouldBe(true);
			(rn >= 7).ShouldBe(true);
			(rn == 7).ShouldBe(true);
			(rn != 7).ShouldBe(false);

			(7 < rn).ShouldBe(false);
			(7 > rn).ShouldBe(false);
			(7 <= rn).ShouldBe(true);
			(7 >= rn).ShouldBe(true);
			(7 == rn).ShouldBe(true);
			(7 != rn).ShouldBe(false);

			(null == rnNull).ShouldBe(true);
			(7 == rnNull).ShouldBe(false);
			(rnNull == null).ShouldBe(true);
			(rnNull == 7).ShouldBe(false);
			(null != rnNull).ShouldBe(false);
			(7 != rnNull).ShouldBe(true);
			(rnNull != null).ShouldBe(false);
			(rnNull != 7).ShouldBe(true);

			(rn == rnNull).ShouldBe(false);
			(rn != rnNull).ShouldBe(true);
			(rnNull == rn).ShouldBe(false);
			(rnNull != rn).ShouldBe(true);
		}

		[Test]
		public void OperatorTest()
		{
			RestrictedNumber rn = 5;

			rn.Current.ShouldBe(5);
			rn.Minimum.ShouldBe(int.MinValue);
			rn.Maximum.ShouldBe(int.MaxValue);
			rn.Remainder.ShouldBe(0);

			(rn * 5).ShouldBe(25);
			(rn / 5).ShouldBe(1);
			(rn + 5).ShouldBe(10);
			(rn - 5).ShouldBe(0);

			(5 * rn).ShouldBe(25);
			(5 / rn).ShouldBe(1);
			(5 + rn).ShouldBe(10);
			(5 - rn).ShouldBe(0);

			var rn2 = new RestrictedNumber(5, 12, 10);

			var rn3 = rn2 + rn2;
			rn3.Current.ShouldBe(20);
			rn3.Minimum.ShouldBe(5);
			rn3.Maximum.ShouldBe(24);
			rn3.Remainder.ShouldBe(0);

			rn3 = rn2 - rn2;
			rn3.Current.ShouldBe(5);
			rn3.Minimum.ShouldBe(5);
			rn3.Maximum.ShouldBe(5);
			rn3.Remainder.ShouldBe(5);
		}

		[Test]
		public void MinimumCantBeHigherThanMaximumTest()
		{
			Action action = (() => {
				var rn = new RestrictedNumber(6, 5);
			});

			Should.Throw<ArgumentException>(action);
		}

		[Test]
		public void ShouldConcatenateTextTest()
		{
			RestrictedNumber rn = 5;

			("test: " + rn).ShouldBe("test: [RestrictedNumber: _maximum=2147483647, _minimum=-2147483648, _current=5, _remainder=0]");
		}

		[Test]
		public void AsPercentageTest()
		{
			RestrictedNumber rn = new RestrictedNumber(0, 1000, 500);

			rn.AsPercent().ShouldBe(50);
			rn.Set(499).AsPercent().ShouldBe(49);
			rn.Set(501).AsPercent().ShouldBe(50);
		}

		[Test]
		public void HashcodeShouldNotThrowException()
		{
			RestrictedNumber rn = new RestrictedNumber(0, int.MaxValue, int.MaxValue);

			Should.NotThrow(() => rn.GetHashCode());
		}
	}
}

