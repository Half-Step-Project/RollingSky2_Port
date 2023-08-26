using System.Collections.Generic;
using UnityEngine;

public class PropsPool : BasePool<BaseProp>
{
	private Transform m_parent;

	public PropsPool(Transform parent)
	{
		m_parent = parent;
		OnInstance();
	}

	public void OnInstance()
	{
		Dictionary<PropsName, PropData>.Enumerator enumerator = PropsManager.m_propDatas.GetEnumerator();
		while (enumerator.MoveNext())
		{
			BaseProp baseProp = PropsManager.CreateBaseProp(enumerator.Current.Value);
			baseProp.transform.parent = m_parent;
			baseProp.transform.position = new Vector3(10000f, 10000f, -10000f);
			baseProp.OnInstance();
			Retrieve((int)enumerator.Current.Key, baseProp);
		}
	}

	public override void Retrieve(int key, BaseProp obj)
	{
		base.Retrieve(key, obj);
		if (obj != null)
		{
			obj.transform.position = new Vector3(10000f, 10000f, -10000f);
			obj.transform.parent = m_parent;
		}
	}

	public override void RetrieveAll()
	{
		if (m_closes == null || m_closes.Count == 0)
		{
			return;
		}
		Dictionary<int, List<BaseProp>>.Enumerator enumerator = m_closes.GetEnumerator();
		List<BaseProp> list = null;
		while (enumerator.MoveNext())
		{
			list = enumerator.Current.Value;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.position = new Vector3(10000f, 10000f, -10000f);
				list[i].transform.parent = m_parent;
			}
		}
		base.RetrieveAll();
	}
}
