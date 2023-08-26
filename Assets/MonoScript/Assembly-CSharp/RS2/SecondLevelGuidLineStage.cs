namespace RS2
{
	public class SecondLevelGuidLineStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_SECOND_LEVEL_GUIDLINE);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_NONE);
		}

		public override void InitSteps()
		{
		}

		public override void Start()
		{
			base.Start();
		}
	}
}
