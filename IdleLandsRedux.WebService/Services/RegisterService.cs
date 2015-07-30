using System;
using Newtonsoft.Json;
using NHibernate;
using IdleLandsRedux.WebService.API;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.WebService.Services
{
	public class RegisterService : IService
	{
		public void HandleMessage(ISession session, string message, Action<string> sendAction, ref bool commitTransaction)
		{
			var msg = JsonConvert.DeserializeObject<RegisterMessage>(message);
			commitTransaction = true;

			if (msg != null) {

				var player = session.QueryOver<Player>().Where(x => x.Name == msg.Username).SingleOrDefault();

				if (player != null) {
					sendAction(JsonConvert.SerializeObject(new ResponseMessage {
						Success = false,
						Error = "Username already exists."
					}));
				}

				// TODO check if password is bcrypt or some shit

				player = new Player { Name = msg.Username, Password = msg.Password };
				session.Save(player);

				var loggedInUser = new LoggedInUser { Player = player, Token = Guid.NewGuid().ToString(), Expiration = DateTime.UtcNow.AddHours(1) };
				session.Save(loggedInUser);

				sendAction(JsonConvert.SerializeObject(new ResponseMessage {
					Success = true,
					Token = loggedInUser.Token
				}));

			} else {
				sendAction(JsonConvert.SerializeObject(new ResponseMessage {
					Success = false,
					Error = "Incorrect message."
				}));
			}
		}
	}
}

