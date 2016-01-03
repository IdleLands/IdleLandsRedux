using System;
using System.Collections.Generic;
using Jint;
using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.InteropPlugins.JSPlugin
{
    public class JSPlugin : IPlugin
    {
        private IJSScriptHelper ScriptHelper { get; set; }

        public JSPlugin(IJSScriptHelper scriptHelper)
        {
            ScriptHelper = scriptHelper;
        }

        public IEngine CreateEngineWithCommonScripts(SpecificCharacter character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            var engine = ScriptHelper.CreateScriptEngine();
            var jsEngine = (engine as JSEngine).GetEngine();
            ScriptHelper.ExecuteScript(engine, "./Common.js");
            jsEngine.SetValue("_GetDefaultBonusObject", new Func<StatsModifierCollection>(() =>
            {
                return new StatsModifierCollection();
            }));
            jsEngine.SetValue("_GetDefaultMultiplyBonusObject", new Func<StatsModifierCollection>(() =>
            {
                StatsModifierCollection smo = 1d;
                return smo;
            }));
            jsEngine.SetValue("_HasPersonalitySet", new Func<string, bool>((personalityName) =>
            {
                return character.Personalities.CultureContains(personalityName);
            }));
            jsEngine.SetValue("_HasClassSet", new Func<string, bool>((className) =>
            {
                return character.Class == className;
            }));
            return engine;
        }

        public StatsModifierCollection AddObjectToStatsModifierObject(StatsModifierCollection stats, object obj)
        {
            if (stats == null)
                throw new ArgumentNullException("stats");

            if (obj == null)
                throw new ArgumentNullException("obj");

            StatsModifierCollection statsObject = obj as StatsModifierCollection;

            if (statsObject != null)
            {
                stats += statsObject;
            }
            else
            {
                throw new ArgumentException("Obj is not of a known type.");
            }

            return stats;
        }

        public IList<Tuple<string, string>> GetAllScriptsOf(SpecificCharacter character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();

            foreach (string @class in character.Class.Split(';'))
            {
                ret.Add(new Tuple<string, string>("class", @class));
            }

            foreach (string personality in character.Personalities.Split(';'))
            {
                ret.Add(new Tuple<string, string>("personality", personality));
            }

            return ret;
        }

        public StatsModifierCollection InvokeFunctionWithHooks(IEngine engine, string function, IEnumerable<string> hookFunctions,
            SpecificCharacter character, StatsModifierCollection cumulativeStatsObject)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            if (string.IsNullOrEmpty(function))
                throw new ArgumentNullException("function");

            if (hookFunctions == null)
                throw new ArgumentNullException("hookFunctions");

            if (character == null)
                throw new ArgumentNullException("character");

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

            Engine jsEngine = (engine as JSEngine).GetEngine();

            StatsModifierCollection summedMultiplierObject = 1d;

            foreach (string hookFunction in hookFunctions)
            {
                string filename = function.Substring(0, function.IndexOf('_'));
                StatsModifierCollection modifier = (StatsModifierCollection)jsEngine.Invoke(hookFunction, filename, character).ToObject();

                if (modifier == null)
                    continue;

                summedMultiplierObject *= modifier;
            }

            StatsModifierCollection originalStats = (StatsModifierCollection)jsEngine.Invoke(function, character, cumulativeStatsObject).ToObject();

            if (originalStats == null)
                throw new ArgumentException("Script did not return proper result.");

            originalStats *= summedMultiplierObject;

            return originalStats;
        }

        public StatsModifierCollection InvokeFunction(IEngine engine, string function, SpecificCharacter character, StatsModifierCollection cumulativeStatsObject)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            if (string.IsNullOrEmpty(function))
                throw new ArgumentNullException("function");

            if (character == null)
                throw new ArgumentNullException("character");

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

            Engine jsEngine = (engine as JSEngine).GetEngine();

            StatsModifierCollection originalStats = (StatsModifierCollection)jsEngine.Invoke(function, character, cumulativeStatsObject).ToObject();

            if (originalStats == null)
                throw new ArgumentException("Script did not return proper result.");

            return originalStats;
        }
    }
}

