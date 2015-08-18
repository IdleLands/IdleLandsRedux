using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.Common;
using log4net;

namespace IdleLandsRedux.Managers
{
	public class MessageManager : IMessageManager
	{
		#if DEBUG
		internal static ILog log { get; set; }
		#endif
		private IRandomHelper randomHelper { get; set; }

		public MessageManager(IRandomHelper randomHelper)
		{
			this.randomHelper = randomHelper;
		}

		private string GetRandomLineOfFile(string filepath)
		{
			var lines = File.ReadLines(filepath).ToList();
			return lines[randomHelper.Next(lines.Count())];
		}

		private StringBuilder ReplaceWithLineIfContains(StringBuilder builder, string input, string contains, string filepath)
		{
			if (input.Contains(contains)) {
				builder = builder.Replace(contains, GetRandomLineOfFile(filepath));
			}
			return builder;
		}

		//TODO only a subset of the variables listed in the doc of old IdleLands is supported right now.
		/// <summary>
		/// Parses and replace the event message. See https://github.com/IdleLands/IdleLands/blob/master/docs/EVENTVAR.md for possible variables.
		/// </summary>
		/// <returns>The and replace event message.</returns>
		/// <param name="eventMessage">Event message.</param>
		/// <param name="goldGained">Gold gained.</param>
		/// <param name="xpGained">Xp gained.</param>
		/// <param name="player">Player.</param>
		/// <param name="item">Item.</param>
		public string ParseAndReplaceEventMessage(string eventMessage, int goldGained = 0, int xpGained = 0, Player player = null, Item item = null)
		{
			StringBuilder retString = new StringBuilder(eventMessage);

			#if DEBUG
			//Only check message and parameter validity in debug builds.
			if(eventMessage.ToLower().ContainsAny("%heshe", "%himher", "%hisher", "%she", "%player") && player == null) {
				log.Error("MessageManager needs a player!");
				throw new Exception("MessageManager needs a player!");
			}

			if(eventMessage.ToLower().ContainsAny("%item") && item == null) {
				log.Error("MessageManager needs an item!");
				throw new Exception("MessageManager needs an item!");
			}
			#endif

			if (player != null) {
				retString = retString
					.ReplaceGenderPronoun(player.Gender, "%Heshe")
					.ReplaceGenderPronoun(player.Gender, "%Himher")
					.ReplaceGenderPronoun(player.Gender, "%Hisher")
					.ReplaceGenderPronoun(player.Gender, "%She");

				retString = retString
					.ReplaceGenderPronoun(player.Gender, "%heshe")
					.ReplaceGenderPronoun(player.Gender, "%himher")
					.ReplaceGenderPronoun(player.Gender, "%hisher")
					.ReplaceGenderPronoun(player.Gender, "%she");

				retString = retString.Replace("%player", player.Name);
			}

			if (item != null) {
				retString = retString.Replace("%item", item.Name);
			}

			retString = retString.Replace("%gold", goldGained.ToString())
				.Replace("%xp", xpGained.ToString());


			retString = ReplaceWithLineIfContains(retString, eventMessage, "$random:deity$", "assets/strings/deity.txt");
			retString = ReplaceWithLineIfContains(retString, eventMessage, "$random:placeholder$", "assets/strings/placeholder.txt");

			return retString.ToString();
		}
	}
}

