using System;
using System.Security.Cryptography;

namespace IdleLandsRedux.Common
{
	/// <summary>
	/// To be used for cryptographically secure RNG
	/// </summary>
	public class SecureRandomHelper : IDisposable, ISecureRandomHelper
	{
		private RandomNumberGenerator _rng { get; set;}
		private bool _disposed = false;

		public SecureRandomHelper()
		{
			_rng = new RNGCryptoServiceProvider();
		}

		#region IDisposable members

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if(_rng != null) {
				_rng.Dispose();
				_rng = null;
			}
		}

		#endregion

		public string GetBase64String(uint size) {
			var bytes = new Byte[size];
			_rng.GetBytes(bytes);
			return Convert.ToBase64String(bytes);
		}
	}
}

