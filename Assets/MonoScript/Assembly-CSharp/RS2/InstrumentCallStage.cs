namespace RS2
{
	public class InstrumentCallStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_INSTRUMENT_CALL);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_UPGRADE);
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
