using System;

namespace RisingWin.Library
{
	public class CloudProcess
	{
		public enum ProcessType
		{
			NONE,
			READ_ASYNC,
			WRITE_ASYNC
		}

		public enum ProcessStatus
		{
			NONE,
			PROCESSING,
			FAIL,
			SUCCESS
		}

		public ProcessType processType;

		public ProcessStatus processStatus;

		public Action finishedCallback;

		public bool IsProcessing
		{
			get
			{
				return ProcessStatus.PROCESSING == processStatus;
			}
		}

		public bool IsFail
		{
			get
			{
				return ProcessStatus.FAIL == processStatus;
			}
		}

		public bool IsSuccess
		{
			get
			{
				return ProcessStatus.SUCCESS == processStatus;
			}
		}

		public void Fail()
		{
			processStatus = ProcessStatus.FAIL;
			if (finishedCallback != null)
			{
				finishedCallback();
			}
		}

		public void Success()
		{
			processStatus = ProcessStatus.SUCCESS;
			if (finishedCallback != null)
			{
				finishedCallback();
			}
		}
	}
}
