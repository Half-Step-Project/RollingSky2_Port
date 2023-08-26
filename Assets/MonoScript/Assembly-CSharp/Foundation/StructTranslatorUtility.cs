using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Foundation
{
	public static class StructTranslatorUtility
	{
		private sealed class Marshaller<T> : IDisposable where T : new()
		{
			private IntPtr m_Handle;

			private readonly int m_BufferSize;

			private bool m_Disposed;

			public Marshaller()
			{
				m_BufferSize = Marshal.SizeOf(typeof(T));
				m_Handle = Marshal.AllocHGlobal(m_BufferSize);
			}

			~Marshaller()
			{
				Dispose();
			}

			public void ToByteArray(T structure, out byte[] buffer)
			{
				buffer = new byte[m_BufferSize];
				Marshal.StructureToPtr(structure, m_Handle, true);
				Marshal.Copy(m_Handle, buffer, 0, m_BufferSize);
			}

			public void ToStructure(byte[] buffer, out T structure)
			{
				structure = new T();
				Marshal.Copy(buffer, 0, m_Handle, m_BufferSize);
				Marshal.PtrToStructure(m_Handle, structure);
			}

			public void Dispose()
			{
				if (!m_Disposed)
				{
					Marshal.FreeHGlobal(m_Handle);
					m_Disposed = true;
				}
				GC.SuppressFinalize(this);
			}
		}

		private static SurrogateSelector surrogateSelector;

		static StructTranslatorUtility()
		{
			surrogateSelector = new SurrogateSelector();
			Vector3SerializationSurrogate surrogate = new Vector3SerializationSurrogate();
			ColorSerializationSurrogate surrogate2 = new ColorSerializationSurrogate();
			surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), surrogate);
			surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All), surrogate2);
		}

		public static byte[] ToByteArrayCommon<T>(T obj)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.SurrogateSelector = surrogateSelector;
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}

		public static T ToStructureCommon<T>(byte[] buffer)
		{
			using (MemoryStream serializationStream = new MemoryStream(buffer))
			{
				return (T)new BinaryFormatter
				{
					SurrogateSelector = surrogateSelector
				}.Deserialize(serializationStream);
			}
		}

		public static byte[] ToByteArray<T>(T obj) where T : IReadWriteBytes
		{
			return obj.WriteBytes();
		}

		public static T ToStructure<T>(byte[] buffer) where T : struct, IReadWriteBytes
		{
			T result = default(T);
			result.ReadBytes(buffer);
			return result;
		}

		public static T ToClass<T>(byte[] buffer) where T : class, IReadWriteBytes, new()
		{
			T val = new T();
			val.ReadBytes(buffer);
			return val;
		}

		public static byte[] ToByteArrayWithMarshall<T>(T obj) where T : new()
		{
			byte[] buffer;
			new Marshaller<T>().ToByteArray(obj, out buffer);
			return buffer;
		}

		public static T ToStructureWithMarshall<T>(byte[] buffer) where T : new()
		{
			T structure;
			new Marshaller<T>().ToStructure(buffer, out structure);
			return structure;
		}
	}
}
