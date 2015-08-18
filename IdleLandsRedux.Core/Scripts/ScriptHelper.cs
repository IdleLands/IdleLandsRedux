using System;
using System.IO;
using System.Collections.Generic;
using Jint;

namespace IdleLandsRedux.Core.Scripts
{
	public class ScriptHelper
	{
		private static string appPath = AppDomain.CurrentDomain.BaseDirectory;

		public static T ExecuteFuncAfterScript<T>(string script, string func)
		{
			var engine = new Engine();
			var scriptText = File.ReadAllText(appPath + "/Scripts/" + script);

			engine.Execute(scriptText);
			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public static T ExecuteFuncAfterScript<T>(string script, Dictionary<string, object> parameters, string func)
		{
			var engine = new Engine();
			var scriptText = File.ReadAllText(appPath + "/Scripts/" + script);

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

		public static T ExecuteFuncAfterScript<T>(List<string> scripts, string func)
		{
			var engine = new Engine();

			foreach (string script in scripts) {
				var scriptText = File.ReadAllText(appPath + "/Scripts/" + script);
				engine.Execute(scriptText);
			}

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public static T ExecuteFuncAfterScript<T>(List<string> scripts, Dictionary<string, object> parameters, string func)
		{
			var engine = new Engine();

			foreach (var keyValue in parameters) {
				engine.SetValue(keyValue.Key, keyValue.Value);
			}

			foreach (string script in scripts) {
				var scriptText = File.ReadAllText(appPath + "/Scripts/" + script);
				engine.Execute(scriptText);
			}

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public static T ExecuteFunc<T>(string func)
		{
			var engine = new Engine();

			var result = engine.Execute(func).GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}

		public static Engine CreateScriptEngine()
		{
			return new Engine();
		}

		public static void ExecuteScript(ref Engine engine, string scriptPath, Dictionary<string, object> parameters = null)
		{
			var scriptText = File.ReadAllText(appPath + "/Scripts/" + scriptPath);
			ExecuteFunc(ref engine, scriptText, parameters);
		}

		public static void ExecuteFunc(ref Engine engine, string func, Dictionary<string, object> parameters = null)
		{
			if (parameters != null) {
				foreach (var keyValue in parameters) {
					engine.SetValue(keyValue.Key, keyValue.Value);
				}
			}

			engine.Execute(func);
		}

		public static T GetValue<T>(ref Engine engine)
		{
			var result = engine.GetCompletionValue().ToObject();

			if (result is T) {
				return (T)result;
			}

			throw new ArgumentException("Expected " + typeof(T).FullName + " but got " + result.GetType().FullName);
		}
	}
}

