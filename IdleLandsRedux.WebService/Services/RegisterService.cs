using System;
using Newtonsoft.Json;
using NHibernate;
using IdleLandsRedux.Contracts.API;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.WebService.Services
{
	public class RegisterService : IService
	{
		public bool HandleMessage(ISession session, string message, Action<string> sendAction)
		{
			if (session == null) {
				throw new ArgumentNullException(nameof(session));
			}

			if (sendAction == null) {
				throw new ArgumentNullException(nameof(sendAction));
			}

			var msg = JsonConvert.DeserializeObject<RegisterMessage>(message);
			bool commitTransaction = true;

			if (msg != null) {

				var player = session.QueryOver<Player>().Where(x => x.Name == msg.Username).SingleOrDefault();

				if (player != null) {
					sendAction(JsonConvert.SerializeObject(new ResponseMessage {
						Success = false,
						Error = "Username already exists."
					}));
					return commitTransaction;
				}

				// TODO check if password is bcrypt or some shit

				player = new Player { Name = msg.Username, Password = msg.Password };
				session.Save(player);

				var loggedInUser = new LoggedInUser {
					Player = player,
					Token = Guid.NewGuid().ToString(),
					Expiration = DateTime.UtcNow.AddHours(1),
					LastAction = null
				};
				session.Save(loggedInUser);

				sendAction(JsonConvert.SerializeObject(new ResponseMessage {
					Success = true,
					Token = loggedInUser.Token
				}));

			} else {
				commitTransaction = false;

				sendAction(JsonConvert.SerializeObject(new ResponseMessage {
					Success = false,
					Error = "Incorrect message."
				}));
			}
			
			return commitTransaction;
		}
	}
}

