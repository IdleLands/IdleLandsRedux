using NUnit.Framework;
using FluentAssertions;

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

			smo.Total.Should().Be(10d);

			smo.Percent = 10;

			smo.Total.Should().Be(11d);

			smo.Percent = -10;

			smo.Total.Should().Be(9d);

			smo.Value = -10;

			smo.Total.Should().Be(-9d);
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

			(smo1 + smo2).Value.Should().Be(20d);
			(smo1 + smo2).Percent.Should().Be(13d);
			(smo1 - smo2).Value.Should().Be(0d);
			(smo1 - smo2).Percent.Should().Be(-3d);
			(smo1 * smo2).Value.Should().Be(100d);
			(smo1 * smo2).Percent.AlmostEqual2sComplement(13.4d, 5).Should().Be(true);
			(smo1 / smo2).Value.Should().Be(1d);
			(smo1 / smo2).Percent.AlmostEqual2sComplement(1.05d/1.08d*100d-100d, 50).Should().Be(true);

			(smo1 + 5d).Value.Should().Be(15d);
			(smo1 + 5d).Percent.Should().Be(5d);
			(smo1 - 5d).Value.Should().Be(5d);
			(smo1 - 5d).Percent.Should().Be(5d);
			(smo1 * 5d).Value.Should().Be(50d);
			(smo1 * 5d).Percent.Should().Be(5d);
			(smo1 / 5d).Value.Should().Be(2d);
			(smo1 / 5d).Percent.Should().Be(5d);

			(100d + smo1).Value.Should().Be(110d);
			(100d + smo1).Percent.Should().Be(5d);
			(100d - smo1).Value.Should().Be(90d);
			(100d - smo1).Percent.Should().Be(5d);
			(100d * smo1).Value.Should().Be(1000d);
			(100d * smo1).Percent.Should().Be(5d);
			(100d / smo1).Value.Should().Be(10d);
			(100d / smo1).Percent.Should().Be(5d);

			//Checking if original values are not altered
			smo1.Value.Should().Be(10);
			smo1.Percent.Should().Be(5);
			smo2.Value.Should().Be(10);
			smo2.Percent.Should().Be(8);
		}
	}
}

