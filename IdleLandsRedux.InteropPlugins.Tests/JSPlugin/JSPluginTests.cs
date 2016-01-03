using IdleLandsRedux.GameLogic.DataEntities;
using IdleLandsRedux.InteropPlugins.JSPlugin;
using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.GameLogic.Tests.BusinessLogic.Interop
{
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
	[TestFixture]
	public class JSPluginTests
	{
		[Test]
		public void GetAllScriptsOfTest()
		{
			SpecificCharacter sc = new SpecificCharacter();
			sc.Class = "Test;Archer";
			sc.Personalities = "DPS;King";

			JSPlugin plugin = new JSPlugin(null);
			var scripts = plugin.GetAllScriptsOf(sc);
			scripts.Count.Should().Be(4);
			scripts[0].Item1.Should().Be("class");
			scripts[0].Item2.Should().Be("Test");
			scripts[1].Item1.Should().Be("class");
			scripts[1].Item2.Should().Be("Archer");
			scripts[2].Item1.Should().Be("personality");
			scripts[2].Item2.Should().Be("DPS");
			scripts[3].Item1.Should().Be("personality");
			scripts[3].Item2.Should().Be("King");
		}
	}
}

