using System;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IdleLandsRedux.Common
{
	public class StatsModifierObject
	{
		public double Percent { get; set; }
		public double Value { get; set; }
		public double Total { get { return Value * (100d + Percent) / 100d; } }

		#region operator overloads

		/// <param name="value">Value only</param>
		public static implicit operator StatsModifierObject(double value)
		{
			StatsModifierObject so = new StatsModifierObject();

			so.Value = value;

			return so;
		}

		public static StatsModifierObject operator*(StatsModifierObject smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value * smo2.Value;
			smo.Percent = ((100d + smo1.Percent) / 100d * (100d + smo2.Percent) / 100d) * 100d - 100d;

			return smo;
		}

		public static StatsModifierObject operator/(StatsModifierObject smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value / smo2.Value;
			double A = (100d + smo1.Percent) / 100d;
			double B = (100d + smo2.Percent) / 100d;
			smo.Percent = A / B * 100d - 100d;

			return smo;
		}

		public static StatsModifierObject operator+(StatsModifierObject smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value + smo2.Value;
			smo.Percent = smo1.Percent + smo2.Percent;

			return smo;
		}

		public static StatsModifierObject operator-(StatsModifierObject smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value - smo2.Value;
			smo.Percent = smo1.Percent - smo2.Percent;

			return smo;
		}

		public static StatsModifierObject operator*(StatsModifierObject smo1, double smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value * smo2;
			smo.Percent = smo1.Percent;

			return smo;
		}

		public static StatsModifierObject operator/(StatsModifierObject smo1, double smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value / smo2;
			smo.Percent = smo1.Percent;

			return smo;
		}

		public static StatsModifierObject operator+(StatsModifierObject smo1, double smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value + smo2;
			smo.Percent = smo1.Percent;

			return smo;
		}

		public static StatsModifierObject operator-(StatsModifierObject smo1, double smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1.Value - smo2;
			smo.Percent = smo1.Percent;

			return smo;
		}

		public static StatsModifierObject operator*(double smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1 * smo2.Value;
			smo.Percent = smo2.Percent;

			return smo;
		}

		public static StatsModifierObject operator/(double smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1 / smo2.Value;
			smo.Percent = smo2.Percent;

			return smo;
		}

		public static StatsModifierObject operator+(double smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1 + smo2.Value;
			smo.Percent = smo2.Percent;

			return smo;
		}

		public static StatsModifierObject operator-(double smo1, StatsModifierObject smo2)
		{
			StatsModifierObject smo = new StatsModifierObject();

			smo.Value = smo1 - smo2.Value;
			smo.Percent = smo2.Percent;

			return smo;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("[Percent={0}, Value={1}, Total={2}]", Percent, Value, Total);
		}
	}
}

