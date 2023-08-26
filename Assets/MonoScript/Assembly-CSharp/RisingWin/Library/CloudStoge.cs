using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RisingWin.Library
{
	public abstract class CloudStoge : MonoBehaviour
	{
		public bool hasEnabled = true;

		private bool hasCurrentProcess;

		private CloudData currentProcess;

		private List<CloudData> listProcess;

		public bool initizlize { get; protected set; }

		private void Process()
		{
			switch (currentProcess.processType)
			{
			case CloudProcess.ProcessType.READ_ASYNC:
				FileReadAsyncProcess();
				break;
			case CloudProcess.ProcessType.WRITE_ASYNC:
				FileWriteAsyncProcess();
				break;
			}
		}

		private void Update()
		{
			if (!initizlize || !hasEnabled)
			{
				return;
			}
			if (hasCurrentProcess)
			{
				if (currentProcess.IsProcessing)
				{
					return;
				}
				hasCurrentProcess = false;
			}
			if (listProcess.Count > 0)
			{
				hasCurrentProcess = true;
				currentProcess = listProcess[0];
				listProcess.RemoveAt(0);
				Process();
			}
		}

		public CloudData GetCurrentProcess()
		{
			return currentProcess;
		}

		public virtual void Init()
		{
			listProcess = new List<CloudData>();
			initizlize = true;
		}

		private void AddProcessingList(CloudData cloudData)
		{
			listProcess.Add(cloudData);
		}

		public virtual int GetFileCount()
		{
			return 0;
		}

		public virtual void GetFileNameAndSize(int index, out string name, out int fileSizeInBytes)
		{
			name = string.Empty;
			fileSizeInBytes = 0;
		}

		public virtual bool FileExists(string filename)
		{
			return false;
		}

		public virtual bool FileDelete(string filename)
		{
			return false;
		}

		public virtual void DeleteAllFile()
		{
		}

		public virtual void FileReadAsync(string filename, int filesize, CloudData cloudData, Action finishedCallback)
		{
			cloudData.filename = filename;
			cloudData.filesize = filesize;
			cloudData.finishedCallback = finishedCallback;
			cloudData.processType = CloudProcess.ProcessType.READ_ASYNC;
			cloudData.processStatus = CloudProcess.ProcessStatus.NONE;
			AddProcessingList(cloudData);
		}

		public virtual void FileWriteAsync(string filename, string stringData, CloudData cloudData, Action finishedCallback)
		{
			cloudData.filename = filename;
			cloudData.data = Encoding.UTF8.GetBytes(stringData);
			cloudData.filesize = cloudData.data.Length;
			cloudData.finishedCallback = finishedCallback;
			cloudData.processType = CloudProcess.ProcessType.WRITE_ASYNC;
			cloudData.processStatus = CloudProcess.ProcessStatus.NONE;
			AddProcessingList(cloudData);
		}

		public virtual void FileWriteAsync(string filename, byte[] data, CloudData cloudData, Action finishedCallback)
		{
			cloudData.filename = filename;
			cloudData.filesize = data.Length;
			cloudData.data = new byte[cloudData.filesize];
			Buffer.BlockCopy(data, 0, cloudData.data, 0, cloudData.filesize);
			cloudData.finishedCallback = finishedCallback;
			cloudData.processType = CloudProcess.ProcessType.WRITE_ASYNC;
			cloudData.processStatus = CloudProcess.ProcessStatus.NONE;
			AddProcessingList(cloudData);
		}

		public abstract void FileReadAsyncProcess();

		public abstract void FileWriteAsyncProcess();

		public void ProcessFail()
		{
			if (currentProcess != null)
			{
				currentProcess.Fail();
			}
		}

		public void ProcessSuccess()
		{
			if (currentProcess != null)
			{
				currentProcess.Success();
			}
		}
	}
}
