using System;
using Foundation;

namespace RS2
{
	public class NewFeatureTutorialStep : BaseTutorialStep
	{
		public System.Action closeCallback;

		public NewFeatureTutorialStep(System.Action closeCallback)
		{
			this.closeCallback = closeCallback;
		}

		public override void Do()
		{
			NewFeatureForm.Data data = new NewFeatureForm.Data();
			data.closeCallback = closeCallback;
			Mod.UI.OpenUIForm(UIFormId.NewFeatureForm, data);
		}
	}
}
