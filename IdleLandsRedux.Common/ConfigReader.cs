using System;
using System.Configuration;

namespace IdleLandsRedux.Common
{
	public class ConfigReader
	{
		public static string ReadSetting(string key)
		{
			try
			{
				var appSettings = ConfigurationManager.AppSettings;
				if(appSettings[key] == null)
				{
					Console.WriteLine("Error reading app settings. Aborting program.");
					throw new Exception("AppSettings in app.config not filled correctly. Missing key \"" + key + "\".");
				}
				return appSettings[key];
			}
			catch (ConfigurationErrorsException cex)
			{
				Console.WriteLine("Error reading app settings. Aborting program.");
				throw cex;
			}
		}
	}
}

