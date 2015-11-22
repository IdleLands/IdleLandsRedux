using System.Collections.Generic;
using IdleLandsRedux.GameLogic.BusinessLogic;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.InteropPlugins;
using IdleLandsRedux.InteropPlugins.JSPlugin;
using IdleLandsRedux.Common;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Practices.Unity;
using Moq;

namespace IdleLandsRedux.GameLogic.Tests.BusinessLogic
{
	[TestFixture]
	public class BattleTests
	{
		private IJSScriptHelper scriptHelper { get; set; }
		private IRandomHelper randomHelper { get; set; }
		private IPlugin pluginInterop { get; set; }
		private IUnityContainer container { get; set; }

		[TestFixtureSetUp]
		public void TestSetup()
		{
			container = new UnityContainer();
			Common.Bootstrapper.BootstrapUnity(container);
			InteropPlugins.Bootstrapper.BootstrapUnity(container);
			scriptHelper = container.Resolve<IJSScriptHelper>();
			randomHelper = container.Resolve<IRandomHelper>();
			scriptHelper.ScriptDir = "/TestScripts/";
			pluginInterop = new JSPlugin(scriptHelper);
		}

		[Test]
		public void CalculateStaticStatsTest()
		{
			Battle battle = new Battle(new List<List<Character>>(), pluginInterop, scriptHelper, randomHelper);

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
			ret.HitPoints.Total.Should().Be(142);
			ret.MagicPoints.Total.Should().Be(35);
			ret.Strength.Percent.Should().Be(5);
		}

		[Test]
		public void CalculateStaticStatsWithHooksTest()
		{
			Battle battle = new Battle(new List<List<Character>>(), pluginInterop, scriptHelper, randomHelper);

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
			ret.HitPoints.Total.Should().Be(192);
			ret.MagicPoints.Total.Should().Be(35);
			ret.Strength.Percent.Should().Be(5);
		}

		[Test]
		public void CalculateAllStats()
		{
			Battle battle = new Battle(new List<List<Character>>(), pluginInterop, scriptHelper, randomHelper);

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
			ret.HitPoints.Total.Should().Be(1400);
			ret.MagicPoints.Percent.Should().Be(180);
		}

		[Test]
		public void MockedBattleCalculateStatsTest()
		{
			Mock<IPlugin> mockBattleInterop = new Mock<IPlugin>();
			Mock<IJSScriptHelper> mockScriptHelper = new Mock<IJSScriptHelper>();
			Mock<IRandomHelper> mockRandomHelper = new Mock<IRandomHelper>();
			Mock<Jint.Engine> mockJintEngine = new Mock<Jint.Engine>();
			Mock<JSEngine> mockEngine = new Mock<JSEngine>(mockJintEngine.Object);
			IEngine actualEngine = mockEngine.Object;

			mockBattleInterop.Setup(x => x.CreateEngineWithCommonScripts(It.IsAny<SpecificCharacter>())).Returns(mockEngine.Object);
			mockBattleInterop.Setup(x => x.InvokeFunctionWithHooks(mockEngine.Object, It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
				It.IsAny<SpecificCharacter>(), It.IsAny<StatsModifierCollection>())).Returns(new StatsModifierCollection { Agility = new StatsModifierObject { Percent = 5} });
			mockBattleInterop.Setup(x => x.InvokeFunction(mockEngine.Object, It.IsAny<string>(),
				It.IsAny<SpecificCharacter>(), It.IsAny<StatsModifierCollection>())).Returns(new StatsModifierCollection { Agility = new StatsModifierObject { Percent = 0} });
			mockBattleInterop.Setup(x => x.addObjectToStatsModifierObject(It.IsAny<StatsModifierCollection>(), It.IsAny<StatsModifierCollection>()))
				.Returns((StatsModifierCollection a, StatsModifierCollection b) => a + b);

			mockScriptHelper.Setup(x => x.ExecuteScript(ref actualEngine, It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

			Battle battle = new Battle(new List<List<Character>>(), mockBattleInterop.Object, mockScriptHelper.Object, mockRandomHelper.Object);

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

			ret.Agility.Percent.Should().Be(40);

			mockBattleInterop.Verify(x => x.CreateEngineWithCommonScripts(It.IsAny<SpecificCharacter>()), Times.Exactly(1));
			mockBattleInterop.Verify(x => x.InvokeFunctionWithHooks(mockEngine.Object, It.IsAny<string>(), It.IsAny<IEnumerable<string>>(),
				It.IsAny<SpecificCharacter>(), It.IsAny<StatsModifierCollection>()), Times.Exactly(8));
			mockBattleInterop.Verify(x => x.addObjectToStatsModifierObject(It.IsAny<StatsModifierCollection>(), It.IsAny<StatsModifierCollection>()),
				Times.Exactly(12));

			mockScriptHelper.Verify(x => x.ExecuteScript(ref actualEngine, It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()),
				Times.Exactly(4));
		}
	}
}

