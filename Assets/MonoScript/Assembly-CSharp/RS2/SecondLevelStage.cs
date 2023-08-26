namespace RS2
{
	public class SecondLevelStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_FIRST_LEVEL);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_FIRST_LEVEL_BUFF);
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
