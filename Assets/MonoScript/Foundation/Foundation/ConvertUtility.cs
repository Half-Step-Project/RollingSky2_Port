using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Foundation
{
	public static class ConvertUtility
	{
		private const float InchesToCentimeters = 2.54f;

		private const float CentimetersToInches = 0.39370078f;

		[CompilerGenerated]
		private static readonly bool _003CIsLittleEndian_003Ek__BackingField = BitConverter.IsLittleEndian;

		public static float ScreenDpi { get; set; }

		public static bool IsLittleEndian
		{
			[CompilerGenerated]
			get
			{
				return _003CIsLittleEndian_003Ek__BackingField;
			}
		}

		public static float GetCentimetersFromPixels(float pixels)
		{
			if (ScreenDpi <= 0f)
			{
				Log.Warning("You must set screen DPI first.");
				return pixels;
			}
			return 2.54f * pixels / ScreenDpi;
		}

		public static float GetPixelsFromCentimeters(float centimeters)
		{
			if (ScreenDpi <= 0f)
			{
				Log.Warning("You must set screen DPI first.");
				return centimeters;
			}
			return 0.39370078f * centimeters * ScreenDpi;
		}

		public static float GetInchesFromPixels(float pixels)
		{
			if (ScreenDpi <= 0f)
			{
				Log.Warning("You must set screen DPI first.");
				return pixels;
			}
			return pixels / ScreenDpi;
		}

		public static float GetPixelsFromInches(float inches)
		{
			if (ScreenDpi <= 0f)
			{
				Log.Warning("You must set screen DPI first.");
				return inches;
			}
			return inches * ScreenDpi;
		}

		public static byte[] GetBytes(this bool value)
		{
			return BitConverter.GetBytes(value);
		}

		public static bool GetBoolean(this byte[] bytes)
		{
			return BitConverter.ToBoolean(bytes, 0);
		}

		public static bool GetBoolean(this byte[] bytes, ref int startIndex)
		{
			bool result = BitConverter.ToBoolean(bytes, startIndex);
			startIndex++;
			return result;
		}

		public static byte[] GetBytes(this char value)
		{
			return BitConverter.GetBytes(value);
		}

		public static char GetChar(this byte[] bytes)
		{
			return BitConverter.ToChar(bytes, 0);
		}

		public static char GetChar(this byte[] bytes, ref int startIndex)
		{
			char result = BitConverter.ToChar(bytes, startIndex);
			startIndex += 2;
			return result;
		}

		public static byte[] GetBytes(this short value)
		{
			return BitConverter.GetBytes(value);
		}

		public static short GetInt16(this byte[] bytes)
		{
			return BitConverter.ToInt16(bytes, 0);
		}

		public static short GetInt16(this byte[] bytes, ref int startIndex)
		{
			short result = BitConverter.ToInt16(bytes, startIndex);
			startIndex += 2;
			return result;
		}

		public static byte[] GetBytes(this ushort value)
		{
			return BitConverter.GetBytes(value);
		}

		public static ushort GetUInt16(this byte[] bytes)
		{
			return BitConverter.ToUInt16(bytes, 0);
		}

		public static ushort GetUInt16(this byte[] bytes, ref int startIndex)
		{
			ushort result = BitConverter.ToUInt16(bytes, startIndex);
			startIndex += 2;
			return result;
		}

		public static byte[] GetBytes(this int value)
		{
			return BitConverter.GetBytes(value);
		}

		public static int GetInt32(this byte[] bytes)
		{
			return BitConverter.ToInt32(bytes, 0);
		}

		public static int GetInt32(this byte[] bytes, ref int startIndex)
		{
			int result = BitConverter.ToInt32(bytes, startIndex);
			startIndex += 4;
			return result;
		}

		public static byte[] GetBytes(this uint value)
		{
			return BitConverter.GetBytes(value);
		}

		public static uint GetUInt32(this byte[] bytes)
		{
			return BitConverter.ToUInt32(bytes, 0);
		}

		public static uint GetUInt32(this byte[] bytes, ref int startIndex)
		{
			uint result = BitConverter.ToUInt32(bytes, startIndex);
			startIndex += 4;
			return result;
		}

		public static byte[] GetBytes(this long value)
		{
			return BitConverter.GetBytes(value);
		}

		public static long GetInt64(this byte[] bytes)
		{
			return BitConverter.ToInt64(bytes, 0);
		}

		public static long GetInt64(this byte[] bytes, ref int startIndex)
		{
			long result = BitConverter.ToInt64(bytes, startIndex);
			startIndex += 8;
			return result;
		}

		public static byte[] GetBytes(this ulong value)
		{
			return BitConverter.GetBytes(value);
		}

		public static ulong GetUInt64(this byte[] bytes)
		{
			return BitConverter.ToUInt64(bytes, 0);
		}

		public static ulong GetUInt64(this byte[] bytes, ref int startIndex)
		{
			ulong result = BitConverter.ToUInt64(bytes, startIndex);
			startIndex += 8;
			return result;
		}

		public static byte[] GetBytes(this float value)
		{
			return BitConverter.GetBytes(value);
		}

		public static float GetSingle(this byte[] bytes)
		{
			return BitConverter.ToSingle(bytes, 0);
		}

		public static float GetSingle(this byte[] bytes, ref int startIndex)
		{
			float result = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			return result;
		}

		public static byte[] GetBytes(this double value)
		{
			return BitConverter.GetBytes(value);
		}

		public static double GetDouble(this byte[] bytes)
		{
			return BitConverter.ToDouble(bytes, 0);
		}

		public static double GetDouble(this byte[] bytes, ref int startIndex)
		{
			double result = BitConverter.ToDouble(bytes, startIndex);
			startIndex += 8;
			return result;
		}

		public static byte[] GetBytes(this string value)
		{
			return Encoding.UTF8.GetBytes(value);
		}

		public static string GetString(this byte[] bytes)
		{
			if (bytes == null)
			{
				Log.Warning("Value is invalid.");
				return null;
			}
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public static string GetString(this byte[] bytes, ref int startIndex)
		{
			if (bytes == null)
			{
				Log.Warning("Value is invalid.");
				return null;
			}
			startIndex += bytes.Length;
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public static byte[] GetBytes(this Vector2 value)
		{
			byte[] bytes = BitConverter.GetBytes(value.x);
			byte[] bytes2 = BitConverter.GetBytes(value.y);
			byte[] array = new byte[bytes.Length + bytes2.Length];
			Array.Copy(bytes, 0, array, 0, bytes.Length);
			Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
			return array;
		}

		public static Vector2 GetVector2(this byte[] bytes, ref int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float y = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			return new Vector2(x, y);
		}

		public static byte[] GetBytes(this Vector3 value)
		{
			byte[] bytes = BitConverter.GetBytes(value.x);
			byte[] bytes2 = BitConverter.GetBytes(value.y);
			byte[] bytes3 = BitConverter.GetBytes(value.z);
			byte[] array = new byte[bytes.Length + bytes2.Length + bytes3.Length];
			Array.Copy(bytes, 0, array, 0, bytes.Length);
			Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
			Array.Copy(bytes3, 0, array, bytes.Length + bytes2.Length, bytes3.Length);
			return array;
		}

		public static Vector3 GetVector3(this byte[] value, ref int startIndex)
		{
			float x = BitConverter.ToSingle(value, startIndex);
			startIndex += 4;
			float y = BitConverter.ToSingle(value, startIndex);
			startIndex += 4;
			float z = BitConverter.ToSingle(value, startIndex);
			startIndex += 4;
			return new Vector3(x, y, z);
		}

		public static byte[] GetBytes(this Vector3[] value)
		{
			int num = value.Length;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(num.GetBytes(), ref offset);
				for (int i = 0; i < num; i++)
				{
					memoryStream.WriteByteArray(value[i].GetBytes(), ref offset);
				}
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}

		public static Vector3[] GetVector3Array(this byte[] bytes, ref int startIndex)
		{
			int @int = bytes.GetInt32(ref startIndex);
			Vector3[] array = new Vector3[@int];
			for (int i = 0; i < @int; i++)
			{
				array[i] = bytes.GetVector3(ref startIndex);
			}
			return array;
		}

		public static byte[] GetBytes(this Vector2[] value)
		{
			int num = value.Length;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(num.GetBytes(), ref offset);
				for (int i = 0; i < num; i++)
				{
					memoryStream.WriteByteArray(value[i].GetBytes(), ref offset);
				}
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}

		public static Vector2[] GetVector2Array(this byte[] bytes, ref int startIndex)
		{
			int @int = bytes.GetInt32(ref startIndex);
			Vector2[] array = new Vector2[@int];
			for (int i = 0; i < @int; i++)
			{
				array[i] = bytes.GetVector2(ref startIndex);
			}
			return array;
		}

		public static byte[] GetBytes(this Color value)
		{
			byte[] bytes = BitConverter.GetBytes(value.r);
			byte[] bytes2 = BitConverter.GetBytes(value.g);
			byte[] bytes3 = BitConverter.GetBytes(value.b);
			byte[] bytes4 = BitConverter.GetBytes(value.a);
			byte[] array = new byte[bytes.Length + bytes2.Length + bytes3.Length + bytes4.Length];
			Array.Copy(bytes, 0, array, 0, bytes.Length);
			Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
			Array.Copy(bytes3, 0, array, bytes.Length + bytes2.Length, bytes3.Length);
			Array.Copy(bytes4, 0, array, bytes.Length + bytes2.Length + bytes3.Length, bytes4.Length);
			return array;
		}

		public static Color GetColor(this byte[] bytes, ref int startIndex)
		{
			float r = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float g = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float b = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float a = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			return new Color(r, g, b, a);
		}

		public static byte[] GetBytes(this Quaternion quaternion)
		{
			byte[] bytes = BitConverter.GetBytes(quaternion.x);
			byte[] bytes2 = BitConverter.GetBytes(quaternion.y);
			byte[] bytes3 = BitConverter.GetBytes(quaternion.z);
			byte[] bytes4 = BitConverter.GetBytes(quaternion.w);
			byte[] array = new byte[bytes.Length + bytes2.Length + bytes3.Length + bytes4.Length];
			Array.Copy(bytes, 0, array, 0, bytes.Length);
			Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
			Array.Copy(bytes3, 0, array, bytes.Length + bytes2.Length, bytes3.Length);
			Array.Copy(bytes4, 0, array, bytes.Length + bytes2.Length + bytes3.Length, bytes4.Length);
			return array;
		}

		public static Quaternion GetQuaternion(this byte[] bytes, ref int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float y = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float z = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			float w = BitConverter.ToSingle(bytes, startIndex);
			startIndex += 4;
			return new Quaternion(x, y, z, w);
		}
	}
}
