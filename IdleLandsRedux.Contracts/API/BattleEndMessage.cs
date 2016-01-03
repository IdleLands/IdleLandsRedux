using System.Collections.Generic;

namespace IdleLandsRedux.Contracts.API
{
    public class BattleEndMessage : Message
    {
        public IEnumerable<string> WinningUsers { get; set; }
        public IEnumerable<string> LosingUsers { get; set; }
        public IEnumerable<long> PlayerIds { get; set; }
    }
}

