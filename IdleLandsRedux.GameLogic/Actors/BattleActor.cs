using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.BusinessLogic;
using IdleLandsRedux.InteropPlugins;
using log4net;
using Microsoft.Practices.Unity;
using IdleLandsRedux.Common;
using IdleLandsRedux.Contracts.API;

namespace IdleLandsRedux.GameLogic.Actors
{
    public sealed class BattleActorMessage
    {
        //Maybe use ReadOnlyCollection?
        public readonly ICollection<ICollection<Character>> Teams;

        public BattleActorMessage(ICollection<ICollection<Character>> teams)
        {
            this.Teams = teams;
        }
    }

    public class BattleActor : ReceiveActor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));

        public BattleActor()
        {
            Receive<BattleActorMessage>(msg =>
            {
                log.Info(string.Format("Received: {0} players", msg.Teams.Count));
                HandleBattle(msg);
            });
        }

        private void HandleBattle(BattleActorMessage message)
        {
            using(var container = GameLogic.Bootstrapper.BootstrapUnity())
            {
                IPlugin battleInterop = container.Resolve<IPlugin>("JavascriptPlugin");
                IJSScriptHelper scriptHelper = container.Resolve<IJSScriptHelper>();
                IRandomHelper randomHelper = container.Resolve<IRandomHelper>();
                Battle battle = new Battle(message.Teams, battleInterop, scriptHelper, randomHelper);
                List<string> battleLog = new List<string>();
    
                while (battle.MoreThanOneTeamAlive())
                {
                    battle.TakeTurn(battleLog);
                }
    
                var winningTeam = battle.GetVictoriousTeam();
                var losingTeam = battle.GetLosingTeam();
    
                BattleEndMessage bem = new BattleEndMessage()
                {
                    WinningUsers = winningTeam.Select(user => user.Name),
                    LosingUsers = losingTeam.Select(user => user.Name)
                };
                
                //SendMessageActor send bem
            }
        }
    }
}

