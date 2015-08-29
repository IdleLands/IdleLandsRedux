using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace IdleLandsRedux.DataAccess.Conventions
{
	internal sealed class TableNameConvention : IClassConvention
	{
		//See bug CORE-1
		public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
		{
			instance.Table(instance.EntityType.Name.ToLower());
		}
	}
}

