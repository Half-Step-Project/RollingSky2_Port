using Foundation;
using UnityEngine;

namespace RS2
{
	public class ExchangeStoreForm : UGUIForm
	{
		public ExchangeProductList mProductList;

		public GiftPackageController mGiftPackageController;

		public UIPersonalAssetsList mPersonalAssets;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if ((bool)mGiftPackageController)
			{
				mGiftPackageController.Init();
			}
			if ((bool)mProductList)
			{
				mProductList.OnInit();
			}
			if ((bool)mPersonalAssets)
			{
				if (DeviceManager.Instance.IsNeedSpecialAdapte())
				{
					MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(mPersonalAssets.transform as RectTransform);
				}
				mPersonalAssets.OnInit();
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			if ((bool)mGiftPackageController)
			{
				int data = 1;
				mGiftPackageController.OnOpen();
				mGiftPackageController.SetData(data);
			}
			if ((bool)mProductList)
			{
				mProductList.OnOpen();
			}
			if ((bool)mPersonalAssets)
			{
				mPersonalAssets.OnOpen(UIPersonalAssetsList.ParentType.ExchangeStore);
			}
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if ((bool)mProductList)
			{
				mProductList.OnTick(elapseSeconds, realElapseSeconds);
			}
			if ((bool)mPersonalAssets)
			{
				mPersonalAssets.OnUpdate();
			}
		}

		protected override void OnClose(object userData)
		{
			if ((bool)mGiftPackageController)
			{
				mGiftPackageController.OnReset();
			}
			if ((bool)mProductList)
			{
				mProductList.OnClose();
			}
			if ((bool)mPersonalAssets)
			{
				mPersonalAssets.OnClose();
			}
			base.OnClose(userData);
		}

		protected override void OnUnload()
		{
			if ((bool)mProductList)
			{
				mProductList.OnRelease();
			}
			if ((bool)mPersonalAssets)
			{
				mPersonalAssets.OnRelease();
			}
			base.OnUnload();
		}

		public void OnClickClose()
		{
			Mod.UI.CloseUIForm(UIFormId.ExchangeStoreForm);
		}
	}
}
