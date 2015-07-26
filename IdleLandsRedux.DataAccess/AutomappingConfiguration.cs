using System;
using FluentNHibernate.Automapping;

namespace IdleLandsRedux.DataAccess
{
	public class AutomappingConfiguration : DefaultAutomappingConfiguration
	{
		public override bool ShouldMap(Type type)
		{
			return type.Namespace == "IdleLandsRedux.DataAccess.Mappings";
		}
	}
}

