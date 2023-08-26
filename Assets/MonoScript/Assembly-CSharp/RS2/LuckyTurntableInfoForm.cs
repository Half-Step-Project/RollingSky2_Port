using System;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LuckyTurntableInfoForm : UGUIForm
	{
		[Serializable]
		public class Item
		{
			public Image iconImg;

			public Text nameText;

			public Text probabilityText;

			private int _count;

			private int _weigth;

			private bool _released;

			private object _iconAsset;

			public int Weigth
			{
				get
				{
					return _weigth;
				}
			}

			public void Init(LuckyTurntable_table table)
			{
				int goodsId = table.GoodsId;
				_count = table.GoodsTime;
				if (_count <= 0)
				{
					_count = table.GoodsCount;
				}
				_weigth = table.WeightNess;
				_released = false;
				nameText.text = Mod.Localization.GetInfoById(table.Desc);
				int goodsIconIdByGoodsId = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(table.GoodsId);
				Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(goodsIconIdByGoodsId.ToString()), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
				{
					if (_released)
					{
						Mod.Resource.UnloadAsset(asset);
					}
					else
					{
						_iconAsset = asset;
						iconImg.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					}
				}, delegate(string assetName, string errorMessage, object data2)
				{
					Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
				}));
			}

			public void Release()
			{
				_released = true;
				if (_iconAsset != null)
				{
					Mod.Resource.UnloadAsset(_iconAsset);
				}
			}

			public void SetProbabilityText(int totalWeight)
			{
				probabilityText.text = string.Format("{0:00.00}%", (float)_weigth * 100f / (float)totalWeight);
			}
		}

		public Button closeBtn;

		public Item[] items;

		private int _totalWeight;

		private int _itemNum;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if (items != null)
			{
				for (int i = 0; i < items.Length; i++)
				{
					Item item = items[i];
				}
				int num = 0;
				_itemNum = items.Length;
				IDataTable<LuckyTurntable_table> dataTable = Mod.DataTable.Get<LuckyTurntable_table>();
				if (dataTable.Count < _itemNum)
				{
					_itemNum = dataTable.Count;
				}
				for (int j = 0; j < _itemNum; j++)
				{
					items[j].Init(dataTable[j + 1]);
					num += items[j].Weigth;
				}
				for (int k = 0; k < _itemNum; k++)
				{
					items[k].SetProbabilityText(num);
				}
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClickCloseBtn;
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseBtn));
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < _itemNum; i++)
			{
				items[i].Release();
			}
		}

		private void OnClickCloseBtn(GameObject go)
		{
			Mod.UI.CloseUIForm(UIFormId.LuckyTurntableInfoForm);
		}
	}
}
