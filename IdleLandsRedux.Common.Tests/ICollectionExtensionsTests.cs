using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace IdleLandsRedux.Common.Tests
{
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
	[SuppressMessage("Gendarme.Rules.Naming", "UseCorrectPrefixRule", Justification = "This name is best name.")]
	[TestFixture]
	public class ICollectionExtensionsTests
	{
		[Test]
		public void RemoveAllTest()
		{
			ICollection<int> range = Enumerable.Range(0,10).ToList();
			range.Count().Should().Be(10);
			
			range.RemoveAll(no => no > 4 && no < 9);
			
			range.Count().Should().Be(6);
			
			range.Clear();
			
			Action emptyRangeAction = () => range.RemoveAll(no => no == 4);
			
			emptyRangeAction.ShouldNotThrow<Exception>();
		}
	}
}

