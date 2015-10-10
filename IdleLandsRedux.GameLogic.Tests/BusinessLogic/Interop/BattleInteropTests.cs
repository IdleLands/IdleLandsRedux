using System;
using System.Dynamic;
using System.Collections.Generic;
using IdleLandsRedux.GameLogic.BusinessLogic;
using IdleLandsRedux.GameLogic.BusinessLogic.Interop;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.Common;
using NUnit.Framework;
using FluentAssertions;

namespace IdleLandsRedux.GameLogic.Tests.BusinessLogic.Interop
{
	[TestFixture]
	public class BattleInteropTests
	{
		[Test]
		public void CheckOnExpandoAndCastTest()
		{
			dynamic obj = new ExpandoObject();
			obj.test = 5;

			BattleInterop battleInterop = new BattleInterop(null);

			int ret = battleInterop.CheckOnExpandoAndCast<int>(obj, "test"); //Compiler should compile this to int, but doesn't. Not a bug though, feature!
			ret.Should().Be(5);

			Action action = () => battleInterop.CheckOnExpandoAndCast<int>(null, "test");
			Action action2 = () => battleInterop.CheckOnExpandoAndCast<int>(obj, "");
			Action action3 = () => battleInterop.CheckOnExpandoAndCast<int>(obj, null);

			action.ShouldThrow<NullReferenceException>();
			action2.ShouldThrow<NullReferenceException>();
			action3.ShouldThrow<NullReferenceException>();
		}

		[Test]
		public void GetAllScriptsOfTest()
		{
			SpecificCharacter sc = new SpecificCharacter();
			sc.Class = "Test;Archer";
			sc.Personalities = "DPS;King";

			BattleInterop battleInterop = new BattleInterop(null);
			var scripts = battleInterop.GetAllScriptsOf(sc);
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

