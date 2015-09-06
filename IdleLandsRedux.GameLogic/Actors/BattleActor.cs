﻿using System.Collections.Generic;
using System.Linq;
using Akka;
using Akka.Actor;
using Akka.Event;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.BussinessLogic;
using log4net;

namespace IdleLandsRedux.GameLogic.Actors
{
	public sealed class BattleActorMessage
	{
		//Maybe use ReadOnlyCollection?
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
				HandleBattle(msg);
			});
		}

		private void HandleBattle(BattleActorMessage message)
		{
			Battle battle = new Battle(message.teams);

			while (battle.MoreThanOneTeamAlive()) { 
				
			}

			//
		}


	}
}

