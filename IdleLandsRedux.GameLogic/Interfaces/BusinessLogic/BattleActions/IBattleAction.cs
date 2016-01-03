using System.Collections.Generic;
using IdleLandsRedux.GameLogic.DataEntities;

namespace IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.BattleActions
{
    public interface IBattleAction
    {
        string ActionName { get; }
        SpecificCharacter Originator { get; }
        IEnumerable<SpecificCharacter> Targets { get; }

        void ActionExecute();
        void ActionDone();
    }
}

