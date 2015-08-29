using System;
using System.IO;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using IdleLandsRedux.Common;
using IdleLandsRedux.DataAccess.Conventions;
using log4net;

namespace IdleLandsRedux.DataAccess
{
	public class Bootstrapper : IDisposable
	{
		//One session factory for the entire process, one session per thread.
		//See http://stackoverflow.com/questions/242961/nhibernate-session-and-multithreading
		private static ISessionFactory _sessionFactory = null;
		internal static ILog log { get; set; }
		private bool _disposed = false;

		public Bootstrapper(ILog _log)
		{
			if (_log == null)
				throw new ArgumentNullException("_log");

			log = _log;

			if (_sessionFactory != null) {
				log.Error("Do not instantiate bootstrapper twice!");
				throw new InvalidOperationException("Do not instantiate bootstrapper twice!");
			}
			
			_sessionFactory = CreateSessionFactory();
			if (_sessionFactory == null) {
				log.Error("Could not instantiate sessionFactory?");
				throw new InvalidOperationException("Could not instantiate sessionFactory?");
			}
		}

		public void Dispose()
		{ 
			Dispose(true);
			GC.SuppressFinalize(this);           
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;
			
			if(_sessionFactory != null) {
				_sessionFactory.Close();
				_sessionFactory.Dispose();
				_sessionFactory = null;
			}
		}

		public static ISession CreateSession()
		{
			if (_sessionFactory == null) {
				log.Error("Initialize bootstrapper first!");
				throw new InvalidOperationException("Initialize bootstrapper first!");
			}

			return _sessionFactory.OpenSession();
		}

		private ISessionFactory CreateSessionFactory()
		{
			string host = ConfigReader.ReadSetting("Host");
			int port = Int32.Parse(ConfigReader.ReadSetting("Port"));
			string db = ConfigReader.ReadSetting("Database");
			string user = ConfigReader.ReadSetting("User");
			string pass = ConfigReader.ReadSetting("Password");

			string connString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password=;SSL=true;SslMode=Require;", host, port, db, user);
			log.Info("nhibernate connecting to connstring \"" + connString + "\"");
			connString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};SSL=true;SslMode=Require;", host, port, db, user, pass);

			try
			{
			return Fluently.Configure()
				.Database(
					PostgreSQLConfiguration.PostgreSQL82
						.ConnectionString(connString)
				)
				.Mappings(m =>
						m.AutoMappings
						.Add(AutoMap.AssemblyOf<RestrictedNumber>(new AutomappingConfiguration())
							.Conventions.Add<CascadeConvention>()
							.Conventions.Add<TableNameConvention>())
						.Add(AutoMap.AssemblyOf<Mappings.Player>(new AutomappingConfiguration())
							.IgnoreBase<Mappings.Character>()
							.Conventions.Add<CascadeConvention>()
							.Conventions.Add<TableNameConvention>())
						
				)
				.ExposeConfiguration(TreatConfiguration)
				.BuildSessionFactory();
			} catch (Exception e) {
				log.Error(e.Message);
				throw;
			}
		}

		private void TreatConfiguration(NHibernate.Cfg.Configuration configuration)
		{
			string updateOrTruncate = ConfigReader.ReadSetting("UpdateOrTruncate");

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
			using (var file = new FileStream(DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_") + "update.sql", FileMode.Append, FileAccess.Write))
			{
				using (var sw = new StreamWriter(file))
				{
					sw.Write(sql);
				}
			}
		}


	}
}

