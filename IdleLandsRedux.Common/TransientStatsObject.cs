using System.Collections.Generic;

namespace IdleLandsRedux.Common
{
    public class TransientStatsObject
    {
        public int HitPointsDrained { get; set; }
        public int MagicPointsDrained { get; set; }
        public ICollection<IEffect> Effects {get;set;}
        
        public TransientStatsObject()
        {
            Effects = new List<IEffect>();
        }
    }
}