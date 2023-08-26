using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourcesManager
{
	public delegate void OnLoadAsyncFinished(string path, Object obj);

	public static VersionType m_versionType = VersionType.DEBUG;

	private static Dictionary<string, Object> m_resourcesDatas = new Dictionary<string, Object>();

	public static T Load<T>(string path) where T : Object
	{
		Object value = null;
		if (!m_resourcesDatas.TryGetValue(path, out value) || value == null)
		{
			value = Resources.Load<T>(path);
			m_resourcesDatas[path] = value;
		}
		return value as T;
	}

	public static void LoadAsync(string path, OnLoadAsyncFinished finished, CoroutineManagerType coroutineType = CoroutineManagerType.RESOURCES)
	{
		bool flag = true;
		Object value = null;
		if (!m_resourcesDatas.TryGetValue(path, out value) || value == null)
		{
			flag = false;
			ResourceRequest request = Resources.LoadAsync(path);
			CoroutineManager.CreateCoroutine(coroutineType, WaitResourcesLoadAsync(path, request, finished));
		}
		if (flag && finished != null)
		{
			finished(path, value);
		}
	}

	public static void ResourcesLoadAsync(List<string> paths, OnResourcesLoadAsyncProgress onProgress, OnResourcesLoadAsyncFinished onFinished, CoroutineManagerType coroutineType = CoroutineManagerType.LOADINGRESOURCES, int advanceLoadCount = 10)
	{
		Dictionary<string, Object> _pool = new Dictionary<string, Object>();
		if (paths.Count > 0)
		{
			if (onProgress != null)
			{
				onProgress(0f);
			}
			int _currentProgress = 0;
			int _pathsCount = paths.Count;
			for (int i = 0; i < _pathsCount; i += advanceLoadCount)
			{
				List<string> list = new List<string>();
				int num = Mathf.Min(advanceLoadCount, paths.Count - i);
				for (int j = 0; j < num; j++)
				{
					list.Add(paths[i + j]);
				}
				int index = 0;
				OnResourcesLoadAsync(list, index, delegate
				{
					_currentProgress++;
					if (onProgress != null)
					{
						onProgress((float)_currentProgress * 1f / (float)_pathsCount);
					}
				}, delegate
				{
					if (_currentProgress >= _pathsCount && onFinished != null)
					{
						onFinished(_pool);
					}
				}, _pool, coroutineType);
			}
		}
		else
		{
			if (onProgress != null)
			{
				onProgress(1f);
			}
			if (onFinished != null)
			{
				onFinished(_pool);
			}
		}
	}

	private static void OnResourcesLoadAsync(List<string> paths, int index, OnResourcesLoadAsyncProgress onProgress, OnResourcesLoadAsyncFinished onFinished, Dictionary<string, Object> pool, CoroutineManagerType coroutineType = CoroutineManagerType.LOADINGRESOURCES)
	{
		LoadAsync(paths[index], delegate(string p, Object x)
		{
			Object value = null;
			if (!pool.TryGetValue(p, out value) || value == null)
			{
				pool[p] = x;
			}
			index++;
			if (onProgress != null)
			{
				onProgress((float)index * 1f / (float)paths.Count);
			}
			if (index >= paths.Count)
			{
				if (onFinished != null)
				{
					onFinished(pool);
				}
			}
			else
			{
				OnResourcesLoadAsync(paths, index, onProgress, onFinished, pool, coroutineType);
			}
		}, CoroutineManagerType.LOADINGRESOURCES);
	}

	public static void StopLoadAsync(CoroutineManagerType coroutineType = CoroutineManagerType.RESOURCES)
	{
		CoroutineManager.DestroyCoroutine(coroutineType);
	}

	private static IEnumerator WaitResourcesLoadAsync(string path, ResourceRequest request, OnLoadAsyncFinished finished)
	{
		yield return request;
		if (request.isDone)
		{
			if (finished != null)
			{
				finished(path, request.asset);
			}
			Object value = null;
			if (!m_resourcesDatas.TryGetValue(path, out value) || value == null)
			{
				m_resourcesDatas[path] = request.asset;
			}
		}
	}

	public static void Unload(params string[] paths)
	{
		int num = paths.Length;
		for (int i = 0; i < num; i++)
		{
			m_resourcesDatas.Remove(paths[i]);
		}
		Resources.UnloadUnusedAssets();
	}
}
