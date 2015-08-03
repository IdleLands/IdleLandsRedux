using System;
using NetMQ;
using NetMQ.Sockets;

namespace IdleLandsRedux.WebService
{
	public class IdleLandsMQ : IDisposable
	{
		private NetMQContext _context { get; set;}
		private PushSocket _serverSocket { get; set;}
		private bool _disposed = false;

		public IdleLandsMQ()
		{
			_context = NetMQContext.Create();
			_serverSocket = _context.CreatePushSocket();
			_serverSocket.Bind("tcp://localhost:8172");
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

		public void SendTask()
		{
			_serverSocket.Send("Hey!");
		}
	}
}

