using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ShowCharactorForm : UGUIForm
	{
		private GameObject back;

		private GameObject CharactorContainer;

		private AssetLoadCallbacks assetLoadCallBack;

		private GameObject charactor;

		private Camera charactorCamera;

		private CharactorUIData charactorData;

		private List<object> loadedAsserts = new List<object>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			back = dictionary["back"];
			CharactorContainer = dictionary["CharactorContainer"];
			charactorCamera = dictionary["charactorCamera"].GetComponent<Camera>();
			float x = Mod.UI.GetUIGroup("First").transform.localScale.x;
			float num = 1f / x;
			CharactorContainer.transform.localScale = new Vector3(num, num, num);
			float fieldOfView = Mathf.Atan(1f / Mathf.Abs(charactorCamera.transform.localPosition.z)) * 2f * 57.29578f;
			charactorCamera.fieldOfView = fieldOfView;
			assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				charactor = Object.Instantiate((GameObject)asset);
				if (charactor != null)
				{
					charactor.transform.SetParent(CharactorContainer.transform);
				}
				if (charactorData != null)
				{
					charactor.transform.position = charactorData.Pos;
					charactor.transform.rotation = charactorData.Rotation;
					charactor.transform.localScale = charactorData.Scale;
				}
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventListener();
			charactorData = userData as CharactorUIData;
			if (charactorData != null)
			{
				ShowCharactor(charactorData);
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
			charactor = null;
		}

		private void ShowCharactor(CharactorUIData data)
		{
			Mod.Resource.LoadAsset(data.CharactorPath, assetLoadCallBack);
		}

		private void AddEventListener()
		{
		}

		private void RemoveEventListener()
		{
		}
	}
}
