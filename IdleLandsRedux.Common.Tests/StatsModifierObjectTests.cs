using NUnit.Framework;
using Shouldly;
using System;

namespace IdleLandsRedux.Common.Tests
{
	[TestFixture]
	public class StatsModifierObjectTests
	{
		[Test]
		public void ShouldHaveFields()
		{
			StatsModifierObject smo = new StatsModifierObject();

			var fields = smo.GetProperties();
			fields.ShouldNotBeNull();
			fields.Length.ShouldBeGreaterThan(0);
		}

		[Test]
		public void ShouldAddTest()
		{
			StatsModifierObject smo = new StatsModifierObject();
			StatsModifierObject smo2 = new StatsModifierObject();

			smo.PercentageLuck = 5;
			smo2.PercentageLuck = 5;
			smo2.StaticWisdom = 50;

			smo += smo2;

			smo.PercentageLuck.ShouldBe(10);
			smo.StaticWisdom.ShouldBe(50);
		}

		[Test]
		public void ShouldMultiplyTests()
		{
			StatsModifierObject smo1 = new StatsModifierObject();
			smo1.PercentageLuck = 10;
			StatsModifierObject smo2 = 1.5d;

			foreach (var info in smo2.GetProperties()) {
				((double)info.GetValue(smo2)).ShouldBe(1.5d);
			}

			smo1 *= smo2;

			smo1.PercentageLuck.ShouldBe(15d);
		}
	}
}

