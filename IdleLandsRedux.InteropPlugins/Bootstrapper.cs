using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Unity;
using IdleLandsRedux.InteropPlugins.JSPlugin;

namespace IdleLandsRedux.InteropPlugins
{
	public static class Bootstrapper
	{
		[SuppressMessage("Gendarme.Rules.Correctness", "EnsureLocalDisposalRule", Justification = "This is actually an incorrect finding.")]
		public static void BootstrapUnity(IUnityContainer container)
		{
			if (container == null) {
				throw new ArgumentNullException(nameof(container));
			}

			container.RegisterType<IJSScriptHelper, JSScriptHelper>();
			container.RegisterType<IEngine, JSEngine>("JavascriptEngine");
			container.RegisterType<IPlugin, JSPlugin.JSPlugin>("JavascriptPlugin");
		}
	}
}

