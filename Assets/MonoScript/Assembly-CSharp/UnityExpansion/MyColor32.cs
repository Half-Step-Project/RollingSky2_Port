using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityExpansion
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MyColor32
	{
		[FieldOffset(0)]
		private int rgba;

		[FieldOffset(0)]
		public byte r;

		[FieldOffset(1)]
		public byte g;

		[FieldOffset(2)]
		public byte b;

		[FieldOffset(3)]
		public byte a;

		public MyColor32(byte r, byte g, byte b, byte a)
		{
			rgba = 0;
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public static implicit operator MyColor32(MyColor c)
		{
			return new MyColor32((byte)(Mathf.Clamp01(c.r) * 255f), (byte)(Mathf.Clamp01(c.g) * 255f), (byte)(Mathf.Clamp01(c.b) * 255f), (byte)(Mathf.Clamp01(c.a) * 255f));
		}

		public static implicit operator MyColor(MyColor32 c)
		{
			return new MyColor((float)(int)c.r / 255f, (float)(int)c.g / 255f, (float)(int)c.b / 255f, (float)(int)c.a / 255f);
		}

		public static MyColor32 Lerp(MyColor32 a, MyColor32 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new MyColor32((byte)((float)(int)a.r + (float)(b.r - a.r) * t), (byte)((float)(int)a.g + (float)(b.g - a.g) * t), (byte)((float)(int)a.b + (float)(b.b - a.b) * t), (byte)((float)(int)a.a + (float)(b.a - a.a) * t));
		}

		public static MyColor32 LerpUnclamped(MyColor32 a, MyColor32 b, float t)
		{
			return new MyColor32((byte)((float)(int)a.r + (float)(b.r - a.r) * t), (byte)((float)(int)a.g + (float)(b.g - a.g) * t), (byte)((float)(int)a.b + (float)(b.b - a.b) * t), (byte)((float)(int)a.a + (float)(b.a - a.a) * t));
		}

		public override string ToString()
		{
			return string.Format("RGBA({0}, {1}, {2}, {3})", r, g, b, a);
		}

		public string ToString(string format)
		{
			return string.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format), g.ToString(format), b.ToString(format), a.ToString(format));
		}
	}
}
