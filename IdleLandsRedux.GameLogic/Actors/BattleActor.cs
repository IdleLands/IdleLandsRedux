using System.Collections.Generic;
using Akka;
using Akka.Actor;
using Akka.Event;
using IdleLandsRedux.GameLogic.SpecificMappings;
using log4net;

namespace IdleLandsRedux.GameLogic.Actors
{
	public sealed class BattleActorMessage
	{
		public readonly List<List<SpecificCharacter>> teams;

		public BattleActorMessage(List<List<SpecificCharacter>> teams)
		{
			this.teams = teams;
		}
	}

	public class BattleActor : UntypedActor
	{
		//private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));

		public BattleActor()
		{
			
			/*Receive<BattleActorMessage>(msg => {
				log.Info(string.Format("Received: {0} players", msg.teams.Count));
				foreach(var team in msg.teams) {
					foreach(var character in team) {
						log.Info("Received player " + character.Name);
					}
				}
			});*/
		}

		protected override void OnReceive(object message)
		{
			System.Console.WriteLine("RECEIVED: " + message);
		}
	}
}

