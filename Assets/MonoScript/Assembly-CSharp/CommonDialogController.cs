using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CommonDialogController : MonoBehaviour
{
	public System.Action OnYesCallBack;

	public System.Action OnNoCallBack;

	public string Message = "";

	public bool IfAutoClose = true;

	private Button yesBtn;

	private Button cancelBtn;

	private Text yesLabel;

	private Text cancelLabel;

	private Text messageLabel;

	private Image backGround;

	private Image upback;

	private Vector3 yesBtnLocalPos;

	private List<object> loadedAsserts = new List<object>();

	private bool isPress;

	public void Init()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		messageLabel = dictionary["MessageLabel"].GetComponent<Text>();
		yesBtn = dictionary["YesBtn"].GetComponent<Button>();
		cancelBtn = dictionary["CancelBtn"].GetComponent<Button>();
		yesLabel = dictionary["YesLabel"].GetComponent<Text>();
		cancelLabel = dictionary["CancelLabel"].GetComponent<Text>();
		backGround = dictionary["MainUI"].GetComponent<Image>();
		upback = dictionary["upback"].GetComponent<Image>();
		yesBtnLocalPos = yesBtn.transform.localPosition;
		isPress = false;
	}

	public void Clear()
	{
		OnYesCallBack = null;
		OnNoCallBack = null;
	}

	private void ChangeBack()
	{
		int num = -1;
		if (backGround != null)
		{
			string spriteName = "levelback_" + num;
			Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data)
			{
				backGround.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", spriteName, assetName, errorMessage));
			}));
		}
		ChangeBgByLevelId(num);
	}

	public void ChangeBgByLevelId(int levelId)
	{
	}

	public void OnOpen()
	{
		AddEventListener();
	}

	public void OnClose()
	{
		RemoveEventListener();
		Clear();
	}

	public void BindData(string msg, CommonDialogUtil.ShowType showType, string yesStr, string noStr, System.Action onYes, System.Action onNo, CommonDialogUtil.ShowStyle style, bool ifAutoClose = true)
	{
		Message = msg;
		switch (showType)
		{
		case CommonDialogUtil.ShowType.YesNo:
			cancelBtn.gameObject.SetActive(true);
			yesBtn.transform.localPosition = yesBtnLocalPos;
			OnYesCallBack = onYes;
			OnNoCallBack = onNo;
			break;
		case CommonDialogUtil.ShowType.Yes:
			cancelBtn.gameObject.SetActive(false);
			yesBtn.transform.localPosition = new Vector3(0f, yesBtnLocalPos.y, yesBtnLocalPos.z);
			OnYesCallBack = onYes;
			break;
		}
		switch (style)
		{
		case CommonDialogUtil.ShowStyle.IN_MENU:
			backGround.color = new Color(1f, 1f, 1f, 1f);
			break;
		case CommonDialogUtil.ShowStyle.IN_GAME:
			backGround.color = new Color(0f, 0f, 0f, 0.5f);
			break;
		}
		IfAutoClose = ifAutoClose;
	}

	private void AddEventListener()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(yesBtn.gameObject);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnYesBtnClick));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(cancelBtn.gameObject);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnNoBtnClick));
	}

	private void RemoveEventListener()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(yesBtn.gameObject);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnYesBtnClick));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(cancelBtn.gameObject);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnNoBtnClick));
	}

	private void OnYesBtnClick(GameObject gameObj)
	{
		if (OnYesCallBack != null)
		{
			OnYesCallBack();
		}
		if (IfAutoClose)
		{
			CloseUI();
		}
	}

	private void CloseUI()
	{
		CommonDialogUtil.Instance.onShow = false;
		Mod.UI.CloseUIForm(UIFormId.CommonDialogForm);
	}

	private void OnNoBtnClick(GameObject gameObj)
	{
		if (OnNoCallBack != null)
		{
			OnNoCallBack();
		}
		if (IfAutoClose)
		{
			CloseUI();
		}
	}

	public void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	private void Update()
	{
		if (InputService.KeyDown_A)
		{
			isPress = true;
			OnYesBtnClick(null);
		}
		else if (InputService.KeyDown_B)
		{
			isPress = true;
			OnNoBtnClick(null);
		}
		if (!Input.anyKeyDown && isPress)
		{
			isPress = false;
		}
	}
}
