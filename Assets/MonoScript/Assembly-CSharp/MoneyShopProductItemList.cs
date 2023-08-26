using Foundation;
using RS2;
using UnityEngine;

public class MoneyShopProductItemList : MonoBehaviour
{
	public UILoopList m_uiLoopList;

	public UIPersonalAssetsList mPersonalAssets;

	private const int LeaveMoneyShopFormPluginAdId = 10;

	private static int m_leaveMoneyShowFormTotalTime;

	private ShopResponseData m_data;

	public void OnInit(ShopResponseData data)
	{
		m_data = data;
		m_uiLoopList.Data(data.shopItemList);
		if ((bool)mPersonalAssets)
		{
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(mPersonalAssets.transform as RectTransform);
			}
			mPersonalAssets.OnInit();
		}
	}

	public void OnOpen(ShopResponseData data)
	{
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnOpen(UIPersonalAssetsList.ParentType.Shop);
		}
	}

	public void OnClose()
	{
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnClose();
		}
		if (m_data != null && m_data.shopItemList.Count > 0 && base.gameObject.activeSelf)
		{
			int buyType = m_data.shopItemList[1].buyType;
			int num = 1;
		}
	}

	public void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnUpdate();
		}
	}

	public void OnRelease()
	{
		UILoopItem[] componentsInChildren = m_uiLoopList.gameObject.GetComponentsInChildren<UILoopItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				componentsInChildren[i].OnRelease();
			}
		}
		if ((bool)mPersonalAssets)
		{
			mPersonalAssets.OnRelease();
		}
	}

	private bool DealMoneyShopPluginAd()
	{
		m_leaveMoneyShowFormTotalTime++;
		int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(10);
		ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
		bool flag = false;
		if (screenPluginsAd_table != null)
		{
			flag |= m_leaveMoneyShowFormTotalTime >= screenPluginsAd_table.TriggerNum;
			flag &= !PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.Contains(UIFormId.MoneyShopForm);
			if (flag)
			{
				flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
				if (flag)
				{
					PluginAdData pluginAdData = new PluginAdData();
					pluginAdData.PluginId = 10;
					pluginAdData.EndHandler = delegate
					{
						ClearMoneyShopPluginAdData();
						ClearMoneyShopPluginAdCountData();
					};
					Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
				}
			}
		}
		ClearMoneyShopPluginAdData();
		return flag;
	}

	private void ClearMoneyShopPluginAdData()
	{
		PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.RemoveAll((UIFormId x) => x == UIFormId.MoneyShopForm);
	}

	private void ClearMoneyShopPluginAdCountData()
	{
		m_leaveMoneyShowFormTotalTime = 0;
	}
}
