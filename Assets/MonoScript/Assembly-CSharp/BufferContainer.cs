using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class BufferContainer : MonoBehaviour
{
	public RectTransform m_bufferButton;

	[Header("bufferItem")]
	public UIBuffItem shieldBuffItem;

	public UIBuffItem powerBuffItem;

	public UIBuffItem rebirthBuffItem;

	private Dictionary<int, UIBuffItem> m_items = new Dictionary<int, UIBuffItem>();

	[Header("是否忽略倒计时的时间显示")]
	public bool m_ignoreShowTime;

	[Header("倒计时的显示,刷新间隔")]
	public float m_intervalTime = 0.2f;

	[Label]
	public float m_currentTime;

	[Label]
	public bool m_isNeedTime;

	[Header("其中的按钮是否可以点击")]
	[Label]
	public bool m_isCanClickButtons = true;

	public void OnOpen()
	{
		bool num = PlayerDataModule.Instance.BufferIsEnableForever(GameCommon.START_FREE_SHIELD);
		bool flag = PlayerDataModule.Instance.BufferIsEnableForever(GameCommon.EVERY_DAY_GIVE_POWER);
		bool flag2 = PlayerDataModule.Instance.BufferIsEnableForever(GameCommon.ORIGIN_REBIRTH_FREE);
		if (!num && !flag && !flag2)
		{
			base.gameObject.SetActive(false);
			return;
		}
		m_items.Add(GameCommon.START_FREE_SHIELD, shieldBuffItem);
		m_items.Add(GameCommon.EVERY_DAY_GIVE_POWER, powerBuffItem);
		m_items.Add(GameCommon.ORIGIN_REBIRTH_FREE, rebirthBuffItem);
		RefreshBuffItems();
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Subscribe(EventArgs<BufferTimeChanged>.EventId, OnBufferTimeChanged);
	}

	public void OnClose()
	{
		m_items.Clear();
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Unsubscribe(EventArgs<BufferTimeChanged>.EventId, OnBufferTimeChanged);
	}

	public void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		if (m_isNeedTime)
		{
			m_currentTime += realElapseSeconds;
			if (m_currentTime >= m_intervalTime)
			{
				RefreshTime();
				m_currentTime = 0f;
			}
		}
	}

	private void RefreshTime()
	{
		foreach (KeyValuePair<int, UIBuffItem> item in m_items)
		{
			if (!(item.Value == null) && item.Value.m_currentState == UIBuffItem.ItemState.TimeLimit && item.Value.m_bufferData != null)
			{
				long num = item.Value.m_bufferData.LeftBufferTime();
				item.Value.SetTime(item.Value.m_bufferData.LeftBufferTime());
				if (num <= 0)
				{
					RefreshBuffItems();
					break;
				}
			}
		}
	}

	private void RefreshBuffItems()
	{
		m_isNeedTime = false;
		foreach (KeyValuePair<int, UIBuffItem> item in m_items)
		{
			item.Value.m_bufferData = PlayerDataModule.Instance.GetPlayerBufferDataByBufferID(item.Key);
			UIBuffItem.ItemState itemStateByBufferData = GetItemStateByBufferData(item.Value.m_bufferData);
			if (itemStateByBufferData == UIBuffItem.ItemState.TimeLimit)
			{
				m_isNeedTime = true;
			}
			item.Value.SwitchState(itemStateByBufferData, m_ignoreShowTime);
		}
		RefreshTime();
	}

	private UIBuffItem.ItemState GetItemStateByBufferData(PlayerBufferData data)
	{
		bool num = PlayerDataModule.Instance.BufferIsEnable(data);
		bool flag = PlayerDataModule.Instance.BufferIsEnableForever(data);
		bool flag2 = PlayerDataModule.Instance.BufferIsEnableByTime(data);
		UIBuffItem.ItemState result = UIBuffItem.ItemState.Close;
		if (num)
		{
			if (flag)
			{
				result = UIBuffItem.ItemState.Permanent;
			}
			else if (flag2)
			{
				result = UIBuffItem.ItemState.TimeLimit;
			}
		}
		return result;
	}

	private void OnClickBufferButton(GameObject obj)
	{
		if (m_isCanClickButtons)
		{
			Mod.UI.OpenUIForm(UIFormId.BufferShowForm);
		}
	}

	private void OnPlayerAssetChange(object sender, EventArgs e)
	{
		GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
		if (gameGoodsNumChangeEventArgs != null)
		{
			int goodsId = gameGoodsNumChangeEventArgs.GoodsId;
			if (goodsId == GameCommon.START_FREE_SHIELD || goodsId == GameCommon.EVERY_DAY_GIVE_POWER || goodsId == GameCommon.ORIGIN_REBIRTH_FREE)
			{
				RefreshBuffItems();
			}
		}
	}

	private void OnBufferTimeChanged(object sender, EventArgs e)
	{
		if (e is BufferTimeChanged)
		{
			RefreshBuffItems();
		}
	}
}
