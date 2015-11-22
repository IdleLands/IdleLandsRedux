using System.Globalization;
using System.Threading;

namespace IdleLandsRedux.Common
{
	public static class StringExtensions
	{
		public static bool CultureContains(this string haystack, string needle, CompareOptions comparison = CompareOptions.IgnoreCase)
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US") {
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}

			return Thread.CurrentThread.CurrentCulture.CompareInfo.IndexOf(haystack, needle, comparison) >= 0;
		}
	}
}

