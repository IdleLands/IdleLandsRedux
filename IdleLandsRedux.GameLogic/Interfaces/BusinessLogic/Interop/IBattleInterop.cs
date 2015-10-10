using System;
using System.Collections.Generic;
using System.Dynamic;
using IdleLandsRedux.Common;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.DataAccess.Mappings;
using Jint;

namespace IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.Interop
{
	public interface IBattleInterop
	{
		Engine CreateJSEngineWithCommonScripts(SpecificCharacter character);
		T CheckOnExpandoAndCast<T>(ExpandoObject expando, string property) where T : struct;
		StatsModifierCollection addObjectToStatsModifierObject(StatsModifierCollection stats, object obj);
		List<Tuple<string, string>> GetAllScriptsOf(SpecificCharacter character);
		StatsModifierCollection InvokeFunctionWithHooks(Engine engine, string function, IEnumerable<string> hookFunctions,
			SpecificCharacter character, StatsModifierCollection cumulativeStatsObject);
		StatsModifierCollection InvokeFunction(Engine engine, string function, SpecificCharacter character, StatsModifierCollection cumulativeStatsObject);
	}
}

