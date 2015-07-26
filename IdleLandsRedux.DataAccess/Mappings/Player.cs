using System;
using FluentNHibernate.Mapping;

namespace IdleLandsRedux.DataAccess
{
	public class Player
	{
		public virtual int Id { get; protected set; }
		public virtual string Name { get; set; }
		public virtual string Password { get; set; }
		public virtual StatsObject Stats { get; set; }
	}
}

