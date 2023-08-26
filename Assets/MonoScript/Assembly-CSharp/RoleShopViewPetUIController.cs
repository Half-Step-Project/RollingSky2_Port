using System;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class RoleShopViewPetUIController : MonoBehaviour, IRoleShopChildViewUIController
{
	private GameObject petItem;

	public GameObject buyBtn;

	public Text nameTxt;

	public Text descTxt;

	public Text priceTxt;

	public UILoopList petContentList;

	private ShopItemData currentPetData;

	private ShopResponseData responseData;

	public void Init()
	{
		if (petContentList != null)
		{
			petContentList.onSelectedEvent = PetItemClickHandler;
		}
	}

	private void PetItemClickHandler(UILoopItem item)
	{
		ShopItemData shopItemData = item.GetData() as ShopItemData;
		if (currentPetData != shopItemData)
		{
			currentPetData = shopItemData;
			UpdateInfo();
		}
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(petBuyBtnHandler));
	}

	private void RemvoeEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(petBuyBtnHandler));
	}

	private void petBuyBtnHandler(GameObject go)
	{
	}

	public void Reset()
	{
		RemvoeEventHandler();
	}

	public void Begin()
	{
		AddEventHandler();
		RequestPetShopData();
	}

	private void UpdateInfo()
	{
		nameTxt.text = Mod.Localization.GetInfoById(currentPetData.name);
		Goods_goodsTable goodsTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetGoodsTableById(currentPetData.itemId);
		descTxt.text = Mod.Localization.GetInfoById(goodsTableById.Desc);
		if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(currentPetData.itemId) > 0.0)
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

	private void SendShopBuyMessage()
	{
		ShopBuyResquestData shopBuyResquestData = new ShopBuyResquestData();
		shopBuyResquestData.shopItemIds.Add(currentPetData.itemId);
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.POST_SHOPBUY;
		netMessageData.content = JsonUtility.ToJson(shopBuyResquestData);
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			Debug.Log("RequestShopBuyData:" + content);
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
		netMessageData.errorHandler = delegate(string content)
		{
			Debug.Log("RequestShopBuyData:" + content);
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}

	private void RequestPetShopData()
	{
		ShopReqestData shopReqestData = new ShopReqestData();
		shopReqestData.shopTypes.Add(3);
		NetMessageData netMessageData = ObjectPool<NetMessageData>.Instance.Get();
		netMessageData.messageId = NetMessageId.GET_SHOPDATA;
		netMessageData.content = JsonUtility.ToJson(shopReqestData);
		netMessageData.succesHandler = delegate(NetMessageResultData content)
		{
			Debug.Log("RequestShopData:" + content);
			responseData = JsonUtility.FromJson<ShopResponseData>(content.data);
			if (responseData != null && responseData.shopItemList != null && responseData.shopItemList != null && responseData.shopItemList.Count > 0)
			{
				petContentList.Data(responseData.shopItemList);
			}
		};
		netMessageData.errorHandler = delegate(string content)
		{
			Debug.Log("RequestShopData:" + content);
		};
		MonoSingleton<NetWorkManager>.Instacne.Send(netMessageData);
	}
}
