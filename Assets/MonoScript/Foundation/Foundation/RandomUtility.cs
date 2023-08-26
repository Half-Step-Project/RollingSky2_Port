using System;

namespace Foundation
{
	public static class RandomUtility
	{
		private static Random _random = new Random();

		public static void SetSeed(int seed)
		{
			_random = new Random(seed);
		}

		public static int GetRandom()
		{
			return _random.Next();
		}

		public static int GetRandom(int maxValue)
		{
			return _random.Next(maxValue);
		}

		public static int GetRandom(int minValue, int maxValue)
		{
			return _random.Next(minValue, maxValue);
		}

		public static double GetRandomDouble()
		{
			return _random.NextDouble();
		}

		public static void GetRandomBytes(byte[] buffer)
		{
			_random.NextBytes(buffer);
		}
	}
}
