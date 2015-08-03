using System;

namespace IdleLandsRedux.DataAccess.Mappings
{
	public class LoggedInUser
	{
		public virtual int Id { get; set; }
		public virtual Player Player { get; set; }
		public virtual string Token { get; set; }
		public virtual DateTime Expiration { get; set; }
		public virtual DateTime? LastAction { get; set; }
	}
}

