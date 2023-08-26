using UnityEngine;

namespace My.Core
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static string goName = string.Format("__{0}__", typeof(T).Name);

		private static T instance;

		private static Transform rootTransform;

		public static T Instance
		{
			get
			{
				if ((Object)instance == (Object)null)
				{
					GameObject obj = new GameObject(goName);
					obj.transform.SetParent(RootTransform, false);
					instance = obj.AddComponent<T>();
				}
				return instance;
			}
		}

		protected static Transform RootTransform
		{
			get
			{
				if (rootTransform == null)
				{
					string text = "__Singleton__";
					GameObject gameObject = GameObject.Find(text);
					if (gameObject == null)
					{
						gameObject = new GameObject(text);
						Object.DontDestroyOnLoad(gameObject);
					}
					rootTransform = gameObject.transform;
				}
				return rootTransform;
			}
		}
	}
}
