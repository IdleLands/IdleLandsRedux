﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using NHibernate;
using Newtonsoft.Json;
using log4net;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.Common;
using Microsoft.Practices.Unity;
using Akka.Actor;
using Akka.Configuration;

namespace IdleLandsRedux.Core
{
	public class Program
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private bool _stop = false;

		public Program()
		{
		}

		internal static void Main()
		{
			log.Info("Starting IdleLands");
			var program = new Program();
			//new DataAccess.Bootstrapper(log);

			Console.CancelKeyPress += (sender, e) => {
				program._stop = true;
				log.Info("Ctrl^C received, stopping program, please wait up to 10 seconds.");
				e.Cancel = true;
			};

			var systemName = ConfigReader.ReadSetting("AkkaSystemName");

			using (ActorSystem.Create(systemName))
			while (!program._stop) {
				Thread.Sleep(1000);
			}

			log.Info("Stopping IdleLands");

			Console.WriteLine();
		}
	}
}
