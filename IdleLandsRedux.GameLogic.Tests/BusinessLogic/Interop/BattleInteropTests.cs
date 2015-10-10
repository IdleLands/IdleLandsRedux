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
using Shouldly;

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
			ret.ShouldBe(5);

			Should.Throw<NullReferenceException>(() => battleInterop.CheckOnExpandoAndCast<int>(null, "test"));
			Should.Throw<NullReferenceException>(() => battleInterop.CheckOnExpandoAndCast<int>(obj, ""));
			Should.Throw<NullReferenceException>(() => battleInterop.CheckOnExpandoAndCast<int>(obj, null));
		}

		[Test]
		public void GetAllScriptsOfTest()
		{
			SpecificCharacter sc = new SpecificCharacter();
			sc.Class = "Test;Archer";
			sc.Personalities = "DPS;King";

			BattleInterop battleInterop = new BattleInterop(null);
			var scripts = battleInterop.GetAllScriptsOf(sc);
			scripts.Count.ShouldBe(4);
			scripts[0].Item1.ShouldBe("class");
			scripts[0].Item2.ShouldBe("Test");
			scripts[1].Item1.ShouldBe("class");
			scripts[1].Item2.ShouldBe("Archer");
			scripts[2].Item1.ShouldBe("personality");
			scripts[2].Item2.ShouldBe("DPS");
			scripts[3].Item1.ShouldBe("personality");
			scripts[3].Item2.ShouldBe("King");
		}
	}
}

