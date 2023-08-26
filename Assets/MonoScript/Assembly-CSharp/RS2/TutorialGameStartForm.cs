using Foundation;
using RisingWin.Library;

namespace RS2
{
	public class TutorialGameStartForm : UGUIForm
	{
		private TutorialGameStartManager tutorialManager;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if (tutorialManager == null)
			{
				tutorialManager = base.gameObject.GetComponent<TutorialGameStartManager>();
			}
			tutorialManager.Init();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			tutorialManager.OnOpen();
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			int button_source = 0;
			if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).CurrentEnterLevelType == LevelEnterType.MENU)
			{
				button_source = 1;
			}
			InfocUtils.Report_rollingsky2_games_pageshow(3, 30, 1, curLevelId, button_source);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			tutorialManager.OnClose();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			if (tutorialManager != null)
			{
				tutorialManager.Release();
			}
		}

		protected override void OnPause()
		{
			base.gameObject.SetActive(false);
		}

		protected override void OnResume()
		{
			base.gameObject.SetActive(true);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (tutorialManager != null)
			{
				tutorialManager.OnTick(elapseSeconds, realElapseSeconds);
			}
		}

		public override void OnButtonTrigger(JoystickButton pButton)
		{
			base.OnButtonTrigger(pButton);
			switch (pButton)
			{
			case JoystickButton.CONFIRM:
				if (tutorialManager.startGameBtn.activeInHierarchy)
				{
					tutorialManager.OnPlayBtnClick(null);
				}
				break;
			case JoystickButton.CANCEL:
				if (tutorialManager.backGameBtn.activeInHierarchy && !Mod.UI.HasUIForm(UIFormId.CommonTutorialForm))
				{
					tutorialManager.OnBackClick(null);
				}
				break;
			case JoystickButton.MENU:
				if (Mod.UI.HasUIForm(UIFormId.CommonTutorialForm))
				{
					CommonTutorialForm commonTutorialForm = Mod.UI.GetUIForm(UIFormId.CommonTutorialForm) as CommonTutorialForm;
					if (commonTutorialForm.fingerRoot.gameObject.activeInHierarchy)
					{
						commonTutorialForm.OnButtonTrigger(pButton);
					}
				}
				break;
			}
		}
	}
}
