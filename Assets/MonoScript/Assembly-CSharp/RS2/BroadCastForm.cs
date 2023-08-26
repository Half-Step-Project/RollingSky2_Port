using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class BroadCastForm : UGUIForm
	{
		public GameObject broadCastGoodsItem;

		public GameObject broadCastInfoItem;

		public Text broadCastGoodsTxt;

		public Text broadCastInfoTxt;

		public Image itemIcon;

		private bool isBroadCasting;

		private bool m_releases;

		private List<object> loadedAsserts = new List<object>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Init();
			m_releases = false;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			MonoSingleton<BroadCastManager>.Instacne.IsInited = false;
			MonoSingleton<BroadCastManager>.Instacne.Reset();
		}

		private void Init()
		{
			itemIcon.gameObject.SetActive(false);
			broadCastGoodsItem.SetActive(false);
			broadCastInfoItem.SetActive(false);
		}

		public void BroadCast()
		{
			if (isBroadCasting)
			{
				return;
			}
			if (MonoSingleton<BroadCastManager>.Instacne.BroadCastList.Count > 0)
			{
				BroadCastData castData = MonoSingleton<BroadCastManager>.Instacne.BroadCastList.Dequeue();
				switch (castData.Type)
				{
				case BroadCastType.GOODS:
				{
					broadCastGoodsItem.SetActive(true);
					int goodsIconId = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(castData.GoodId);
					Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(goodsIconId.ToString()), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
					{
						if (m_releases)
						{
							Mod.Resource.UnloadAsset(asset);
						}
						else if (asset != null)
						{
							loadedAsserts.Add(asset);
							if (itemIcon != null)
							{
								itemIcon.gameObject.SetActive(true);
								itemIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
								itemIcon.SetNativeSize();
							}
							if (broadCastGoodsTxt != null)
							{
								broadCastGoodsTxt.text = castData.Info;
							}
							TweenEffect(broadCastGoodsItem);
						}
					}, delegate(string assetName, string errorMessage, object data2)
					{
						Log.Error(string.Format("Can not load Icon '{0}' from '{1}' with error message '{2}'.", goodsIconId.ToString(), assetName, errorMessage));
					}));
					isBroadCasting = true;
					break;
				}
				case BroadCastType.INFO:
					broadCastInfoTxt.text = castData.Info;
					broadCastInfoItem.SetActive(true);
					TweenEffect(broadCastInfoItem);
					isBroadCasting = true;
					break;
				}
			}
			else
			{
				Mod.UI.CloseUIForm(UIFormId.BroadCastForm);
			}
		}

		private void TweenEffect(GameObject obj)
		{
			DOTweenAnimation[] components = obj.gameObject.GetComponents<DOTweenAnimation>();
			for (int i = 0; i < components.Length; i++)
			{
				if (i == 0)
				{
					List<Tween> tweens = components[i].GetTweens();
					for (int j = 0; j < tweens.Count; j++)
					{
						if (j == 0)
						{
							tweens[j].OnComplete(delegate
							{
								obj.transform.localPosition = Vector3.zero;
								obj.SetActive(false);
								isBroadCasting = false;
								BroadCast();
							});
						}
					}
				}
				components[i].DORestart();
				components[i].DOPlayForward();
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			m_releases = true;
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}
	}
}
