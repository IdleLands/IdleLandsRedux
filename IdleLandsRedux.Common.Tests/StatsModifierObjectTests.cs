using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Reflection;

namespace IdleLandsRedux.Common.Tests
{
	[TestFixture]
	public class StatsModifierObjectTests
	{
		[Test]
		public void TotalTest()
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = 10;

			smo.Total.ShouldBe(10d);

			smo.Percent = 10;

			smo.Total.ShouldBe(11d);

			smo.Percent = -10;

			smo.Total.ShouldBe(9d);

			smo.Value = -10;

			smo.Total.ShouldBe(-9d);
		}

		[Test]
		public void OperatorTests()
		{
			StatsModifierObject smo1 = new StatsModifierObject();
			StatsModifierObject smo2 = new StatsModifierObject();

			smo1.Value = 10;
			smo1.Percent = 5;
			smo2.Value = 10;
			smo2.Percent = 8;

			(smo1 + smo2).Value.ShouldBe(20d);
			(smo1 + smo2).Percent.ShouldBe(13d);
			(smo1 - smo2).Value.ShouldBe(0d);
			(smo1 - smo2).Percent.ShouldBe(-3d);
			(smo1 * smo2).Value.ShouldBe(100d);
			(smo1 * smo2).Percent.AlmostEqual2sComplement(13.4d, 5).ShouldBe(true);
			(smo1 / smo2).Value.ShouldBe(1d);
			(smo1 / smo2).Percent.AlmostEqual2sComplement(1.05d/1.08d*100d-100d, 50).ShouldBe(true);

			(smo1 + 5d).Value.ShouldBe(15d);
			(smo1 + 5d).Percent.ShouldBe(5d);
			(smo1 - 5d).Value.ShouldBe(5d);
			(smo1 - 5d).Percent.ShouldBe(5d);
			(smo1 * 5d).Value.ShouldBe(50d);
			(smo1 * 5d).Percent.ShouldBe(5d);
			(smo1 / 5d).Value.ShouldBe(2d);
			(smo1 / 5d).Percent.ShouldBe(5d);

			(100d + smo1).Value.ShouldBe(110d);
			(100d + smo1).Percent.ShouldBe(5d);
			(100d - smo1).Value.ShouldBe(90d);
			(100d - smo1).Percent.ShouldBe(5d);
			(100d * smo1).Value.ShouldBe(1000d);
			(100d * smo1).Percent.ShouldBe(5d);
			(100d / smo1).Value.ShouldBe(10d);
			(100d / smo1).Percent.ShouldBe(5d);

			//Checking if original values are not altered
			smo1.Value.ShouldBe(10);
			smo1.Percent.ShouldBe(5);
			smo2.Value.ShouldBe(10);
			smo2.Percent.ShouldBe(8);
		}
	}
}

