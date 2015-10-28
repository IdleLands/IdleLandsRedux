using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using IdleLandsRedux.InteropPlugins;
using Microsoft.Practices.Unity;

namespace IdleLandsRedux.GameLogic.Tests.Scripts
{
	[TestFixture]
	public class ScriptHelperTests
	{
		private IJSScriptHelper scriptHelper { get; set; }
		private IUnityContainer container { get; set; }

		[TestFixtureSetUp]
		public void TestSetup()
		{
			container = new UnityContainer();
			InteropPlugins.Bootstrapper.BootstrapUnity(container);
			scriptHelper = container.Resolve<IJSScriptHelper>();
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
			var engine = scriptHelper.CreateScriptEngine();
			scriptHelper.ExecuteScript(ref engine, "Personalities/Brave.js");
			scriptHelper.ExecuteFunc<Double>(ref engine, "Brave_fleePercent()").Should().Be(-100);
			scriptHelper.ExecuteFunc<Double>(ref engine, "Brave_strPercent()").Should().Be(5);

			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 0 },
					{ "test", 1 }
				}});
			scriptHelper.ExecuteFunc<Boolean>(ref engine, "Brave_canUse(player)", parameters).Should().Be(false);

			parameters.Clear();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 1 },
					{ "test", 1 }
				}});
			scriptHelper.ExecuteFunc<Boolean>(ref engine, "Brave_canUse(player)", parameters).Should().Be(true);
		}
	}
}

