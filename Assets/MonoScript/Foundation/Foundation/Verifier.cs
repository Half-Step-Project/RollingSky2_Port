using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Foundation
{
	public static class Verifier
	{
		private sealed class Crc32 : HashAlgorithm
		{
			private const uint DefaultPolynomial = 3988292384u;

			private const uint DefaultSeed = uint.MaxValue;

			private static uint[] _defaultTable;

			private readonly uint _seed;

			private readonly uint[] _table;

			private uint _hash;

			public Crc32()
			{
				_seed = uint.MaxValue;
				_table = InitializeTable(3988292384u);
				_hash = uint.MaxValue;
			}

			public Crc32(uint polynomial, uint seed)
			{
				_seed = seed;
				_table = InitializeTable(polynomial);
				_hash = seed;
			}

			public override void Initialize()
			{
				_hash = _seed;
			}

			protected override void HashCore(byte[] array, int ibStart, int cbSize)
			{
				_hash = CalculateHash(_table, _hash, array, ibStart, cbSize);
			}

			protected override byte[] HashFinal()
			{
				return HashValue = UInt32ToBigEndianBytes(~_hash);
			}

			private static uint[] InitializeTable(uint polynomial)
			{
				if (_defaultTable != null && polynomial == 3988292384u)
				{
					return _defaultTable;
				}
				uint[] array = new uint[256];
				for (int i = 0; i < 256; i++)
				{
					uint num = (uint)i;
					for (int j = 0; j < 8; j++)
					{
						num = (((num & 1) != 1) ? (num >> 1) : ((num >> 1) ^ polynomial));
					}
					array[i] = num;
				}
				if (polynomial == 3988292384u)
				{
					_defaultTable = array;
				}
				return array;
			}

			private static uint CalculateHash(uint[] table, uint seed, byte[] bytes, int start, int size)
			{
				uint num = seed;
				for (int i = start; i < size; i++)
				{
					num = (num >> 8) ^ table[bytes[i] ^ (num & 0xFF)];
				}
				return num;
			}

			private static byte[] UInt32ToBigEndianBytes(uint x)
			{
				return new byte[4]
				{
					(byte)((x >> 24) & 0xFFu),
					(byte)((x >> 16) & 0xFFu),
					(byte)((x >> 8) & 0xFFu),
					(byte)(x & 0xFFu)
				};
			}
		}

		private static readonly byte[] _zero = new byte[4];

		public static byte[] GetCrc32(byte[] bytes)
		{
			if (bytes == null)
			{
				return _zero;
			}
			using (MemoryStream inputStream = new MemoryStream(bytes))
			{
				Crc32 crc = new Crc32();
				byte[] result = crc.ComputeHash(inputStream);
				crc.Clear();
				return result;
			}
		}

		public static byte[] GetCrc32(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return _zero;
			}
			using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				Crc32 crc = new Crc32();
				byte[] result = crc.ComputeHash(inputStream);
				crc.Clear();
				return result;
			}
		}

		public static string GetMd5(byte[] bytes)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
	}
}
