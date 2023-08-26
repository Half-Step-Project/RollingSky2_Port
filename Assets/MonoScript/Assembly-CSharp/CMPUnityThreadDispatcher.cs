using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CMPUnityThreadDispatcher : MonoBehaviour
{
	public struct TimeProcess
	{
		private float latestTime;

		private long serverTime;

		public long leftTime
		{
			get
			{
				long num = serverTime - (long)((Time.realtimeSinceStartup - latestTime) * 1000f);
				if (num >= 0)
				{
					return num;
				}
				return 0L;
			}
		}

		public void Set(long serverTime)
		{
			this.serverTime = serverTime;
			latestTime = Time.realtimeSinceStartup;
		}
	}

	public struct ActionInfo
	{
		public Action action;

		public TimeProcess time;
	}

	private const string GameObject_Name = "CMPThreadDispatcher";

	private static CMPUnityThreadDispatcher _instance;

	private LinkedList<ActionInfo> actions = new LinkedList<ActionInfo>();

	public static CMPUnityThreadDispatcher Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<CMPUnityThreadDispatcher>();
				if (_instance == null)
				{
					GameObject obj = new GameObject("CMPThreadDispatcher");
					UnityEngine.Object.DontDestroyOnLoad(obj);
					_instance = obj.AddComponent<CMPUnityThreadDispatcher>();
				}
			}
			return _instance;
		}
	}

	public static void Initialize()
	{
	}

	private void Update()
	{
		for (LinkedListNode<ActionInfo> linkedListNode = actions.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			ActionInfo value = linkedListNode.Value;
			if (value.action != null && value.time.leftTime <= 0)
			{
				value.action();
				lock (actions)
				{
					actions.Remove(linkedListNode);
				}
			}
		}
	}

	public void AddAction(ActionInfo a)
	{
		lock (actions)
		{
			actions.AddLast(a);
		}
	}

	public static void RunOnMainThread(Action a, float delayTime = 0f)
	{
		if (a != null)
		{
			ActionInfo a2 = default(ActionInfo);
			a2.action = a;
			if (delayTime > 0f)
			{
				a2.time.Set((long)(delayTime * 1000f));
			}
			Instance.AddAction(a2);
		}
	}
}
