using System;
using NUnit.Framework;
using Shouldly;

namespace IdleLandsRedux.Common
{
	[TestFixture]
	public class StringHelperTests
	{
		[Test]
		public void SanitizeTest()
		{
			var ret = StringHelper.SanitizeString("Thi$ is_ (a) test; for the 4th.");
			ret.ShouldBe("Thi is a test for the 4th");

			ret = StringHelper.SanitizeString("Thi$ is_ (a) test; for the 4th.", true);
			ret.ShouldBe("Thi is_ a test; for the 4th.");
		}
	}
}

