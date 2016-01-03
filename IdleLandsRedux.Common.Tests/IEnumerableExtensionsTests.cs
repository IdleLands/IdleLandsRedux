using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Common.Tests
{
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
	[SuppressMessage("Gendarme.Rules.Naming", "UseCorrectPrefixRule", Justification = "This name is best name.")]
	[TestFixture]
	public class IEnumerableExtensionsTests
	{
		[Test]
		public void SkipLastNTest()
		{
			var range = Enumerable.Range(0,10);
			range.Count().Should().Be(10);
			
			range.SkipLastN(3).Count().Should().Be(7);
			range.SkipLastN(10).Count().Should().Be(0);
			range.SkipLastN(100).Count().Should().Be(0);
			range.SkipLastN(3).Last().Should().Be(6);
			
			Action negativeSkipAction = () => range.SkipLastN(-1).ToArray();
			
			negativeSkipAction.ShouldThrow<ArgumentException>();
		}
	}
}

