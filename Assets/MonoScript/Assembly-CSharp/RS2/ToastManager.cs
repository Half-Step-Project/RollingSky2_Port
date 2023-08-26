using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ToastManager : MonoSingleton<ToastManager>
	{
		private const string ToastParent = "Framework/UI/UI Group - Forth";

		private const string ToastPrefabPath = "Assets/_RS2Art/UI/UIForms/ToastForm.prefab";

		public List<ToastHandler> m_toastList = new List<ToastHandler>();

		public void ShowToast(string str, object obj)
		{
			Transform transform = GameObject.Find("Framework/UI/UI Group - Forth").transform;
			GameObject gameObject = Object.Instantiate(obj as GameObject, transform.transform.position, transform.transform.rotation, transform.transform);
			ToastHandler comp = gameObject.GetComponent<ToastHandler>();
			m_toastList.Insert(0, comp);
			comp.InitToast(str, delegate
			{
				Mod.Resource.UnloadAsset(obj);
				m_toastList.Remove(comp);
			});
			ToastMove(0.2f);
		}

		public void CreatToast(string str)
		{
			Mod.Resource.LoadAsset("Assets/_RS2Art/UI/UIForms/ToastForm.prefab", new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object userData)
			{
				ShowToast(str, asset as GameObject);
			}, delegate(string assetName, string errorMessage, object userData)
			{
				Log.Error(string.Format("Can not load resource from '{0}' with errormessage {1}", assetName, errorMessage));
			}));
		}

		public void ToastMove(float speed)
		{
			for (int i = 0; i < m_toastList.Count; i++)
			{
				m_toastList[i].Move(speed, i + 1);
			}
		}
	}
}
