using System.Collections.Generic;

namespace IdleLandsRedux.Common
{
    public class TransientStatsObject
    {
        public int HitPointsDrained { get; set; }
        public int MagicPointsDrained { get; set; }
        public List<IEffect> Effects {get;set;}
    }
}