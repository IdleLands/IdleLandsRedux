using System;
using Microsoft.Practices.Unity;


namespace IdleLandsRedux.Common
{
	public class Bootstrapper
	{
		public static void BootstrapUnity(ref IUnityContainer container)
		{
			container.RegisterType<IRandomHelper, RandomHelper>();
		}
	}
}

