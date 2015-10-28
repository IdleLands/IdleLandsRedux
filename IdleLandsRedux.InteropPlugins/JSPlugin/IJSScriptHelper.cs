using System;
using System.Collections.Generic;

namespace IdleLandsRedux.InteropPlugins
{
	public interface IJSScriptHelper
	{
		string ScriptDir { get; set; }

		T ExecuteFunc<T>(string func);
		IEngine CreateScriptEngine();
		void ExecuteScript(ref IEngine engine, string scriptPath, Dictionary<string, object> parameters = null);
		void ExecuteFunc(ref IEngine engine, string func, Dictionary<string, object> parameters = null);
		T ExecuteFunc<T>(ref IEngine engine, string func, Dictionary<string, object> parameters = null);
		object GetFunc(ref IEngine engine, string func);
		T GetValue<T>(ref IEngine engine);
	}
}

