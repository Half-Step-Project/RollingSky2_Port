using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RS2
{
	public class GetGoodsProGetTotal : MonoBehaviour
	{
		public Image icon;

		public Text nameText;

		public Text descText;

		public Text countText;

		private List<object> loadedAsserts = new List<object>();

		private bool hasReleased;

		private UnityAction closeCallback;

		public void SetData(int goodsId, int count, UnityAction closeCallback)
		{
			this.closeCallback = closeCallback;
			nameText.text = MonoSingleton<GameTools>.Instacne.GetGoodsName(goodsId);
			descText.text = MonoSingleton<GameTools>.Instacne.GetGoodsDesc(goodsId);
			int goodsIconIdByGoodsId = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(goodsId);
			if (count > 1)
			{
				countText.gameObject.SetActive(true);
				countText.text = string.Format("+{0}", count);
			}
			else
			{
				countText.gameObject.SetActive(false);
			}
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(goodsIconIdByGoodsId), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
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
					icon.gameObject.SetActive(true);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			}));
			CancelInvoke();
			Invoke("Close", 1.5f);
		}

		public void OnClickConfirm()
		{
			Close();
		}

		private void Close()
		{
			CancelInvoke();
			base.gameObject.SetActive(false);
			Mod.Event.Fire(this, UI3DShowOrHideEvent.Make(true));
			if (closeCallback != null)
			{
				closeCallback();
			}
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
}
