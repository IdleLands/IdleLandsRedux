namespace IdleLandsRedux.DataAccess.Mappings
{
	public class Item
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Slot { get; set; }
		public virtual StatsObject Stats { get; set; }
	}
}

