using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RS2
{
	public class ResultGetGoodsItem : MonoBehaviour
	{
		public const float INCREASE_TIME = 1f;

		public GameObject root;

		public Image icon;

		public Text goodsCountText;

		public Text nameText;

		public GameObject getEffect;

		public ResultMoveItem moveItem;

		private List<object> loadedAsserts = new List<object>();

		private bool hasReleased;

		private ResultGetGoods.GoodsData goodsData;

		private Tweener increaseTweener;

		public bool StartMove(int multi, UnityAction finishedCallback = null)
		{
			if (goodsData.moveTarget == null)
			{
				return false;
			}
			int goodsId = 0;
			Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(goodsData.goodsId);
			if (goods_goodsTable != null)
			{
				goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(goods_goodsTable.FullGoodsId);
			}
			if (goods_goodsTable != null)
			{
				goodsId = goods_goodsTable.Id;
			}
			goodsData.moveTarget.SetData(goodsData.startCount, goodsData.EndCount(multi), goodsData.GetPartNum(), icon.sprite, goodsId);
			ResultMiniBagItem component = goodsData.moveTarget.GetComponent<ResultMiniBagItem>();
			if ((bool)component)
			{
				component.SetTotal(goodsData.DeltaCount() * (double)multi);
			}
			int count = (int)goodsData.DeltaCount() * multi;
			if (goodsData.goodsId == 3)
			{
				count = 20;
			}
			if (goodsData.goodsId == 6 || goodsData.goodsId == 3)
			{
				moveItem.transform.position = icon.transform.position;
				moveItem.StartMove(goodsData.goodsId, goodsData.moveTarget, count, Vector2.zero, "gold", finishedCallback);
			}
			else
			{
				float time = 1.35f;
				goodsData.moveTarget.StartIncrease(time, finishedCallback);
			}
			return true;
		}

		private double IncreaseMultiGetter()
		{
			return goodsData.DeltaCount();
		}

		private void IncreaseMultiSetter(double x)
		{
			goodsCountText.text = "X" + MonoSingleton<GameTools>.Instacne.DoubleToFormatString(x);
		}

		public void StartIncreaseMulti(int multi)
		{
			increaseTweener = DOTween.To(IncreaseMultiGetter, IncreaseMultiSetter, goodsData.DeltaCount() * (double)multi, 1f).OnComplete(delegate
			{
			});
		}

		public void SkipIncreaseMulti(int multi)
		{
			if (increaseTweener != null)
			{
				increaseTweener.Kill();
			}
			goodsCountText.text = "X" + MonoSingleton<GameTools>.Instacne.DoubleToFormatString(goodsData.DeltaCount() * (double)multi);
		}

		public void Init()
		{
			if ((bool)moveItem)
			{
				moveItem.Init();
			}
		}

		public void SetData(ResultGetGoods.GoodsData goodsData)
		{
			if (root != null)
			{
				root.SetActive(true);
			}
			this.goodsData = goodsData;
			SetIcon(goodsData.goodsId);
			goodsCountText.text = "X" + MonoSingleton<GameTools>.Instacne.DoubleToFormatString(goodsData.DeltaCount());
			if (nameText != null)
			{
				nameText.text = MonoSingleton<GameTools>.Instacne.GetGoodsName(goodsData.goodsId);
			}
		}

		private void SetIcon(int goodsId)
		{
			icon.gameObject.SetActive(false);
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(goodsId)), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
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

		public void OnClick()
		{
			if (goodsData.goodsId != -1)
			{
				AssetPromptBoxFormData assetPromptBoxFormData = new AssetPromptBoxFormData();
				assetPromptBoxFormData.m_target = GetComponent<RectTransform>();
				assetPromptBoxFormData.m_goodID = goodsData.goodsId;
				Mod.UI.OpenUIForm(UIFormId.AssetPromptBoxForm, assetPromptBoxFormData);
			}
		}

		private void OnDestroy()
		{
			Release();
		}
	}
}
