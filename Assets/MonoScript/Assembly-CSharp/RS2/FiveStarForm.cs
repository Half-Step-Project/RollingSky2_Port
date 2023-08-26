using Foundation;

namespace RS2
{
	public class FiveStarForm : UGUIForm
	{
		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			PlayerDataModule.Instance.PlayerLocalVideoAwardData.HasShowFiveStarToday = true;
			Mod.Event.Fire(this, UI3DShowOrHideEvent.Make(false));
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Event.Fire(this, UI3DShowOrHideEvent.Make(true));
		}

		public void OnClickClose()
		{
			Mod.UI.CloseUIForm(UIFormId.FiveStarForm);
		}

		public void OnClickGood()
		{
			PlayerDataModule.Instance.PlayerLocalVideoAwardData.HasDoFiveStar = true;
			StoreAgent.StoreReview();
			Mod.UI.CloseUIForm(UIFormId.FiveStarForm);
		}

		public void OnClickBad()
		{
			NativeUtils.Instance.OpenFeedbackView();
			Mod.UI.CloseUIForm(UIFormId.FiveStarForm);
		}
	}
}
