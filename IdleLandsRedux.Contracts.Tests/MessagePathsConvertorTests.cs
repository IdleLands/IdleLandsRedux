using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Contracts.Tests
{
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
	[TestFixture]
	public class MessagePathsConvertorTest
	{
		[Test]
		public void EnumToStringTest()
		{
			foreach (MessagePath path in MessagePath.GetValues(typeof(MessagePath))) {
				MessagePathConvertor.GetMessagePath(path).Should().NotBeNull();
			}
		}
	}
}

