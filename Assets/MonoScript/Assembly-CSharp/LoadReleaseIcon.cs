using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class LoadReleaseIcon
{
	private List<object> loadedAsserts = new List<object>();

	private bool hasReleased;

	public void Load(Image icon, int iconId)
	{
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(iconId), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (hasReleased)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else if (icon != null)
			{
				icon.gameObject.SetActive(true);
				icon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		}));
	}

	public void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		hasReleased = true;
	}
}
