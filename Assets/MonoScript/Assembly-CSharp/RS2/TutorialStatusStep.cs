namespace RS2
{
	public class TutorialStatusStep : BaseTutorialStep
	{
		private bool isInTutorial;

		public TutorialStatusStep(bool isInTutorial)
		{
			this.isInTutorial = isInTutorial;
		}

		public override void Do()
		{
			Next();
		}
	}
}
