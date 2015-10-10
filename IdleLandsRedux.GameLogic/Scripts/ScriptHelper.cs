using System;
using System.IO;
using System.Collections.Generic;
using Jint;
using Jint.Native;
using IdleLandsRedux.GameLogic.Interfaces.Scripts;

namespace IdleLandsRedux.GameLogic.Scripts
{
	public class ScriptHelper : IScriptHelper
	{
		private static string appPath = AppDomain.CurrentDomain.BaseDirectory;
		public string ScriptDir { get; set; }

		public T ExecuteFuncAfterScript<T>(string script, string func)
		{
			var engine = new Engine();
			var scriptText = File.ReadAllText(appPath + ScriptDir + script);

			engine.Execute(scriptText);
			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public T ExecuteFuncAfterScript<T>(string script, Dictionary<string, object> parameters, string func)
		{
			var engine = new Engine();
			var scriptText = File.ReadAllText(appPath + ScriptDir + script);

			foreach (var keyValue in parameters) {
				engine.SetValue(keyValue.Key, keyValue.Value);
			}

			engine.Execute(scriptText);
			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public T ExecuteFuncAfterScript<T>(List<string> scripts, string func)
		{
			var engine = new Engine();

			foreach (string script in scripts) {
				var scriptText = File.ReadAllText(appPath + ScriptDir + script);
				engine.Execute(scriptText);
			}

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public T ExecuteFuncAfterScript<T>(List<string> scripts, Dictionary<string, object> parameters, string func)
		{
			var engine = new Engine();

			foreach (var keyValue in parameters) {
				engine.SetValue(keyValue.Key, keyValue.Value);
			}

			foreach (string script in scripts) {
				var scriptText = File.ReadAllText(appPath + ScriptDir + script);
				engine.Execute(scriptText);
			}

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public T ExecuteFunc<T>(string func)
		{
			var engine = new Engine();

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public Engine CreateScriptEngine()
		{
			return new Engine(cfg => cfg.AllowDebuggerStatement().AllowClr());
		}

		public void ExecuteScript(ref Engine engine, string scriptPath, Dictionary<string, object> parameters = null)
		{
			var scriptText = File.ReadAllText(appPath + ScriptDir + scriptPath);
			ExecuteFunc(ref engine, scriptText, parameters);
		}

		public void ExecuteFunc(ref Engine engine, string func, Dictionary<string, object> parameters = null)
		{
			if (parameters != null) {
				foreach (var keyValue in parameters) {
					engine.SetValue(keyValue.Key, keyValue.Value);
				}
			}

			engine.Execute(func);
		}

		public JsValue GetFunc(ref Engine engine, string func)
		{
			return engine.GetValue(func);
		}

		public T GetValue<T>(ref Engine engine)
		{
			var result = engine.GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}
	}
}

