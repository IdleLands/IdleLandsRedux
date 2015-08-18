using System;
using IdleLandsRedux.DataAccess.Mappings;
namespace IdleLandsRedux
{
	public interface IMessageManager
	{
		string ParseAndReplaceEventMessage(string eventMessage, int goldGained = 0, int xpGained = 0, Player player = null, Item item = null);
	}
}

