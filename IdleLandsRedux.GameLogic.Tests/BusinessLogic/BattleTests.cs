using System;
using System.Dynamic;
using System.Collections.Generic;
using IdleLandsRedux.GameLogic.BusinessLogic;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.Interfaces.Scripts;
using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.Interop;
using IdleLandsRedux.GameLogic.BusinessLogic.Interop;
using IdleLandsRedux.Common;
using NUnit.Framework;
using Shouldly;
using Microsoft.Practices.Unity;
using Moq;

namespace IdleLandsRedux.GameLogic.Tests.BusinessLogic
{
	[TestFixture]
	public class BattleTests
	{
		private IScriptHelper scriptHelper { get; set; }
		private IBattleInterop battleInterop { get; set; }
		private IUnityContainer container { get; set; }

		[TestFixtureSetUp]
		public void TestSetup()
		{
			container = GameLogic.Bootstrapper.BootstrapUnity();
			scriptHelper = container.Resolve<IScriptHelper>();
			scriptHelper.ScriptDir = "/TestScripts/";
			battleInterop = container.Resolve<IBattleInterop>();
		}

		[Test]
		public void CalculateStaticStatsTest()
		{
			Battle battle = new Battle(new List<List<Character>>(), battleInterop, scriptHelper);

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

			StatsModifierCollection ret = battle.CalculateStats(ch);
			ret.HitPoints.Total.ShouldBe(142);
			ret.MagicPoints.Total.ShouldBe(35);
			ret.Strength.Percent.ShouldBe(5);
		}

		[Test]
		public void CalculateStaticStatsWithHooksTest()
		{
			Battle battle = new Battle(new List<List<Character>>(), battleInterop, scriptHelper);

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

			StatsModifierCollection ret = battle.CalculateStats(ch);
			ret.HitPoints.Total.ShouldBe(192);
			ret.MagicPoints.Total.ShouldBe(35);
			ret.Strength.Percent.ShouldBe(5);
		}

		[Test]
		public void CalculateAllStats()
		{
			Battle battle = new Battle(new List<List<Character>>(), battleInterop, scriptHelper);

			SpecificCharacter ch = new SpecificCharacter {
				Name = "test",
				Class = "Barbarian",
				Stats = new StatsObject { 
					Level = 1,
					Strength = 10,
					HitPoints = 10,
					MagicPoints = 10
				}
			};

			StatsModifierCollection ret = battle.CalculateStats(ch);
			ret.HitPoints.Total.ShouldBe(1400);
			ret.MagicPoints.Percent.ShouldBe(180);
		}

		[Test]
		public void MockedBattleCalculateStatsTest()
		{
			Mock<IBattleInterop> mockBattleInterop = new Mock<IBattleInterop>();
			Mock<IScriptHelper> mockScriptHelper = new Mock<IScriptHelper>();
			Mock<Jint.Engine> mockEngine = new Mock<Jint.Engine>();
			Jint.Engine actualEngine = mockEngine.Object;

			mockBattleInterop.Setup(x => x.CreateJSEngineWithCommonScripts(It.IsAny<SpecificCharacter>())).Returns(mockEngine.Object);
			mockBattleInterop.Setup(x => x.InvokeFunctionWithHooks(mockEngine.Object, It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
				It.IsAny<SpecificCharacter>(), It.IsAny<StatsModifierCollection>())).Returns(new StatsModifierCollection { Agility = new StatsModifierObject { Percent = 5} });
			mockBattleInterop.Setup(x => x.InvokeFunction(mockEngine.Object, It.IsAny<string>(),
				It.IsAny<SpecificCharacter>(), It.IsAny<StatsModifierCollection>())).Returns(new StatsModifierCollection { Agility = new StatsModifierObject { Percent = 0} });
			mockBattleInterop.Setup(x => x.addObjectToStatsModifierObject(It.IsAny<StatsModifierCollection>(), It.IsAny<StatsModifierCollection>()))
				.Returns((StatsModifierCollection a, StatsModifierCollection b) => a + b);

			mockScriptHelper.Setup(x => x.ExecuteScript(ref actualEngine, It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

			Battle battle = new Battle(new List<List<Character>>(), mockBattleInterop.Object, mockScriptHelper.Object);

			SpecificCharacter ch = new SpecificCharacter {
				Name = "test",
				Class = "Archer1",
				Personalities = "Brave1;Haha;What",
				Stats = new StatsObject { 
					Level = 1,
					Constitution = 2,
					Intelligence = 3
				}
			};

			StatsModifierCollection ret = battle.CalculateStats(ch);

			ret.Agility.Percent.ShouldBe(40);

			mockBattleInterop.Verify(x => x.CreateJSEngineWithCommonScripts(It.IsAny<SpecificCharacter>()), Times.Exactly(1));
			mockBattleInterop.Verify(x => x.InvokeFunctionWithHooks(mockEngine.Object, It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
				It.IsAny<SpecificCharacter>(), It.IsAny<StatsModifierCollection>()), Times.Exactly(8));
			mockBattleInterop.Verify(x => x.addObjectToStatsModifierObject(It.IsAny<StatsModifierCollection>(), It.IsAny<StatsModifierCollection>()),
				Times.Exactly(12));

			mockScriptHelper.Verify(x => x.ExecuteScript(ref actualEngine, It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()),
				Times.Exactly(4));
		}
	}
}

