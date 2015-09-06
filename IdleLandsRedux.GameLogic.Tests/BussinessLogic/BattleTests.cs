using System;
using System.Dynamic;
using System.Collections.Generic;
using IdleLandsRedux.GameLogic.BussinessLogic;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.GameLogic.Scripts;
using NUnit.Framework;
using Shouldly;

namespace IdleLandsRedux.GameLogic.Tests.BussinessLogic
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
		public void CalculateStatsTest()
		{
			Battle battle = new Battle(new List<List<Character>>());

			SpecificCharacter ch = new SpecificCharacter {
				Name = "test",
				Class = "Archer",
				Stats = new StatsObject { 
					Level = 1,
					Constitution = 2,
					Intelligence = 3
				}
			};

			dynamic ret = battle.CalculateStats(ch);
			((double)ret.staticHitPoints).ShouldBe(92);
			((double)ret.staticMagicPoints).ShouldBe(35);
		}
	}
}

