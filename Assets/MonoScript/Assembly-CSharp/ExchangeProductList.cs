using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeProductList : MonoBehaviour, IScroll
{
	public GameObject mPrefab;

	public ScrollRect mScrollRect;

	public List<ExchangeProductItem> mItems;

	private ExchangeStoreDataModule GetDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ExchangeStoreDataModule>(DataNames.ExchangeStoreDataModule);
		}
	}

	public void OnInit()
	{
		ExchangeStoreDataModule.ProductData[] mProductDatas = GetDataModule.mSaveData.mProductDatas;
		for (int i = 0; i < mProductDatas.Length; i++)
		{
			GameObject obj = Object.Instantiate(mPrefab);
			obj.SetActive(true);
			obj.transform.SetParent(mScrollRect.content);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			ExchangeProductItem component = obj.GetComponent<ExchangeProductItem>();
			component.SetScroll(this);
			component.SetData(i, i, mProductDatas[i].mID);
			component.OnInit();
			mItems.Add(component);
		}
	}

	public void OnOpen()
	{
		for (int i = 0; i < mItems.Count; i++)
		{
			mItems[i].OnOpen();
		}
	}

	public void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		for (int i = 0; i < mItems.Count; i++)
		{
			mItems[i].OnTick(elapseSeconds, realElapseSeconds);
		}
	}

	public void OnClose()
	{
		for (int i = 0; i < mItems.Count; i++)
		{
			mItems[i].OnClose();
		}
	}

	public void OnRefresh()
	{
		for (int i = 0; i < mItems.Count; i++)
		{
			mItems[i].OnRefresh();
		}
	}

	public void OnRelease()
	{
		for (int i = 0; i < mItems.Count; i++)
		{
			mItems[i].OnRelease();
		}
	}
}
