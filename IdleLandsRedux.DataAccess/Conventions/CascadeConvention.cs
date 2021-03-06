﻿using System.Diagnostics.CodeAnalysis;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace IdleLandsRedux.DataAccess.Conventions
{
	//http://ayende.com/blog/1890/nhibernate-cascades-the-different-between-all-all-delete-orphans-and-save-update
	/// <summary>
	/// This is a convention that will be applied to all entities in your application. What this particular
	/// convention does is to specify that many-to-one, one-to-many, and many-to-many relationships will all
	/// have their Cascade option set to All.
	/// </summary>
	[SuppressMessage("Gendarme.Rules.Performance", "AvoidUninstantiatedInternalClassesRule", Justification = "FluentNHibernate instantiates internally.")]
	internal sealed class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
	{
		public void Apply(IManyToOneInstance instance)
		{
			instance.Cascade.All();
		}

		public void Apply(IOneToManyCollectionInstance instance)
		{
			instance.Cascade.All();
		}

		public void Apply(IManyToManyCollectionInstance instance)
		{
			instance.Cascade.All();
		}
	}
}

