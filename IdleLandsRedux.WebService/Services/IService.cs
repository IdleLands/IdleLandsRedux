using System;
using NHibernate;

namespace IdleLandsRedux.WebService.Services
{
	public interface IService
	{
		void HandleMessage(ISession session, string message, Action<string> sendAction, ref bool commitTransaction);
	}
}

