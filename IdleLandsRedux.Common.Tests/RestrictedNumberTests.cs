using NUnit.Framework;
using FluentAssertions;
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

			rn.Current.Should().Be(rn.Maximum);
			rn.Current.Should().Be(12);
			rn.Remainder.Should().Be(3);

			rn.Sub(5);

			rn.Current.Should().Be(7);
			rn.Remainder.Should().Be(0);

			rn.Sub(6);

			rn.Current.Should().Be(5);
			rn.Current.Should().Be(rn.Minimum);
			rn.Remainder.Should().Be(4);
		}

		[Test]
		public void ComparisonTest()
		{
			RestrictedNumber rn = new RestrictedNumber(5, 12);
			RestrictedNumber rnNull = null;

			rn.Set(7);

			(rn < 7).Should().Be(false);
			(rn > 7).Should().Be(false);
			(rn <= 7).Should().Be(true);
			(rn >= 7).Should().Be(true);
			(rn == 7).Should().Be(true);
			(rn != 7).Should().Be(false);

			(7 < rn).Should().Be(false);
			(7 > rn).Should().Be(false);
			(7 <= rn).Should().Be(true);
			(7 >= rn).Should().Be(true);
			(7 == rn).Should().Be(true);
			(7 != rn).Should().Be(false);

			(null == rnNull).Should().Be(true);
			(7 == rnNull).Should().Be(false);
			(rnNull == null).Should().Be(true);
			(rnNull == 7).Should().Be(false);
			(null != rnNull).Should().Be(false);
			(7 != rnNull).Should().Be(true);
			(rnNull != null).Should().Be(false);
			(rnNull != 7).Should().Be(true);

			(rn == rnNull).Should().Be(false);
			(rn != rnNull).Should().Be(true);
			(rnNull == rn).Should().Be(false);
			(rnNull != rn).Should().Be(true);
		}

		[Test]
		public void OperatorTest()
		{
			RestrictedNumber rn = 5;

			rn.Current.Should().Be(5);
			rn.Minimum.Should().Be(int.MinValue);
			rn.Maximum.Should().Be(int.MaxValue);
			rn.Remainder.Should().Be(0);

			(rn * 5).Should().Be(25);
			(rn / 5).Should().Be(1);
			(rn + 5).Should().Be(10);
			(rn - 5).Should().Be(0);

			(5 * rn).Should().Be(25);
			(5 / rn).Should().Be(1);
			(5 + rn).Should().Be(10);
			(5 - rn).Should().Be(0);

			var rn2 = new RestrictedNumber(5, 12, 10);

			var rn3 = rn2 + rn2;
			rn3.Current.Should().Be(20);
			rn3.Minimum.Should().Be(5);
			rn3.Maximum.Should().Be(24);
			rn3.Remainder.Should().Be(0);

			rn3 = rn2 - rn2;
			rn3.Current.Should().Be(5);
			rn3.Minimum.Should().Be(5);
			rn3.Maximum.Should().Be(5);
			rn3.Remainder.Should().Be(5);
		}

		[Test]
		public void MinimumCantBeHigherThanMaximumTest()
		{
			Action action = (() => {
				var rn = new RestrictedNumber(6, 5);
			});

			action.ShouldThrow<ArgumentException>();
		}

		[Test]
		public void ShouldConcatenateTextTest()
		{
			RestrictedNumber rn = 5;

			("test: " + rn).Should().Be("test: [RestrictedNumber: _maximum=2147483647, _minimum=-2147483648, _current=5, _remainder=0]");
		}

		[Test]
		public void AsPercentageTest()
		{
			RestrictedNumber rn = new RestrictedNumber(0, 1000, 500);

			rn.AsPercent().Should().Be(50);
			rn.Set(499).AsPercent().Should().Be(49);
			rn.Set(501).AsPercent().Should().Be(50);
		}

		[Test]
		public void HashcodeShouldNotThrowException()
		{
			RestrictedNumber rn = new RestrictedNumber(0, int.MaxValue, int.MaxValue);

			Action action = () => rn.GetHashCode();


			action.ShouldNotThrow();
		}
	}
}

