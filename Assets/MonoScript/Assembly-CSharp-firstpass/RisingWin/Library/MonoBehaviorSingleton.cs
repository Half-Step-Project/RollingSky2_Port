using UnityEngine;

namespace RisingWin.Library
{
	public class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static T _instance;

		public static T Instance
		{
			get
			{
				if ((Object)null == (Object)_instance)
				{
					_instance = Object.FindObjectOfType<T>();
					if ((Object)null == (Object)_instance)
					{
						GameObject obj = new GameObject("(singleton)" + typeof(T).Name.ToString());
						_instance = obj.AddComponent<T>();
						Object.DontDestroyOnLoad(obj);
					}
					else
					{
						Object.DontDestroyOnLoad(_instance.gameObject);
					}
				}
				return _instance;
			}
		}

		public virtual void Awake()
		{
			if ((Object)null == (Object)_instance)
			{
				Object.DontDestroyOnLoad(this);
				_instance = GetComponent<T>();
			}
		}

		public static T GetCurrentInstance()
		{
			return _instance;
		}
	}
}
