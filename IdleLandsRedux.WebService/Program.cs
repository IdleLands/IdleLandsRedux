using System;
using System.Threading;
using System.Reflection;
using System.Linq;
using log4net;
using WebSocketSharp;
using WebSocketSharp.Server;
using IdleLandsRedux.Contracts.MQ;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.WebService
{
	public class Program
	{
		static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private static volatile bool _stop = false;

		private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
		{
			return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
		}

		private static void GenerateActivityThread()
		{
			log.Info("Starting GenerateActivityThread");
			var idleLandMQ = new IdleLandsMQ();
			while (!_stop) {
				var session = Bootstrapper.CreateSession();
				using (var transaction = session.BeginTransaction()) {
					DateTime now = DateTime.UtcNow;
					Player player = null;
					StatsObject statsObject = null;

					try {
						var users = session.QueryOver<LoggedInUser>()
							.JoinAlias(x => x.Player, () => player, NHibernate.SqlCommand.JoinType.InnerJoin)
							.JoinAlias(() => player.Stats, () => statsObject, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
							.Where(x => (x.LastAction == null || x.LastAction < now.AddSeconds(-10)))
							.List();

						foreach (var user in users.Where(x => x.Expiration <= now)) {
							session.Delete(user);
						}

						foreach (var user in users.Where(x => x.Expiration > now)) {
							user.LastAction = now;
							session.SaveOrUpdate(user);
							idleLandMQ.SendTask(new Task { Type = TaskType.Battle });
						}

						transaction.Commit();
					} catch (Exception e) {
						log.Error(e.Message);
						throw e;
					}
				}

				Thread.Sleep(1000);
			}
		}

		public static void Main(string[] args)
		{
			log.Info("Starting WebService");

			new Bootstrapper(log);

			var wssv = new WebSocketServer("ws://localhost:2345");

			wssv.AddWebSocketService<IdleLandsMainService>("/IdleLands");
			wssv.Log.Level = LogLevel.Trace;
			wssv.Log.File = "IdleLands.Websocket.log";
			wssv.Start();

			if (!wssv.IsListening) {
				log.Error("Service failed to start");
				return;
			}

			var activityThread = new Thread(GenerateActivityThread);
			activityThread.Start();

			log.Info("WebService started");

			Console.CancelKeyPress += (sender, e) => {
				_stop = true;
				log.Info("Ctrl^C received, stopping program.");
				e.Cancel = true;
			};

			while (!_stop) {
				System.Threading.Thread.Sleep(1000);
			}

			log.Info("Stopping IdleLands.WebService activity thread.");

			activityThread.Join();

			log.Info("Stopping IdleLands.WebService websocket.");

			wssv.Stop();

			log.Info("Quitting");
		}
	}
}

