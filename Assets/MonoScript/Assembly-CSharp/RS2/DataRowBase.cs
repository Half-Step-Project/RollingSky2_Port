using System;
using System.Net;
using Foundation;

namespace RS2
{
	public abstract class DataRowBase : IRecord
	{
		private static readonly long _time19700101 = new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;

		private static readonly int _timeFactor = 10000;

		protected byte[] DataRowBytes;

		protected int Position;

		public int TheId
		{
			get
			{
				return GetId();
			}
		}

		public abstract int GetId();

		public int Parse(byte[] dataRowBytes, int position)
		{
			if (dataRowBytes == null)
			{
				Log.Error("The dataRowBytes is invalid.");
				return position;
			}
			if (position < 0 || position >= dataRowBytes.Length)
			{
				Log.Error("Position is invalid.");
				return position;
			}
			DataRowBytes = dataRowBytes;
			Position = position;
			short num = ReadShort();
			ParseRow();
			if (Position - position != num)
			{
				Log.Error("Parse datarow failed.");
				return position + num;
			}
			DataRowBytes = null;
			position = Position;
			Position = 0;
			return position;
		}

		protected abstract void ParseRow();

		protected byte[] ReadBytes(byte[] datas, int length)
		{
			if (length < 0 || length > datas.Length)
			{
				Log.Warning("The length is invalid, reset to max length of datas.");
				length = datas.Length;
			}
			if (!IsValidRead(length))
			{
				Log.Error("The row datas is invalid.");
				return datas;
			}
			Array.Copy(DataRowBytes, Position, datas, 0, length);
			Position += length;
			return datas;
		}

		protected bool ReadBool()
		{
			return ReadShort() == 1;
		}

		protected short ReadShort()
		{
			if (!IsValidRead(2))
			{
				Log.Error("The row datas is invalid.");
				return 0;
			}
			return IPAddress.NetworkToHostOrder(DataRowBytes.GetInt16(ref Position));
		}

		protected int ReadInt()
		{
			if (!IsValidRead(4))
			{
				Log.Error("The row datas is invalid.");
				return 0;
			}
			return IPAddress.NetworkToHostOrder(DataRowBytes.GetInt32(ref Position));
		}

		protected long ReadLong()
		{
			if (!IsValidRead(8))
			{
				Log.Error("The row datas is invalid.");
				return 0L;
			}
			return IPAddress.NetworkToHostOrder(DataRowBytes.GetInt64(ref Position));
		}

		protected DateTime ReadDate()
		{
			long num = ReadLong() * _timeFactor;
			num += _time19700101;
			return new DateTime(num);
		}

		protected float ReadFloat()
		{
			return ReadInt().GetBytes().GetSingle();
		}

		protected double ReadDouble()
		{
			return BitConverter.Int64BitsToDouble(ReadLong());
		}

		protected string ReadLocalString()
		{
			short num = ReadShort();
			num = (short)(num - 2);
			if (num > 0)
			{
				byte[] array = new byte[num];
				ReadBytes(array, num);
				try
				{
					return array.GetString();
				}
				catch (Exception ex)
				{
					Log.Error("Get string encode error " + ex.Message);
					return string.Empty;
				}
			}
			return null;
		}

		protected string ReadCommonString()
		{
			string key = ReadLocalString();
			return ToCommonString(key);
		}

		private string ToCommonString(string key)
		{
			return key;
		}

		private bool IsValidRead(int length)
		{
			return DataRowBytes.Length - Position >= length;
		}
	}
}
