using System.Collections.Generic;
using Foundation;
using RS2;

public class UIPopupManager : Singleton<UIPopupManager>
{
	public enum PriorityType
	{
		Priority_10 = 10,
		Priority_20 = 20,
		Priority_30 = 30,
		Priority_40 = 40,
		Priority_50 = 50,
		Priority_60 = 60,
		Priority_70 = 70
	}

	public class UIPriorityData
	{
		public class Comparer : IComparer<UIPriorityData>
		{
			public int Compare(UIPriorityData x, UIPriorityData y)
			{
				return x.priority - y.priority;
			}
		}

		public int priority;

		public UIFormId formId;

		public object userData;

		public UIPriorityData(int priority, UIFormId formId, object userData)
		{
			this.priority = priority;
			this.formId = formId;
			this.userData = userData;
		}
	}

	private PriorityQueue<UIPriorityData> priorityQueue = new PriorityQueue<UIPriorityData>(new UIPriorityData.Comparer());

	private UIPriorityData current;

	public void Init()
	{
		Mod.Event.Subscribe(EventArgs<UIPopUpFormCloseEvent>.EventId, OnPopUpFormClose);
	}

	private void OnPopUpFormClose(object sender, EventArgs args)
	{
		UIPopUpFormCloseEvent uIPopUpFormCloseEvent = args as UIPopUpFormCloseEvent;
		if (current != null && uIPopUpFormCloseEvent.UIFormId == current.formId)
		{
			current = priorityQueue.Pop();
			OpenCurrent();
		}
	}

	private void OpenCurrent()
	{
		if (current != null)
		{
			Mod.UI.OpenUIForm(current.formId, current.userData);
		}
	}

	public void PopupUI(UIFormId formId, object userData = null, PriorityType priority = PriorityType.Priority_40)
	{
		UIPriorityData v = new UIPriorityData((int)priority, formId, userData);
		if (current == null)
		{
			current = v;
			OpenCurrent();
		}
		else
		{
			priorityQueue.Push(v);
		}
	}
}
