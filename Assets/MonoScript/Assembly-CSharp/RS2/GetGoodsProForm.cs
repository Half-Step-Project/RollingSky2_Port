using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RS2
{
	public class GetGoodsProForm : UGUIForm
	{
		public class GoodsData
		{
			public int goodsId;

			public int startCount;

			public int endCount;

			private Goods_goodsTable goodsTable;

			private Goods_goodsTable totalGoodsTable;

			public GoodsData(int goodsId, int startCount, int endCount)
			{
				this.goodsId = goodsId;
				this.startCount = startCount;
				this.endCount = endCount;
			}

			public int DeltaCount()
			{
				return endCount - startCount;
			}

			public void CombineWithOther(GoodsData other)
			{
				if (goodsId == other.goodsId)
				{
					endCount += other.DeltaCount();
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
				return GetTotalGoodsTable().PartsNum;
			}

			public int GetTotalCount()
			{
				return endCount / GetPartNum();
			}

			public bool IsTotal()
			{
				if (goodsTable == null)
				{
					goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(goodsId);
				}
				return goodsTable.FullGoodsId == -1;
			}

			public bool IsPiece()
			{
				return !IsTotal();
			}
		}

		public class GetGoodsDataPro
		{
			public List<GoodsData> goodsDatas = new List<GoodsData>();

			public bool showAnotherButton;

			public bool showNoButton;

			public bool showConfirmButton;

			public bool isRare;

			public float noButtonDelayTime;

			public UnityAction anotherButtonCallback;

			public UnityAction openFinishedCallback;

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

		private const float PIECE_INTERVAL = 0.15f;

		private const float PIECE_MOVE_TIME = 0.4f;

		private const float MIDDLE_ITEM_SCALE = 1.5f;

		private const float MOVE_TO_MIDDLE_TIME = 0.5f;

		private const float MOVE_TO_ITEM_TIME = 0.3f;

		private const int MAX_PIECE_COUNT = 5;

		public GetGoodsProGetTotal getTotal;

		public Button anotherButton;

		public GameObject noButton;

		public GameObject confirmButton;

		public GetGoodsProItem moveToMiddle;

		public GetGoodsProItem middleItem;

		public Transform itemRoot;

		private List<GetGoodsProItem> items = new List<GetGoodsProItem>();

		public Transform piecesRoot;

		private List<AutoReleaseImage> pieces = new List<AutoReleaseImage>();

		private GetGoodsDataPro data;

		public SetUIGrey setUIGrey;

		private int gotItemCount;

		private int totalItemCount;

		private bool isOpening;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			for (int i = 0; i < itemRoot.childCount; i++)
			{
				items.Add(itemRoot.GetChild(i).GetComponent<GetGoodsProItem>());
			}
			for (int j = 0; j < piecesRoot.childCount; j++)
			{
				pieces.Add(piecesRoot.GetChild(j).GetComponent<AutoReleaseImage>());
			}
		}

		protected override bool EnableInputAfterOpen()
		{
			return true;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			data = userData as GetGoodsDataPro;
			if (data == null)
			{
				Debug.LogError("GetGoodsProForm OnOpen ");
				return;
			}
			totalItemCount = data.goodsDatas.Count;
			if (totalItemCount == 0)
			{
				Debug.LogWarning("totalItemCount == 0 ");
				Mod.UI.CloseUIForm(UIFormId.GetGoodsProForm);
				return;
			}
			gotItemCount = 0;
			StartCoroutine(Open());
			anotherButton.gameObject.SetActive(false);
			noButton.SetActive(false);
			confirmButton.SetActive(false);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			middleItem.Release();
			moveToMiddle.Release();
			getTotal.Release();
			foreach (GetGoodsProItem item in items)
			{
				item.Release();
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			CancelInvoke();
			StopAllCoroutines();
			Mod.Event.Fire(this, UIFormCloseEvent.Make(UIFormId.GetGoodsProForm));
		}

		private void ShowButtons()
		{
			anotherButton.gameObject.SetActive(data.showAnotherButton);
			if (data.showAnotherButton)
			{
				InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
			}
			SetNoButton();
			confirmButton.SetActive(data.showConfirmButton);
		}

		private void SetNoButton()
		{
			noButton.SetActive(data.showNoButton);
			if (data.showNoButton && data.noButtonDelayTime > 0f)
			{
				noButton.SetActive(false);
				Invoke("ShowNoButton", data.noButtonDelayTime);
			}
		}

		private void ShowNoButton()
		{
			noButton.SetActive(true);
		}

		private IEnumerator Open()
		{
			yield return new WaitForSeconds(0.967f);
			isOpening = true;
			OpenOneItem();
		}

		private void OpenFinished()
		{
			ShowButtons();
			isOpening = false;
			if (data.openFinishedCallback != null)
			{
				data.openFinishedCallback();
			}
		}

		private void OpenOneItem()
		{
			if (gotItemCount == totalItemCount)
			{
				OpenFinished();
				return;
			}
			GoodsData goodsData = data.goodsDatas[gotItemCount];
			moveToMiddle.SetData(goodsData, 0f, true);
			middleItem.gameObject.SetActive(false);
			moveToMiddle.gameObject.SetActive(true);
			moveToMiddle.transform.localPosition = Vector3.zero;
			middleItem.transform.localPosition = Vector3.zero;
			moveToMiddle.transform.DOMove(middleItem.transform.position, 0.5f).OnComplete(delegate
			{
				middleItem.gameObject.SetActive(true);
				middleItem.transform.localScale = Vector3.one * 1.5f;
				moveToMiddle.gameObject.SetActive(false);
				int num = Mathf.Min(goodsData.DeltaCount(), 5);
				middleItem.SetData(goodsData, (float)(num - 1) * 0.15f + 0.4f, true, 0.4f);
				if (goodsData.IsPiece())
				{
					MovePieces(num);
				}
				else
				{
					MoveMiddleItemToPos();
				}
			});
			moveToMiddle.transform.localScale = Vector3.one * 0.3f;
			moveToMiddle.transform.DOScale(2f, 0.3f).OnComplete(delegate
			{
				moveToMiddle.transform.DOScale(1.5f, 0.2f);
			});
			Mod.Sound.PlayUISound(20011);
		}

		private void CheckAds()
		{
			bool flag = MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.ResultView);
			anotherButton.interactable = flag;
			setUIGrey.SetGrey(!flag);
		}

		private void MovePieces(int count)
		{
			for (int i = 0; i < count; i++)
			{
				StartCoroutine(MoveOnePiece(i));
			}
			Invoke("MovePiecesFinished", (float)(count - 1) * 0.15f + 0.8f);
		}

		private IEnumerator MoveOnePiece(int index)
		{
			yield return new WaitForSeconds((float)index * 0.15f);
			AutoReleaseImage piece = pieces[index];
			piece.gameObject.SetActive(true);
			piece.transform.localPosition = Vector3.zero;
			GoodsData goodsData = data.goodsDatas[gotItemCount];
			piece.SetImageByGoodsId(goodsData.goodsId);
			piece.transform.DOMove(middleItem.transform.position, 0.4f).SetEase(Ease.Linear).OnComplete(delegate
			{
				piece.gameObject.SetActive(false);
				middleItem.ShowGetEffect();
				middleItem.icon.transform.DOScale(1.2f, 0.1f).OnComplete(delegate
				{
					middleItem.icon.transform.DOScale(1f, 0.1f);
				});
				Mod.Sound.PlayUISound(20013);
			});
		}

		private void MovePiecesFinished()
		{
			GoodsData goodsData = data.goodsDatas[gotItemCount];
			int totalCount = goodsData.GetTotalCount();
			if (totalCount > 0)
			{
				getTotal.gameObject.SetActive(true);
				Mod.Sound.PlayUISound(10001);
				Mod.Event.Fire(this, UI3DShowOrHideEvent.Make(false));
				getTotal.SetData(goodsData.goodsId, totalCount, delegate
				{
					MoveMiddleItemToPos();
				});
			}
			else
			{
				MoveMiddleItemToPos();
			}
		}

		private void MoveMiddleItemToPos()
		{
			GoodsData goodsData = data.goodsDatas[gotItemCount];
			middleItem.SetData(goodsData, 0f, false);
			middleItem.transform.DOMove(items[gotItemCount].transform.position, 0.3f).OnComplete(delegate
			{
				middleItem.gameObject.SetActive(false);
				items[gotItemCount].gameObject.SetActive(true);
				items[gotItemCount].SetData(goodsData, 0f, false);
				items[gotItemCount].SetTotalGetCount(goodsData);
				gotItemCount++;
				OpenOneItem();
			});
			middleItem.transform.DOScale(1f, 0.3f);
			Mod.Sound.PlayUISound(20012);
		}

		private void SkipToEnd()
		{
			CancelInvoke();
			StopAllCoroutines();
			moveToMiddle.transform.DOKill();
			moveToMiddle.gameObject.SetActive(false);
			middleItem.transform.DOKill();
			middleItem.gameObject.SetActive(false);
			foreach (AutoReleaseImage piece in pieces)
			{
				piece.transform.DOKill();
				piece.gameObject.SetActive(false);
			}
			for (int i = 0; i < totalItemCount; i++)
			{
				GoodsData goodsData = data.goodsDatas[i];
				items[i].gameObject.SetActive(true);
				items[i].SetData(goodsData, 0f, false);
				items[i].SetTotalGetCount(goodsData);
			}
			OpenFinished();
		}

		public void OnClickBack()
		{
			if (isOpening)
			{
				SkipToEnd();
			}
			else
			{
				Mod.UI.CloseUIForm(UIFormId.GetGoodsProForm);
			}
		}

		public void OnClickNo()
		{
			Mod.UI.CloseUIForm(UIFormId.GetGoodsProForm);
		}

		public void OnClickAnother()
		{
			if (data != null && data.anotherButtonCallback != null)
			{
				data.anotherButtonCallback();
			}
		}
	}
}
