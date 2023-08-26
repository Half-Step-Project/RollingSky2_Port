using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class GrabTreasureDesk : BaseEnemy
	{
		public static readonly string NodeTreasureBox = "treasureBox";

		public GameObject TreasureBox;

		public override void Initialize()
		{
			base.Initialize();
			Transform transform = base.transform.Find(NodeTreasureBox);
			if ((bool)transform)
			{
				TreasureBox = transform.gameObject;
				TreasureBox.SetActive(true);
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if ((bool)TreasureBox)
			{
				TreasureBox.SetActive(true);
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<CoupleThiefAnimEventArgs>.EventId, OnCallGrabTreasure);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<CoupleThiefAnimEventArgs>.EventId, OnCallGrabTreasure);
		}

		private void OnCallGrabTreasure(object sender, EventArgs e)
		{
			ThiefGrabTreasureEventArgs thiefGrabTreasureEventArgs = e as ThiefGrabTreasureEventArgs;
			if (thiefGrabTreasureEventArgs != null && TreasureBox != null)
			{
				bool ifGrab = thiefGrabTreasureEventArgs.IfGrab;
				TreasureBox.SetActive(!ifGrab);
			}
		}
	}
}
