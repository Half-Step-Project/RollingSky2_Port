namespace RS2
{
	public class TaptapStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_TAPTAP);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_NEXT_LEVEL);
		}

		public override void InitSteps()
		{
		}

		public override void Start()
		{
			base.Start();
			base.IsTutoiralGoing = true;
		}
	}
}
