using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
	public GameObject m_lockState;

	public GameObject m_Selected;

	public GameObject m_downloadFlag;

	public GameObject m_enterLevelBtn;

	public GameObject m_GoldBaseContainer;

	public GameObject m_levelInfoContainer;

	public Text m_diamondNumTxt;

	public Text m_crownNumTxt;

	public Text m_levelNameTxt;

	public Text m_progress;

	public Text m_GoldBaseTxt;

	public Text m_lockTxt;

	public Color m_NormalColor;

	public Color m_SelectColor;

	public GameObject cup0;

	public GameObject cup1;

	public GameObject cup2;

	public GameObject topCup0;

	public GameObject topCup1;

	public GameObject topCup2;

	public GameObject topCup0ClickEffect;

	public GameObject topCup1ClickEffect;

	public GameObject unlockEffect;

	public HomeGetCupRoot getCupRoot;

	private LevelListController m_levelListController;

	private Levels_levelTable levelData;

	private LevelUnLock_table levelUnLockTable;

	private PlayerLocalLevelData m_levelLogicData;

	private int m_currentLevelId = -1;

	private Dictionary<int, int> m_unLockDic = new Dictionary<int, int>();

	public bool m_isSelected;

	private List<int> m_keyUnLockList = new List<int>();

	private Scenes_sceneTable SceneData;

	private bool m_isPrepared;

	private bool EnterLevelIfPossible;

	private LevelUpdateData m_updateData;

	private int m_downloadID;

	private bool m_needShowUnlockEffect;

	private LevelMetaTableData m_metaData;

	private bool enterLevelAnim;

	private bool isFirstSetSelect = true;

	public LevelMetaTableData MetaData
	{
		get
		{
			return m_metaData;
		}
		set
		{
			m_metaData = value;
		}
	}

	public bool BundleReady { get; private set; }

	private bool LevelLocked
	{
		get
		{
			return m_levelLogicData.LockState > 0;
		}
	}

	public void SetData(LevelMetaTableData metaData, LevelListController controller)
	{
		AddEventHandler();
		m_metaData = metaData;
		m_currentLevelId = MetaData.LevelId;
		m_levelListController = controller;
		m_isPrepared = MonoSingleton<GameTools>.Instacne.IsPreparing(m_currentLevelId);
		levelData = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(metaData.LevelId);
		SceneData = GetSceneData(levelData.Id);
		m_levelLogicData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(levelData.Id);
		CheckLevelBundle();
		ShowContent();
		EnterLevelIfPossible = false;
		m_downloadID = -1;
	}

	public void OnShow()
	{
		if (m_isSelected)
		{
			SetTopCups();
		}
		if (m_needShowUnlockEffect)
		{
			unlockEffect.SetActive(true);
			m_needShowUnlockEffect = false;
		}
	}

	public void OnHide()
	{
		getCupRoot.gameObject.SetActive(false);
	}

	private void AddEventHandler()
	{
		Mod.Event.Subscribe(EventArgs<LevelBundleCheckCompleteEventArgs>.EventId, OnLevelBundleCheckComplete);
		Mod.Event.Subscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnBunldeUpdateAllComplete);
		Mod.Event.Subscribe(EventArgs<ChangeLanguageArgs>.EventId, ChangeLanguage);
	}

	private void RemoveEventHandler()
	{
		Mod.Event.Unsubscribe(EventArgs<LevelBundleCheckCompleteEventArgs>.EventId, OnLevelBundleCheckComplete);
		Mod.Event.Unsubscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnBunldeUpdateAllComplete);
		Mod.Event.Unsubscribe(EventArgs<ChangeLanguageArgs>.EventId, ChangeLanguage);
	}

	public void ClickTips0()
	{
		PlayerDataModule.Instance.PlayerAllLocalData.SetLocalLevelIsClickCupTips0(m_currentLevelId, 1);
		HideTopCupClickEffect();
	}

	public void ClickTips1()
	{
		PlayerDataModule.Instance.PlayerAllLocalData.SetLocalLevelIsClickCupTips1(m_currentLevelId, 1);
		HideTopCupClickEffect();
	}

	private void ShowContent()
	{
		if (!LevelLocked)
		{
			ShowUnLockContent();
		}
		else
		{
			ShowLockContent();
		}
	}

	private void ShowUnLockContent()
	{
		ShowLevelInfo(false);
	}

	private void ShowLockContent()
	{
		ShowLevelInfo(true);
		RefreshLockState(m_currentLevelId, (LevelDifficulty)MetaData.DifficultDrgee);
	}

	private void RefreshLockState(int levelId, LevelDifficulty degree)
	{
		if (m_isPrepared)
		{
			return;
		}
		int num = -1;
		int num2 = -1;
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		if (CompatibleOldVersionUnLock())
		{
			num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(LevelLockType.NONE, 0, ChengJiuType.PROGRESS);
			dataModule.SetLevelLockState(m_currentLevelId, num2);
			ShowUnLockContent();
			Log.Info(string.Format("{0} 关卡,兼容老用户打开", m_currentLevelId));
			return;
		}
		dataModule.GetPlayerLevelData(levelId);
		Levels_levelTable levelTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId);
		levelUnLockTable = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelUnLockById(int.Parse(levelTableById.UnLockIds));
		if (levelUnLockTable == null)
		{
			return;
		}
		LevelLockType lockType = (LevelLockType)levelUnLockTable.LockType;
		if (lockType != LevelLockType.CHENGJIU)
		{
			return;
		}
		num = levelUnLockTable.LockTypeId;
		int unLockNum = levelUnLockTable.UnLockNum;
		List<int> list = MonoSingleton<GameTools>.Instacne.StringToIntList(levelUnLockTable.LinkLevel);
		if (num == 3)
		{
			int num3 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num3 += dataModule.GetLevelMaxProgress(list[i]);
			}
			if (num3 >= unLockNum)
			{
				num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(lockType, 0, ChengJiuType.PROGRESS);
				dataModule.SetLevelLockState(m_currentLevelId, num2);
				ShowUnLockContent();
				m_needShowUnlockEffect = true;
			}
		}
	}

	public bool CompatibleOldVersionUnLock()
	{
		return m_levelLogicData.MaxProgress > 0;
	}

	private void ShowLevelInfo(bool islock)
	{
		if (m_isPrepared)
		{
			m_progress.text = "Coming soon";
			m_lockState.SetActive(false);
			m_progress.gameObject.SetActive(true);
		}
		else if (islock)
		{
			m_lockState.SetActive(true);
			m_progress.gameObject.SetActive(false);
		}
		else
		{
			m_lockState.SetActive(false);
			m_progress.gameObject.SetActive(true);
			int maxProgress = m_levelLogicData.MaxProgress;
			m_progress.text = maxProgress + "%";
		}
		if (MetaData.DifficultDrgee == 0)
		{
			m_levelNameTxt.text = Mod.Localization.GetInfoById(221);
		}
		else if (MetaData.DifficultDrgee == 1)
		{
			m_levelNameTxt.text = Mod.Localization.GetInfoById(135);
		}
		else
		{
			m_levelNameTxt.text = Mod.Localization.GetInfoById(136);
		}
		string infoById = Mod.Localization.GetInfoById(258);
		m_GoldBaseTxt.text = string.Format(infoById, levelData.GoldAwardBase);
		SetCups();
	}

	private void HideTopCupClickEffect()
	{
		topCup0ClickEffect.SetActive(false);
		topCup1ClickEffect.SetActive(false);
	}

	private void HideTopCups()
	{
		topCup0.SetActive(false);
		topCup1.SetActive(false);
		topCup2.SetActive(false);
	}

	private void SetCups()
	{
		HideTopCups();
		HideTopCupClickEffect();
		cup0.SetActive(false);
		cup1.SetActive(false);
		cup2.SetActive(false);
		if (!m_isPrepared)
		{
			if (m_levelLogicData.IsPerfect == 1)
			{
				cup2.SetActive(true);
			}
			else if (m_levelLogicData.GetCollectedCrownCount() >= levelData.Crowns && m_levelLogicData.GetCollectedDiamoundCount() >= levelData.Diamonds)
			{
				cup1.SetActive(true);
			}
			else if (m_levelLogicData.MaxProgress >= 100)
			{
				cup0.SetActive(true);
			}
		}
	}

	public void SetTopCups()
	{
		if (HomeForm.Form != null && HomeForm.Form.State != HomeFormState.SelectLevel)
		{
			return;
		}
		HideTopCups();
		HideTopCupClickEffect();
		if (m_levelLogicData.IsPerfect == 1)
		{
			if (m_levelLogicData.isPerfectAllStarPlay == 0)
			{
				PlayerDataModule.Instance.PlayerAllLocalData.SetLocalLevelIsPerfectAllStarPlay(m_currentLevelId, 1);
				PlayerDataModule.Instance.PlayerAllLocalData.SetLocalLevelIsAllStarPlay(m_currentLevelId, 1);
				getCupRoot.Show(true);
				getCupRoot.moveFinished = delegate
				{
					topCup2.SetActive(true);
				};
			}
			else
			{
				topCup2.SetActive(true);
			}
		}
		else if (m_levelLogicData.GetCollectedCrownCount() >= levelData.Crowns && m_levelLogicData.GetCollectedDiamoundCount() >= levelData.Diamonds)
		{
			if (m_levelLogicData.isAllStarPlay == 0)
			{
				PlayerDataModule.Instance.PlayerAllLocalData.SetLocalLevelIsAllStarPlay(m_currentLevelId, 1);
				getCupRoot.Show(false);
				getCupRoot.moveFinished = delegate
				{
					topCup1.SetActive(true);
					topCup1ClickEffect.SetActive(m_levelLogicData.isClickCupTips1 == 0);
				};
			}
			else
			{
				topCup1.SetActive(true);
				topCup1ClickEffect.SetActive(m_levelLogicData.isClickCupTips1 == 0);
			}
		}
		else if (m_levelLogicData.MaxProgress >= 100)
		{
			topCup0.SetActive(true);
			topCup0ClickEffect.SetActive(m_levelLogicData.isClickCupTips0 == 0);
		}
	}

	public void SetItemSelected(bool isSelected)
	{
		m_isSelected = isSelected;
		if (isSelected)
		{
			m_Selected.SetActive(true);
			m_levelNameTxt.color = m_SelectColor;
			if (m_isPrepared)
			{
				m_levelInfoContainer.SetActive(false);
				m_enterLevelBtn.SetActive(false);
				m_lockTxt.gameObject.SetActive(true);
				m_lockTxt.text = Mod.Localization.GetInfoById(180);
				m_GoldBaseContainer.SetActive(false);
			}
			else
			{
				m_levelInfoContainer.SetActive(true);
				m_GoldBaseContainer.SetActive(false);
				if (!LevelLocked)
				{
					m_lockTxt.gameObject.SetActive(false);
					m_enterLevelBtn.SetActive(true);
				}
				else
				{
					m_lockTxt.gameObject.SetActive(true);
					if (levelUnLockTable != null)
					{
						m_lockTxt.text = Mod.Localization.GetInfoById(levelUnLockTable.Desc);
					}
					m_enterLevelBtn.SetActive(false);
				}
				if (!isFirstSetSelect)
				{
					SetTopCups();
				}
			}
			RefreshUpdateState();
		}
		else
		{
			m_Selected.SetActive(false);
			m_levelNameTxt.color = m_NormalColor;
		}
		int diamonds = levelData.Diamonds;
		int crowns = levelData.Crowns;
		int num = Mathf.Min(m_levelLogicData.GetCollectedDiamoundCount(), diamonds);
		m_diamondNumTxt.text = num + "/" + levelData.Diamonds;
		int num2 = Mathf.Min(m_levelLogicData.GetCollectedCrownCount(), crowns);
		m_crownNumTxt.text = num2 + "/" + levelData.Crowns;
		isFirstSetSelect = false;
	}

	private Scenes_sceneTable GetSceneData(int level)
	{
		int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(level);
		return Mod.DataTable.Get<Scenes_sceneTable>()[sceneIdByLevelId];
	}

	public void OnClick()
	{
		if (base.gameObject.activeSelf)
		{
			m_levelListController.SwitchLevel(m_currentLevelId);
			EnterLevelHandler();
		}
	}

	public void BtnSelect()
	{
		if (base.gameObject.activeSelf)
		{
			m_levelListController.SwitchLevel(m_currentLevelId);
		}
	}

	public void EnterLevelHandler()
	{
		if (!m_isPrepared && !LevelLocked && !enterLevelAnim && base.gameObject.activeInHierarchy && MenuForm.State == MenuFormState.SelectLevel)
		{
			NetworkVerifyForm.Verify(EnterLevelHandlerInternal);
		}
	}

	private void EnterLevelHandlerInternal()
	{
		MonoSingleton<GameTools>.Instacne.DisableInputForAWhile(1200u);
		StartCoroutine(DelayEnterLevel());
		enterLevelAnim = true;
		MenuForm.enteringLevel = true;
		InfocUtils.Report_rollingsky2_games_pageshow(2, 2, 2, m_currentLevelId);
	}

	private void EnterLevelFromMenuById(int levelId)
	{
		EnterLevelOperate(levelId);
		PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(levelId);
		if (playerLevelData != null && playerLevelData.LockState > 0 && !playerLevelData.IsCanFreeEnterLevel())
		{
			PlayerDataModule.Instance.SetLevelAdEnterTime(levelId, GameCommon.adFreeLevelEnterTime);
		}
	}

	private void EnterLevelOperate(int levelId)
	{
		MenuProcedure menuProcedure = Mod.Procedure.Current as MenuProcedure;
		if (menuProcedure != null)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId = levelId;
			PlayerDataModule.Instance.LastEndterLevelData.InitFromOther(MetaData);
			PlayerDataModule.Instance.SaveLastEnterLevelData();
			int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(levelId);
			menuProcedure.WillToScene(sceneIdByLevelId);
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).CurrentEnterLevelType = LevelEnterType.MENU;
		}
	}

	private IEnumerator DelayEnterLevel()
	{
		yield return new WaitForSeconds(0.3f);
		EnterLevelFromMenuById(m_currentLevelId);
		enterLevelAnim = false;
		MenuForm.enteringLevel = false;
	}

	private void FadeUI()
	{
		(Mod.UI.GetUIForm(UIFormId.HomeForm) as HomeForm).GetComponent<CanvasGroup>().DOFade(0f, 1f);
		HideTopCupClickEffect();
	}

	public void CheckLevelBundle()
	{
		if (SceneData.IsBuiltin == 1 || Mod.Core.EditorMode)
		{
			BundleReady = true;
			RefreshUpdateState();
		}
		else
		{
			Mod.Resource.LevelBundleCheck(SceneData.Id);
		}
	}

	public void RefreshUpdateState()
	{
		if (m_isSelected)
		{
			m_downloadFlag.gameObject.SetActive(!BundleReady);
		}
		if (m_isPrepared)
		{
			m_downloadFlag.gameObject.SetActive(false);
		}
	}

	private void OnLevelBundleCheckComplete(object sender, EventArgs args)
	{
		LevelBundleCheckCompleteEventArgs levelBundleCheckCompleteEventArgs = args as LevelBundleCheckCompleteEventArgs;
		if (levelBundleCheckCompleteEventArgs != null && levelBundleCheckCompleteEventArgs.Level == SceneData.Id)
		{
			BundleReady = !levelBundleCheckCompleteEventArgs.NeedUpdate;
			RefreshUpdateState();
			m_updateData = new LevelUpdateData(levelBundleCheckCompleteEventArgs.Level, levelBundleCheckCompleteEventArgs.NeedUpdate, levelBundleCheckCompleteEventArgs.UpdateCount, levelBundleCheckCompleteEventArgs.UpdateTotalLength);
			if (BundleReady && EnterLevelIfPossible && PlayerDataModule.Instance.GetPlayerLevelData(levelData.Id) != null)
			{
				EnterLevelFromMenuById(levelData.Id);
			}
		}
	}

	private void OnBunldeUpdateAllComplete(object sender, EventArgs args)
	{
		if (SceneData.Id == m_downloadID)
		{
			Mod.Resource.LevelBundleCheck(SceneData.Id);
			EnterLevelIfPossible = true;
		}
	}

	public void TryDownloadLevel()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
			commonAlertData.alertContent = Mod.Localization.GetInfoById(86);
			commonAlertData.callBackFunc = delegate
			{
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
			return;
		}
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			Mod.UI.OpenUIForm(UIFormId.LevelUpdateForm, m_updateData);
			Mod.Resource.StartUpdateLevelBundle(SceneData.Id);
			m_downloadID = SceneData.Id;
			return;
		}
		CommonAlertData commonAlertData2 = new CommonAlertData();
		commonAlertData2.showType = CommonAlertData.AlertShopType.COMMON;
		string arg = MonoSingleton<GameTools>.Instacne.CountSize(m_updateData.UpdateTotalLength);
		string text = (commonAlertData2.alertContent = string.Format(Mod.Localization.GetInfoById(85), arg));
		commonAlertData2.callBackFunc = delegate
		{
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			Mod.UI.OpenUIForm(UIFormId.LevelUpdateForm, m_updateData);
			Mod.Resource.StartUpdateLevelBundle(SceneData.Id);
			m_downloadID = SceneData.Id;
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData2);
	}

	private void ChangeLanguage(object sender, EventArgs e)
	{
		if (MetaData.DifficultDrgee == 0)
		{
			m_levelNameTxt.text = Mod.Localization.GetInfoById(221);
		}
		else if (MetaData.DifficultDrgee == 1)
		{
			m_levelNameTxt.text = Mod.Localization.GetInfoById(135);
		}
		else
		{
			m_levelNameTxt.text = Mod.Localization.GetInfoById(136);
		}
	}

	public void Release()
	{
		levelUnLockTable = null;
		m_levelLogicData = null;
		m_currentLevelId = -1;
		m_isSelected = false;
		SceneData = null;
		m_isPrepared = false;
		EnterLevelIfPossible = false;
		m_downloadID = -1;
		levelData = null;
		m_keyUnLockList.Clear();
		RemoveEventHandler();
	}
}
