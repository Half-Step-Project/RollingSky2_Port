using System.Collections.Generic;
using Foundation;
using RS2;

public class BroadCastManager : MonoSingleton<BroadCastManager>
{
	private Queue<BroadCastData> broadCastList = new Queue<BroadCastData>();

	private bool isInited;

	private BroadCastForm castForm;

	public Queue<BroadCastData> BroadCastList
	{
		get
		{
			return broadCastList;
		}
	}

	public bool IsInited
	{
		get
		{
			return isInited;
		}
		set
		{
			isInited = value;
		}
	}

	protected override void Init()
	{
		Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
		Mod.UI.OpenUIForm(UIFormId.BroadCastForm, this);
		isInited = true;
	}

	private void OnOpenUIFormSuccess(object sender, EventArgs e)
	{
		UIMod.OpenSuccessEventArgs openSuccessEventArgs = (UIMod.OpenSuccessEventArgs)e;
		castForm = openSuccessEventArgs.UIForm.Logic as BroadCastForm;
		if (castForm != null)
		{
			Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
			castForm.BroadCast();
		}
	}

	public void Reset()
	{
		Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenUIFormSuccess);
	}

	public void BroadCast(BroadCastData data)
	{
		if (data != null)
		{
			broadCastList.Enqueue(data);
			if (!isInited)
			{
				Init();
			}
			if (castForm != null)
			{
				castForm.BroadCast();
			}
		}
	}
}
