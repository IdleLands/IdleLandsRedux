using System;
using Jint;

namespace IdleLandsRedux.InteropPlugins.JSPlugin
{
	public class JSEngine : IEngine
	{
		public string Name { get { return "JavascriptEngine"; } }
		private Engine _engine { get; set;}

		public JSEngine(Engine engine)
		{
			if (engine == null)
				throw new ArgumentNullException("engine");

			_engine = engine;
		}

		public Engine GetEngine()
		{
			return _engine;
		}
	}
}

