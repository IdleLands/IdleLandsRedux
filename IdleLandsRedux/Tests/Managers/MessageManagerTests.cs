using System;
using NUnit.Framework;
using Shouldly;
using IdleLandsRedux.Managers;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux
{
	[TestFixture]
	public class MessageManagerTests
	{
		[Test]
		public void GenderPronounTests()
		{
			string input = "%heshe %Heshe";
			string output = MessageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "male" });
			output.ShouldBe("he He");

			input = "%heshe %Heshe";
			output = MessageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "female" });
			output.ShouldBe("she She");
		}
	}
}

