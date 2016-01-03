namespace IdleLandsRedux.GameLogic.BusinessLogic.BattleActions
{
	public class DefendAction : BattleAction
	{
		public override string ActionName
		{
			get { return "DefendAction"; }
		}
		
		public DefendAction()
		{
		}

		public override void ActionExecute()
		{
			foreach (var target in Targets) {
				target.CalculatedStats.DamageReduction.Percent += 25;
			}
		}

		public override void ActionDone()
		{
			foreach (var target in Targets) {
				target.CalculatedStats.DamageReduction.Percent -= 25;
			}
		}
	}
}

