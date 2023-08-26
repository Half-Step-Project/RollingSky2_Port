using Foundation;

namespace RS2
{
	public class TrigerTutorialStepEventArgs : EventArgs<TrigerTutorialStepEventArgs>
	{
		public int StageStepId { get; private set; }

		public TrigerTutorialStepEventArgs Initialize(int id)
		{
			StageStepId = id;
			return this;
		}

		protected override void OnRecycle()
		{
			StageStepId = -1;
		}
	}
}
