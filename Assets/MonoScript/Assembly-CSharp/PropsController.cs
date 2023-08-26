using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class PropsController : MonoBehaviour, IOriginRebirth
{
	public Dictionary<int, BaseProp> m_props = new Dictionary<int, BaseProp>();

	public PropsPool m_propPool;

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public static PropsController Builder()
	{
		GameObject gameObject = new GameObject("PropsController");
		PropsController propsController = gameObject.AddComponent<PropsController>();
		propsController.m_propPool = new PropsPool(gameObject.transform);
		propsController.SubscribeEvents();
		return propsController;
	}

	private void SubscribeEvents()
	{
		Mod.Event.Subscribe(EventArgs<PropsAddEventArgs>.EventId, PropsAddHandler);
		Mod.Event.Subscribe(EventArgs<PropsRemoveEventArgs>.EventId, PropsRemoveHandler);
		Mod.Event.Subscribe(EventArgs<PropsRemoveAllEventArgs>.EventId, PropsRemoveAllHandler);
		Mod.Event.Subscribe(EventArgs<PropsTriggerEventArgs>.EventId, PropsTriggerHandler);
	}

	private void UnSubscribeEvents()
	{
		Mod.Event.Unsubscribe(EventArgs<PropsAddEventArgs>.EventId, PropsAddHandler);
		Mod.Event.Unsubscribe(EventArgs<PropsRemoveEventArgs>.EventId, PropsRemoveHandler);
		Mod.Event.Unsubscribe(EventArgs<PropsRemoveAllEventArgs>.EventId, PropsRemoveAllHandler);
		Mod.Event.Unsubscribe(EventArgs<PropsTriggerEventArgs>.EventId, PropsTriggerHandler);
	}

	public void Add(PropsName propsName)
	{
		BaseProp value = null;
		if (PropsManager.GetPropData(propsName) != null)
		{
			m_props.TryGetValue((int)propsName, out value);
			if (value == null)
			{
				value = m_propPool.Acquire((int)propsName);
			}
			value.IsTriggering = false;
			value.Add();
			m_props[(int)propsName] = value;
		}
	}

	public void Remove(PropsName propsName)
	{
		BaseProp basePropByPropsName = GetBasePropByPropsName(propsName);
		if (basePropByPropsName != null)
		{
			basePropByPropsName.Remove();
			m_propPool.Retrieve((int)propsName, basePropByPropsName);
		}
		m_props.Remove((int)propsName);
	}

	public void RemoveAll()
	{
		if (m_props == null || m_props.Count == 0)
		{
			return;
		}
		Dictionary<int, BaseProp>.Enumerator enumerator = m_props.GetEnumerator();
		while (enumerator.MoveNext())
		{
			BaseProp value = enumerator.Current.Value;
			if (value != null)
			{
				value.Remove();
				m_propPool.Retrieve(enumerator.Current.Key, value);
			}
		}
		m_props.Clear();
	}

	public void OnTrigger(PropsName propsName)
	{
		BaseProp basePropByPropsName = GetBasePropByPropsName(propsName);
		if (basePropByPropsName != null && !basePropByPropsName.IsTriggering)
		{
			basePropByPropsName.OnTrigger();
			basePropByPropsName.IsTriggering = true;
		}
	}

	public bool IsContains(PropsName propsName)
	{
		return m_props.ContainsKey((int)propsName);
	}

	public PropsName[] GetAllNames()
	{
		if (m_props == null || m_props.Count == 0)
		{
			return new PropsName[0];
		}
		PropsName[] array = new PropsName[m_props.Count];
		int num = 0;
		foreach (KeyValuePair<int, BaseProp> prop in m_props)
		{
			array[num] = (PropsName)prop.Key;
			num++;
		}
		return array;
	}

	public BaseProp GetBasePropByPropsName(PropsName propsName)
	{
		BaseProp value = null;
		m_props.TryGetValue((int)propsName, out value);
		return value;
	}

	public void DestroyLocal()
	{
		UnSubscribeEvents();
		RemoveAll();
		Object.Destroy(base.gameObject);
	}

	private void PropsAddHandler(object sender, EventArgs e)
	{
		PropsAddEventArgs propsAddEventArgs = e as PropsAddEventArgs;
		Add(propsAddEventArgs.m_propsName);
	}

	private void PropsRemoveHandler(object sender, EventArgs e)
	{
		PropsRemoveEventArgs propsRemoveEventArgs = e as PropsRemoveEventArgs;
		Remove(propsRemoveEventArgs.m_propsName);
	}

	private void PropsRemoveAllHandler(object sender, EventArgs e)
	{
		RemoveAll();
	}

	private void PropsTriggerHandler(object sender, EventArgs e)
	{
		PropsTriggerEventArgs propsTriggerEventArgs = e as PropsTriggerEventArgs;
		OnTrigger(propsTriggerEventArgs.m_propsName);
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return string.Empty;
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		RemoveAll();
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.REBIRTH));
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsTriggerEventArgs>().Initialize(PropsName.REBIRTH));
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return new byte[0];
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		RemoveAll();
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.REBIRTH));
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsTriggerEventArgs>().Initialize(PropsName.REBIRTH));
	}
}
