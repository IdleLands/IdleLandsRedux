using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Common.Tests
{
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
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

