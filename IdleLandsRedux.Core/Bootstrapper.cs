using System;
using Microsoft.Practices.Unity;
using IdleLandsRedux.Common;
using IdleLandsRedux.Core.Managers;
using IdleLandsRedux.Core.Interfaces.Managers;

namespace IdleLandsRedux.Core
{
	public class Bootstrapper
	{
		public static IUnityContainer BootstrapUnity()
		{
			IUnityContainer container = new UnityContainer();
			Common.Bootstrapper.BootstrapUnity(ref container);
			container.RegisterType<IMessageManager, MessageManager>();
			container.RegisterType<IBattleManager, BattleManager>();
			return container;
		}
	}
}

