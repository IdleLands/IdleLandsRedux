﻿using System;
using FluentNHibernate.Mapping;

namespace IdleLandsRedux.DataAccess.Mappings
{
	public class ActivityQueue
	{
		public virtual int Id { get; protected set; }
		public virtual Player Player { get; set; }
	}
}

