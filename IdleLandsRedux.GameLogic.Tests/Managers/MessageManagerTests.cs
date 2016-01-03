using System;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using IdleLandsRedux.GameLogic.Managers;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Interfaces.Managers;
using IdleLandsRedux.Common;
using Microsoft.Practices.Unity;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.GameLogic.Tests.Managers
{
	[SuppressMessage("Gendarme.Rules.Design", "TypesWithDisposableFieldsShouldBeDisposableRule", Justification = "Gets disposed in the test teardown.")]
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
	[SuppressMessage("Gendarme.Rules.Smells", "AvoidCodeDuplicatedInSameClassRule", Justification = "Function parameter checks are hard to de-duplicate.")]
	[TestFixture]
	public class MessageManagerTests
	{
		private IUnityContainer container { get; set; }

		[TestFixtureSetUp]
		public void TestSetup()
		{
			container = GameLogic.Bootstrapper.BootstrapUnity();
		}

		[Test]
		public void GenderPronounTests()
		{
			IMessageManager messageManager = container.Resolve<IMessageManager>();
			string input = "%heshe %Heshe";
			string output = messageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "male" });
			output.Should().Be("he He");

			input = "%heshe %Heshe";
			output = messageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "female" });
			output.Should().Be("she She");

			input = "%player %gold";
			output = messageManager.ParseAndReplaceEventMessage(input, player: new Player { Gender = "male", Name = "female" }, goldGained: 5);
			output.Should().Be("female 5");

			Action action = () => messageManager.ParseAndReplaceEventMessage(input, player: new Player { Name = "female" });

			action.ShouldThrow<ArgumentException>();
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
			output.Should().Be("Ishkalorht, The God of Rampage and Brawling");
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
			output.Should().Be("a glass shark");
		}
	}
}

