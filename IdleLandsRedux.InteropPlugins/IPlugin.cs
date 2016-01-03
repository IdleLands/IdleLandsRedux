using System;
using System.Collections.Generic;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.InteropPlugins
{
	public interface IPlugin
	{
		IEngine CreateEngineWithCommonScripts(SpecificCharacter character);
		IList<Tuple<string, string>> GetAllScriptsOf(SpecificCharacter character);
		StatsModifierCollection InvokeFunctionWithHooks(IEngine engine, string function, IEnumerable<string> hookFunctions,
			SpecificCharacter character, StatsModifierCollection cumulativeStatsObject);
		StatsModifierCollection InvokeFunction(IEngine engine, string function, SpecificCharacter character, StatsModifierCollection cumulativeStatsObject);
		StatsModifierCollection AddObjectToStatsModifierObject(StatsModifierCollection stats, object obj);
	}
}

