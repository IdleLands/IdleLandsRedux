using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Actors;
using Akka.Actor;
using NHibernate.SqlCommand;

namespace IdleLandsRedux.WebService
{
    public static class Program
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static bool _stop; //defaults to false

        private static void Main()
        {
            log.Info("Starting WebService");

            //new Bootstrapper(log);

            /*var wssv = new WebSocketServer("ws://localhost:2345");

			wssv.AddWebSocketService<IdleLandsMainService>("/IdleLands");
			wssv.Log.Level = LogLevel.Trace;
			wssv.Log.File = "IdleLands.Websocket.log";
			wssv.Start();

			if (!wssv.IsListening) {
				log.Error("Service failed to start");
				return;
			}*/

            log.Info("WebService started");

            Console.CancelKeyPress += (sender, e) =>
            {
                _stop = true;
                log.Info("Ctrl^C received, stopping program.");
                e.Cancel = true;
            };

            while (!_stop)
            {
				System.Threading.Thread.Sleep(1000);
            }

            using (var system = ActorSystem.Create("DispatchSystem"))
            {
                var RemoteBattleActor = system.ActorOf(Props.Create(() => new BattleActor()), "remoteactor");

                while (!_stop)
                {
                    using(var session = Bootstrapper.CreateSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        DateTime now = DateTime.UtcNow;
                        Player player = null;
                        StatsObject statsObject = null;

                        try
                        {
                            var users = session.QueryOver<LoggedInUser>()
                            .JoinAlias(x => x.Player, () => player, JoinType.InnerJoin)
                            .JoinAlias(() => player.Stats, () => statsObject, JoinType.LeftOuterJoin)
                            .Where(x => (x.LastAction == null || x.LastAction < now.AddSeconds(-10)))
                            .List();

                            foreach (var user in users.Where(x => x.Expiration <= now))
                            {
                                session.Delete(user);
                            }

                            ICollection<ICollection<Character>> battleList = new List<ICollection<Character>>();

                            foreach (var user in users.Where(x => x.Expiration > now))
                            {
                                user.LastAction = now;
                                session.SaveOrUpdate(user);
                                var tempList = new List<Character>();
                                tempList.Add(user.Player);
                                battleList.Add(tempList);
                            }

                            BattleActorMessage bam = new BattleActorMessage(battleList);
                            RemoteBattleActor.Tell(bam);

                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            log.Error(e.Message);
                            throw;
                        }
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }

            log.Info("Stopping IdleLands.WebService websocket.");

            //wssv.Stop();

            log.Info("Quitting");
        }
    }
}

