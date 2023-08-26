using UnityEngine;

namespace RS2
{
	public class Slot : MonoBehaviour
	{
		public int Index { get; private set; }

		public MusicalInstrument MI { get; private set; }

		public bool IsEmpty
		{
			get
			{
				return MI == null;
			}
		}

		public void AddMusicalInstrument(MusicalInstrument mi)
		{
			MI = mi;
			mi.transform.parent = base.transform;
			mi.transform.localPosition = Vector3.zero;
		}

		public void RemoveMusicalInstrument()
		{
			Object.Destroy(MI.gameObject);
		}

		private void PlayMusicalInstrumentAnim(MusicalInstrument.AnimState trigger)
		{
			MI.PlayAnim(trigger);
		}

		public void Release()
		{
			MI.Release();
			MI = null;
		}
	}
}
