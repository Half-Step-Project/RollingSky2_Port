using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace RisingWin.Library
{
	public class AsyncFileIO : IFileIO
	{
		private FileStream streaming;

		private byte[] readBuffer;

		public AsyncFileIO(string pPath, string pFile)
		{
			string text = Path.Combine(Path.Combine(PathUtility.GetPersistentDataPath(), pPath), pFile);
			Debug.Log("File Path: " + text);
			CreateDirectoryIfNeeded(text);
			streaming = new FileStream(text, FileMode.OpenOrCreate);
		}

		private static void CreateDirectoryIfNeeded(string pPath)
		{
			string directoryName = Path.GetDirectoryName(pPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}

		public void Read(Action<string> pDoneCallback)
		{
			streaming.Position = 0L;
			readBuffer = new byte[streaming.Length];
			IAsyncResult asyncResult = streaming.BeginRead(readBuffer, 0, readBuffer.Length, null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			if (streaming.EndRead(asyncResult) > 0)
			{
				string @string = Encoding.UTF8.GetString(readBuffer);
				if (pDoneCallback != null)
				{
					pDoneCallback(@string);
				}
			}
			else if (pDoneCallback != null)
			{
				pDoneCallback(string.Empty);
			}
			asyncResult.AsyncWaitHandle.Close();
		}

		public void Write(string pContent, Action pDoneCallback)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(pContent);
			streaming.Position = 0L;
			IAsyncResult asyncResult = streaming.BeginWrite(bytes, 0, bytes.Length, null, null);
			asyncResult.AsyncWaitHandle.WaitOne();
			streaming.Flush();
			streaming.SetLength(streaming.Position);
			streaming.EndWrite(asyncResult);
			if (pDoneCallback != null)
			{
				pDoneCallback();
			}
			asyncResult.AsyncWaitHandle.Close();
		}

		public void Close()
		{
			streaming.Close();
		}
	}
}
