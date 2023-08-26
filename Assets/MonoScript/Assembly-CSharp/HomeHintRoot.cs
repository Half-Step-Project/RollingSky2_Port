using Foundation;
using RS2;
using UnityEngine;

public class HomeHintRoot : MonoBehaviour
{
	public Transform leftLayoutRoot;

	public Transform rightLayoutRoot;

	public HomeHintLoginAwardItem loginAwardItem;

	public HomeHintGiftItem giftItem;

	public HomeHintRemoveAdItem removeAdItem;

	private void Awake()
	{
		Mod.Event.Subscribe(EventArgs<GetLoginAwardEvent>.EventId, OnGetLoginAward);
		Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, OnBuySuccess);
		Mod.Event.Subscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseCompleteHandler);
		Mod.Event.Subscribe(EventArgs<RemoveAdListCloseEventArgs>.EventId, OnRemoveAdListCloseHandler);
		if (DeviceManager.Instance.IsNeedSpecialAdapte())
		{
			MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(leftLayoutRoot as RectTransform);
			MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(rightLayoutRoot as RectTransform);
		}
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<GetLoginAwardEvent>.EventId, OnGetLoginAward);
		Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, OnBuySuccess);
		Mod.Event.Unsubscribe(EventArgs<UIMod.CloseCompleteEventArgs>.EventId, OnCloseCompleteHandler);
		Mod.Event.Unsubscribe(EventArgs<RemoveAdListCloseEventArgs>.EventId, OnRemoveAdListCloseHandler);
		CancelInvoke();
	}

	private void OnGetLoginAward(object sender, EventArgs e)
	{
		UpdateLoginAward();
	}

	private void OnBuySuccess(object sender, EventArgs e)
	{
		UpdateGift();
		if (!(PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0.0))
		{
			return;
		}
		if ((bool)removeAdItem)
		{
			removeAdItem.gameObject.SetActive(false);
		}
		if (Mod.UI.UIFormIsOpen(UIFormId.MoneyShopForm))
		{
			MoneyShopForm moneyShopForm = Mod.UI.GetUIForm(UIFormId.MoneyShopForm) as MoneyShopForm;
			if (moneyShopForm != null && moneyShopForm.CurrentMoneyType == MoneyShopForm.MoneyType.RemoveAd)
			{
				Mod.UI.CloseUIForm(UIFormId.MoneyShopForm);
			}
		}
	}

	private void Start()
	{
		InitState();
	}

	public void InitState()
	{
		UpdateLoginAward();
		UpdateGift();
		if ((bool)removeAdItem)
		{
			removeAdItem.gameObject.SetActive(false);
		}
		OnCheckRemoveAd();
	}

	private void OnCloseCompleteHandler(object sender, EventArgs e)
	{
		UIMod.CloseCompleteEventArgs closeCompleteEventArgs = e as UIMod.CloseCompleteEventArgs;
		if (closeCompleteEventArgs != null && PlayerDataModule.Instance.PlayerRecordData.m_shopRemoveAdFormOpenCount == 0)
		{
			bool flag = Mod.Procedure.Current is MenuProcedure;
			bool flag2 = closeCompleteEventArgs.AssetName == UIExtension.GetUIFormAssetName(UIFormId.ScreenPluginsForm);
			bool flag3 = closeCompleteEventArgs.AssetName == UIExtension.GetUIFormAssetName(UIFormId.LevelEnterForm);
			if ((flag2 || flag3) && flag)
			{
				OnCheckRemoveAd();
			}
		}
	}

	private void OnRemoveAdListCloseHandler(object sender, EventArgs e)
	{
		if (e is RemoveAdListCloseEventArgs && PlayerDataModule.Instance.PlayerRecordData.m_shopRemoveAdFormOpenCount == 1)
		{
			bool num = PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0.0;
			bool flag = IsHomeForm();
			bool flag2 = IsTipForm();
			if (!num && flag && !flag2 && (bool)removeAdItem)
			{
				removeAdItem.gameObject.SetActive(true);
				InfocUtils.Report_rollingsky2_games_pageshow(11, 11, 1);
			}
		}
	}

	public void OnSelectRefresh(bool isSelect)
	{
		if (isSelect)
		{
			OnCheckRemoveAd();
		}
	}

	private void OnCheckRemoveAd()
	{
		bool flag = PlayerDataModule.Instance.IsInNewPlayerProtectedStage();
		bool flag2 = PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0.0;
		bool flag3 = PlayerDataModule.Instance.PlayerRecordData.m_shopRemoveAdFormOpenCount == 0;
		bool flag4 = PlayerDataModule.Instance.PlayerRecordData.m_ScreenPluginsFormOpenTime == 0;
		bool flag5 = PlayerDataModule.Instance.PlayerRecordData.m_currentShowRemoveAdForScreenCount >= GameCommon.showRemoveAdForScreenCount;
		bool num = IsHomeForm();
		bool flag6 = IsTipForm();
		if (!num)
		{
			return;
		}
		if (!flag && !flag2 && !flag3)
		{
			if ((bool)removeAdItem)
			{
				removeAdItem.gameObject.SetActive(true);
				InfocUtils.Report_rollingsky2_games_pageshow(11, 11, 1);
			}
		}
		else if ((bool)removeAdItem)
		{
			removeAdItem.gameObject.SetActive(false);
		}
		if (!flag && !flag4 && !flag2 && flag3 && !flag6)
		{
			PlayerDataModule.Instance.PlayerRecordData.m_currentShowRemoveAdForScreenCount = 0;
			if (Mod.UI.UIFormIsOpen(UIFormId.MoneyShopForm))
			{
				MoneyShopForm moneyShopForm = Mod.UI.GetUIForm(UIFormId.MoneyShopForm) as MoneyShopForm;
				if (moneyShopForm != null)
				{
					moneyShopForm.Refresh(MoneyShopForm.MoneyType.RemoveAd);
				}
			}
			else
			{
				Singleton<UIPopupManager>.Instance.PopupUI(UIFormId.MoneyShopForm, MoneyShopForm.MoneyType.RemoveAd);
			}
		}
		if (flag || flag4 || !flag5 || flag2 || flag3 || flag6)
		{
			return;
		}
		PlayerDataModule.Instance.PlayerRecordData.m_currentShowRemoveAdForScreenCount = 0;
		if (Mod.UI.UIFormIsOpen(UIFormId.MoneyShopForm))
		{
			MoneyShopForm moneyShopForm2 = Mod.UI.GetUIForm(UIFormId.MoneyShopForm) as MoneyShopForm;
			if (moneyShopForm2 != null)
			{
				moneyShopForm2.Refresh(MoneyShopForm.MoneyType.RemoveAd);
			}
		}
		else
		{
			Singleton<UIPopupManager>.Instance.PopupUI(UIFormId.MoneyShopForm, MoneyShopForm.MoneyType.RemoveAd);
		}
	}

	private bool IsHomeForm()
	{
		bool result = false;
		if (Mod.UI.UIFormIsOpen(UIFormId.MenuViewForm))
		{
			MenuForm menuForm = Mod.UI.GetUIForm(UIFormId.MenuViewForm) as MenuForm;
			if (menuForm != null && menuForm.CurrentOpenFormId == UIFormId.HomeForm)
			{
				result = true;
			}
		}
		return result;
	}

	private bool IsTipForm()
	{
		if (Mod.UI.UIFormIsOpen(UIFormId.LevelEnterForm))
		{
			return true;
		}
		return false;
	}

	private void UpdateLoginAward()
	{
		if (PlayerDataModule.Instance.SequenceLoginData.IsEnd())
		{
			loginAwardItem.gameObject.SetActive(false);
		}
		else
		{
			loginAwardItem.gameObject.SetActive(true);
		}
	}

	private void UpdateGift()
	{
		if (MonoSingleton<GameTools>.Instacne.CanShowGiftIcon())
		{
			giftItem.gameObject.SetActive(true);
		}
		else
		{
			giftItem.gameObject.SetActive(false);
		}
	}
}
