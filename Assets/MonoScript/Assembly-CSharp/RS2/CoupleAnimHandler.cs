using Foundation;
using UnityEngine;

namespace RS2
{
	public class CoupleAnimHandler : MonoBehaviour
	{
		private void OnGrabTreasure()
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<ThiefGrabTreasureEventArgs>().Initialize(true));
		}
	}
}
