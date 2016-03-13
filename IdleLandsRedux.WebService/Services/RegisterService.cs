using System;
using Newtonsoft.Json;
using NHibernate;
using IdleLandsRedux.Contracts.API;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Actors;
using Akka.Actor;

namespace IdleLandsRedux.WebService.Services
{
	public class RegisterService : IdleLandsBehaviour
	{
		public RegisterService(IActorRef activeUsersActor, Inbox inbox) : base(activeUsersActor, inbox)
		{
		}

		protected override bool HandleMessage(ISession session, string message, Action<string> sendAction)
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
                
                var isUserLoggedInTask = _activeUsersActor.Ask(new AddUserMessage(msg.Username), TimeSpan.FromMinutes(1));
                var response = isUserLoggedInTask.Result as AddUserMessageResponse;

                if (response == null)
                {
                    //log stuff
                    sendAction(JsonConvert.SerializeObject(new ResponseMessage
                    {
                        Success = false,
                        Error = "Unknown error."
                    }));
                    return commitTransaction;
                }

                if (response.Code == AddUserMessageResponse.ERR_CODE.ALREADY_LOGGED_IN)
                {
                    sendAction(JsonConvert.SerializeObject(new ResponseMessage
                    {
                        Success = false,
                        Error = "Already logged in"
                    }));
                    return commitTransaction;
                }
                else if (response.Code == AddUserMessageResponse.ERR_CODE.SUCCESS)
                {
                    sendAction(JsonConvert.SerializeObject(new ResponseMessage
                    {
                        Success = true,
                        Token = response.Token
                    }));
                }
                else
                {
                    //log stuff
                    sendAction(JsonConvert.SerializeObject(new ResponseMessage
                    {
                        Success = false,
                        Error = "Unknown error."
                    }));
                    return commitTransaction;
                }

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

