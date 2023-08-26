using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	internal class MonoTimerManager : MonoBehaviour
	{
		private const string NAME = "TimerManager";

		private Dictionary<MonoTimer, Coroutine> _coroutines;

		public static MonoTimerManager Instance
		{
			get
			{
				return GetOrCreate();
			}
		}

		public void StartTimer(MonoTimer monoTimer)
		{
			Coroutine value = StartCoroutine(monoTimer.StartInternal());
			if (_coroutines == null)
			{
				_coroutines = new Dictionary<MonoTimer, Coroutine>();
			}
			_coroutines[monoTimer] = value;
		}

		public void StopTimer(MonoTimer monoTimer)
		{
			Coroutine value;
			if (_coroutines != null && _coroutines.TryGetValue(monoTimer, out value))
			{
				StopCoroutine(value);
				_coroutines.Remove(monoTimer);
			}
		}

		private static MonoTimerManager GetOrCreate()
		{
			MonoTimerManager monoTimerManager = UnityEngine.Object.FindObjectOfType<MonoTimerManager>();
			if (monoTimerManager == null)
			{
				GameObject obj = new GameObject("TimerManager");
				obj.hideFlags |= HideFlags.HideInHierarchy;
				monoTimerManager = obj.AddComponent<MonoTimerManager>();
			}
			if (monoTimerManager == null)
			{
				throw new Exception("Get TimerManager failed.");
			}
			return monoTimerManager;
		}

		private void OnDestroy()
		{
			if (_coroutines == null)
			{
				return;
			}
			foreach (KeyValuePair<MonoTimer, Coroutine> coroutine in _coroutines)
			{
				StopCoroutine(coroutine.Value);
				coroutine.Key.StopInternal();
			}
			_coroutines.Clear();
		}
	}
}
