using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class SetUIGrey : MonoBehaviour
{
	public List<GameObject> greyObjs = new List<GameObject>();

	public List<GameObject> commonObjs = new List<GameObject>();

	public Image[] targets;

	public Button adButton;

	public Material greyMat;

	public Material originalMat;

	private AssetLoadCallbacks assetLoadCallBack;

	private List<object> loadedAsserts = new List<object>();

	private bool m_isReleased;

	private int mLastState = -1;

	private void Awake()
	{
		assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isReleased)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			greyMat = asset as Material;
			loadedAsserts.Add(asset);
			if (greyMat != null)
			{
				RealSetGrey(true);
			}
		}, delegate(string assetName, string status, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' failed.", assetName));
		});
	}

	private void RealSetGrey(bool isGrey)
	{
		foreach (GameObject greyObj in greyObjs)
		{
			greyObj.SetActive(isGrey);
		}
		foreach (GameObject commonObj in commonObjs)
		{
			commonObj.SetActive(!isGrey);
		}
		Image[] array = targets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = (isGrey ? greyMat : originalMat);
		}
		if ((bool)adButton)
		{
			adButton.interactable = !isGrey;
		}
	}

	public void SetGrey(bool isGrey)
	{
		int num = (isGrey ? 1 : 0);
		if (mLastState != num)
		{
			m_isReleased = false;
			if (isGrey && greyMat == null)
			{
				string assetName = "CommonGrey";
				Mod.Resource.LoadAsset(AssetUtility.GetUIMaterialAsset(assetName), assetLoadCallBack);
			}
			else
			{
				RealSetGrey(isGrey);
			}
			mLastState = num;
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		m_isReleased = true;
		mLastState = -1;
	}
}
