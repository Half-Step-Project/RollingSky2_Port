namespace RS2
{
	public class FirstLevelStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_FIRST_LEVEL);
			if (!TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_FIRST_LEVEL_BUFF))
			{
				TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_FIRST_LEVEL_BUFF);
			}
			else
			{
				TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_NEXT_LEVEL);
			}
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
