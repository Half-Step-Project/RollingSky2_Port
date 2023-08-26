namespace RS2
{
	public class HomeMenuStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_HOME_MENU);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_INSTRUMENT_CALL);
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
