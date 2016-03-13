using Akka.Actor;
using log4net;
using System.Collections.Generic;
using System;
using IdleLandsRedux.DataAccess.Mappings;
using Akka.Event;
using System.Linq;

namespace IdleLandsRedux.GameLogic.Actors
{
    public sealed class AddUserMessage
    {
        public readonly string Username;

        public AddUserMessage(string username)
        {
            Username = username;
        }
    }

    public sealed class AddUserMessageResponse
    {
        public enum ERR_CODE
        {
            ALREADY_LOGGED_IN,
            UNKNOWN_ERR,
            SUCCESS
        }

        public readonly ERR_CODE Code;
        public readonly string Token;

        public AddUserMessageResponse(ERR_CODE code, string token)
        {
            Code = code;
            Token = token;
        }
    }

    public sealed class ExpireUser
    {
        public readonly string User;

        public ExpireUser(string user)
        {
            User = user;
        }
    }

    public sealed class ReceivedPingFromUser
    {
        public readonly string User;

        public ReceivedPingFromUser(string user)
        {
            User = user;
        }
    }

    public sealed class GetInactiveUsersPing
    {

    }

    public sealed class InitializeInbox
    {

    }

    public class ActiveUser
    {
        public string Username { get; set; }
        public Player Player { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime? LastAction { get; set; }
        public DateTime LastPing { get; set; }
        public IActorRef Sender { get; set; }
    }

    public class ActiveUsersActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);
        private List<ActiveUser> _activeUsers;
        private IActorRef _webserviceInbox { get; set; }
        private readonly ICancelable _cancelPinging;

        public ActiveUsersActor()
        {
            //init socket etc
            _activeUsers = new List<ActiveUser>();
            _cancelPinging = new Cancelable(Context.System.Scheduler);
            _log.Warning("Started actor");

            Receive<InitializeInbox>(msg =>
            {
                _log.Info("Initializing inbox");
                _webserviceInbox = Sender;
                Sender.Tell("Sure!");
            });

            //Received from login/register services
            Receive<AddUserMessage>(msg =>
            {
                _log.Info("Adding user");
                var response = AddUser(msg);
                Sender.Tell(response, Self);
            });

            //Scheduled to run once per minute
            Receive<GetInactiveUsersPing>(msg =>
            {
                _log.Info("Checking expired users");

                if (_webserviceInbox == null)
                {
                    return;
                }

                for (int i = _activeUsers.Count - 1; i >= 0; i--)
                {
                    var user = _activeUsers[i];
                    
                    _log.Info("Checking user " + user.Username + " - " + user.Expiration.ToLongTimeString());
                    if (DateTime.UtcNow > user.Expiration)
                    {
                        _log.Info("User " + user.Username + " has expired");
                        _webserviceInbox.Tell(new ExpireUser(user.Username));
                        _activeUsers.RemoveAt(i);
                    }
                }
            });

            Receive<ReceivedPingFromUser>(msg =>
            {
                _log.Info("Received ping from user " + msg.User);
                _activeUsers.Single(u => u.Username == msg.User).LastPing = DateTime.UtcNow;
            });
        }

        #region Actor lifecycle methods

        protected override void PreStart()
        {
            try
            {
                Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), Self,
                    new GetInactiveUsersPing(), ActorRefs.Nobody, _cancelPinging);
            }
            catch
            {
                Console.WriteLine("Exception in PreStart");
                _log.Info("Exception in PreStart");
            }
        }

        protected override void PostStop()
        {
            try
            {
                _log.Info("PostStop");
                //terminate the scheduled task
                _cancelPinging.Cancel(false);
            }
            catch
            {
                _log.Info("Exception");
                //don't care about additional "ObjectDisposed" exceptions
            }
            finally
            {
                base.PostStop();
            }
        }

        #endregion

        private AddUserMessageResponse AddUser(AddUserMessage message)
        {
            if (_activeUsers.Any(u => u.Username == message.Username))
            {
                return new AddUserMessageResponse(AddUserMessageResponse.ERR_CODE.ALREADY_LOGGED_IN, string.Empty);
            }

            DateTime now = DateTime.UtcNow;
            _activeUsers.Add(new ActiveUser
            {
                Username = message.Username,
                Expiration = now.AddSeconds(5),
                LastPing = now,
                Sender = Sender
            });

            return new AddUserMessageResponse(AddUserMessageResponse.ERR_CODE.SUCCESS, Guid.NewGuid().ToString());
        }
    }
}

