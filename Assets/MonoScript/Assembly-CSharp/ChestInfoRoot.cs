using System;
using UnityEngine;

public class ChestInfoRoot : MonoBehaviour
{
	public GameObject root;

	public GameObject infoGo;

	public GameObject ruleGo;

	public GameObject ruleButton;

	public GameObject infoCloseButton;

	private bool isShowRule;

	private bool isShow;

	private int levelId;

	private void Awake()
	{
		OnOpen();
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(ruleButton);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(RuleButtonClickHandler));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(infoCloseButton);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RuleButtonClickHandler));
	}

	private void OnDestroy()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(ruleButton);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(RuleButtonClickHandler));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(infoCloseButton);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RuleButtonClickHandler));
	}

	public void OnOpen()
	{
		isShow = false;
		isShowRule = false;
		root.SetActive(false);
	}

	public void ToggleShowOrHide()
	{
		isShow = !isShow;
		if (isShow)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void RuleButtonClickHandler(GameObject go)
	{
		isShowRule = !isShowRule;
		ToggleInfoOrRule();
	}

	private void Show()
	{
		root.SetActive(true);
		isShowRule = false;
		ToggleInfoOrRule();
	}

	private void Hide()
	{
		root.SetActive(false);
	}

	private void ToggleInfoOrRule()
	{
		infoGo.SetActive(!isShowRule);
		ruleGo.SetActive(isShowRule);
	}
}
