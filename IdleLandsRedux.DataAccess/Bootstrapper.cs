using System;
using System.IO;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
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
					//PostgreSQLConfiguration.Standard.ConnectionString(c => c.Host(host).Port(port).Database(db).Username(user).Password(pass))
					PostgreSQLConfiguration.PostgreSQL82
					.ConnectionString(string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};SSL=true;SslMode=Require;", host, port, db, user, pass))
				)
				.Mappings(m =>
					m.AutoMappings.Add(AutoMap.AssemblyOf<Mappings.Player>(new AutomappingConfiguration())
						.Conventions.Add<CascadeConvention>())
				)
				.ExposeConfiguration(TreatConfiguration)
				.BuildSessionFactory();
		}

		private void TreatConfiguration(NHibernate.Cfg.Configuration configuration)
		{
			var update = new SchemaUpdate(configuration);
			update.Execute(LogAutoMigration, true);
		}

		private static void LogAutoMigration(string sql)
		{
			using (var file = new FileStream(DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_") + "update.sql", FileMode.Append))
			{
				using (var sw = new StreamWriter(file))
				{
					sw.Write(sql);
				}
			}
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

