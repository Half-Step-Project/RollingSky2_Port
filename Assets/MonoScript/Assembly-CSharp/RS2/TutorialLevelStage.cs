namespace RS2
{
	public class TutorialLevelStage : BaseTutorialStage
	{
		public override void End()
		{
			base.End();
			InfocUtils.Report_rollingsky2_games_gamefiller(3, 10000);
			TutorialManager.Instance.SetTutorialStageFlag(TutorialStageId.STAGE_TUTORIAL_LEVEL);
			TutorialManager.Instance.SwitchStage(TutorialStageId.STAGE_HOME_MENU);
		}

		public override void InitSteps()
		{
			InfocUtils.Report_rollingsky2_games_gamefiller(24);
		}

		public override void Start()
		{
			base.Start();
			base.IsTutoiralGoing = true;
		}
	}
}
