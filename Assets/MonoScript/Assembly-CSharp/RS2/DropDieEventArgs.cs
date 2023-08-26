using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class DropDieEventArgs : EventArgs<DropDieEventArgs>
	{
		public Vector3 DiePosition { get; private set; }

		public DropDieEventArgs Initialize(Vector3 diePos)
		{
			DiePosition = diePos;
			return this;
		}

		protected override void OnRecycle()
		{
			DiePosition = Vector3.zero;
		}
	}
}
