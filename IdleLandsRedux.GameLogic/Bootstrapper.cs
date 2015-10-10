using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.Unity;
using Microsoft.Practices.Unity;
using IdleLandsRedux.GameLogic.Managers;
using IdleLandsRedux.GameLogic.Interfaces.Managers;
using IdleLandsRedux.GameLogic.Interfaces.BusinessLogic.Interop;
using IdleLandsRedux.GameLogic.BusinessLogic.Interop;
using IdleLandsRedux.GameLogic.Scripts;
using IdleLandsRedux.GameLogic.Interfaces.Scripts;

namespace IdleLandsRedux.GameLogic
{
	public class Bootstrapper
	{
		public static IDependencyResolver GetAkkaResolver(IUnityContainer container, ActorSystem system)
		{
			return new UnityDependencyResolver(container, system);
		}

		public static IUnityContainer BootstrapUnity()
		{
			IUnityContainer container = new UnityContainer();
			Common.Bootstrapper.BootstrapUnity(container);
			container.RegisterType<IMessageManager, MessageManager>();
			container.RegisterType<IBattleInterop, BattleInterop>();
			container.RegisterType<IScriptHelper, ScriptHelper>(new PerThreadLifetimeManager());
			return container;
		}
	}
}

