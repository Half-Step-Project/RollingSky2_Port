using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ResultMiniBagItem : MonoBehaviour
{
	public Image icon;

	public Text count;

	public Image pieceSlider;

	public Text pieceCountText;

	public Text pieceTotalCountText;

	private bool hasReleased;

	private List<object> loadedAsserts = new List<object>();

	private int goodsId;

	private long goodsCount;

	public void SetData(int goodsId, long goodsCount, long pieceCount, long pieceMaxCount)
	{
		this.goodsId = goodsId;
		this.goodsCount = goodsCount;
		SetIcon(goodsId);
		count.text = goodsCount.ToString();
		pieceCountText.text = string.Format("{0}/{1}", pieceCount, pieceMaxCount);
		pieceSlider.fillAmount = (float)pieceCount / (float)pieceMaxCount;
		pieceTotalCountText.gameObject.SetActive(false);
	}

	public void SetTotal(double total)
	{
		pieceTotalCountText.gameObject.SetActive(true);
		pieceTotalCountText.text = "+" + total;
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

	private void OnGetTotalItem(object sender, EventArgs e)
	{
		UIMoveTargetGetTotalItemEvent uIMoveTargetGetTotalItemEvent = e as UIMoveTargetGetTotalItemEvent;
		if (uIMoveTargetGetTotalItemEvent != null && goodsId == uIMoveTargetGetTotalItemEvent.GoodId)
		{
			goodsCount++;
			count.text = goodsCount.ToString();
		}
	}

	private void OnMoveTargetFinished(object sender, EventArgs e)
	{
		UIMoveTargetFinishedEvent uIMoveTargetFinishedEvent = e as UIMoveTargetFinishedEvent;
		if (uIMoveTargetFinishedEvent != null && goodsId == uIMoveTargetFinishedEvent.GoodId)
		{
			pieceTotalCountText.DOFade(0f, 0.5f);
		}
	}

	private void Awake()
	{
		Mod.Event.Subscribe(EventArgs<UIMoveTargetGetTotalItemEvent>.EventId, OnGetTotalItem);
		Mod.Event.Subscribe(EventArgs<UIMoveTargetFinishedEvent>.EventId, OnMoveTargetFinished);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<UIMoveTargetGetTotalItemEvent>.EventId, OnGetTotalItem);
		Mod.Event.Unsubscribe(EventArgs<UIMoveTargetFinishedEvent>.EventId, OnMoveTargetFinished);
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		hasReleased = true;
	}
}
