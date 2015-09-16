using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using IdleLandsRedux.Common;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.SpecificMappings;
using IdleLandsRedux.DataAccess.Mappings;
using Jint;

namespace IdleLandsRedux.GameLogic.BusinessLogic.Interop
{
	//TODO make this into a dependency injection class, for unit testing Battle.cs better.
	public class BattleInterop
	{
		public BattleInterop()
		{
		}

		public static Engine CreateJSEngineWithCommonScripts(SpecificCharacter character)
		{
			var engine = ScriptHelper.CreateScriptEngine();
			ScriptHelper.ExecuteScript(ref engine, "./Common.js");
			engine.SetValue("_GetDefaultBonusObject", new Func<StatsModifierObject>(() => {
				return new StatsModifierObject();
			}));
			engine.SetValue("_GetDefaultMultiplyBonusObject", new Func<StatsModifierObject>(() => {
				StatsModifierObject smo = 1d;
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

		public static T CheckOnExpandoAndCast<T>(ExpandoObject expando, string property) where T : struct
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

		public static StatsModifierObject addObjectToStatsModifierObject(StatsModifierObject stats, object obj)
		{
			if (stats == null)
				throw new NullReferenceException("stats");

			if (obj == null)
				throw new NullReferenceException("obj");

			ExpandoObject expando = obj as ExpandoObject;
			StatsModifierObject statsObject = obj as StatsModifierObject;

			if (expando != null) {
				foreach (PropertyInfo info in stats.GetProperties()) {
					double oldVal = (double)info.GetValue(stats);
					info.SetValue(stats, CheckOnExpandoAndCast<double>(expando, info.Name) + oldVal);
				}
			} else if (statsObject != null) {
				stats += statsObject;
			} else {
				throw new ArgumentException("Obj is not of a known type.");
			}

			return stats;
		}

		public static List<Tuple<string, string>> GetAllScriptsOf(SpecificCharacter character)
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

		public static StatsModifierObject InvokeStaticBonusWithHooks(Engine engine, string function, IEnumerable<string> hookFunctions, SpecificCharacter character)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (string.IsNullOrEmpty(function))
				throw new NullReferenceException("function");

			if (hookFunctions == null)
				throw new NullReferenceException("hookFunctions");

			if (character == null)
				throw new NullReferenceException("character");

			StatsModifierObject summedMultiplierObject = 1d;

			foreach (string hookFunction in hookFunctions) {
				string filename = function.Substring(0, function.IndexOf("_"));
				StatsModifierObject modifier = (StatsModifierObject)engine.Invoke(hookFunction, filename, character).ToObject();

				if (modifier == null)
					continue;

				summedMultiplierObject *= modifier;
			}

			StatsModifierObject originalStats = (StatsModifierObject)engine.Invoke(function, character).ToObject();

			if(originalStats == null)
				throw new ArgumentException("Script did not return proper result.");

			originalStats *= summedMultiplierObject;

			return originalStats;
		}
	}
}

