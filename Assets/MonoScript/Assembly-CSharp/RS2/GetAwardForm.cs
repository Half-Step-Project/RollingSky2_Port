using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RS2
{
	public class GetAwardForm : UGUIForm
	{
		public static GetAwardForm Form;

		public int goldMoveTargetId;

		public int noteMoveTargetId;

		public int diamoundMoveTargetId;

		private UIMoveTarget goldMoveTarget;

		private UIMoveTarget noteMoveTarget;

		private UIMoveTarget diamoundMoveTarget;

		private List<ResultMoveItem> moveItems = new List<ResultMoveItem>();

		private void Awake()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				ResultMoveItem component = base.transform.GetChild(i).GetComponent<ResultMoveItem>();
				component.Init();
				moveItems.Add(component);
			}
			GetMoveTarget();
		}

		private void GetMoveTarget()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("ItemMoveTarget");
			for (int i = 0; i < array.Length; i++)
			{
				UIMoveTarget component = array[i].GetComponent<UIMoveTarget>();
				if (!(component == null))
				{
					if (component.id == goldMoveTargetId)
					{
						goldMoveTarget = component;
					}
					else if (component.id == noteMoveTargetId)
					{
						noteMoveTarget = component;
					}
					else if (component.id == diamoundMoveTargetId)
					{
						diamoundMoveTarget = component;
					}
				}
			}
		}

		private ResultMoveItem GetMoveItem()
		{
			foreach (ResultMoveItem moveItem in moveItems)
			{
				if (!moveItem.IsMoving)
				{
					return moveItem;
				}
			}
			return null;
		}

		public void StartMove(RectTransform item, int goodsId, int count, UnityAction moveFinishedCallback = null, float randomStartPosRange = 150f)
		{
			UIMoveTarget uIMoveTarget = null;
			string animTrigger = "";
			if (goodsId == 3)
			{
				animTrigger = "gold";
				uIMoveTarget = goldMoveTarget;
			}
			else if (goodsId == GameCommon.REPUTATION_ID)
			{
				animTrigger = "note";
				uIMoveTarget = noteMoveTarget;
			}
			else if (goodsId == 6)
			{
				animTrigger = "diamound";
				uIMoveTarget = diamoundMoveTarget;
			}
			if ((bool)uIMoveTarget)
			{
				double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(goodsId);
				uIMoveTarget.SetData(playGoodsNum, playGoodsNum + (double)count, -1);
				StartMove(item.transform.position, item.sizeDelta, animTrigger, goodsId, uIMoveTarget, count, moveFinishedCallback, randomStartPosRange);
			}
		}

		public void StartMove(RectTransform item, int goodsId, UIMoveTarget moveTarget, int count, UnityAction moveFinishedCallback = null, float randomStartPosRange = 150f)
		{
			string animTrigger = "";
			if (goodsId == 3)
			{
				animTrigger = "gold";
			}
			else if (goodsId == GameCommon.REPUTATION_ID)
			{
				animTrigger = "note";
			}
			else if (goodsId == 6)
			{
				animTrigger = "diamound";
			}
			if ((bool)moveTarget)
			{
				StartMove(item.transform.position, item.sizeDelta, animTrigger, goodsId, moveTarget, count, moveFinishedCallback, randomStartPosRange);
			}
		}

		public float StartMove(Vector3 pos, Vector2 size, string animTrigger, int goodsId, UIMoveTarget moveTarget, int count, UnityAction moveFinishedCallback = null, float randomStartPosRange = 150f)
		{
			ResultMoveItem moveItem = GetMoveItem();
			if (moveItem == null)
			{
				return 0f;
			}
			if (pos != Vector3.zero)
			{
				moveItem.transform.position = pos;
			}
			return moveItem.StartMove(goodsId, moveTarget, count, size, animTrigger, moveFinishedCallback, randomStartPosRange);
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			Form = this;
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
		}
	}
}
