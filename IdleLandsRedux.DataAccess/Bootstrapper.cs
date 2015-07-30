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
	public class Bootstrapper : IDisposable
	{
		private static ISessionFactory sessionFactory { get; set; }
		private bool disposed = false;

		public Bootstrapper()
		{
			sessionFactory = CreateSessionFactory();
		}

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			
			if(sessionFactory != null) {
				sessionFactory.Close();
				sessionFactory.Dispose();
				sessionFactory = null;
			}
		}

		public static ISession CreateSession()
		{
			if (sessionFactory == null)
				throw new Exception("Initialize bootstrapper first!");

			return sessionFactory.OpenSession();
		}

		private ISessionFactory CreateSessionFactory()
		{
			string host = ReadSetting("Host");
			int port = Int32.Parse(ReadSetting("Port"));
			string db = ReadSetting("Database");
			string user = ReadSetting("User");
			string pass = ReadSetting("Password");

			try
			{
			return Fluently.Configure()
				.Database(
					PostgreSQLConfiguration.PostgreSQL82
					.ConnectionString(string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};SSL=true;SslMode=Require;", host, port, db, user, pass))
				)
				.Mappings(m =>
					m.AutoMappings.Add(AutoMap.AssemblyOf<Mappings.Player>(new AutomappingConfiguration())
						.Conventions.Add<CascadeConvention>())
				)
				.ExposeConfiguration(TreatConfiguration)
				.BuildSessionFactory();
			} catch (Exception e) { 
				return null; //Usually a mapping exception.
			}
		}

		private void TreatConfiguration(NHibernate.Cfg.Configuration configuration)
		{
			string updateOrTruncate = ReadSetting("UpdateOrTruncate");

			if (updateOrTruncate.ToLower() == "update") {
				var update = new SchemaUpdate(configuration);
				update.Execute(LogAutoMigration, true);
			} else if(updateOrTruncate.ToLower() == "truncate") {
				var export = new SchemaExport(configuration);
				export.Drop(false, true);
				export.Create(false, true);
			}
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

