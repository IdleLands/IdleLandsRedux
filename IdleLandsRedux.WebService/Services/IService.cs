using System;

namespace IdleLandsRedux.WebService.Services
{
	public interface IService
	{
		void HandleMessage(string message, Action<string> sendAction);
	}
}

