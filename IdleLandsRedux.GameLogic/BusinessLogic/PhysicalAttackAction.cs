using System;

namespace IdleLandsRedux.GameLogic.BusinessLogic.BattleActions
{
    public class PhysicalAttackAction : BattleAction
    {
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
            foreach (var target in Targets)
            {
                if (target.TransientStats.HitPointsDrained >= target.TotalStats.HitPoints.Maximum)
                {
                    target.Dead = true;
                }
            }
        }
    }
}

