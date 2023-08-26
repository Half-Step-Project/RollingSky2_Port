using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class FairyController : MonoBehaviour
{
	[SerializeField]
	[Label]
	private int mID;

	[SerializeField]
	private BaseFairy mFairy;

	private InsideGameDataModule GetInsideGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		}
	}

	public static FairyController Builder()
	{
		FairyController fairyController = new GameObject("FairyController").AddComponent<FairyController>();
		fairyController.SubscribeEvents();
		return fairyController;
	}

	public void CreateFairy(int id)
	{
		mID = id;
		mFairy = FairysManager.CreateFairy(id);
		if (mFairy != null)
		{
			mFairy.transform.parent = base.transform;
			mFairy.OnInstantiate();
			mFairy.SwitchState(FairyState.Ready);
		}
	}

	public void OnUpdate()
	{
		if (mFairy != null)
		{
			mFairy.UpdateState();
		}
	}

	public void DestroyLocal()
	{
		UnSubscribeEvents();
		if (mFairy != null)
		{
			mFairy.DestroyLocal();
		}
		Object.Destroy(base.gameObject);
	}

	public bool IsSkillPlaying()
	{
		if (mFairy == null)
		{
			return false;
		}
		return mFairy.IsSkillPlaying();
	}

	private void SubscribeEvents()
	{
		Mod.Event.Subscribe(EventArgs<SelectSkillEventArgs>.EventId, HandleSelectSkill);
		Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, HandleFormOpen);
		Mod.Event.Subscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, HandleClickGameStart);
		Mod.Event.Subscribe(EventArgs<GameFailEventArgs>.EventId, HandleGameFail);
		Mod.Event.Subscribe(EventArgs<GameSucessEventArgs>.EventId, HandleGameSucess);
		Mod.Event.Subscribe(EventArgs<GameResetEventArgs>.EventId, HandleGameReset);
		Mod.Event.Subscribe(EventArgs<GameOriginRebirthResetEventArgs>.EventId, HandleGameRebirthReset);
	}

	private void UnSubscribeEvents()
	{
		Mod.Event.Unsubscribe(EventArgs<SelectSkillEventArgs>.EventId, HandleSelectSkill);
		Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, HandleFormOpen);
		Mod.Event.Unsubscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, HandleClickGameStart);
		Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, HandleGameFail);
		Mod.Event.Unsubscribe(EventArgs<GameSucessEventArgs>.EventId, HandleGameSucess);
		Mod.Event.Unsubscribe(EventArgs<GameResetEventArgs>.EventId, HandleGameReset);
		Mod.Event.Unsubscribe(EventArgs<GameOriginRebirthResetEventArgs>.EventId, HandleGameRebirthReset);
	}

	private void HandleSelectSkill(object sender, EventArgs e)
	{
		if (!(mFairy == null))
		{
			SelectSkillEventArgs selectSkillEventArgs = e as SelectSkillEventArgs;
			if (selectSkillEventArgs != null)
			{
				mFairy.PlaySkill(selectSkillEventArgs.mActive, selectSkillEventArgs.mSkillNames);
			}
		}
	}

	private void HandleFormOpen(object sender, EventArgs e)
	{
		if (mFairy == null)
		{
			return;
		}
		UIMod.OpenSuccessEventArgs openSuccessEventArgs = e as UIMod.OpenSuccessEventArgs;
		if (openSuccessEventArgs != null && (bool)(openSuccessEventArgs.UIForm.Logic as TutorialGameStartForm) && Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).CurrentOriginRebirth != null)
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<SelectSkillEventArgs>().Initialize(false, FairySkillsName.REBIRTH));
			Log.Info("------------->被动触发 复活时 精灵的技能写在这里 ");
			if (openSuccessEventArgs.UIForm.Logic as TutorialGameStartForm != null)
			{
				Mod.Event.Fire(this, Mod.Reference.Acquire<GameStartButtonActiveEventArgs>().Initialize(false));
			}
		}
	}

	private void HandleClickGameStart(object sender, EventArgs e)
	{
		if (!(mFairy == null) && e is ClickGameStartButtonEventArgs)
		{
			if (GetInsideGameDataModule.CurrentOriginRebirth != null)
			{
				mFairy.SwitchState(FairyState.RebirthStartRun);
			}
			else
			{
				mFairy.SwitchState(FairyState.StartRun);
			}
		}
	}

	private void HandleGameFail(object sender, EventArgs e)
	{
		if (!(mFairy == null) && e is GameFailEventArgs)
		{
			mFairy.SwitchState(FairyState.Failure);
		}
	}

	private void HandleGameSucess(object sender, EventArgs e)
	{
		if (!(mFairy == null) && e is GameSucessEventArgs)
		{
			mFairy.SwitchState(FairyState.Success);
		}
	}

	private void HandleGameReset(object sender, EventArgs e)
	{
		if (!(mFairy == null) && e is GameResetEventArgs)
		{
			mFairy.SwitchState(FairyState.Ready);
		}
	}

	private void HandleGameRebirthReset(object sender, EventArgs e)
	{
		if (!(mFairy == null) && e is GameOriginRebirthResetEventArgs)
		{
			mFairy.SwitchState(FairyState.RebirthReady);
		}
	}

	private FairySkillsName[] IsEnable()
	{
		List<FairySkillsName> list = new List<FairySkillsName>();
		if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.START_FREE_SHIELD))
		{
			list.Add(FairySkillsName.SHIELD);
		}
		if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.ORIGIN_REBIRTH_FREE))
		{
			list.Add(FairySkillsName.REBIRTH);
		}
		if (PlayerDataModule.Instance.BufferIsEnable(GameCommon.GuideLine))
		{
			list.Add(FairySkillsName.PATHGUIDE);
		}
		return list.ToArray();
	}
}
