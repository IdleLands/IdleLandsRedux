using System;
using System.Collections.Generic;
using NUnit.Framework;
using IdleLandsRedux.DataAccess;

namespace IdleLandsRedux
{
	[TestFixture]
	public class DatabaseConnectionTests
	{
		public DatabaseConnectionTests()
		{
		}

		[Test]
		public void TestBootstrapper()
		{
			Bootstrapper bootstrapper = new Bootstrapper();
			bootstrapper.CreateSession();
		}
	}
}

