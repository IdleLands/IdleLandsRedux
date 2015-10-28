using System;
using System.Dynamic;
using System.Collections.Generic;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.InteropPlugins.JSPlugin;
using NUnit.Framework;
using FluentAssertions;

namespace IdleLandsRedux.GameLogic.Tests.BusinessLogic.Interop
{
	[TestFixture]
	public class JSPluginTests
	{
		[Test]
		public void CheckOnExpandoAndCastTest()
		{
			dynamic obj = new ExpandoObject();
			obj.test = 5;

			JSPlugin plugin = new JSPlugin(null);

			int ret = plugin.CheckOnExpandoAndCast<int>(obj, "test"); //Compiler should compile this to int, but doesn't. Not a bug though, feature!
			ret.Should().Be(5);

			Action action = () => plugin.CheckOnExpandoAndCast<int>(null, "test");
			Action action2 = () => plugin.CheckOnExpandoAndCast<int>(obj, "");
			Action action3 = () => plugin.CheckOnExpandoAndCast<int>(obj, null);

			action.ShouldThrow<ArgumentNullException>();
			action2.ShouldThrow<ArgumentNullException>();
			action3.ShouldThrow<ArgumentNullException>();
		}

		[Test]
		public void GetAllScriptsOfTest()
		{
			SpecificCharacter sc = new SpecificCharacter();
			sc.Class = "Test;Archer";
			sc.Personalities = "DPS;King";

			JSPlugin plugin = new JSPlugin(null);
			var scripts = plugin.GetAllScriptsOf(sc);
			scripts.Count.Should().Be(4);
			scripts[0].Item1.Should().Be("class");
			scripts[0].Item2.Should().Be("Test");
			scripts[1].Item1.Should().Be("class");
			scripts[1].Item2.Should().Be("Archer");
			scripts[2].Item1.Should().Be("personality");
			scripts[2].Item2.Should().Be("DPS");
			scripts[3].Item1.Should().Be("personality");
			scripts[3].Item2.Should().Be("King");
		}
	}
}

