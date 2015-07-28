using System;
using System.Collections.Generic;
using log4net;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;
using IdleLandsRedux.WebService.API;
using IdleLandsRedux.WebService.Services;

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
		}

		protected override void OnMessage (MessageEventArgs e)
		{
			log.Info("Received message: " + e.Data);

			if (string.IsNullOrEmpty(e.Data))
				return;
			
			var msg = JsonConvert.DeserializeObject<Message>(e.Data);

			if (msg == null || string.IsNullOrEmpty(msg.Path) || _servicesDict[msg.Path] == null)
				return;

			_servicesDict[msg.Path].HandleMessage(e.Data, (string message) => Send(message));
		}
	}
}

