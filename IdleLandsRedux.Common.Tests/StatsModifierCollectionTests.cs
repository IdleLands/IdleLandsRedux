﻿using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;
using System.Reflection;

namespace IdleLandsRedux.Common.Tests
{
	[TestFixture]
	public class StatsModifierCollectionTests
	{
		[Test]
		public void ShouldHaveFields()
		{
			StatsModifierCollection smo = new StatsModifierCollection();

			var fields = smo.GetProperties();
			fields.ShouldNotBeNull();
			fields.Length.ShouldBeGreaterThan(0);
			fields.Where(x => x.Name == "AllowValueModification").ShouldBeEmpty();
		}

		[Test]
		public void ShouldAddTest()
		{
			StatsModifierCollection smc1 = new StatsModifierCollection();
			StatsModifierCollection smc2 = new StatsModifierCollection();

			smc1.Luck.Percent = 5;
			smc2.Luck.Percent = 5;
			smc2.Wisdom.Value = 50;

			smc1 += smc2;

			smc1.Luck.Percent.ShouldBe(10);
			smc1.Wisdom.Value.ShouldBe(50);
		}

		[Test]
		public void ShouldMultiplyTests()
		{
			StatsModifierCollection smo1 = new StatsModifierCollection();
			smo1.Luck.Value = 10;
			StatsModifierCollection smo2 = 1.5d;

			foreach (var info in smo2.GetProperties()) {
				((StatsModifierObject)info.GetValue(smo2)).Value.ShouldBe(1.5d);
			}

			smo1 *= smo2;

			smo1.Luck.Value.ShouldBe(15d);
		}

		[Test]
		public void DisallowedModificationShouldThrowException()
		{
			StatsModifierCollection smc1 = new StatsModifierCollection();
			smc1.AllowValueModification = false;

			foreach (var property in smc1.GetProperties()) {
				bool success = false;
				try {
					StatsModifierObject smo = 2d;
					property.SetValue(smc1, smo);
					Console.WriteLine(property.Name);
				} catch (Exception e) {
					if (e.InnerException != null && e.InnerException.GetType() == typeof(DisallowedValueModificationException) && e.InnerException.Message == property.Name) {
						success = true;
					} else {
						Console.WriteLine(property.Name);
					}
				}
				success.ShouldBe(true);
			}
		}

		[Test]
		public void EveryPropertyShouldBeMappedProperly()
		{
			StatsModifierCollection smc1 = new StatsModifierCollection();
			StatsModifierObject smo = 2d;

			foreach (var property1 in smc1.GetProperties()) {
				smc1 = 1;
				property1.SetValue(smc1, smo);

				((StatsModifierObject)property1.GetValue(smc1)).Value.ShouldBe(2d);

				foreach (var property2 in smc1.GetProperties().Where(x => x.Name != property1.Name)) {
					((StatsModifierObject)property2.GetValue(smc1)).Value.ShouldBe(1d);
				}
			}
		}
	}
}
