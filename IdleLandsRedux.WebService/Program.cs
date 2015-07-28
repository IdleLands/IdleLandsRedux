using System;
using System.Reflection;
using System.Linq;
using log4net;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace IdleLandsRedux.WebService
{
	public class Program
	{
		static readonly ILog log = LogManager.GetLogger(typeof(Program));

		private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
		{
			return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
		}


		public static void Main(string[] args)
		{
			log.Info("Starting WebService");

			var wssv = new WebSocketServer("ws://localhost:2345");

			Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "IdleLandsRedux.WebService.Services");

			for (int i = 0; i < typelist.Length; i++)
			{
				Console.WriteLine("Adding: " + typelist[i].Name);
			}

			wssv.AddWebSocketService<IdleLandsMainService>("/IdleLands");
			wssv.Log.Level = LogLevel.Trace;
			wssv.Log.File = "IdleLands.Websocket.log";
			wssv.Start();

			if (!wssv.IsListening) {
				log.Error("Service failed to start");
				return;
			}

			log.Info("WebService started");

			bool stop = false;

			Console.CancelKeyPress += (sender, e) => {
				stop = true;
				e.Cancel = true;
			};

			while (!stop) {
				System.Threading.Thread.Sleep(1000);
			}

			wssv.Stop();
			log.Info("Quitting");
		}
	}
}

