using System;
using System.IO;
using System.Collections.Generic;
using Jint;
using Jint.Native;
using IdleLandsRedux.InteropPlugins.JSPlugin;

namespace IdleLandsRedux.InteropPlugins
{
	public class JSScriptHelper : IJSScriptHelper
	{
		private static string appPath = AppDomain.CurrentDomain.BaseDirectory;
		public string ScriptDir { get; set; }

		public T ExecuteFunc<T>(string func)
		{
			var engine = new Engine();

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public IEngine CreateScriptEngine()
		{
			return new JSEngine(new Engine(cfg => cfg.AllowDebuggerStatement().AllowClr()));
		}

		public void ExecuteScript(ref IEngine engine, string scriptPath, Dictionary<string, object> parameters = null)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (engine.Name != "JavascriptEngine")
				throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

			Console.WriteLine("scriptdir: " + ScriptDir);
			var scriptText = File.ReadAllText(appPath + ScriptDir + scriptPath);
			ExecuteFunc(ref engine, scriptText, parameters);
		}

		public void ExecuteFunc(ref IEngine engine, string func, Dictionary<string, object> parameters = null)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (engine.Name != "JavascriptEngine")
				throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

			if (parameters != null) {
				foreach (var keyValue in parameters) {
					(engine as JSEngine).GetEngine().SetValue(keyValue.Key, keyValue.Value);
				}
			}

			(engine as JSEngine).GetEngine().Execute(func);
		}

		public T ExecuteFunc<T>(ref IEngine engine, string func, Dictionary<string, object> parameters = null)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (engine.Name != "JavascriptEngine")
				throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

			if (parameters != null) {
				foreach (var keyValue in parameters) {
					(engine as JSEngine).GetEngine().SetValue(keyValue.Key, keyValue.Value);
				}
			}

			T ret = (T)(engine as JSEngine).GetEngine().Execute(func).GetCompletionValue().ToObject();

			if(ret == null)
				throw new ArgumentException("Script did not return proper result.");

			return ret;
		}

		public object GetFunc(ref IEngine engine, string func)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (engine.Name != "JavascriptEngine")
				throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);

			return (engine as JSEngine).GetEngine().GetValue(func);
		}

		public T GetValue<T>(ref IEngine engine)
		{
			if (engine == null)
				throw new NullReferenceException("engine");

			if (engine.Name != "JavascriptEngine")
				throw new ArgumentException("Expected provided engine type to be JavascriptEngine but got " + engine.Name);
			
			var result = (engine as JSEngine).GetEngine().GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}
	}
}

