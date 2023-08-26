using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;

public class ResultMiniBag : MonoBehaviour
{
	public class ItemData
	{
		public int goodsId;

		public int goodsCount;

		public int pieceCount;

		public int pieceMaxCount;

		public ItemData(int goodsId, int goodsCount, int pieceCount, int pieceMaxCount)
		{
			this.goodsId = goodsId;
			this.goodsCount = goodsCount;
			this.pieceCount = pieceCount;
			this.pieceMaxCount = pieceMaxCount;
		}
	}

	public class ShowData
	{
		public UnityAction moveFinishedCallback;
	}

	public readonly List<int> ConstGoodsIds = new List<int> { 33, 34, 35 };

	public const float MOVE_TIME = 0.2f;

	private ShowData showData;

	public ResultMiniBagItem itemTemplate;

	public Transform itemRoot;

	public Transform root;

	public Transform movePosStart;

	public Transform movePosEnd;

	private Dictionary<int, ResultMiniBagItem> items = new Dictionary<int, ResultMiniBagItem>();

	private void Awake()
	{
		root.gameObject.SetActive(false);
	}

	public void Init()
	{
		int i = 0;
		for (int childCount = itemRoot.childCount; i < childCount; i++)
		{
			Object.Destroy(itemRoot.GetChild(i).gameObject);
		}
		items.Clear();
		foreach (int constGoodsId in ConstGoodsIds)
		{
			Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(constGoodsId);
			Goods_goodsTable goods_goodsTable2 = Mod.DataTable.Get<Goods_goodsTable>().Get(goods_goodsTable.FullGoodsId);
			int partsNum = goods_goodsTable2.PartsNum;
			int num = (int)PlayerDataModule.Instance.GetPlayGoodsNum(constGoodsId);
			int num2 = (int)PlayerDataModule.Instance.GetPlayGoodsNum(goods_goodsTable2.Id);
			ResultMiniBagItem resultMiniBagItem = Object.Instantiate(itemTemplate);
			resultMiniBagItem.transform.SetParent(itemRoot, false);
			resultMiniBagItem.gameObject.SetActive(true);
			resultMiniBagItem.SetData(goods_goodsTable2.Id, num2, num, partsNum);
			items.Add(constGoodsId, resultMiniBagItem);
		}
	}

	public void ShowAndMove(ShowData showData)
	{
		this.showData = showData;
		root.gameObject.SetActive(true);
		root.localPosition = movePosStart.localPosition;
		root.DOLocalMove(movePosEnd.localPosition, 0.2f).OnComplete(delegate
		{
			if (showData.moveFinishedCallback != null)
			{
				showData.moveFinishedCallback();
			}
		});
	}

	public void Hide(UnityAction hideMoveCallback)
	{
		root.localPosition = movePosEnd.localPosition;
		root.DOLocalMove(movePosStart.localPosition, 0.2f).OnComplete(delegate
		{
			root.gameObject.SetActive(false);
			if (hideMoveCallback != null)
			{
				hideMoveCallback();
			}
		});
	}

	public ResultMiniBagItem GetItemByGoodsId(int goodsId)
	{
		if (items.ContainsKey(goodsId))
		{
			return items[goodsId];
		}
		return null;
	}
}
