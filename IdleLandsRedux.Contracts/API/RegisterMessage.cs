namespace IdleLandsRedux.Contracts.API
{
	public class RegisterMessage : Message
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}

