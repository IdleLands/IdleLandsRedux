using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
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
			scriptHelper.ExecuteFunc<Double>("5*5+2").Should().Be(27);
			scriptHelper.ExecuteFunc<String>("'hey!'").Should().Be("hey!");

			Action action = () => scriptHelper.ExecuteFunc<Int32>("5*5+2");

			action.ShouldThrow<ArgumentException>();
		}

		[Test]
		public void BravePersonalityTest()
		{
			scriptHelper.ExecuteFuncAfterScript<Double>("Personalities/Brave.js", "Brave_fleePercent()").Should().Be(-100);
			scriptHelper.ExecuteFuncAfterScript<Double>("Personalities/Brave.js", "Brave_strPercent()").Should().Be(5);

			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 0 },
					{ "test", 1 }
				}});
			scriptHelper.ExecuteFuncAfterScript<Boolean>("Personalities/Brave.js", parameters, "Brave_canUse(player)").Should().Be(false);

			parameters.Clear();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 1 },
					{ "test", 1 }
				}});
			scriptHelper.ExecuteFuncAfterScript<Boolean>("Personalities/Brave.js", parameters, "Brave_canUse(player)").Should().Be(true);
		}
	}
}

