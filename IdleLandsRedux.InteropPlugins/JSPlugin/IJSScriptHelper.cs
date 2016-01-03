using System.Collections.Generic;

namespace IdleLandsRedux.InteropPlugins
{
	public interface IJSScriptHelper
	{
		string ScriptDir { get; set; }

		T ExecuteFunc<T>(string func);
		IEngine CreateScriptEngine();
		void ExecuteScript(IEngine engine, string scriptPath, Dictionary<string, object> parameters = null);
		void ExecuteFunc(IEngine engine, string func, Dictionary<string, object> parameters = null);
		T ExecuteFunc<T>(IEngine engine, string func, Dictionary<string, object> parameters = null);
		object GetFunc(IEngine engine, string func);
		T GetValue<T>(IEngine engine);
	}
}

