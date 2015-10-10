using System;
using System.Collections.Generic;
using Jint;
using Jint.Native;

namespace IdleLandsRedux.GameLogic.Interfaces.Scripts
{
	public interface IScriptHelper
	{
		string ScriptDir { get; set; }

		T ExecuteFuncAfterScript<T>(string script, string func);
		T ExecuteFuncAfterScript<T>(string script, Dictionary<string, object> parameters, string func);
		T ExecuteFuncAfterScript<T>(List<string> scripts, string func);
		T ExecuteFuncAfterScript<T>(List<string> scripts, Dictionary<string, object> parameters, string func);
		T ExecuteFunc<T>(string func);
		Engine CreateScriptEngine();
		void ExecuteScript(ref Engine engine, string scriptPath, Dictionary<string, object> parameters = null);
		void ExecuteFunc(ref Engine engine, string func, Dictionary<string, object> parameters = null);
		JsValue GetFunc(ref Engine engine, string func);
		T GetValue<T>(ref Engine engine);
	}
}

