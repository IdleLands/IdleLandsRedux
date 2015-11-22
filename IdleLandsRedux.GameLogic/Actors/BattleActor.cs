using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.BusinessLogic;
using IdleLandsRedux.InteropPlugins;
using log4net;
using Microsoft.Practices.Unity;
using IdleLandsRedux.Common;

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
			var container = GameLogic.Bootstrapper.BootstrapUnity();
			IPlugin battleInterop = container.Resolve<IPlugin>("JavascriptPlugin");
			IJSScriptHelper scriptHelper = container.Resolve<IJSScriptHelper>();
			IRandomHelper randomHelper = container.Resolve<IRandomHelper>();
			Battle battle = new Battle(message.teams, battleInterop, scriptHelper, randomHelper);

			var allCharacters = battle.AllCharactersInTeams.SelectMany(x => x.Value).ToList();

			while (battle.MoreThanOneTeamAlive()) {
				battle.TakeTurn();
			}

			//
		}


	}
}

