using System.Collections.Generic;
using UnityEngine;

namespace RisingWin.Library
{
	public abstract class CloudAchievements : MonoBehaviour
	{
		public bool hasEnabled = true;

		private bool hasCurrentProcess;

		private CloudAchievementData currentProcess;

		private List<CloudAchievementData> listProcess;

		public bool initizlize { get; protected set; }

		public Dictionary<string, CloudAchievementData> mapAchievementData { get; protected set; }

		private void Process()
		{
			switch (currentProcess.processType)
			{
			case CloudProcess.ProcessType.READ_ASYNC:
				AchievementReadAsyncProcess();
				break;
			case CloudProcess.ProcessType.WRITE_ASYNC:
				AchievementWriteAsyncProcess();
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

		public CloudAchievementData GetCurrentProcess()
		{
			return currentProcess;
		}

		public abstract void AchievementReadAsyncProcess();

		public abstract void AchievementWriteAsyncProcess();

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

		public virtual void Init()
		{
			mapAchievementData = new Dictionary<string, CloudAchievementData>();
			listProcess = new List<CloudAchievementData>();
			initizlize = true;
		}

		protected void AddProcessingList(CloudAchievementData cloudAchievementData)
		{
			listProcess.Add(cloudAchievementData);
		}

		public virtual void Add(CloudAchievementData cloudAchievementData)
		{
			if (mapAchievementData.ContainsKey(cloudAchievementData.id))
			{
				mapAchievementData[cloudAchievementData.id] = cloudAchievementData;
			}
			else
			{
				mapAchievementData.Add(cloudAchievementData.id, cloudAchievementData);
			}
		}

		public virtual CloudAchievementData Get(string id)
		{
			CloudAchievementData value = null;
			mapAchievementData.TryGetValue(id, out value);
			return value;
		}

		public virtual void UnlockAll()
		{
			foreach (KeyValuePair<string, CloudAchievementData> mapAchievementDatum in mapAchievementData)
			{
				Unlock(mapAchievementDatum.Key);
			}
		}

		public virtual void Lock(string id)
		{
			CloudAchievementData value;
			if (mapAchievementData.TryGetValue(id, out value))
			{
				value.achieved = false;
				value.processType = CloudProcess.ProcessType.WRITE_ASYNC;
				AddProcessingList(value);
			}
		}

		public virtual void Unlock(string id)
		{
			CloudAchievementData value;
			if (mapAchievementData.TryGetValue(id, out value))
			{
				value.achieved = true;
				value.processType = CloudProcess.ProcessType.WRITE_ASYNC;
				AddProcessingList(value);
			}
		}
	}
}
