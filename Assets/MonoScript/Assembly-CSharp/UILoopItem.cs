using System;
using UnityEngine;

public abstract class UILoopItem : MonoBehaviour
{
	[NonSerialized]
	public int itemIndex;

	[NonSerialized]
	public GameObject itemObject;

	public void UpdateItem(int index, GameObject item)
	{
		itemIndex = index;
		itemObject = item;
	}

	public virtual void Data(object data)
	{
	}

	public virtual object GetData()
	{
		return null;
	}

	public virtual void SetSelected(bool selected)
	{
	}

	public virtual void OnRelease()
	{
	}
}
