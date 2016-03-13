using System;
using log4net;
using WebSocketSharp;
using WebSocketSharp.Server;
using IdleLandsRedux.DataAccess;
using Akka.Actor;
using NHibernate;

namespace IdleLandsRedux.WebService
{
	public abstract class IdleLandsBehaviour : WebSocketBehavior
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Program));
		protected IActorRef _activeUsersActor;
		protected Inbox _inbox;

        public IdleLandsBehaviour(IActorRef system, Inbox inbox)
        {
            _activeUsersActor = system;
			_inbox = inbox;
            this.EmitOnPing = true;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            log.Info("Received message: \"" + e.Data + "\" of type \"" + e.Type.ToString() + "\"");

            if (string.IsNullOrEmpty(e.Data))
                return;

            using (var session = Bootstrapper.CreateSession())
            using (var transaction = session.BeginTransaction())
            {
                bool commitTransaction = HandleMessage(session, e.Data, (string message) =>
                {
                    log.Info("Sending message: " + message);
                    Send(message);
                });

                if (commitTransaction)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }
        }

		protected abstract bool HandleMessage(ISession session, string message, Action<string> sendAction);
    }
}

