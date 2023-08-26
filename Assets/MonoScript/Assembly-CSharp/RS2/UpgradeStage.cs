namespace RS2
{
	public class UpgradeStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_UPGRADE);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_TAPTAP);
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
