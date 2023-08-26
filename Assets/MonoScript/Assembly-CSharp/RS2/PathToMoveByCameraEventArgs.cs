using Foundation;

namespace RS2
{
	public sealed class PathToMoveByCameraEventArgs : EventArgs<PathToMoveByCameraEventArgs>
	{
		public PathToMoveByCameraTrigger.ElementData m_elementData;

		protected override void OnRecycle()
		{
		}

		public PathToMoveByCameraEventArgs Initialize(PathToMoveByCameraTrigger.ElementData data)
		{
			m_elementData = data;
			return this;
		}
	}
}
