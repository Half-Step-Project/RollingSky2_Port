using Foundation;
using RS2;
using UnityEngine;

public class NormalBackground : BaseBackgroundElement
{
	public static readonly string NodeName = "model";

	public int SelectIndex;

	private Transform[] BackItems = new Transform[0];

	public new virtual bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public new virtual bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	private void OnEnable()
	{
		Mod.Event.Subscribe(EventArgs<ChangeBackgroundEventArgs>.EventId, OnChangeBackground);
	}

	private void OnDisable()
	{
		Mod.Event.Unsubscribe(EventArgs<ChangeBackgroundEventArgs>.EventId, OnChangeBackground);
	}

	public override void Initialize(Transform parent)
	{
		base.Initialize(parent);
		InitBackItems();
	}

	public override void ResetElement()
	{
		base.ResetElement();
		SelectIndex = 0;
		ShowBackByIndex(SelectIndex);
	}

	private void InitBackItems()
	{
		Transform transform = base.transform.Find(NodeName);
		BackItems = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			BackItems[i] = transform.GetChild(i);
			BackItems[i].gameObject.SetActive(i == SelectIndex);
		}
	}

	private void OnChangeBackground(object sender, EventArgs e)
	{
		ChangeBackgroundEventArgs changeBackgroundEventArgs = e as ChangeBackgroundEventArgs;
		if (changeBackgroundEventArgs != null)
		{
			ShowBackByIndex(changeBackgroundEventArgs.BackItemIndex);
		}
	}

	private void ShowBackByIndex(int index)
	{
		for (int i = 0; i < BackItems.Length; i++)
		{
			BackItems[i].gameObject.SetActive(i == index);
		}
		SelectIndex = index;
	}

	public override byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return Bson.ToBson(new RD_NormalBackground_DATA
		{
			BackIndex = SelectIndex
		});
	}

	public override void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		RD_NormalBackground_DATA rD_NormalBackground_DATA = Bson.ToObject<RD_NormalBackground_DATA>(dataInfo);
		ShowBackByIndex(rD_NormalBackground_DATA.BackIndex);
	}

	public override void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_NormalBackground_DATA rD_NormalBackground_DATA = Bson.ToObject<RD_NormalBackground_DATA>(rd_data);
		ShowBackByIndex(rD_NormalBackground_DATA.BackIndex);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_NormalBackground_DATA
		{
			BackIndex = SelectIndex
		});
	}

	public new virtual void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
