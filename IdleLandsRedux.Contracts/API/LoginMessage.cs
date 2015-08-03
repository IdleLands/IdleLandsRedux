namespace IdleLandsRedux.Contracts.API
{
	public class LoginMessage : Message
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}

