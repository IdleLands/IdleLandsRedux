using System;
using System.Dynamic;
using System.Collections.Generic;
using IdleLandsRedux.GameLogic.BusinessLogic;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.Common;
using NUnit.Framework;
using Shouldly;

namespace IdleLandsRedux.GameLogic.Tests.BusinessLogic
{
	[TestFixture]
	public class BattleTests
	{
		[TestFixtureSetUp]
		public void TestSetup()
		{
			ScriptHelper.ScriptDir = "/TestScripts/";
		}

		[Test]
		public void CalculateStaticStatsTest()
		{
			Battle battle = new Battle(new List<List<Character>>());

			SpecificCharacter ch = new SpecificCharacter {
				Name = "test",
				Class = "Archer",
				Personalities = "Brave1",
				Stats = new StatsObject { 
					Level = 1,
					Constitution = 2,
					Intelligence = 3
				}
			};

			StatsModifierObject ret = battle.CalculateStats(ch);
			ret.StaticHitPoints.ShouldBe(142);
			ret.StaticMagicPoints.ShouldBe(35);
			ret.PercentageStrength.ShouldBe(5);
		}

		[Test]
		public void CalculateStaticStatsWithHooksTest()
		{
			Battle battle = new Battle(new List<List<Character>>());

			SpecificCharacter ch = new SpecificCharacter {
				Name = "test",
				Class = "Archer1",
				Personalities = "Brave1",
				Stats = new StatsObject { 
					Level = 1,
					Constitution = 2,
					Intelligence = 3
				}
			};

			StatsModifierObject ret = battle.CalculateStats(ch);
			ret.StaticHitPoints.ShouldBe(192);
			ret.StaticMagicPoints.ShouldBe(35);
			ret.PercentageStrength.ShouldBe(5);
		}
	}
}

