using System;
using Microsoft.Practices.Unity;
using IdleLandsRedux.Common;
using IdleLandsRedux.Managers;

namespace IdleLandsRedux
{
	public class Bootstrapper
	{
		public static IUnityContainer BootstrapUnity()
		{
			IUnityContainer container = new UnityContainer();
			Common.Bootstrapper.BootstrapUnity(ref container);
			container.RegisterType<IMessageManager, MessageManager>();
			return container;
		}
	}
}

