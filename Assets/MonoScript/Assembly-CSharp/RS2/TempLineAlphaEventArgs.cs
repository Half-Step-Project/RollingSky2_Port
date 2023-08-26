using Foundation;

namespace RS2
{
	public class TempLineAlphaEventArgs : EventArgs<TempLineAlphaEventArgs>
	{
		public float dis { get; private set; }

		public float maxAlpha { get; private set; }

		public void Initialize(float _dis, float _maxAlpha)
		{
			dis = _dis;
			maxAlpha = _maxAlpha;
		}

		protected override void OnRecycle()
		{
			dis = 0f;
			maxAlpha = 0f;
		}
	}
}
