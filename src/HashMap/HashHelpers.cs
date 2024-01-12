using System;
using System.Runtime.CompilerServices;
using SMath = System.Math;

namespace HashMapNamespace
{
	public static class HashHelpers<T>
	{
		internal static readonly uint[] primes = {
			3, 5, 7, 11, 13, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 157, 191, 229, 277, 337, 409, 491, 593, 719, 863, 1039, 1249,
			1499, 1801, 2161, 2593, 3119, 3761, 4513, 5417, 6521, 7829, 9397, 11279, 13537, 16249, 19501, 23417, 28109, 33739, 40487,
			48589, 58309, 69991, 84011, 100823, 120997, 145207, 174257, 209123, 250949, 301141, 361373, 433651, 520381, 624467, 749383,
			899263, 1079123, 1294957, 1553971, 1864769, 2237743, 2685301, 3222379, 3866857, 4640231, 5568287, 6681947, 8018347, 9622021,
			11546449, 13855747, 16626941, 19952329, 23942797, 28731359, 34477637, 41373173, 49647809, 59577379, 71492873, 85791451, 102949741,
			123539747, 148247713, 177897311, 213476789, 256172149, 307406587, 368887919, 442665511, 531198691, 637438433, 764926171, 917911471,
			1104942151, 1337193553, 1611621881, 1941741533, 2360737661, 2837628649, 3427971229, 4244403883 };

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint CalculateUintCapacity(uint initCapacity, float loadFactor, out uint limitCapacity)
		{
			uint capacity = SMath.Max(1, (uint)SMath.Min(uint.MaxValue, (long)SMath.Round(initCapacity / loadFactor)));

			int low = 0, high = primes.Length;
			while (low != high)
			{
				int mid = (low + high) >> 1;
				if (primes[mid] < capacity)
				{
					low = mid + 1;
				}
				else
				{
					high = mid;
				}
			}

			if (low >= primes.Length)
				low = primes.Length - 1;

			capacity = primes[low];
			if (capacity > 1941741533)
				capacity = 1941741533;

			limitCapacity = (uint)SMath.Round(capacity * loadFactor);

			return capacity;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CalculateCapacity(int initCapacity, float loadFactor, out int limitCapacity)
		{
			uint cap, limitCap;
			cap = CalculateUintCapacity((uint)initCapacity, loadFactor, out limitCap);

			limitCapacity = (int)limitCap;
			return (int)cap;
		}
	}
}
