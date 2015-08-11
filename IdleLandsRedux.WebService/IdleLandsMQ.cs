using System;
using log4net;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using IdleLandsRedux.Contracts.MQ;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.WebService
{
	public class IdleLandsMQ : IDisposable
	{
		private NetMQContext _context { get; set;}
		private PushSocket _serverSocket { get; set;}
		private bool _disposed = false;
		static readonly ILog log = LogManager.GetLogger(typeof(Program));

		public IdleLandsMQ()
		{
			_context = NetMQContext.Create();
			_serverSocket = _context.CreatePushSocket();
			var mqHost = ConfigReader.ReadSetting("MqHost");
			_serverSocket.Bind("tcp://" + mqHost);
			log.Info("Bound MQ push socket to " + mqHost);
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

			if(_serverSocket != null) {
				_serverSocket.Dispose();
				_serverSocket = null;
			}

			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
		}

		public bool SendTask<T>(T task) where T : Task
		{
			//Perhaps instead of sending it directly, queue it up and have a background thread try to send it.
			//Because this might clog up the program. Or it might not.
			var message = JsonConvert.SerializeObject(task);
			log.Info("Sending \"" + message + "\"");
			return _serverSocket.TrySendFrame(new TimeSpan(0, 0, 10), message);
		}
	}
}

