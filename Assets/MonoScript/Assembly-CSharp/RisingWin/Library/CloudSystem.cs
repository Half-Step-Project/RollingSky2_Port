using System;
using System.Collections;
using UnityEngine;

namespace RisingWin.Library
{
	public class CloudSystem : MonoBehaviorSingleton<CloudSystem>
	{
		public bool enableAchievements;

		public bool enableCloudStoge;

		public bool initizlize { get; private set; }

		public CloudBase cloudBase { get; private set; }

		public CloudAchievements cloudAchievements { get; private set; }

		public CloudStoge cloudStoge { get; private set; }

		public bool isOnline
		{
			get
			{
				if (!initizlize)
				{
					return false;
				}
				return cloudBase.isOnline;
			}
		}

		public void AddCloudComponent<T>() where T : Component
		{
			Component component = base.gameObject.AddComponent<T>();
			if (null != component as CloudBase)
			{
				cloudBase = component as CloudBase;
			}
			else if (null != component as CloudAchievements)
			{
				cloudAchievements = component as CloudAchievements;
			}
			else if (null != component as CloudStoge)
			{
				cloudStoge = component as CloudStoge;
			}
		}

		public void Init()
		{
			if (!(null == cloudBase))
			{
				if (null != cloudAchievements)
				{
					enableAchievements = false;
				}
				if (null != cloudStoge)
				{
					enableCloudStoge = true;
				}
				StartCoroutine(CloudSystemInitCoroutine());
			}
		}

		private IEnumerator CloudSystemInitCoroutine()
		{
			cloudBase.Init();
			while (!cloudBase.initizlize)
			{
				yield return new WaitForEndOfFrame();
			}
			if (null != cloudStoge)
			{
				cloudStoge.Init();
				while (!cloudStoge.initizlize)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			initizlize = true;
		}

		public uint GetServerRealTime()
		{
			if (initizlize && isOnline)
			{
				return cloudBase.GetServerRealTime();
			}
			return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}
	}
}
