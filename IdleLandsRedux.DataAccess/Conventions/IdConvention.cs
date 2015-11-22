using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System.Collections.Generic;
using NHibernate.Dialect;
using NHibernate.Mapping;

namespace IdleLandsRedux.DataAccess.Conventions
{
	/// <summary>
	/// Use the HighLow identity generation mechanism, with an entity row per table
	/// http://www.anthonydewhirst.blogspot.co.uk/2012/02/fluent-nhibernate-solution-to-enable.html
	/// </summary>
	public class CustomIdentityHiLoGeneratorConvention :IIdConvention
	{
		public const string NextHiValueColumnName= "NextHiValue";
		public const string NHibernateHiLoIdentityTableName = "NHibernateHiLoIdentity";
		public const string TableColumnName = "Entity";

		#region Implementation of IConvention<IIdentityInspector,IIdentityInstance>

		public void Apply(IIdentityInstance instance)
		{
			instance.GeneratedBy.HiLo(NHibernateHiLoIdentityTableName, NextHiValueColumnName, "100", builder =>
				builder.AddParam("where", string.Format("{0} = '[{1}]'", TableColumnName, instance.EntityType.Name)));
		}

		#endregion

		public static void CreateHighLowScript(NHibernate.Cfg.Configuration config)
		{
			var script = new StringBuilder();
			script.AppendFormat("DELETE FROM {0};", NHibernateHiLoIdentityTableName);
			script.AppendLine();
			script.AppendFormat("ALTER TABLE {0} ADD {1} VARCHAR(128) NOT NULL;", NHibernateHiLoIdentityTableName, TableColumnName);
			script.AppendLine();
			script.AppendFormat("CREATE NONCLUSTERED INDEX IX_{0}_{1} ON {0} (Entity ASC);", NHibernateHiLoIdentityTableName, TableColumnName);
			script.AppendLine();
			script.AppendLine("GO");
			script.AppendLine();
			foreach (var tableName in config.ClassMappings.Select(m => m.Table.Name).Distinct())
			{
				script.AppendFormat(string.Format("INSERT INTO [{0}] ({1}, {2}) VALUES ('[{3}]',1);", NHibernateHiLoIdentityTableName, TableColumnName, NextHiValueColumnName, tableName));
				script.AppendLine();
			}

			config.AddAuxiliaryDatabaseObject(new SimpleAuxiliaryDatabaseObject(script.ToString(), null, new HashSet<string> { typeof(PostgreSQL82Dialect).FullName, typeof(SQLiteDialect).FullName, typeof(MsSql2008Dialect).FullName }));
		}
	}
}

