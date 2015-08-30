using System.Collections.Generic;
using Akka;
using Akka.Actor;
using Akka.Event;
using IdleLandsRedux.DataAccess.Mappings;
using log4net;

namespace IdleLandsRedux.GameLogic.Actors
{
	public sealed class BattleActorMessage
	{
		public readonly List<List<Character>> teams;

		public BattleActorMessage(List<List<Character>> teams)
		{
			this.teams = teams;
		}
	}

	public class BattleActor : ReceiveActor
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));

		public BattleActor()
		{
			
			Receive<BattleActorMessage>(msg => {
				log.Info(string.Format("Received: {0} players", msg.teams.Count));
				foreach(var team in msg.teams) {
					foreach(var character in team) {
						log.Info("Received player " + character.Name);
					}
				}
				HandleBattle(msg);
			});
		}

		private void HandleBattle(BattleActorMessage message)
		{
			
		}
	}
}

