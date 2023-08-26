using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

public class LoadMaterialByPath : MonoBehaviour
{
	private AssetLoadCallbacks assetLoadCallBack;

	private List<object> loadedAsserts = new List<object>();

	private bool m_isReleased;

	public Image m_Image;

	private Material m_material;

	public string m_materialPathName;

	private void Awake()
	{
		assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isReleased)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			m_material = asset as Material;
			m_Image.material = m_material;
			loadedAsserts.Add(asset);
		}, delegate(string assetName, string status, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' failed.", assetName));
		});
	}

	private void Start()
	{
		if (m_Image != null && (m_Image.material == null || m_Image.material.shader.name == "UI/Default"))
		{
			Mod.Resource.LoadAsset(m_materialPathName, assetLoadCallBack);
		}
	}

	private void OnDestroy()
	{
		m_material = null;
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}
}
