using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instacne
	{
		get
		{
			if ((Object)instance == (Object)null)
			{
				instance = Object.FindObjectOfType<T>();
				if ((Object)instance == (Object)null)
				{
					instance = new GameObject(typeof(T).Name).AddComponent<T>();
				}
			}
			return instance;
		}
	}

	public static void DestroyInstance()
	{
		if ((Object)instance != (Object)null)
		{
			Object.Destroy(instance.gameObject);
			instance = null;
		}
	}

	protected virtual void Awake()
	{
		if ((Object)instance == (Object)null)
		{
			instance = base.gameObject.GetComponent<T>();
		}
		Object.DontDestroyOnLoad(base.gameObject);
		Init();
	}

	protected virtual void Init()
	{
	}

	protected virtual void OnDestroy()
	{
		if ((Object)instance != (Object)null && instance.gameObject == base.gameObject)
		{
			instance = null;
		}
	}
}
