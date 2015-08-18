using System;
using NUnit.Framework;
using Shouldly;
using Moq;
using IdleLandsRedux.Managers;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.Common;
using Microsoft.Practices.Unity;

namespace IdleLandsRedux
{
	[TestFixture]
	public class MessageManagerTests
	{
		private IUnityContainer container { get; set; }

		[TestFixtureSetUp]
		public void TestSetup()
		{
			container = IdleLandsRedux.Bootstrapper.BootstrapUnity();
		}

		[Test]
		public void GenderPronounTests()
		{
			IMessageManager messageManager = container.Resolve<IMessageManager>();
			string input = "%heshe %Heshe";
			string output = messageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "male" });
			output.ShouldBe("he He");

			input = "%heshe %Heshe";
			output = messageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "female" });
			output.ShouldBe("she She");

			input = "%player %gold";
			output = messageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "male", Name = "female" }, goldGained: 5);
			output.ShouldBe("female 5");

			Should.Throw<ArgumentNullException>(() => messageManager.ParseAndReplaceEventMessage(input, player: new Player { Name = "female" }));
		}

		[Test]
		public void RandomDeityLineTest()
		{
			var randomHelperMock = new Mock<IRandomHelper>();
			randomHelperMock.Setup(x => x.Next(It.IsAny<int>())).Returns(1);

			MessageManager messageManager = new MessageManager(randomHelperMock.Object);

			string input = "$random:deity$";
			var output = messageManager.ParseAndReplaceEventMessage(input);

			randomHelperMock.Verify(x => x.Next(7), Times.Once);
			output.ShouldBe("Ishkalorht, The God of Rampage and Brawling");
		}

		[Test]
		public void RandomPlaceholderLineTest()
		{
			var randomHelperMock = new Mock<IRandomHelper>();
			randomHelperMock.Setup(x => x.Next(It.IsAny<int>())).Returns(1);

			MessageManager messageManager = new MessageManager(randomHelperMock.Object);

			string input = "$random:placeholder$";
			var output = messageManager.ParseAndReplaceEventMessage(input);

			randomHelperMock.Verify(x => x.Next(8), Times.Once);
			output.ShouldBe("a glass shark");
		}
	}
}

