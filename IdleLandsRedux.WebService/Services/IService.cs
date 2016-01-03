using System;
using NHibernate;

namespace IdleLandsRedux.WebService.Services
{
	public interface IService
	{
		bool HandleMessage(ISession session, string message, Action<string> sendAction);
	}
}

