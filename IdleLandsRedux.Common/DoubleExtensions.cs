using System;
using System.Diagnostics.CodeAnalysis;

namespace IdleLandsRedux.Common
{
	public static class DoubleExtensions
	{
		/*
		 * // Usable AlmostEqual function

		bool AlmostEqual2sComplement(float A, float B, int maxUlps)

		{

			// Make sure maxUlps is non-negative and small enough that the

			// default NAN won't compare as equal to anything.

			assert(maxUlps > 0 && maxUlps < 4 * 1024 * 1024);

			int aInt = *(int*)&A;

			// Make aInt lexicographically ordered as a twos-complement int

			if (aInt < 0)

				aInt = 0x80000000 - aInt;

			// Make bInt lexicographically ordered as a twos-complement int

			int bInt = *(int*)&B;

			if (bInt < 0)

				bInt = 0x80000000 - bInt;

			int intDiff = abs(aInt - bInt);

			if (intDiff <= maxUlps)

				return true;

			return false;

		}*/

		//see http://www.cygnus-software.com/papers/comparingfloats/comparingfloats.htm
		

		[SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule")]
		public static bool AlmostEqual2sComplement(this double A, double B, int maxUlps)
		{
			if (maxUlps <= 0 || maxUlps >= 4 * 1024 * 1024) {
				throw new ArgumentException("maxUlps invalid. This way, no floats or almost all floats will be equal.");
			}

			long bitsA = BitConverter.DoubleToInt64Bits(A);
			long bitsB = BitConverter.DoubleToInt64Bits(B);

			if (bitsA < 0) {
				bitsA = (long)(0x8000000000000000 - (ulong)bitsA);
			}

			if (bitsB < 0) {
				bitsB = (long)(0x8000000000000000 - (ulong)bitsB);
			}

			long longDiff = Math.Abs(bitsA - bitsB);
			if (longDiff <= maxUlps) {
				return true;
			}

			return false;
		}
	}
}

