using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RS2
{
	public class ResultGetGoods : MonoBehaviour
	{
		public class GoodsData
		{
			public int goodsId;

			public double startCount;

			public double deltaCount;

			public UIMoveTarget moveTarget;

			private Goods_goodsTable goodsTable;

			private Goods_goodsTable totalGoodsTable;

			public GoodsData(int goodsId, double startCount, double deltaCount)
			{
				this.goodsId = goodsId;
				this.startCount = startCount;
				this.deltaCount = deltaCount;
			}

			public double DeltaCount()
			{
				return deltaCount;
			}

			public double EndCount(int multi)
			{
				return startCount + deltaCount * (double)multi;
			}

			public void CombineWithOther(GoodsData other)
			{
				if (goodsId == other.goodsId)
				{
					deltaCount += other.DeltaCount();
				}
			}

			private Goods_goodsTable GetGoodsTable()
			{
				if (goodsTable == null)
				{
					goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(goodsId);
				}
				if (goodsTable == null)
				{
					Debug.LogError("goodsTable == null goodsId " + goodsId);
				}
				return goodsTable;
			}

			private Goods_goodsTable GetTotalGoodsTable()
			{
				if (GetGoodsTable().FullGoodsId == -1)
				{
					return null;
				}
				if (totalGoodsTable == null)
				{
					totalGoodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(GetGoodsTable().FullGoodsId);
				}
				return totalGoodsTable;
			}

			public int GetPartNum()
			{
				if (IsTotal())
				{
					return -1;
				}
				return GetTotalGoodsTable().PartsNum;
			}

			public bool IsTotal()
			{
				if (goodsTable == null)
				{
					goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(goodsId);
				}
				return goodsTable.FullGoodsId == -1;
			}
		}

		public class ShowData
		{
			public List<GoodsData> goodsDatas = new List<GoodsData>();

			public GoodsData goldData;

			public UnityAction openFinishedCallback;

			public UnityAction showItemsFinishedCallback;

			public UnityAction firstShowItemMoveFinishedCallback;

			private GoodsData GetGoodsData(int goodsId)
			{
				foreach (GoodsData goodsData in goodsDatas)
				{
					if (goodsData.goodsId == goodsId)
					{
						return goodsData;
					}
				}
				return null;
			}

			public void AddGoodsData(GoodsData newGoodsData)
			{
				GoodsData goodsData = GetGoodsData(newGoodsData.goodsId);
				if (goodsData != null)
				{
					goodsData.CombineWithOther(newGoodsData);
				}
				else
				{
					goodsDatas.Add(newGoodsData);
				}
			}
		}

		private const float GET_ITEM_MOVE_TIME = 0.3f;

		private const float GET_ITEM_INTERVAL = 0.1f;

		private const float SHOW_ITEM_MOVE_TIME = 0.25f;

		private const float SHOW_ITEM_INTERVAL = 0.15f;

		public ResultGetGoodsItem goldItem;

		public Transform itemRoot;

		private List<ResultGetGoodsItem> items = new List<ResultGetGoodsItem>();

		public Transform showItemRoot;

		private List<ResultGetGoodsItem> showItems = new List<ResultGetGoodsItem>();

		public Transform getItemRoot;

		private List<ResultGetGoodsItem> getItems = new List<ResultGetGoodsItem>();

		public GameObject root;

		private ShowData data;

		private int totalItemCount;

		private bool isOpening;

		private UnityAction increaseFinishedCallback;

		private int multi;

		private bool increasing;

		public GridLayoutGroup showItemsLayout;

		public void Init()
		{
			for (int i = 0; i < itemRoot.childCount; i++)
			{
				ResultGetGoodsItem component = itemRoot.GetChild(i).GetComponent<ResultGetGoodsItem>();
				component.Init();
				items.Add(component);
			}
			for (int j = 0; j < showItemRoot.childCount; j++)
			{
				ResultGetGoodsItem component2 = showItemRoot.GetChild(j).GetComponent<ResultGetGoodsItem>();
				showItems.Add(component2);
			}
			for (int k = 0; k < getItemRoot.childCount; k++)
			{
				ResultGetGoodsItem component3 = getItemRoot.GetChild(k).GetComponent<ResultGetGoodsItem>();
				getItems.Add(component3);
			}
		}

		public void MoveToBag(int multi, UnityAction moveFinishedCallback)
		{
			int finishedCount = 0;
			int startCount = 0;
			root.SetActive(false);
			for (int i = 0; i < totalItemCount; i++)
			{
				if (items[i].StartMove(multi, delegate
				{
					finishedCount++;
					if (finishedCount == startCount && moveFinishedCallback != null)
					{
						moveFinishedCallback();
					}
				}))
				{
					startCount++;
				}
			}
		}

		public void StartIncreaseMulti(int multi, UnityAction increaseFinishedCallback)
		{
			increasing = true;
			this.increaseFinishedCallback = increaseFinishedCallback;
			this.multi = multi;
			for (int i = 0; i < totalItemCount; i++)
			{
				items[i].StartIncreaseMulti(multi);
			}
			StartCoroutine(IncreaseFinished());
			Mod.Sound.PlayUISound(20019);
		}

		private void SkipIncreaseMulti()
		{
			StopAllCoroutines();
			for (int i = 0; i < totalItemCount; i++)
			{
				items[i].SkipIncreaseMulti(multi);
			}
			if (increaseFinishedCallback != null)
			{
				increaseFinishedCallback();
			}
			increasing = false;
		}

		private IEnumerator IncreaseFinished()
		{
			yield return new WaitForSeconds(1f);
			if (increaseFinishedCallback != null)
			{
				increaseFinishedCallback();
			}
		}

		public void Show(object userData)
		{
			base.gameObject.SetActive(true);
			data = userData as ShowData;
			if (data == null)
			{
				Debug.LogError("ResultGetGoods OnOpen data == null");
				return;
			}
			totalItemCount = data.goodsDatas.Count;
			if (data.goldData != null)
			{
				totalItemCount++;
			}
			totalItemCount = Mathf.Min(itemRoot.childCount, totalItemCount);
			for (int i = 0; i < totalItemCount; i++)
			{
				items[i].gameObject.SetActive(true);
			}
			for (int j = 0; j < totalItemCount - 1; j++)
			{
				showItems[j].gameObject.SetActive(true);
			}
			isOpening = true;
			MoveGetItemsToShow();
		}

		private void OpenFinished()
		{
			isOpening = false;
			if (data.openFinishedCallback != null)
			{
				data.openFinishedCallback();
			}
		}

		private void MoveGetItemsToShow()
		{
			for (int i = 0; i < totalItemCount - 1; i++)
			{
				StartCoroutine(MoveOneGetItemToShow(i));
			}
			float time = (float)(totalItemCount - 1) * 0.1f + 0.3f;
			StartCoroutine(MoveGetItemsFinished(time));
		}

		private IEnumerator MoveOneGetItemToShow(int index)
		{
			yield return new WaitForSeconds(0.1f * (float)index);
			ResultGetGoodsItem getItem = getItems[index];
			getItem.gameObject.SetActive(true);
			getItem.transform.localPosition = Vector3.zero;
			getItem.transform.localScale = Vector3.one * 0.2f;
			getItem.transform.DOScale(1f, 0.3f);
			GoodsData goodsData = data.goodsDatas[index];
			getItem.SetData(goodsData);
			getItem.transform.DOMove(showItems[index].transform.position, 0.3f).OnComplete(delegate
			{
				getItem.gameObject.SetActive(false);
				showItems[index].SetData(goodsData);
			});
			Mod.Sound.PlayUISound(20011);
		}

		private IEnumerator MoveGetItemsFinished(float time)
		{
			yield return new WaitForSeconds(time);
			ShineShowItems();
		}

		private void MoveShowItemsToPos()
		{
			MoveGoldItemToPos();
			showItemsLayout.enabled = false;
			for (int i = 0; i < totalItemCount - 1; i++)
			{
				StartCoroutine(MoveOneShowItemToPos(i));
			}
			float time = (float)(totalItemCount - 1) * 0.15f + 0.25f;
			StartCoroutine(MoveShowItemsFinished(time));
		}

		private IEnumerator MoveOneShowItemToPos(int index)
		{
			yield return new WaitForSeconds(0.15f * (float)index);
			ResultGetGoodsItem showItem = showItems[index];
			showItem.gameObject.SetActive(true);
			showItem.icon.transform.DOScale(1f, 0.25f);
			GoodsData goodsData = data.goodsDatas[index];
			showItem.transform.DOMove(items[index + 1].transform.position, 0.25f).OnComplete(delegate
			{
				showItem.gameObject.SetActive(false);
				items[index + 1].SetData(goodsData);
				if (index == 0 && data.firstShowItemMoveFinishedCallback != null)
				{
					data.firstShowItemMoveFinishedCallback();
				}
				Mod.Sound.PlayUISound(20012);
			});
		}

		private IEnumerator MoveShowItemsFinished(float time)
		{
			yield return new WaitForSeconds(time);
			OpenFinished();
		}

		private void MoveGoldItemToPos()
		{
			goldItem.gameObject.SetActive(true);
			goldItem.transform.localPosition = Vector3.zero;
			goldItem.SetData(data.goldData);
			goldItem.transform.DOMove(items[0].transform.position, 0.25f).OnComplete(delegate
			{
				goldItem.gameObject.SetActive(false);
				items[0].SetData(data.goldData);
			});
		}

		private void ShineShowItems()
		{
			for (int i = 0; i < totalItemCount - 1; i++)
			{
				showItems[i].icon.GetComponent<Animation>().Play();
			}
			StartCoroutine(ShineShowItemsFinished());
		}

		private IEnumerator ShineShowItemsFinished()
		{
			yield return new WaitForSeconds(1f);
			MoveShowItemsToPos();
			if (data.showItemsFinishedCallback != null)
			{
				data.showItemsFinishedCallback();
			}
		}

		private void SkipToEnd()
		{
			if (isOpening)
			{
				CancelInvoke();
				StopAllCoroutines();
				items[0].SetData(data.goldData);
				for (int i = 1; i < totalItemCount; i++)
				{
					GoodsData goodsData = data.goodsDatas[i - 1];
					items[i].SetData(goodsData);
				}
				OpenFinished();
			}
		}

		public void OnClickBack()
		{
			if (isOpening)
			{
				SkipToEnd();
			}
			if (increasing)
			{
				SkipIncreaseMulti();
			}
		}

		private void OnDestroy()
		{
			CancelInvoke();
			StopAllCoroutines();
		}
	}
}
