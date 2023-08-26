using System;
using UnityEngine;

namespace UnityExpansion
{
	[Serializable]
	public struct MyColor
	{
		public float r;

		public float g;

		public float b;

		public float a;

		public static MyColor red
		{
			get
			{
				return new MyColor(1f, 0f, 0f, 1f);
			}
		}

		public static MyColor green
		{
			get
			{
				return new MyColor(0f, 1f, 0f, 1f);
			}
		}

		public static MyColor blue
		{
			get
			{
				return new MyColor(0f, 0f, 1f, 1f);
			}
		}

		public static MyColor white
		{
			get
			{
				return new MyColor(1f, 1f, 1f, 1f);
			}
		}

		public static MyColor black
		{
			get
			{
				return new MyColor(0f, 0f, 0f, 1f);
			}
		}

		public static MyColor yellow
		{
			get
			{
				return new MyColor(1f, 47f / 51f, 4f / 255f, 1f);
			}
		}

		public static MyColor cyan
		{
			get
			{
				return new MyColor(0f, 1f, 1f, 1f);
			}
		}

		public static MyColor magenta
		{
			get
			{
				return new MyColor(1f, 0f, 1f, 1f);
			}
		}

		public static MyColor gray
		{
			get
			{
				return new MyColor(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		public static MyColor grey
		{
			get
			{
				return new MyColor(0.5f, 0.5f, 0.5f, 1f);
			}
		}

		public static MyColor clear
		{
			get
			{
				return new MyColor(0f, 0f, 0f, 0f);
			}
		}

		public float grayscale
		{
			get
			{
				return 0.299f * r + 0.587f * g + 0.114f * b;
			}
		}

		public float maxColorComponent
		{
			get
			{
				return Mathf.Max(Mathf.Max(r, g), b);
			}
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return r;
				case 1:
					return g;
				case 2:
					return b;
				case 3:
					return a;
				default:
					throw new IndexOutOfRangeException("Invalid Vector3 index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					r = value;
					break;
				case 1:
					g = value;
					break;
				case 2:
					b = value;
					break;
				case 3:
					a = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Vector3 index!");
				}
			}
		}

		public MyColor(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public MyColor(float r, float g, float b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			a = 1f;
		}

		public override string ToString()
		{
			return string.Format("RGBA({0:F3}, {1:F3}, {2:F3}, {3:F3})", r, g, b, a);
		}

		public string ToString(string format)
		{
			return string.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format), g.ToString(format), b.ToString(format), a.ToString(format));
		}

		public override int GetHashCode()
		{
			return ((MyVector4)this).GetHashCode();
		}

		public override bool Equals(object other)
		{
			if (!(other is MyColor))
			{
				return false;
			}
			MyColor myColor = (MyColor)other;
			if (r.Equals(myColor.r) && g.Equals(myColor.g) && b.Equals(myColor.b))
			{
				return a.Equals(myColor.a);
			}
			return false;
		}

		public static MyColor operator +(MyColor a, MyColor b)
		{
			return new MyColor(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
		}

		public static MyColor operator -(MyColor a, MyColor b)
		{
			return new MyColor(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		public static MyColor operator *(MyColor a, MyColor b)
		{
			return new MyColor(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
		}

		public static MyColor operator *(MyColor a, float b)
		{
			return new MyColor(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		public static MyColor operator *(float b, MyColor a)
		{
			return new MyColor(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		public static MyColor operator /(MyColor a, float b)
		{
			return new MyColor(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		public static bool operator ==(MyColor lhs, MyColor rhs)
		{
			return (MyVector4)lhs == (MyVector4)rhs;
		}

		public static bool operator !=(MyColor lhs, MyColor rhs)
		{
			return !(lhs == rhs);
		}

		public static MyColor Lerp(MyColor a, MyColor b, float t)
		{
			t = Mathf.Clamp01(t);
			return new MyColor(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		public static MyColor LerpUnclamped(MyColor a, MyColor b, float t)
		{
			return new MyColor(a.r + (b.r - a.r) * t, a.g + (b.g - a.g) * t, a.b + (b.b - a.b) * t, a.a + (b.a - a.a) * t);
		}

		internal MyColor RGBMultiplied(float multiplier)
		{
			return new MyColor(r * multiplier, g * multiplier, b * multiplier, a);
		}

		internal MyColor AlphaMultiplied(float multiplier)
		{
			return new MyColor(r, g, b, a * multiplier);
		}

		internal MyColor RGBMultiplied(MyColor multiplier)
		{
			return new MyColor(r * multiplier.r, g * multiplier.g, b * multiplier.b, a);
		}

		public static implicit operator MyVector4(MyColor c)
		{
			return new MyVector4(c.r, c.g, c.b, c.a);
		}

		public static implicit operator MyColor(MyVector4 v)
		{
			return new MyColor(v.x, v.y, v.z, v.w);
		}

		public static void RGBToHSV(MyColor rgbColor, out float H, out float S, out float V)
		{
			if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r)
			{
				RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
			}
			else if (rgbColor.g > rgbColor.r)
			{
				RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
			}
			else
			{
				RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
			}
		}

		private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
		{
			V = dominantcolor;
			if (V != 0f)
			{
				float num = 0f;
				num = ((!(colorone > colortwo)) ? colorone : colortwo);
				float num2 = V - num;
				if (num2 != 0f)
				{
					S = num2 / V;
					H = offset + (colorone - colortwo) / num2;
				}
				else
				{
					S = 0f;
					H = offset + (colorone - colortwo);
				}
				H /= 6f;
				if (H < 0f)
				{
					H += 1f;
				}
			}
			else
			{
				S = 0f;
				H = 0f;
			}
		}

		public static MyColor HSVToRGB(float H, float S, float V)
		{
			return HSVToRGB(H, S, V, true);
		}

		public static MyColor HSVToRGB(float H, float S, float V, bool hdr)
		{
			MyColor result = white;
			if (S == 0f)
			{
				result.r = V;
				result.g = V;
				result.b = V;
			}
			else if (V == 0f)
			{
				result.r = 0f;
				result.g = 0f;
				result.b = 0f;
			}
			else
			{
				result.r = 0f;
				result.g = 0f;
				result.b = 0f;
				float num = H * 6f;
				int num2 = (int)Mathf.Floor(num);
				float num3 = num - (float)num2;
				float num4 = V * (1f - S);
				float num5 = V * (1f - S * num3);
				float num6 = V * (1f - S * (1f - num3));
				switch (num2)
				{
				case 0:
					result.r = V;
					result.g = num6;
					result.b = num4;
					break;
				case 1:
					result.r = num5;
					result.g = V;
					result.b = num4;
					break;
				case 2:
					result.r = num4;
					result.g = V;
					result.b = num6;
					break;
				case 3:
					result.r = num4;
					result.g = num5;
					result.b = V;
					break;
				case 4:
					result.r = num6;
					result.g = num4;
					result.b = V;
					break;
				case 5:
					result.r = V;
					result.g = num4;
					result.b = num5;
					break;
				case 6:
					result.r = V;
					result.g = num6;
					result.b = num4;
					break;
				case -1:
					result.r = V;
					result.g = num4;
					result.b = num5;
					break;
				}
				if (!hdr)
				{
					result.r = Mathf.Clamp(result.r, 0f, 1f);
					result.g = Mathf.Clamp(result.g, 0f, 1f);
					result.b = Mathf.Clamp(result.b, 0f, 1f);
				}
			}
			return result;
		}
	}
}
