using NUnit.Framework;
using FluentAssertions;

namespace IdleLandsRedux.Common.Tests
{
	[TestFixture]
	public class StringHelperTests
	{
		[Test]
		public void SanitizeTest()
		{
			var ret = StringHelper.SanitizeString("Thi$ is_ (a) test; for the 4th.");
			ret.Should().Be("Thi is a test for the 4th");

			ret = StringHelper.SanitizeString("Thi$ is_ (a) test; for the 4th.", true);
			ret.Should().Be("Thi is_ a test; for the 4th.");
		}
	}
}

