using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class GetGoodsProItem : MonoBehaviour
	{
		public GameObject sliderBg;

		public Image icon;

		public Text progress;

		public Image slider;

		public Text totalCountText;

		public Text goodsCountText;

		public GameObject getEffect;

		public Text totalGetText;

		private List<object> loadedAsserts = new List<object>();

		private bool hasReleased;

		private int goodsId = -1;

		public void SetData(GetGoodsProForm.GoodsData goodsData, float increaseTime, bool showStartCount, float delayTime = 0f)
		{
			icon.gameObject.SetActive(false);
			goodsId = goodsData.goodsId;
			if (goodsData.IsPiece())
			{
				int partNum = goodsData.GetPartNum();
				if (increaseTime > 0f)
				{
					SetPieceIncrease(goodsId, goodsData.startCount, goodsData.endCount, partNum, increaseTime, delayTime);
					return;
				}
				if (showStartCount)
				{
					SetPiece(goodsId, goodsData.startCount, partNum);
					return;
				}
				int num = goodsData.endCount / partNum;
				int picecCount = goodsData.endCount - num * partNum;
				SetTotalAndPiece(goodsId, picecCount, partNum, num);
			}
			else
			{
				SetTotal(goodsId, goodsData.DeltaCount());
			}
		}

		public void SetTotalGetCount(GetGoodsProForm.GoodsData goodsData)
		{
			if (!(totalGetText == null))
			{
				if (goodsData.IsPiece())
				{
					totalGetText.gameObject.SetActive(true);
					totalGetText.text = "+" + goodsData.DeltaCount();
				}
				else
				{
					totalGetText.gameObject.SetActive(false);
				}
			}
		}

		private void SetTotal(int goodsId, int count)
		{
			SetIcon(goodsId);
			totalCountText.gameObject.SetActive(false);
			goodsCountText.gameObject.SetActive(true);
			sliderBg.SetActive(false);
			goodsCountText.text = "+" + count;
		}

		private void SetPiece(int goodsId, int count, int maxCount)
		{
			SetIcon(goodsId);
			totalCountText.gameObject.SetActive(false);
			goodsCountText.gameObject.SetActive(false);
			sliderBg.SetActive(true);
			slider.fillAmount = (float)count / (float)maxCount;
			progress.text = string.Format("{0}/{1}", count, maxCount);
		}

		private void SetTotalAndPiece(int goodsId, int picecCount, int maxCount, int totalCount)
		{
			SetIcon(goodsId);
			goodsCountText.gameObject.SetActive(false);
			if (totalCount > 0)
			{
				totalCountText.gameObject.SetActive(true);
				totalCountText.text = "+" + totalCount;
			}
			else
			{
				totalCountText.gameObject.SetActive(false);
			}
			sliderBg.SetActive(true);
			slider.fillAmount = (float)picecCount / (float)maxCount;
			progress.text = string.Format("{0}/{1}", picecCount, maxCount);
		}

		private void SetPieceIncrease(int goodsId, int startCount, int endCount, int maxCount, float time, float delayTime)
		{
			SetIcon(goodsId);
			goodsCountText.gameObject.SetActive(false);
			totalCountText.gameObject.SetActive(false);
			sliderBg.SetActive(true);
			progress.text = string.Format("{0}/{1}", startCount, maxCount);
			slider.fillAmount = (float)startCount / (float)maxCount;
			StartCoroutine(DoIncrease(startCount, endCount, maxCount, time, delayTime));
		}

		private IEnumerator DoIncrease(int startCount, int endCount, int maxCount, float time, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			slider.DOFillAmount((float)endCount / (float)maxCount, time);
			DOTween.To(delegate(float x)
			{
				progress.text = string.Format("{0}/{1}", (int)x, maxCount);
			}, startCount, endCount, time);
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

		public void ShowGetEffect()
		{
			if (!(getEffect == null))
			{
				getEffect.SetActive(false);
				getEffect.SetActive(true);
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

		public void OnClick()
		{
			if (goodsId != -1)
			{
				AssetPromptBoxFormData assetPromptBoxFormData = new AssetPromptBoxFormData();
				assetPromptBoxFormData.m_target = GetComponent<RectTransform>();
				assetPromptBoxFormData.m_goodID = goodsId;
				Mod.UI.OpenUIForm(UIFormId.AssetPromptBoxForm, assetPromptBoxFormData);
			}
		}
	}
}
