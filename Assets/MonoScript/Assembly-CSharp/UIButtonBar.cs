using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButtonBar : MonoBehaviour
{
	public List<GameObject> buttonList = new List<GameObject>();

	public List<Text> labelList = new List<Text>();

	public UnityAction<int> selectHandler;

	private int selectIndex = -1;

	private GameObject selectedButton;

	public void Init(string[] labels)
	{
		int i = 0;
		for (int count = buttonList.Count; i < count; i++)
		{
			buttonList[i].name = string.Concat(i);
			labelList[i].text = labels[i];
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buttonList[i]);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(SelectBtnClickHandler));
		}
	}

	private void SelectBtnClickHandler(GameObject obj)
	{
		SetSelectIndex(int.Parse(obj.name));
	}

	public void SetSelectIndex(int index)
	{
		if (selectIndex != index)
		{
			if (selectedButton != null)
			{
				selectedButton.transform.Find("normal").gameObject.SetActive(true);
				selectedButton.transform.Find("selected").gameObject.SetActive(false);
			}
			selectIndex = index;
			selectedButton = buttonList[selectIndex];
			if (selectedButton != null)
			{
				selectedButton.transform.Find("normal").gameObject.SetActive(false);
				selectedButton.transform.Find("selected").gameObject.SetActive(true);
			}
			if (selectHandler != null)
			{
				selectHandler(selectIndex);
			}
		}
	}

	public int GetSelectIndex()
	{
		return selectIndex;
	}

	public void Reset()
	{
		int i = 0;
		for (int count = buttonList.Count; i < count; i++)
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(buttonList[i]);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(SelectBtnClickHandler));
		}
		selectIndex = -1;
		if (selectedButton != null)
		{
			selectedButton.transform.Find("normal").gameObject.SetActive(true);
			selectedButton.transform.Find("selected").gameObject.SetActive(false);
		}
		selectedButton = null;
	}
}
