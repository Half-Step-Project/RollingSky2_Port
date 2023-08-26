using System;

namespace RisingWin.Library
{
	public interface IFileIO
	{
		void Read(Action<string> pDoneCallback);

		void Write(string pContent, Action pDoneCallback);

		void Close();
	}
}
