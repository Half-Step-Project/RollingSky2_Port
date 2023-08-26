using Foundation;

namespace RS2
{
	public class CommonClickTutorialStep : BaseTutorialStep
	{
		private CommonTutorialData tutorialData;

		public CommonClickTutorialStep(CommonTutorialData target)
		{
			tutorialData = target;
		}

		public override void Do()
		{
			if (tutorialData != null)
			{
				Mod.UI.OpenUIForm(UIFormId.CommonTutorialForm, tutorialData);
			}
		}
	}
}
