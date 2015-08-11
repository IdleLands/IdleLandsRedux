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
		//Test to see if we can connect to the server specified in app.config
		public void TestBootstrapper()
		{
			new Bootstrapper(log4net.LogManager.GetLogger(typeof(DatabaseConnectionTests)));
			var session = Bootstrapper.CreateSession();
			Assert.That(session != null);
		}
	}
}

