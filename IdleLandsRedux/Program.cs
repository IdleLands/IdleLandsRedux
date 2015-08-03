using System;
using System.Collections.Concurrent;
using System.Threading;
using NHibernate;
using log4net;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux
{
	public class Program
	{
		static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private bool stop { get; set; }

		public Program()
		{
		}



		public static void Main()
		{
			log.Info("Starting IdleLands");
			//new Bootstrapper();
			var program = new Program();

			program.stop = false;

			Console.CancelKeyPress += (sender, e) => {
				program.stop = true;
				e.Cancel = true;
			};

			/*Thread activityThread = new Thread(program.GenerateActivityThread);
			activityThread.Start();

			while (!program.stop) {
				Thread.Sleep(1000);
			}

			log.Info("Stopping IdleLands");

			activityThread.Join();*/
			Console.WriteLine();
		}
	}
}

