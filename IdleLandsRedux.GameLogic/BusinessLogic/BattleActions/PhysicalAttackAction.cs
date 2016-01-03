using System;

namespace IdleLandsRedux.GameLogic.BusinessLogic.BattleActions
{
    public class PhysicalAttackAction : BattleAction
    {
        public override string ActionName
		{
			get { return "PhysicalAttackAction"; }
		}
        
        public PhysicalAttackAction()
        {
        }

        public override void ActionExecute()
        {
            foreach (var target in Targets)
            {
                double damageDealth = (Originator.TotalStats.Strength.Current - target.CalculatedStats.DamageReduction.Value) *
                    (100 - target.CalculatedStats.DamageReduction.Percent) / 100;
                target.TransientStats.HitPointsDrained += (int)Math.Max(damageDealth, 0);
            }
        }

        public override void ActionDone()
        {
        }
    }
}

