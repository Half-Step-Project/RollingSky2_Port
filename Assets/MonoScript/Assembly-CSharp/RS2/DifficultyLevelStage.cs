namespace RS2
{
	public class DifficultyLevelStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_LEVEL_DIFFICULT);
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
