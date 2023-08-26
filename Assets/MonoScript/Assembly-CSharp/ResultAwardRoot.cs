using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResultAwardRoot : MonoBehaviour
{
	public class GoodsData
	{
		public int goodsId;

		public int goodsCount;

		public GoodsData(int goodsId, int goodsCount)
		{
			this.goodsId = goodsId;
			this.goodsCount = goodsCount;
		}
	}

	public class ShowData
	{
		public UnityAction closeCallback;

		public List<GoodsData> goodsDatas = new List<GoodsData>();

		public GoodsData goldData;
	}

	private ShowData showData;

	public ResultAwardItem itemTemplate;

	public Transform itemRoot;

	public GameObject noAdBtn;

	public void Show(ShowData showData)
	{
		this.showData = showData;
		base.gameObject.SetActive(true);
		noAdBtn.SetActive(false);
		Invoke("ShowNoAdButton", 1.5f);
		if (showData.goldData != null)
		{
			itemTemplate.gameObject.SetActive(true);
			itemTemplate.SetData(showData.goldData.goodsId, showData.goldData.goodsCount);
		}
		int i = 0;
		for (int childCount = itemRoot.childCount; i < childCount; i++)
		{
			Object.Destroy(itemRoot.GetChild(i).gameObject);
		}
		foreach (GoodsData goodsData in showData.goodsDatas)
		{
			ResultAwardItem resultAwardItem = Object.Instantiate(itemTemplate);
			resultAwardItem.transform.SetParent(itemRoot, false);
			resultAwardItem.gameObject.SetActive(true);
			resultAwardItem.SetData(goodsData.goodsId, goodsData.goodsCount);
		}
	}

	private void ShowNoAdButton()
	{
		noAdBtn.SetActive(true);
	}

	public void Hide()
	{
		CancelInvoke();
		base.gameObject.SetActive(false);
		if (showData.closeCallback != null)
		{
			showData.closeCallback();
		}
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}
}
