using Foundation;
using UnityEngine.UI;

namespace RS2
{
	public class BuyOutRebirthForm : UGUIForm
	{
		public const int GOODSTEAMBASEID = 50000;

		public const int SHOPID = 50001;

		public Text hasCount;

		public Text moneyCount;

		public Text itemCount;

		private const int SHOP_ITEM_ID = 1101;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BuyOutRebirthShowEventArgs>().Initialize(true));
			Refresh();
			InfocUtils.Report_rollingsky2_games_pageshow(9, 0, 1);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BuyOutRebirthShowEventArgs>().Initialize(false));
			Mod.UI.CloseUIForm(UIFormId.ShopForm);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
		}

		private void OnPlayerAssetChange(object sender, EventArgs e)
		{
			if ((e as GameGoodsNumChangeEventArgs).GoodsId == 11)
			{
				int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
				InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
				InfocUtils.Report_rollingsky2_games_currency(11, GetNeedCount(), 10, curLevelId, dataModule.BuyOutRebirthIndex);
				UseBtnClick();
			}
		}

		public void CloseBtnClickHandler()
		{
			Mod.UI.CloseUIForm(UIFormId.BuyOutRebirthForm);
		}

		public void OnClickBuy()
		{
			MonoSingleton<GameTools>.Instacne.CommonBuyOperate(50001);
		}

		public void UseBtnClickHandler()
		{
			InfocUtils.Report_rollingsky2_games_pageshow(9, 8, 2);
			UseBtnClick();
		}

		private void UseBtnClick()
		{
			int num = GetHasCount();
			int needCount = GetNeedCount();
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			if (num >= needCount)
			{
				PlayerDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
				dataModule2.ChangePlayerGoodsNum(11, -needCount, AssertChangeType.LEVEL_CONSUME, NetWorkSynType.NORMAL, false);
				dataModule2.SetLevelBuyOutRebirth(curLevelId, 1);
				Mod.Event.FireNow(this, Mod.Reference.Acquire<BuyOutRebirthUnLockEventArgs>().Initialize(curLevelId));
				Mod.UI.CloseUIForm(UIFormId.BuyOutRebirthForm);
				BroadCastData broadCastData = new BroadCastData();
				broadCastData.Type = BroadCastType.INFO;
				broadCastData.Info = Mod.Localization.GetInfoById(114).Replace("\\n", "\n");
				MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
				Mod.UI.OpenUIForm(UIFormId.BuyOutRebirthIntroductionForm);
				InfocUtils.Report_rollingsky2_games_currency(11, needCount, 11, curLevelId, dataModule.BuyOutRebirthIndex);
				return;
			}
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.BUY_SHOPITEM;
			commonAlertData.shopItemId = 1101;
			commonAlertData.lableContent = Mod.Localization.GetInfoById(21);
			commonAlertData.callBackFunc = delegate
			{
				double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
				needCount = int.Parse(Mod.DataTable.Get<Shops_shopTable>().Get(1101).Price);
				if (playGoodsNum >= (double)needCount)
				{
					PlayerDataModule.Instance.ChangePlayerGoodsNum(6, -needCount);
					PlayerDataModule.Instance.ChangePlayerGoodsNum(11, 1.0);
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				}
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}

		private void Refresh()
		{
			int num = GetHasCount();
			int needCount = GetNeedCount();
			hasCount.text = num.ToString();
			itemCount.text = needCount.ToString();
			Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(50001);
			moneyCount.text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(shops_shopTable.Id);
		}

		private int GetHasCount()
		{
			return (int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(11);
		}

		private int GetNeedCount()
		{
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			return Mod.DataTable.Get<Levels_levelTable>().Get(curLevelId).BuyOutRebirthCost;
		}
	}
}
