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

namespace IdleLandsRedux.WebService
{
	public class IdleLandsMainService : WebSocketBehavior
	{
		static readonly ILog log = LogManager.GetLogger(typeof(Program));
		private Dictionary<string, IService> _servicesDict;

		public IdleLandsMainService()
		{
			_servicesDict = new Dictionary<string, IService>();
			_servicesDict.Add("/login", new LoginService());
			_servicesDict.Add("/register", new RegisterService());
		}

		protected override void OnMessage (MessageEventArgs e)
		{
			if (e == null) {
				throw new ArgumentNullException("e");
			}

			log.Info("Received message: " + e.Data);

			if (string.IsNullOrEmpty(e.Data))
				return;
			
			var msg = JsonConvert.DeserializeObject<Message>(e.Data);

			if (msg == null || string.IsNullOrEmpty(msg.Path) || _servicesDict[msg.Path] == null)
				return;

			var session = Bootstrapper.CreateSession();
			using (var transaction = session.BeginTransaction()) {
				bool commitTransaction = true;

				_servicesDict[msg.Path].HandleMessage(session, e.Data, (string message) => {
					log.Info("Sending message: " + message);
					Send(message);
				}, ref commitTransaction);

				if (commitTransaction)
					transaction.Commit();
				else
					transaction.Rollback();
			}
			session.Dispose();
		}
	}
}

