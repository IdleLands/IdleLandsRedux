using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using log4net;
using NHibernate;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;
using IdleLandsRedux.Contracts.API;
using IdleLandsRedux.WebService.Services;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.WebService
{
	public class IdleLandsMainService : WebSocketBehavior, IDisposable
	{
		static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private Dictionary<string, IService> _servicesDict;
		private bool _stop { get; set; }
		private bool _disposed = false;
		private Thread activityThread { get; set; }

		public IdleLandsMainService()
		{
			_servicesDict = new Dictionary<string, IService>();
			_servicesDict.Add("/login", new LoginService());
			_servicesDict.Add("/register", new RegisterService());
			new Bootstrapper();

			activityThread = new Thread(GenerateActivityThread);
			activityThread.Start();
		}

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_stop = true;

			if(activityThread != null) {
				activityThread.Join();
				activityThread = null;
			}
		}

		private void GenerateActivityThread()
		{
			log.Info("Starting GenerateActivityThread");
			while (!_stop) {
				var session = Bootstrapper.CreateSession();
				using (var transaction = session.BeginTransaction()) {
					DateTime now = DateTime.UtcNow;
					Player player;
					StatsObject statsObject;

					var users = session.QueryOver<LoggedInUser>()
						.JoinAlias(x => x.Player, () => player)
						.JoinAlias(() => player.Stats, () => statsObject)
						.Where(x => (x.LastAction == null || x.LastAction < now.AddSeconds(-10)))
						.List();

					foreach (var user in users.Where(x => x.Expiration <= now)) {
						session.Delete(user);
					}

					foreach (var user in users.Where(x => x.Expiration > now)) {
						user.LastAction = now;
						session.SaveOrUpdate(user);
						//Send user.player to MQ
					}
				}

				Thread.Sleep(1000);
			}
		}

		protected override void OnMessage (MessageEventArgs e)
		{
			log.Info("Received message: " + e.Data);

			if (string.IsNullOrEmpty(e.Data))
				return;
			
			var msg = JsonConvert.DeserializeObject<Message>(e.Data);

			if (msg == null || string.IsNullOrEmpty(msg.Path) || _servicesDict[msg.Path] == null)
				return;

			var session = Bootstrapper.CreateSession();
			using (var transaction = session.BeginTransaction()) {
				bool commitTransaction = true;
				_servicesDict[msg.Path].HandleMessage(session, e.Data, (string message) => { log.Info("Sending message: " + message); Send(message); }, ref commitTransaction);
				if (commitTransaction)
					transaction.Commit();
				else
					transaction.Rollback();
			}
		}
	}
}

