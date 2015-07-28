using System;
using Newtonsoft.Json;
using IdleLandsRedux.WebService.API;

namespace IdleLandsRedux.WebService.Services
{
	public class LoginService : IService
	{
		public LoginService()
		{
		}

		public void HandleMessage(string message, Action<string> sendAction)
		{
			var msg = JsonConvert.DeserializeObject<LoginMessage>(message);

			if(msg != null)
				sendAction("Success!");
			else
				sendAction("Failure!");
		}
	}
}

