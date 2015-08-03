using System.Collections.Generic;

namespace IdleLandsRedux.DataAccess.Mappings
{
	public class Character
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Gender { get; set; }
		public virtual StatsObject Stats { get; set; }
		public virtual List<Item> Equipment { get; set; }
	}
}

