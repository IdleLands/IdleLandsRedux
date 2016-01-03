using System;
using Newtonsoft.Json;
using NHibernate;
using IdleLandsRedux.Contracts.API;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux.WebService.Services
{
    public class LoginService : IService
    {
        public bool HandleMessage(ISession session, string message, Action<string> sendAction)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (sendAction == null)
            {
                throw new ArgumentNullException(nameof(sendAction));
            }

            var msg = JsonConvert.DeserializeObject<LoginMessage>(message);
            bool commitTransaction = true;

            if (msg != null)
            {

                var player = session.QueryOver<Player>().Where(x => x.Name == msg.Username && x.Password == msg.Password).SingleOrDefault();
                if (player == null)
                {
                    sendAction(JsonConvert.SerializeObject(new ResponseMessage
                    {
                        Success = false,
                        Error = "Incorrect Username or Password"
                    }));
                    return commitTransaction;
                }

                var loggedInUser = session.QueryOver<LoggedInUser>().Where(x => x.Player.Id == player.Id).SingleOrDefault();

                if (loggedInUser != null)
                {
                    sendAction(JsonConvert.SerializeObject(new ResponseMessage
                    {
                        Success = false,
                        Error = "Already logged in"
                    }));
                    return commitTransaction;
                }

                loggedInUser = new LoggedInUser
                {
                    Player = player,
                    Token = Guid.NewGuid().ToString(),
                    Expiration = DateTime.UtcNow.AddMinutes(10),
                    LastAction = null
                };
                session.Save(loggedInUser);

                sendAction(JsonConvert.SerializeObject(new ResponseMessage
                {
                    Success = true,
                    Token = loggedInUser.Token
                }));
            }
            else
            {
                commitTransaction = false;

                sendAction(JsonConvert.SerializeObject(new ResponseMessage
                {
                    Success = false,
                    Error = "Incorrect message."
                }));
            }
            
            return commitTransaction;
        }
    }
}

