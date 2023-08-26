using Foundation;
using UnityEngine;

namespace RS2
{
	public class AllStarForm : UGUIForm
	{
		public GameObject effect;

		private int levelSeriesId;

		private void Awake()
		{
			effect.SetActive(false);
		}

		private void OnDestroy()
		{
			CancelInvoke();
		}

		private void ShowEffect()
		{
			effect.SetActive(true);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			levelSeriesId = (int)userData;
			Invoke("ShowEffect", 0.5f);
		}

		public void OnClickGetButton()
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<GetAllStarEventArgs>().Initialize(levelSeriesId));
			Mod.UI.CloseUIForm(UIFormId.AllStarForm);
		}
	}
}
