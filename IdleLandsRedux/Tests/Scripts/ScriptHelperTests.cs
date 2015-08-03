using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

namespace IdleLandsRedux.Scripts
{
	[TestFixture]
	public class ScriptHelperTests
	{
		public ScriptHelperTests()
		{
		}

		[Test]
		public void TestReturnTypes()
		{
			ScriptHelper.ExecuteFunc<Double>("5*5+2").ShouldBe<Double>(27);
			ScriptHelper.ExecuteFunc<String>("'hey!'").ShouldBe("hey!");
			Should.Throw<ArgumentException>(() => {
				ScriptHelper.ExecuteFunc<Int32>("5*5+2");
			});
		}

		[Test]
		//Assuming, of course, that Brave.js never changes. Which might not be so!
		public void BravePersonalityTest()
		{
			ScriptHelper.ExecuteFuncAfterScript<Double>("Personalities/Brave.js", "fleePercent()").ShouldBe<Double>(-100);
			ScriptHelper.ExecuteFuncAfterScript<Double>("Personalities/Brave.js", "strPercent()").ShouldBe<Double>(5);

			Dictionary<string, object> parameters = new Dictionary<string, object>();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 0 },
					{ "test", 1 }
				}});
			ScriptHelper.ExecuteFuncAfterScript<Boolean>("Personalities/Brave.js", parameters, "canUse(player)").ShouldBe<Boolean>(false);

			parameters.Clear();
			parameters.Add("player", new {statistics = new Dictionary<string, object> {
					{ "combat self flee", 1 },
					{ "test", 1 }
				}});
			ScriptHelper.ExecuteFuncAfterScript<Boolean>("Personalities/Brave.js", parameters, "canUse(player)").ShouldBe<Boolean>(true);
		}
	}
}

