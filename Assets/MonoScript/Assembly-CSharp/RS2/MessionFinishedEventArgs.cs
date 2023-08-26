using System.Collections.Generic;
using Foundation;

namespace RS2
{
	public class MessionFinishedEventArgs : EventArgs<MessionFinishedEventArgs>
	{
		public List<int> FinisheMessions { get; private set; }

		public MessionFinishedEventArgs Initialize(List<int> messions)
		{
			FinisheMessions = messions;
			return this;
		}

		protected override void OnRecycle()
		{
			if (FinisheMessions != null)
			{
				FinisheMessions.Clear();
			}
		}
	}
}
