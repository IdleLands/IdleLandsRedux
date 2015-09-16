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

			int ret = BattleInterop.CheckOnExpandoAndCast<int>(obj, "test"); //Compiler should compile this to int, but doesn't.
			ret.ShouldBe(5); //See https://bugzilla.xamarin.com/show_bug.cgi?id=33982

			Should.Throw<NullReferenceException>(() => BattleInterop.CheckOnExpandoAndCast<int>(null, "test"));
			Should.Throw<NullReferenceException>(() => BattleInterop.CheckOnExpandoAndCast<int>(obj, ""));
			Should.Throw<NullReferenceException>(() => BattleInterop.CheckOnExpandoAndCast<int>(obj, null));
		}

		[Test]
		public void addExpandoObjectToStatsModifierObjectTest()
		{
			dynamic obj = new ExpandoObject();
			StatsModifierObject smo = new StatsModifierObject();
			obj.PercentageLuck = 5d;

			StatsModifierObject smo2 = BattleInterop.addObjectToStatsModifierObject(smo, obj);
			smo2.ShouldBe(smo);

			smo.PercentageLuck.ShouldBe(5d);

			Should.Throw<NullReferenceException>(() => BattleInterop.addObjectToStatsModifierObject(null, obj));
			Should.Throw<NullReferenceException>(() => BattleInterop.addObjectToStatsModifierObject(smo, null));
		}

		[Test]
		public void GetAllScriptsOfTest()
		{
			SpecificCharacter sc = new SpecificCharacter();
			sc.Class = "Test;Archer";
			sc.Personalities = "DPS;King";

			var scripts = BattleInterop.GetAllScriptsOf(sc);
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

