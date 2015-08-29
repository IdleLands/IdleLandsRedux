using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Unity;


namespace IdleLandsRedux.Common
{
	public static class Bootstrapper
	{
		[SuppressMessage("Gendarme.Rules.Correctness", "EnsureLocalDisposalRule", Justification = "This is actually an incorrect finding.")]
		public static void BootstrapUnity(IUnityContainer container)
		{
			if (container == null) {
				throw new ArgumentNullException("container");
			}

			container.RegisterType<IRandomHelper, RandomHelper>();
		}
	}
}

