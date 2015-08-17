using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using NHibernate;
using Newtonsoft.Json;
using NetMQ;
using NetMQ.Sockets;
using log4net;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.Common;
using IdleLandsRedux.Contracts.MQ;

namespace IdleLandsRedux
{
	public class Program
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private volatile bool _stop = false;

		private class ThreadParam
		{
			public int ThreadNumber { get; set; }
			public string MqHost { get; set; }
			public NetMQSocket socket { get; set; }
		}

		public Program()
		{
		}

		public void Execute(object data)
		{
			var threadParam = (ThreadParam)data;
			log.Info("Starting IdleLands thread #" + threadParam.ThreadNumber);

			threadParam.socket.Connect("tcp://" + threadParam.MqHost);
			while (!_stop) {
				string taskString = String.Empty;
				if (threadParam.socket.TryReceiveFrameString(new TimeSpan(0, 0, 5), out taskString)) {
					log.Info("Thread #" + threadParam.ThreadNumber + " received: \"" + taskString + "\"");
					var task = JsonConvert.DeserializeObject<Task>(taskString);
				}
			}

			threadParam.socket.Dispose();
			log.Info("Stopped IdleLands thread #" + threadParam.ThreadNumber);
		}

		public static void Main()
		{
			log.Info("Starting IdleLands");
			var program = new Program();
			new Bootstrapper(log);

			program._stop = false;

			Console.CancelKeyPress += (sender, e) => {
				program._stop = true;
				log.Info("Ctrl^C received, stopping program, please wait up to 10 seconds.");
				e.Cancel = true;
			};

			var numberOfThreads = Int32.Parse(ConfigReader.ReadSetting("NumberOfThreads"));
			var mqHost = ConfigReader.ReadSetting("MqHost");

			log.Info("Connecting MQ pull sockets to " + mqHost);

			NetMQContext ctx = NetMQContext.Create();
			List<Thread> threads = new List<Thread>();
			for (int i = 0; i < numberOfThreads; i++) {
				Thread thread = new Thread(new ParameterizedThreadStart(program.Execute));
				thread.Start(new ThreadParam {ThreadNumber = i, MqHost = mqHost, socket = ctx.CreatePullSocket()});
				threads.Add(thread);
			}

			while (!program._stop) {
				Thread.Sleep(1000);
			}

			log.Info("Stopping IdleLands");

			foreach (var thread in threads) {
				thread.Join();
			}
			ctx.Dispose();

			Console.WriteLine();
		}
	}
}

