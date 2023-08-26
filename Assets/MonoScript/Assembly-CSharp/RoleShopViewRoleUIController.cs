using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class RoleShopViewRoleUIController : MonoBehaviour, IRoleShopChildViewUIController
{
	private GameObject roleItem;

	public GameObject buyBtn;

	public Text nameTxt;

	public Text descTxt;

	public Text priceTxt;

	public UILoopList roleContentList;

	public List<RoleShopViewClothCellController> roleClothsList = new List<RoleShopViewClothCellController>();

	private ShopItemData currentRoleData;

	private ShopResponseData responseData;

	public void Init()
	{
		if (roleContentList != null)
		{
			roleContentList.onSelectedEvent = RoleItemClickHandler;
		}
	}

	private void RoleItemClickHandler(UILoopItem item)
	{
		ShopItemData shopItemData = item.GetData() as ShopItemData;
		if (currentRoleData != shopItemData)
		{
			currentRoleData = shopItemData;
			UpdateInfo();
		}
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(roleBuyBtnHandler));
	}

	private void RemvoeEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(roleBuyBtnHandler));
	}

	private void roleBuyBtnHandler(GameObject go)
	{
	}

	private void SendShopBuyMessage()
	{
		ShopBuyResquestData shopBuyResquestData = new ShopBuyResquestData();
		shopBuyResquestData.shopItemIds.Add(currentRoleData.itemId);
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_SHOPBUY;
		netMessageData.content = JsonUtility.ToJson(shopBuyResquestData);
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			ShopBuyResponseData shopBuyResponseData = JsonUtility.FromJson<ShopBuyResponseData>(content.data);
			if (shopBuyResponseData != null && shopBuyResponseData.shopBuyResultData != null)
			{
				int i = 0;
				for (int count = shopBuyResponseData.shopBuyResultData.Count; i < count; i++)
				{
					Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(shopBuyResponseData.shopBuyResultData[i].goodsId, shopBuyResponseData.shopBuyResultData[i].num, AssertChangeType.SHOP_KEY, NetWorkSynType.NONE);
				}
			}
		};
		netMessageData.errorHandler = delegate
		{
		};
	}

	private void RequestRoleShopData()
	{
		ShopReqestData shopReqestData = new ShopReqestData();
		shopReqestData.shopTypes.Add(2);
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.GET_SHOPDATA;
		netMessageData.content = JsonUtility.ToJson(shopReqestData);
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			responseData = JsonUtility.FromJson<ShopResponseData>(content.data);
			if (responseData != null && responseData.shopItemList != null && responseData.shopItemList != null && responseData.shopItemList.Count > 0)
			{
				roleContentList.Data(responseData.shopItemList);
			}
		};
		netMessageData.errorHandler = delegate
		{
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}

	public void Reset()
	{
		RemvoeEventHandler();
	}

	public void Begin()
	{
		AddEventHandler();
		RequestRoleShopData();
	}

	private void UpdateInfo()
	{
		nameTxt.text = Mod.Localization.GetInfoById(currentRoleData.name);
		Goods_goodsTable goodsTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetGoodsTableById(currentRoleData.itemId);
		descTxt.text = Mod.Localization.GetInfoById(goodsTableById.Desc);
		if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(currentRoleData.itemId) > 0.0)
		{
			buyBtn.SetActive(false);
		}
		else
		{
			buyBtn.SetActive(true);
		}
		ShowAvatar();
	}

	private void ShowAvatar()
	{
		RemoveAvatar();
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourcesManager.Load<GameObject>("Prefab/Roles/RolePrince"));
		Vector3 cameraPos = new Vector3(0.38f, 0.34f, -3.4f);
		Singleton<ShowUIAvatar>.Instance.ShowAvatar(gameObject, 9, true, cameraPos);
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		gameObject.transform.Find("effect").gameObject.SetActive(false);
		Animator componentInChildren = gameObject.GetComponentInChildren<Animator>();
		if (componentInChildren != null)
		{
			componentInChildren.Play("WaitingState");
		}
		CapsuleCollider component = gameObject.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			component.radius = 0.5f;
		}
	}

	private void RemoveAvatar()
	{
		Singleton<ShowUIAvatar>.Instance.RemoveAvatar();
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}
}
