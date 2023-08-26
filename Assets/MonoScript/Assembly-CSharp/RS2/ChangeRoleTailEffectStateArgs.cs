using Foundation;

namespace RS2
{
	public sealed class ChangeRoleTailEffectStateArgs : EventArgs<ChangeRoleTailEffectStateArgs>
	{
		private TailEffectState m_tailEffectState = TailEffectState.NONE;

		public TailEffectState TailEffectState
		{
			get
			{
				return m_tailEffectState;
			}
		}

		public ChangeRoleTailEffectStateArgs Initialize(TailEffectState tailEffectState)
		{
			m_tailEffectState = tailEffectState;
			return this;
		}

		protected override void OnRecycle()
		{
			m_tailEffectState = TailEffectState.NONE;
		}
	}
}
