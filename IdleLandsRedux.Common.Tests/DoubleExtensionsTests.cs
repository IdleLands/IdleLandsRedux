using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Reflection;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.Common.Tests
{
	[TestFixture]
	public class DoubleExtensionsTests
	{
		[Test]
		public void AlmostEqualTest()
		{
			long magicNumber = BitConverter.DoubleToInt64Bits(10d);
			double A = BitConverter.Int64BitsToDouble(magicNumber);

			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber+1), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber-1), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber+2), 1).ShouldBe(false);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber-2), 1).ShouldBe(false);

			magicNumber = BitConverter.DoubleToInt64Bits(-10d);
			A = BitConverter.Int64BitsToDouble(magicNumber);

			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber+1), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber-1), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber+2), 1).ShouldBe(false);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber-2), 1).ShouldBe(false);

			magicNumber = BitConverter.DoubleToInt64Bits(10000000d);
			A = BitConverter.Int64BitsToDouble(magicNumber);

			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber+1), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber-1), 1).ShouldBe(true);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber+2), 1).ShouldBe(false);
			A.AlmostEqual2sComplement(BitConverter.Int64BitsToDouble(magicNumber-2), 1).ShouldBe(false);

			Action action = () => A.AlmostEqual2sComplement(1d, 0);
			Action action2 = () => A.AlmostEqual2sComplement(1d, 4 * 1024 * 1024);
			action.ShouldThrow<ArgumentException>();
			action2.ShouldThrow<ArgumentException>();
		}
	}
}

