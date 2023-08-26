using Foundation;

namespace RS2
{
	public sealed class CameraParticlePlayEventArgs : EventArgs<CameraParticlePlayEventArgs>
	{
		public bool IfPlay { get; private set; }

		public int ParticleIndex { get; private set; }

		public CameraParticlePlayEventArgs Initialize(bool ifPlay, int particleIndex = -1)
		{
			IfPlay = ifPlay;
			ParticleIndex = particleIndex;
			return this;
		}

		protected override void OnRecycle()
		{
			IfPlay = false;
			ParticleIndex = -1;
		}
	}
}
