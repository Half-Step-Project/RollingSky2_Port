namespace RS2
{
	public class BufferStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_BUFFER);
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
