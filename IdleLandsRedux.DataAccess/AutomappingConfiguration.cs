using System;
using FluentNHibernate.Automapping;
using IdleLandsRedux.Common;

namespace IdleLandsRedux.DataAccess
{
	internal class AutomappingConfiguration : DefaultAutomappingConfiguration
	{
		public override bool ShouldMap(Type type)
		{
			return type.Namespace == "IdleLandsRedux.DataAccess.Mappings" || type.Name == "RestrictedNumber";
		}

		public override bool IsComponent(Type type)
		{
			return type == typeof(RestrictedNumber);
		}
	}
}

