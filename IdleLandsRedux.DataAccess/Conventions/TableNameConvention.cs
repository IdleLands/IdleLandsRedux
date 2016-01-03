using System.Diagnostics.CodeAnalysis;
using FluentNHibernate.Conventions;

namespace IdleLandsRedux.DataAccess.Conventions
{
	[SuppressMessage("Gendarme.Rules.Performance", "AvoidUninstantiatedInternalClassesRule", Justification = "FluentNHibernate instantiates internally.")]
	internal sealed class TableNameConvention : IClassConvention
	{
		//See bug CORE-1
		public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
		{
			instance.Table(instance.EntityType.Name.ToLower());
		}
	}
}

