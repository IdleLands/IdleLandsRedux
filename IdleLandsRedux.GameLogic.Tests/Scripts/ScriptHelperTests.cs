using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.Interfaces.Scripts;
using Microsoft.Practices.Unity;

namespace IdleLandsRedux.GameLogic.Tests.Scripts
{
	[TestFixture]
	public class ScriptHelperTests
	{
		private IScriptHelper scriptHelper { get; set; }
		private IUnityContainer container { get; set; }

		[TestFixtureSetUp]
		public void TestSetup()
		{
			container = GameLogic.Bootstrapper.BootstrapUnity();
			scriptHelper = container.Resolve<IScriptHelper>();
			scriptHelper.ScriptDir = "/TestScripts/";
		}

		[Test]
		public void TestReturnTypes()
		{
			scriptHelper.ExecuteFunc<Double>("5*5+2").ShouldBe<Double>(27);
			scriptHelper.ExecuteFunc<String>("'hey!'").ShouldBe("hey!");
			Should.Throw<ArgumentException>(() => {
				scriptHelper.ExecuteFunc<Int32>("5*5+2");
			});
		}

		[Test]
		public void BravePersonalityTest()
		{
			scriptHelper.ExecuteFuncAfterScript<Double>("Personalities/Brave.js", "Brave_fleePercent()").ShouldBe<Double>(-100);
			scriptHelper.ExecuteFuncAfterScript<Double>("Personalities/Brave.js", "Brave_strPercent()").ShouldBe<Double>(5);

			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 0 },
					{ "test", 1 }
				}});
			scriptHelper.ExecuteFuncAfterScript<Boolean>("Personalities/Brave.js", parameters, "Brave_canUse(player)").ShouldBe<Boolean>(false);

			parameters.Clear();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 1 },
					{ "test", 1 }
				}});
			scriptHelper.ExecuteFuncAfterScript<Boolean>("Personalities/Brave.js", parameters, "Brave_canUse(player)").ShouldBe<Boolean>(true);
		}
	}
}

