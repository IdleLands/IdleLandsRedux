namespace IdleLandsRedux.GameLogic.BusinessLogic.BattleActions
{
    public class RegenAction : BattleAction
    {
        public override string ActionName
		{
			get { return "RegenAction"; }
		}
        
        public RegenAction()
        {
        }

        public override void ActionExecute()
        {
            Originator.TransientStats.HitPointsDrained -= (int)(Originator.CalculatedStats.HitPointsRegen.Value +
                    Originator.CalculatedStats.HitPoints.Total * Originator.CalculatedStats.HitPointsRegen.Percent);
                    
            if(Originator.TransientStats.HitPointsDrained < 0)
            {
                Originator.TransientStats.HitPointsDrained = 0;
            }

            Originator.TransientStats.MagicPointsDrained -= (int)(Originator.CalculatedStats.MagicPointsRegen.Value +
                Originator.CalculatedStats.MagicPoints.Total * Originator.CalculatedStats.MagicPointsRegen.Percent);
                
            if(Originator.TransientStats.MagicPointsDrained < 0)
            {
                Originator.TransientStats.MagicPointsDrained = 0;
            }
        }

        public override void ActionDone()
        {
        }
    }
}

