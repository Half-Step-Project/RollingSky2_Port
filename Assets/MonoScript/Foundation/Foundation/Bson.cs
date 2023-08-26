using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Foundation
{
	public sealed class Bson
	{
		private enum ValueType : byte
		{
			Bool = 1,
			SByte = 2,
			Byte = 3,
			Char = 4,
			Short = 5,
			UShort = 6,
			Int = 7,
			UInt = 8,
			Int64 = 9,
			UInt64 = 10,
			Decimal = 11,
			Float = 12,
			Double = 13,
			DateTime = 14,
			Enum = 15,
			String = 16,
			Array = 17,
			Vector2 = 18,
			Vector3 = 19,
			Vector4 = 20,
			Color = 21,
			Color32 = 22,
			Quaternion = 23,
			Bounds = 24,
			Rect = 25,
			Matrix = 26,
			Object = 27
		}

		private struct PropertyMeta
		{
			public MemberInfo Info { get; set; }

			public bool IsField { get; set; }

			public Type Type { get; set; }
		}

		private struct ArrayMeta
		{
			public Type ItemType { get; set; }

			public bool IsArray { get; set; }

			public bool IsList { get; set; }
		}

		private struct ObjectMeta
		{
			public Type ElemType { get; set; }

			public bool IsDict { get; set; }

			public IDictionary<string, PropertyMeta> Properties { get; set; }
		}

		private sealed class ByteArrayComparer : IEqualityComparer<byte[]>
		{
			public static ByteArrayComparer Default
			{
				get
				{
					return new ByteArrayComparer();
				}
			}

			public bool Equals(byte[] left, byte[] right)
			{
				if (left == null || right == null)
				{
					return left == right;
				}
				if (left == right)
				{
					return true;
				}
				if (left.Length != right.Length)
				{
					return false;
				}
				for (int i = 0; i < left.Length; i++)
				{
					if (left[i] != right[i])
					{
						return false;
					}
				}
				return true;
			}

			public int GetHashCode(byte[] obj)
			{
				if (obj == null)
				{
					throw new ArgumentNullException("obj");
				}
				int num = 0;
				int num2 = 0;
				foreach (byte b in obj)
				{
					num += b;
					num2 += num;
				}
				return num ^ num2;
			}
		}

		public sealed class Reader : IDisposable
		{
			private readonly MemoryStream _stream;

			private readonly BinaryReader _reader;

			private bool _isDisposed;

			public Reader(byte[] bson)
				: this(new MemoryStream(bson))
			{
			}

			public Reader(MemoryStream stream)
			{
				_stream = stream ?? new MemoryStream();
				_reader = new BinaryReader(_stream);
			}

			public object Decode(Type type)
			{
				Type type2 = Nullable.GetUnderlyingType(type) ?? type;
				switch (_reader.ReadByte())
				{
				case 1:
					if ((object)type2 != typeof(bool))
					{
						Log.Error(string.Format("Bson type is bool, but expect {0}", type2));
					}
					return _reader.ReadBoolean();
				case 2:
					if ((object)type2 != typeof(sbyte))
					{
						Log.Error(string.Format("Bson type is sbyte, but expect {0}", type2));
					}
					return _reader.ReadSByte();
				case 3:
					if ((object)type2 != typeof(byte))
					{
						Log.Error(string.Format("Bson type is byte, but expect {0}", type2));
					}
					return _reader.ReadByte();
				case 4:
					if ((object)type2 != typeof(char))
					{
						Log.Error(string.Format("Bson type is char, but expect {0}", type2));
					}
					return _reader.ReadChar();
				case 5:
					if ((object)type2 != typeof(short))
					{
						Log.Error(string.Format("Bson type is short, but expect {0}", type2));
					}
					return _reader.ReadInt16();
				case 6:
					if ((object)type2 != typeof(ushort))
					{
						Log.Error(string.Format("Bson type is ushort, but expect {0}", type2));
					}
					return _reader.ReadUInt16();
				case 7:
					if ((object)type2 != typeof(int))
					{
						Log.Error(string.Format("Bson type is int, but expect {0}", type2));
					}
					return _reader.ReadInt32();
				case 8:
					if ((object)type2 != typeof(uint))
					{
						Log.Error(string.Format("Bson type is uint, but expect {0}", type2));
					}
					return _reader.ReadUInt32();
				case 9:
					if ((object)type2 != typeof(long))
					{
						Log.Error(string.Format("Bson type is long, but expect {0}", type2));
					}
					return _reader.ReadInt64();
				case 10:
					if ((object)type2 != typeof(ulong))
					{
						Log.Error(string.Format("Bson type is ulong, but expect {0}", type2));
					}
					return _reader.ReadUInt64();
				case 11:
					if ((object)type2 != typeof(decimal))
					{
						Log.Error(string.Format("Bson type is decimal, but expect {0}", type2));
					}
					return _reader.ReadDecimal();
				case 14:
					if ((object)type2 != typeof(DateTime))
					{
						Log.Error(string.Format("Bson type is DateTime, but expect {0}", type2));
					}
					return ReadDateTime();
				case 15:
					if (!type2.IsEnum)
					{
						Log.Error(string.Format("Bson type is Enum, but expect {0}", type2));
					}
					return ReadEnum(type2);
				case 12:
					if ((object)type2 != typeof(float))
					{
						Log.Error(string.Format("Bson type is float, but expect {0}", type2));
					}
					return _reader.ReadSingle();
				case 13:
					if ((object)type2 != typeof(double))
					{
						Log.Error(string.Format("Bson type is double, but expect {0}", type2));
					}
					return _reader.ReadDouble();
				case 16:
					if ((object)type2 != typeof(string))
					{
						Log.Error(string.Format("Bson type is string, but expect {0}", type2));
					}
					return _reader.ReadString();
				case 17:
					if (!type2.IsArray && (object)type2.GetInterface("System.Collections.IList") == null)
					{
						Log.Error(string.Format("Bson type is Array, but expect {0}", type2));
					}
					return ReadArray(type2);
				case 18:
					if ((object)type2 != typeof(Vector2))
					{
						Log.Error(string.Format("Bson type is vector2, but expect {0}", type2));
					}
					return ReadVector2();
				case 19:
					if ((object)type2 != typeof(Vector3))
					{
						Log.Error(string.Format("Bson type is vector3, but expect {0}", type2));
					}
					return ReadVector3();
				case 20:
					if ((object)type2 != typeof(Vector4))
					{
						Log.Error(string.Format("Bson type is vector4, but expect {0}", type2));
					}
					return ReadVector4();
				case 21:
					if ((object)type2 != typeof(Color))
					{
						Log.Error(string.Format("Bson type is color, but expect {0}", type2));
					}
					return ReadColor();
				case 22:
					if ((object)type2 != typeof(Color32))
					{
						Log.Error(string.Format("Bson type is color32, but expect {0}", type2));
					}
					return ReadColor32();
				case 23:
					if ((object)type2 != typeof(Quaternion))
					{
						Log.Error(string.Format("Bson type is quaternion, but expect {0}", type2));
					}
					return ReadQuaternion();
				case 24:
					if ((object)type2 != typeof(Bounds))
					{
						Log.Error(string.Format("Bson type is bounds, but expect {0}", type2));
					}
					return ReadBounds();
				case 25:
					if ((object)type2 != typeof(Rect))
					{
						Log.Error(string.Format("Bson type is rect, but expect {0}", type2));
					}
					return ReadRect();
				case 26:
					if ((object)type2 != typeof(Matrix4x4))
					{
						Log.Error(string.Format("Bson type is matrix4x4, but expect {0}", type2));
					}
					return ReadMatrix();
				case 27:
					return ReadObject(type2);
				default:
					return null;
				}
			}

			private DateTime ReadDateTime()
			{
				long num = _reader.ReadInt64();
				return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(num * 10000);
			}

			private object ReadEnum(Type type)
			{
				return Enum.ToObject(type, _reader.ReadInt32());
			}

			private object ReadArray(Type type)
			{
				ArrayMeta arrayMeta = AddArrayMeta(type);
				IList list;
				Type type2;
				if (arrayMeta.IsArray)
				{
					list = new ArrayList();
					type2 = type.GetElementType();
				}
				else
				{
					list = (IList)Activator.CreateInstance(type);
					type2 = arrayMeta.ItemType;
				}
				long num = _reader.ReadInt32() + _reader.BaseStream.Position;
				while (_reader.BaseStream.Position < num)
				{
					object value = Decode(type2);
					list.Add(value);
				}
				object obj;
				if (arrayMeta.IsArray)
				{
					obj = Array.CreateInstance(type2, list.Count);
					for (int i = 0; i < list.Count; i++)
					{
						((Array)obj).SetValue(list[i], i);
					}
				}
				else
				{
					obj = list;
				}
				return obj;
			}

			private Vector2 ReadVector2()
			{
				Vector2 result = default(Vector2);
				result.x = _reader.ReadSingle();
				result.y = _reader.ReadSingle();
				return result;
			}

			private Vector3 ReadVector3()
			{
				Vector3 result = default(Vector3);
				result.x = _reader.ReadSingle();
				result.y = _reader.ReadSingle();
				result.z = _reader.ReadSingle();
				return result;
			}

			private Vector4 ReadVector4()
			{
				Vector4 result = default(Vector4);
				result.x = _reader.ReadSingle();
				result.y = _reader.ReadSingle();
				result.z = _reader.ReadSingle();
				result.w = _reader.ReadSingle();
				return result;
			}

			private Color ReadColor()
			{
				Color result = default(Color);
				result.r = _reader.ReadSingle();
				result.g = _reader.ReadSingle();
				result.b = _reader.ReadSingle();
				result.a = _reader.ReadSingle();
				return result;
			}

			private Color32 ReadColor32()
			{
				Color32 result = default(Color32);
				result.r = _reader.ReadByte();
				result.g = _reader.ReadByte();
				result.b = _reader.ReadByte();
				result.a = _reader.ReadByte();
				return result;
			}

			private Quaternion ReadQuaternion()
			{
				Quaternion result = default(Quaternion);
				result.x = _reader.ReadSingle();
				result.y = _reader.ReadSingle();
				result.z = _reader.ReadSingle();
				result.w = _reader.ReadSingle();
				return result;
			}

			private Bounds ReadBounds()
			{
				Bounds result = default(Bounds);
				result.center = new Vector3
				{
					x = _reader.ReadSingle(),
					y = _reader.ReadSingle(),
					z = _reader.ReadSingle()
				};
				result.extents = new Vector3
				{
					x = _reader.ReadSingle(),
					y = _reader.ReadSingle(),
					z = _reader.ReadSingle()
				};
				return result;
			}

			private Rect ReadRect()
			{
				Rect result = default(Rect);
				result.x = _reader.ReadSingle();
				result.y = _reader.ReadSingle();
				result.width = _reader.ReadSingle();
				result.height = _reader.ReadSingle();
				return result;
			}

			private Matrix4x4 ReadMatrix()
			{
				Matrix4x4 result = default(Matrix4x4);
				result.m00 = _reader.ReadSingle();
				result.m10 = _reader.ReadSingle();
				result.m20 = _reader.ReadSingle();
				result.m30 = _reader.ReadSingle();
				result.m01 = _reader.ReadSingle();
				result.m11 = _reader.ReadSingle();
				result.m21 = _reader.ReadSingle();
				result.m31 = _reader.ReadSingle();
				result.m02 = _reader.ReadSingle();
				result.m12 = _reader.ReadSingle();
				result.m22 = _reader.ReadSingle();
				result.m32 = _reader.ReadSingle();
				result.m03 = _reader.ReadSingle();
				result.m13 = _reader.ReadSingle();
				result.m23 = _reader.ReadSingle();
				result.m33 = _reader.ReadSingle();
				return result;
			}

			private object ReadObject(Type type)
			{
				ObjectMeta objectMeta = AddObjectMeta(type);
				object obj = Activator.CreateInstance(type);
				long num = _reader.ReadInt32() + _reader.BaseStream.Position;
				while (_reader.BaseStream.Position < num)
				{
					string text = _reader.ReadString();
					PropertyMeta value;
					if (objectMeta.Properties.TryGetValue(text, out value))
					{
						object value2 = Decode(value.Type);
						if (value.IsField)
						{
							((FieldInfo)value.Info).SetValue(obj, value2);
							continue;
						}
						PropertyInfo propertyInfo = (PropertyInfo)value.Info;
						if (propertyInfo.CanWrite)
						{
							propertyInfo.SetValue(obj, value2, null);
						}
					}
					else
					{
						object value3 = Decode(objectMeta.ElemType);
						if (objectMeta.IsDict)
						{
							((IDictionary)obj).Add(text, value3);
						}
						else
						{
							Log.Error(string.Format("The type {0} doesn't have the property '{1}'", type, text));
						}
					}
				}
				return obj;
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					BinaryReader reader = _reader;
					if (reader != null)
					{
						reader.Close();
					}
					MemoryStream stream = _stream;
					if (stream != null)
					{
						stream.Dispose();
					}
					_isDisposed = true;
				}
			}
		}

		public sealed class Writer : IDisposable
		{
			private readonly MemoryStream _stream;

			private readonly BinaryWriter _writer;

			private bool _isDisposed;

			public byte[] Bytes
			{
				get
				{
					if (_stream == null)
					{
						return null;
					}
					byte[] array = new byte[_stream.Length];
					Array.Copy(_stream.GetBuffer(), array, array.Length);
					return array;
				}
			}

			public Writer(MemoryStream stream = null)
			{
				_stream = stream ?? new MemoryStream();
				_writer = new BinaryWriter(_stream);
			}

			public void Encode(object obj)
			{
				if (obj == null)
				{
					Log.Error("Obj is null, encode return.");
					return;
				}
				if (obj != null)
				{
					object obj2;
					if ((obj2 = obj) is bool)
					{
						bool flag = (bool)obj2;
						bool value = flag;
						Write(value);
						return;
					}
					if ((obj2 = obj) is sbyte)
					{
						sbyte b = (sbyte)obj2;
						sbyte value2 = b;
						Write(value2);
						return;
					}
					if ((obj2 = obj) is byte)
					{
						byte b2 = (byte)obj2;
						byte value3 = b2;
						Write(value3);
						return;
					}
					if ((obj2 = obj) is char)
					{
						char c = (char)obj2;
						char value4 = c;
						Write(value4);
						return;
					}
					if ((obj2 = obj) is short)
					{
						short num = (short)obj2;
						short value5 = num;
						Write(value5);
						return;
					}
					if ((obj2 = obj) is ushort)
					{
						ushort num2 = (ushort)obj2;
						ushort value6 = num2;
						Write(value6);
						return;
					}
					if ((obj2 = obj) is int)
					{
						int num3 = (int)obj2;
						int value7 = num3;
						Write(value7);
						return;
					}
					if ((obj2 = obj) is uint)
					{
						uint num4 = (uint)obj2;
						uint value8 = num4;
						Write(value8);
						return;
					}
					if ((obj2 = obj) is long)
					{
						long num5 = (long)obj2;
						long value9 = num5;
						Write(value9);
						return;
					}
					if ((obj2 = obj) is ulong)
					{
						ulong num6 = (ulong)obj2;
						ulong value10 = num6;
						Write(value10);
						return;
					}
					if ((obj2 = obj) is decimal)
					{
						decimal num7 = (decimal)obj2;
						decimal value11 = num7;
						Write(value11);
						return;
					}
					if ((obj2 = obj) is float)
					{
						float num8 = (float)obj2;
						float value12 = num8;
						Write(value12);
						return;
					}
					if ((obj2 = obj) is double)
					{
						double num9 = (double)obj2;
						double value13 = num9;
						Write(value13);
						return;
					}
					if ((obj2 = obj) is DateTime)
					{
						DateTime dateTime = (DateTime)obj2;
						DateTime value14 = dateTime;
						Write(value14);
						return;
					}
					Enum @enum;
					if ((@enum = obj as Enum) != null)
					{
						WriteEnum(obj);
						return;
					}
					string text;
					if ((text = obj as string) != null)
					{
						string value15 = text;
						Write(value15);
						return;
					}
					IList list;
					if ((list = obj as IList) != null)
					{
						IList value16 = list;
						Write(value16);
						return;
					}
					IDictionary dictionary;
					if ((dictionary = obj as IDictionary) != null)
					{
						IDictionary value17 = dictionary;
						Write(value17);
						return;
					}
					if ((obj2 = obj) is Vector2)
					{
						Vector2 vector = (Vector2)obj2;
						Vector2 value18 = vector;
						Write(value18);
						return;
					}
					if ((obj2 = obj) is Vector3)
					{
						Vector3 vector2 = (Vector3)obj2;
						Vector3 value19 = vector2;
						Write(value19);
						return;
					}
					if ((obj2 = obj) is Vector4)
					{
						Vector4 vector3 = (Vector4)obj2;
						Vector4 value20 = vector3;
						Write(value20);
						return;
					}
					if ((obj2 = obj) is Color)
					{
						Color color = (Color)obj2;
						Color value21 = color;
						Write(value21);
						return;
					}
					if ((obj2 = obj) is Color32)
					{
						Color32 color2 = (Color32)obj2;
						Color32 value22 = color2;
						Write(value22);
						return;
					}
					if ((obj2 = obj) is Quaternion)
					{
						Quaternion quaternion = (Quaternion)obj2;
						Quaternion value23 = quaternion;
						Write(value23);
						return;
					}
					if ((obj2 = obj) is Bounds)
					{
						Bounds bounds = (Bounds)obj2;
						Bounds value24 = bounds;
						Write(value24);
						return;
					}
					if ((obj2 = obj) is Rect)
					{
						Rect rect = (Rect)obj2;
						Rect value25 = rect;
						Write(value25);
						return;
					}
					if ((obj2 = obj) is Matrix4x4)
					{
						Matrix4x4 matrix4x = (Matrix4x4)obj2;
						Matrix4x4 value26 = matrix4x;
						Write(value26);
						return;
					}
				}
				Write(obj);
			}

			private void Write(bool value)
			{
				_writer.Write((byte)1);
				_writer.Write(value);
			}

			private void Write(sbyte value)
			{
				_writer.Write((byte)2);
				_writer.Write(value);
			}

			private void Write(byte value)
			{
				_writer.Write((byte)3);
				_writer.Write(value);
			}

			private void Write(char value)
			{
				_writer.Write((byte)4);
				_writer.Write(value);
			}

			private void Write(short value)
			{
				_writer.Write((byte)5);
				_writer.Write(value);
			}

			private void Write(ushort value)
			{
				_writer.Write((byte)6);
				_writer.Write(value);
			}

			private void Write(int value)
			{
				_writer.Write((byte)7);
				_writer.Write(value);
			}

			private void Write(uint value)
			{
				_writer.Write((byte)8);
				_writer.Write(value);
			}

			private void Write(long value)
			{
				_writer.Write((byte)9);
				_writer.Write(value);
			}

			private void Write(ulong value)
			{
				_writer.Write((byte)10);
				_writer.Write(value);
			}

			private void Write(decimal value)
			{
				_writer.Write((byte)11);
				_writer.Write(value);
			}

			private void Write(float value)
			{
				_writer.Write((byte)12);
				_writer.Write(value);
			}

			private void Write(double value)
			{
				_writer.Write((byte)13);
				_writer.Write(value);
			}

			private void Write(DateTime value)
			{
				TimeSpan timeSpan = ((value.Kind == DateTimeKind.Local) ? (value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()) : (value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)));
				_writer.Write((byte)14);
				_writer.Write((long)timeSpan.TotalSeconds * 1000);
			}

			private void WriteEnum(object value)
			{
				_writer.Write((byte)15);
				_writer.Write((int)value);
			}

			private void Write(string value)
			{
				_writer.Write((byte)16);
				_writer.Write(value);
			}

			private void Write(IList value)
			{
				using (Writer writer = new Writer())
				{
					foreach (object item in value)
					{
						if (item != null)
						{
							writer.Encode(item);
						}
					}
					int num = (int)writer._stream.Length;
					_writer.Write((byte)17);
					_writer.Write(num);
					_writer.Write(writer._stream.GetBuffer(), 0, num);
				}
			}

			private void Write(IDictionary value)
			{
				using (Writer writer = new Writer())
				{
					foreach (DictionaryEntry item in value)
					{
						if (item.Value != null)
						{
							writer._writer.Write(item.Key.ToString());
							writer.Encode(item.Value);
						}
					}
					int num = (int)writer._stream.Length;
					_writer.Write((byte)27);
					_writer.Write(num);
					_writer.Write(writer._stream.GetBuffer(), 0, num);
				}
			}

			private void Write(Vector2 value)
			{
				_writer.Write((byte)18);
				_writer.Write(value.x);
				_writer.Write(value.y);
			}

			private void Write(Vector3 value)
			{
				_writer.Write((byte)19);
				_writer.Write(value.x);
				_writer.Write(value.y);
				_writer.Write(value.z);
			}

			private void Write(Vector4 value)
			{
				_writer.Write((byte)20);
				_writer.Write(value.x);
				_writer.Write(value.y);
				_writer.Write(value.z);
				_writer.Write(value.w);
			}

			private void Write(Color value)
			{
				_writer.Write((byte)21);
				_writer.Write(value.r);
				_writer.Write(value.g);
				_writer.Write(value.b);
				_writer.Write(value.a);
			}

			private void Write(Color32 value)
			{
				_writer.Write((byte)22);
				_writer.Write(value.r);
				_writer.Write(value.g);
				_writer.Write(value.b);
				_writer.Write(value.a);
			}

			private void Write(Quaternion value)
			{
				_writer.Write((byte)23);
				_writer.Write(value.x);
				_writer.Write(value.y);
				_writer.Write(value.z);
				_writer.Write(value.w);
			}

			private void Write(Bounds value)
			{
				_writer.Write((byte)24);
				_writer.Write(value.center.x);
				_writer.Write(value.center.y);
				_writer.Write(value.center.z);
				_writer.Write(value.extents.x);
				_writer.Write(value.extents.y);
				_writer.Write(value.extents.z);
			}

			private void Write(Rect value)
			{
				_writer.Write((byte)25);
				_writer.Write(value.x);
				_writer.Write(value.y);
				_writer.Write(value.width);
				_writer.Write(value.height);
			}

			private void Write(Matrix4x4 value)
			{
				_writer.Write((byte)26);
				_writer.Write(value.m00);
				_writer.Write(value.m10);
				_writer.Write(value.m20);
				_writer.Write(value.m30);
				_writer.Write(value.m01);
				_writer.Write(value.m11);
				_writer.Write(value.m21);
				_writer.Write(value.m31);
				_writer.Write(value.m02);
				_writer.Write(value.m12);
				_writer.Write(value.m22);
				_writer.Write(value.m32);
				_writer.Write(value.m03);
				_writer.Write(value.m13);
				_writer.Write(value.m23);
				_writer.Write(value.m33);
			}

			private void Write(object value)
			{
				using (Writer writer = new Writer())
				{
					IList<PropertyMeta> list = AddPropertyMetas(value.GetType());
					foreach (PropertyMeta item in list)
					{
						if (item.IsField)
						{
							object value2 = ((FieldInfo)item.Info).GetValue(value);
							if (value2 != null)
							{
								writer._writer.Write(item.Info.Name);
								writer.Encode(value2);
							}
							continue;
						}
						PropertyInfo propertyInfo = (PropertyInfo)item.Info;
						if (propertyInfo.CanRead && propertyInfo.CanWrite)
						{
							object value3 = propertyInfo.GetValue(value, null);
							if (value3 != null)
							{
								writer._writer.Write(item.Info.Name);
								writer.Encode(value3);
							}
						}
					}
					int num = (int)writer._stream.Length;
					_writer.Write((byte)27);
					_writer.Write(num);
					_writer.Write(writer._stream.GetBuffer(), 0, num);
				}
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					BinaryWriter writer = _writer;
					if (writer != null)
					{
						writer.Close();
					}
					MemoryStream stream = _stream;
					if (stream != null)
					{
						stream.Dispose();
					}
					_isDisposed = true;
				}
			}
		}

		private static readonly Dictionary<byte[], object> _bsonObjects = new Dictionary<byte[], object>(ByteArrayComparer.Default);

		private static readonly IDictionary<Type, IList<PropertyMeta>> _propertyMetas = new Dictionary<Type, IList<PropertyMeta>>();

		private static readonly IDictionary<Type, ArrayMeta> _arrayMetas = new Dictionary<Type, ArrayMeta>();

		private static readonly IDictionary<Type, ObjectMeta> _objectMetas = new Dictionary<Type, ObjectMeta>();

		public static byte[] ToBson(object obj, bool cache = false)
		{
			if (obj == null)
			{
				return null;
			}
			using (Writer writer = new Writer())
			{
				writer.Encode(obj);
				return writer.Bytes;
			}
		}

		public static void ToBson(object obj, Writer writer)
		{
			if (obj != null && writer != null)
			{
				writer.Encode(obj);
			}
		}

		public static T ToObject<T>(byte[] bson)
		{
			if (bson == null || bson.Length == 0)
			{
				return default(T);
			}
			object value;
			if (_bsonObjects.TryGetValue(bson, out value) && value != null)
			{
				return (T)value;
			}
			using (Reader reader = new Reader(bson))
			{
				value = reader.Decode(typeof(T));
				return (T)value;
			}
		}

		public static T ToObject<T>(Reader reader)
		{
			if (reader == null)
			{
				return default(T);
			}
			object obj = reader.Decode(typeof(T));
			return (T)obj;
		}

		public static void Clear()
		{
			_bsonObjects.Clear();
			_propertyMetas.Clear();
			_arrayMetas.Clear();
			_objectMetas.Clear();
		}

		private static IList<PropertyMeta> AddPropertyMetas(Type type)
		{
			IList<PropertyMeta> value;
			if (_propertyMetas.TryGetValue(type, out value) && value != null)
			{
				return value;
			}
			value = new List<PropertyMeta>();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (!(propertyInfo.Name == "Item"))
				{
					PropertyMeta propertyMeta = default(PropertyMeta);
					propertyMeta.Info = propertyInfo;
					propertyMeta.IsField = false;
					PropertyMeta item = propertyMeta;
					value.Add(item);
				}
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			FieldInfo[] array2 = fields;
			foreach (FieldInfo info in array2)
			{
				PropertyMeta propertyMeta = default(PropertyMeta);
				propertyMeta.Info = info;
				propertyMeta.IsField = true;
				PropertyMeta item2 = propertyMeta;
				value.Add(item2);
			}
			_propertyMetas[type] = value;
			return value;
		}

		private static ArrayMeta AddArrayMeta(Type type)
		{
			ArrayMeta value;
			if (_arrayMetas.TryGetValue(type, out value))
			{
				return value;
			}
			ArrayMeta arrayMeta = default(ArrayMeta);
			arrayMeta.IsArray = type.IsArray;
			value = arrayMeta;
			if ((object)type.GetInterface("System.Collections.IList") != null)
			{
				value.IsList = true;
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (!(propertyInfo.Name != "Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						value.ItemType = (((object)indexParameters[0].ParameterType == typeof(int)) ? propertyInfo.PropertyType : typeof(object));
					}
				}
			}
			_arrayMetas[type] = value;
			return value;
		}

		private static ObjectMeta AddObjectMeta(Type type)
		{
			ObjectMeta value;
			if (_objectMetas.TryGetValue(type, out value))
			{
				return value;
			}
			value = default(ObjectMeta);
			if ((object)type.GetInterface("System.Collections.IDictionary") != null)
			{
				value.IsDict = true;
			}
			value.Properties = new Dictionary<string, PropertyMeta>();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (propertyInfo.Name == "Item")
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1)
					{
						value.ElemType = (((object)indexParameters[0].ParameterType == typeof(string)) ? propertyInfo.PropertyType : typeof(object));
					}
				}
				else
				{
					PropertyMeta propertyMeta = default(PropertyMeta);
					propertyMeta.Info = propertyInfo;
					propertyMeta.Type = propertyInfo.PropertyType;
					PropertyMeta value2 = propertyMeta;
					value.Properties.Add(propertyInfo.Name, value2);
				}
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			FieldInfo[] array2 = fields;
			foreach (FieldInfo fieldInfo in array2)
			{
				PropertyMeta propertyMeta = default(PropertyMeta);
				propertyMeta.Info = fieldInfo;
				propertyMeta.IsField = true;
				propertyMeta.Type = fieldInfo.FieldType;
				PropertyMeta value3 = propertyMeta;
				value.Properties.Add(fieldInfo.Name, value3);
			}
			_objectMetas[type] = value;
			return value;
		}
	}
}
