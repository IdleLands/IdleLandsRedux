using System;
using System.IO;
using System.Collections.Generic;
using Jint;
using IdleLandsRedux.InteropPlugins.JSPlugin;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.InteropPlugins
{
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidCodeDuplicatedInSameClassRule", Justification = "Function parameter checks are hard to de-duplicate.")]
    public class JSScriptHelper : IJSScriptHelper
    {
        private static string appPath = AppDomain.CurrentDomain.BaseDirectory;
        public string ScriptDir { get; set; }

        public T ExecuteFunc<T>(string func)
        {
            var engine = new Engine();

            var result = engine.Execute(func).GetCompletionValue().ToObject();

            if (result is T)
            {
                return (T)result;
            }

            throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
        }

        public IEngine CreateScriptEngine()
        {
            return new JSEngine(new Engine(cfg => cfg.AllowDebuggerStatement().AllowClr()));
        }

        public void ExecuteScript(IEngine engine, string scriptPath, Dictionary<string, object> parameters = null)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

            Console.WriteLine("scriptdir: " + ScriptDir);
            var scriptText = File.ReadAllText(appPath + ScriptDir + scriptPath);
            ExecuteFunc(engine, scriptText, parameters);
        }

        public void ExecuteFunc(IEngine engine, string func, Dictionary<string, object> parameters = null)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);
				
			var jsEngine = engine as JSEngine;

            if (parameters != null)
            {
                foreach (var keyValue in parameters)
                {
                    jsEngine.GetEngine().SetValue(keyValue.Key, keyValue.Value);
                }
            }

            jsEngine.GetEngine().Execute(func);
        }

        public T ExecuteFunc<T>(IEngine engine, string func, Dictionary<string, object> parameters = null)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

            var jsEngine = engine as JSEngine;

            if (parameters != null)
            {
                foreach (var keyValue in parameters)
                {
                    jsEngine.GetEngine().SetValue(keyValue.Key, keyValue.Value);
                }
            }

            T ret = (T)jsEngine.GetEngine().Execute(func).GetCompletionValue().ToObject();

            if (ret == null)
                throw new ArgumentException("Script did not return proper result.");

            return ret;
        }

        public object GetFunc(IEngine engine, string func)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

            return (engine as JSEngine).GetEngine().GetValue(func);
        }

        public T GetValue<T>(IEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            if (engine.Name != "JavascriptEngine")
                throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

            var result = (engine as JSEngine).GetEngine().GetCompletionValue().ToObject();

            if (result is T)
            {
                return (T)result;
            }

            throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
        }
    }
}

