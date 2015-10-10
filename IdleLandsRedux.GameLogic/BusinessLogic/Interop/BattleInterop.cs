using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using IdleLandsRedux.Common;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.Interop;
using IdleLandsRedux.GameLogic.Interfaces.Scripts;
using IdleLandsRedux.DataAccess.Mappings;
using Jint;

namespace IdleLandsRedux.GameLogic.BusinessLogic.Interop
{
	public class BattleInterop : IBattleInterop
	{
		private IScriptHelper ScriptHelper { get; set; }

		public BattleInterop(IScriptHelper scriptHelper)
		{
			ScriptHelper = scriptHelper;
		}

		public Engine CreateJSEngineWithCommonScripts(SpecificCharacter character)
		{
			var engine = ScriptHelper.CreateScriptEngine();
			ScriptHelper.ExecuteScript(ref engine, "./Common.js");
			engine.SetValue("_GetDefaultBonusObject", new Func<StatsModifierCollection>(() => {
				return new StatsModifierCollection();
			}));
			engine.SetValue("_GetDefaultMultiplyBonusObject", new Func<StatsModifierCollection>(() => {
				StatsModifierCollection smo = 1d;
				return smo;
			}));
			engine.SetValue("_HasPersonalitySet", new Func<string, bool>((personalityName) => {
				return character.Personalities.CultureContains(personalityName);
			}));
			engine.SetValue("_HasClassSet", new Func<string, bool>((className) => {
				return character.Class == className;
			}));
			return engine;
		}

		public T CheckOnExpandoAndCast<T>(ExpandoObject expando, string property) where T : struct
		{
			if (string.IsNullOrEmpty(property))
				throw new NullReferenceException("property");

			if (expando == null)
				throw new NullReferenceException("expando");

			IDictionary<string, object> dictExpando = (IDictionary<string, object>)expando;
			if (dictExpando.ContainsKey(property)) {
				return (T)dictExpando[property];
			}

			return default(T);
		}

		public StatsModifierCollection addObjectToStatsModifierObject(StatsModifierCollection stats, object obj)
		{
			if (stats == null)
				throw new NullReferenceException("stats");

			if (obj == null)
				throw new NullReferenceException("obj");

			StatsModifierCollection statsObject = obj as StatsModifierCollection;

			if (statsObject != null) {
				stats += statsObject;
			} else {
				throw new ArgumentException("Obj is not of a known type.");
			}

			return stats;
		}

		public List<Tuple<string, string>> GetAllScriptsOf(SpecificCharacter character)
		{
			if (character == null)
				throw new NullReferenceException("character");

			List<Tuple<string, string>> ret = new List<Tuple<string, string>>();

			foreach (string @class in character.Class.Split(';')) {
				ret.Add(new Tuple<string, string>("class", @class));
			}

			foreach (string personality in character.Personalities.Split(';')) {
				ret.Add(new Tuple<string, string>("personality", personality));
			}

			return ret;
		}

		public StatsModifierCollection InvokeFunctionWithHooks(Engine engine, string function, IEnumerable<string> hookFunctions,
			SpecificCharacter character, StatsModifierCollection cumulativeStatsObject)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (string.IsNullOrEmpty(function))
				throw new NullReferenceException("function");

			if (hookFunctions == null)
				throw new NullReferenceException("hookFunctions");

			if (character == null)
				throw new NullReferenceException("character");

			StatsModifierCollection summedMultiplierObject = 1d;

			foreach (string hookFunction in hookFunctions) {
				string filename = function.Substring(0, function.IndexOf("_"));
				StatsModifierCollection modifier = (StatsModifierCollection)engine.Invoke(hookFunction, filename, character).ToObject();

				if (modifier == null)
					continue;

				summedMultiplierObject *= modifier;
			}

			StatsModifierCollection originalStats = (StatsModifierCollection)engine.Invoke(function, character, cumulativeStatsObject).ToObject();

			if(originalStats == null)
				throw new ArgumentException("Script did not return proper result.");

			originalStats *= summedMultiplierObject;

			return originalStats;
		}

		public StatsModifierCollection InvokeFunction(Engine engine, string function, SpecificCharacter character, StatsModifierCollection cumulativeStatsObject)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (string.IsNullOrEmpty(function))
				throw new NullReferenceException("function");

			if (character == null)
				throw new NullReferenceException("character");

			StatsModifierCollection originalStats = (StatsModifierCollection)engine.Invoke(function, character, cumulativeStatsObject).ToObject();

			if(originalStats == null)
				throw new ArgumentException("Script did not return proper result.");

			return originalStats;
		}
	}
}

