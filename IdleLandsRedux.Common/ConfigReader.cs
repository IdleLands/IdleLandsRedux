using System;
using System.Configuration;

namespace IdleLandsRedux.Common
{
	public class ConfigReader
	{
		/// <summary>
		/// Reads the setting from app.config or web.config.
		/// </summary>
		/// <returns>The setting.</returns>
		/// <exception cref="Exception">Key not found in config.</exception>
		/// <param name="key">Key.</param>
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

