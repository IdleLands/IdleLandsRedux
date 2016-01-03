using System;
using System.Collections.Generic;

namespace IdleLandsRedux.Contracts
{
	public enum MessagePath
	{
		LoginPath,
		RegisterPath
	}
	
	public static class MessagePathConvertor
	{
		private static readonly Dictionary<MessagePath, string> instance = new Dictionary<MessagePath, string>();
		
		static MessagePathConvertor()
		{
			instance.Add(MessagePath.LoginPath, "/login");
			instance.Add(MessagePath.RegisterPath, "/register");
		}
		
		public static string GetMessagePath(MessagePath path)
		{
			string result;
			if (instance.TryGetValue(path, out result))
				return result;
			else
				throw new InvalidCastException("Path cannot be converted to string");
		}
	}
}

