using RS2;
using UnityEngine.Events;

public class CommonAlertData
{
	public enum AlertShopType
	{
		COMMON,
		AD,
		BUY_SHOPITEM,
		UNLOCK,
		BUY_OR_AD,
		MESSION
	}

	public UnityAction callBackFunc;

	public UnityAction cancelCallBackFunc;

	public UnityAction closeCallBackFunc;

	public UnityAction adCallBackFunc;

	public UnityAction startADCallBackFunc;

	public string alertContent;

	public string lableContent;

	public string adContent;

	public ADScene adScene;

	public AlertShopType showType;

	public int shopItemId = -1;

	public int iconid = -1;

	public int goodsNum = -1;

	public bool isBuyBySinglePrice;

	public bool isCoverHide = true;

	public bool isADBtnShow;

	public int itemNum;

	public bool needCancelButton;

	public string cancelButtonText;

	public void Clear()
	{
		callBackFunc = null;
		closeCallBackFunc = null;
		adCallBackFunc = null;
		startADCallBackFunc = null;
		alertContent = "";
		lableContent = "";
		adContent = "";
		adScene = ADScene.NONE;
		showType = AlertShopType.COMMON;
		shopItemId = -1;
		iconid = -1;
		goodsNum = -1;
		isBuyBySinglePrice = false;
		itemNum = 0;
		isCoverHide = true;
		isADBtnShow = false;
	}
}
