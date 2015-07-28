using System;
using WebSocketSharp;
using Newtonsoft.Json;
using IdleLandsRedux.WebService.API;

namespace IdleLandsRedux.WebService.Test
{
	public class Program
	{
		public static void Main (string[] args)
		{
			using (var ws = new WebSocket ("ws://localhost:2345/IdleLands")) {
				ws.Log.Level = LogLevel.Trace;
				ws.OnMessage += (sender, e) =>
					Console.WriteLine ("Response: " + e.Data);

				ws.Connect ();

				var msg = JsonConvert.SerializeObject(new LoginMessage {
					Path = "/login",
					Password = "test",
					Username = "test"
				});

				ws.Send (msg);
				Console.ReadKey (true);
			}
		}
	}
}

