using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class AwardItem : MonoBehaviour
{
	public Image icon;

	public Text count;

	private List<object> loadedAsserts = new List<object>();

	public void SetData(string spriteName, int minCount, int maxCount)
	{
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (!(icon == null))
			{
				icon.gameObject.SetActive(true);
				icon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		}));
		if (count != null)
		{
			if (minCount != maxCount)
			{
				count.text = string.Format("{0}~{1}", minCount, maxCount);
			}
			else
			{
				count.text = maxCount.ToString();
			}
		}
	}

	public void UnLoad()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}
}
