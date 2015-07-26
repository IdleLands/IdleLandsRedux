using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Configuration;

namespace IdleLandsRedux.DataAccess
{
	public class Bootstrapper
	{
		private ISessionFactory sessionFactory { get; set; }

		public Bootstrapper()
		{
			sessionFactory = CreateSessionFactory();
		}

		public ISession CreateSession()
		{
			return sessionFactory.OpenSession();
		}

		private ISessionFactory CreateSessionFactory()
		{
			string host = ReadSetting("Host");
			int port = Int32.Parse(ReadSetting("Port"));
			string db = ReadSetting("Database");
			string user = ReadSetting("User");
			string pass = ReadSetting("Password");

			return Fluently.Configure()
				.Database(
					PostgreSQLConfiguration.Standard.ConnectionString(c => c.Host(host).Port(port).Database(db).Username(user).Password(pass))
				)
				.Mappings(m =>
					m.AutoMappings.Add(AutoMap.AssemblyOf<Player>())
				)
				.BuildSessionFactory();
		}

		private string ReadSetting(string key)
		{
			try
			{
				var appSettings = ConfigurationManager.AppSettings;
				if(appSettings[key] == null)
				{
					Console.WriteLine("Error reading app settings. Aborting program.");
					throw new Exception("AppSettings in app.config not filled correctly.");
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

