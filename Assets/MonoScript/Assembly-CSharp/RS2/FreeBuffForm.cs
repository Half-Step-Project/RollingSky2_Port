using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class FreeBuffForm : UGUIForm
	{
		public class ShowData
		{
			public bool directActive;

			public ShowData(bool directActive)
			{
				this.directActive = directActive;
			}
		}

		private const float DelayShowCloseButton = 1.5f;

		public GameObject shieldIcon;

		public GameObject freeRebirthIcon;

		public GameObject guideLineIcon;

		public Transform addRoot;

		public GameObject closeButton;

		public Button aDsButton;

		public GameObject confirmButton;

		public GameObject adMessage;

		private ShowData showData;

		public static bool CanShow()
		{
			bool num = PlayerDataModule.Instance.BufferIsEnable(GameCommon.START_FREE_SHIELD);
			bool flag = PlayerDataModule.Instance.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
			bool flag2 = PlayerDataModule.Instance.BufferIsEnable(GameCommon.GuideLine);
			if (num && flag)
			{
				return !flag2;
			}
			return true;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			showData = userData as ShowData;
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).ShowFreeBuffCount = 0;
			bool flag = PlayerDataModule.Instance.PlayerCommonData.CanGetFreeGoodsNoAd();
			PlayerDataModule.Instance.PlayerCommonData.CountFreeGoodsTimes();
			bool flag2 = PlayerDataModule.Instance.BufferIsEnable(GameCommon.START_FREE_SHIELD);
			bool flag3 = PlayerDataModule.Instance.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE);
			bool flag4 = PlayerDataModule.Instance.BufferIsEnable(GameCommon.GuideLine);
			shieldIcon.SetActive(!flag2);
			freeRebirthIcon.SetActive(!flag3);
			guideLineIcon.SetActive(!flag4);
			int num = -1;
			if (!flag2)
			{
				num++;
			}
			if (!flag3)
			{
				num++;
			}
			if (!flag4)
			{
				num++;
			}
			for (int i = 0; i < addRoot.childCount; i++)
			{
				GameObject gameObject = addRoot.GetChild(i).gameObject;
				if (i < num)
				{
					gameObject.SetActive(true);
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			closeButton.SetActive(false);
			if (!flag)
			{
				Invoke("ShowCloseButton", 1.5f);
				aDsButton.gameObject.SetActive(true);
				confirmButton.SetActive(false);
				adMessage.SetActive(true);
			}
			else
			{
				aDsButton.gameObject.SetActive(false);
				confirmButton.SetActive(true);
				adMessage.SetActive(false);
				GetItems();
			}
		}

		private void ShowCloseButton()
		{
			closeButton.SetActive(true);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			CancelInvoke();
		}

		public void OnClickNoAdsButton()
		{
			Mod.UI.CloseUIForm(UIFormId.FreeBuffForm);
		}

		public void OnClickAdsButton()
		{
			DoClickAdsButton();
			InfocUtils.Report_rollingsky2_games_ads(11, 0, 1, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId, 3, 0);
		}

		private void DoClickAdsButton()
		{
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.ResultView, delegate(ADScene adScen)
			{
				OnAdSuccess(adScen);
				MonoSingleton<GameTools>.Instacne.EnableInput();
			});
		}

		private void GetItems()
		{
			PlayerDataModule.Instance.GetTempGoods(showData.directActive);
		}

		private void OnAdSuccess(ADScene adScen = ADScene.NONE)
		{
			GetItems();
			Mod.UI.CloseUIForm(UIFormId.FreeBuffForm);
			InfocUtils.Report_rollingsky2_games_ads(11, 0, 1, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId, 4, 0);
		}
	}
}
