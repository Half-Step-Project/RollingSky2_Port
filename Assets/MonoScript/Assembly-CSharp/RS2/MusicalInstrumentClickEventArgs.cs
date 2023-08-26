using Foundation;
using UnityEngine;

namespace RS2
{
	public class MusicalInstrumentClickEventArgs : EventArgs<MusicalInstrumentClickEventArgs>
	{
		public int MusicalInstrumentID { get; private set; }

		public Vector3 Postion { get; private set; }

		public double Product { get; private set; }

		public int Type { get; private set; }

		public MusicalInstrumentClickEventArgs Initialize(int id, Vector3 postion, int type = 1, double product = 0.0)
		{
			MusicalInstrumentID = id;
			Postion = postion;
			Type = type;
			Product = product;
			return this;
		}

		protected override void OnRecycle()
		{
			MusicalInstrumentID = -1;
			Postion = Vector3.zero;
			Product = 0.0;
			Type = -1;
		}
	}
}
