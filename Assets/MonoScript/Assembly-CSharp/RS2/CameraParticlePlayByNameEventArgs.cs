using Foundation;

namespace RS2
{
	public sealed class CameraParticlePlayByNameEventArgs : EventArgs<CameraParticlePlayByNameEventArgs>
	{
		public ChangeCameraEffectByNameTrigger.Data data;

		public bool IfPlay { get; private set; }

		public CameraParticlePlayByNameEventArgs Initialize(ChangeCameraEffectByNameTrigger.Data data)
		{
			this.data = data;
			return this;
		}

		protected override void OnRecycle()
		{
			IfPlay = false;
			data = default(ChangeCameraEffectByNameTrigger.Data);
		}
	}
}
