using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelItemController : MonoBehaviour
{
	private Levels_levelTable levelData;

	private List<object> loadedAsserts = new List<object>();

	public GameObject m_lockState;

	public Button m_enterLevelBtn;

	public GameObject m_unLockEffect;

	private PlayerLocalLevelData m_levelLogicData;

	private bool m_isRelease;

	public Image m_bg;

	public Image m_RoleImage;

	public GameObject m_unLockContainer;

	public Text m_diamondNumTxt;

	public Text m_crownNumTxt;

	public Text m_levelNameTxt;

	public Text m_progress;

	public List<GameObject> m_starList;

	public GameObject m_lockContainer;

	public GameObject m_NormalContainer;

	public GameObject m_normalBgEffect;

	private bool m_isOpenAlert;

	private bool m_hadPlayUnLockEffect;

	private LevelMetaTableData m_metaData;

	private int m_currentLevelId = -1;

	private int m_ticketState;

	private Dictionary<int, int> m_unLockDic = new Dictionary<int, int>();

	public Color m_NormalColor;

	public Color m_FullColor;

	private AssetLoadCallbacks m_assetLoadBgCallBack;

	private AssetLoadCallbacks m_assetLoadRoleCallBack;

	public GameObject m_content;

	public CanvasGroup m_canvasGroup;

	public Canvas bg_Canvas;

	public Canvas content_Canvas;

	public GraphicRaycaster m_GrayRaycaster;

	public GameObject m_infoContent;

	private Tweener m_hideAnimation;

	private Tweener m_ShowAnimation;

	private bool m_isSelected;

	private List<int> m_keyUnLockList = new List<int>();

	private Scenes_sceneTable SceneData;

	public Image m_downloadFlag;

	public Animator m_animator;

	public CanvasGroup m_InfoCanvas;

	public Text m_PrepareTxt;

	public GameObject m_SelectBg;

	public GameObject m_SelectedLine;

	public GameObject m_UnSelectedLine;

	private bool EnterLevelIfPossible;

	private LevelUpdateData m_updateData;

	private int m_downloadID;

	private uint adTimerId;

	private LevelEnterForm m_maiForm;

	private Tween prePareTween;

	private const int LeaveFormPluginAdId = 11;

	private static int m_leaveFormTotalTime;

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

	public void SetData(LevelMetaTableData metaData, LevelEnterForm maiForm)
	{
		m_metaData = metaData;
		m_maiForm = maiForm;
		m_currentLevelId = MetaData.LevelId;
		levelData = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(metaData.LevelId);
		SceneData = GetSceneData(levelData.Id);
		m_levelLogicData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(levelData.Id);
		m_assetLoadBgCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isRelease)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else
			{
				if (m_bg != null)
				{
					m_bg.gameObject.SetActive(true);
					m_bg.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				}
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		});
		m_assetLoadRoleCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isRelease)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else
			{
				if (m_RoleImage != null)
				{
					m_RoleImage.gameObject.SetActive(true);
					m_RoleImage.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				}
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		});
		CheckLevelBundle();
		ShowContent();
		OnOpen();
		EnterLevelIfPossible = false;
		m_downloadID = -1;
	}

	public void StartTutorial()
	{
		base.gameObject.SetActive(true);
		StartCoroutine(DelayDealTutorial(0.05f));
	}

	private void SetAddButtonState()
	{
		if (BundleReady)
		{
			if (m_maiForm.m_UIGray != null)
			{
				if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.LevelLockView))
				{
					m_maiForm.m_UIGray.SetGrey(false);
					TimerHeap.DelTimer(adTimerId);
					m_maiForm.SetAdTextByState(true);
				}
				else
				{
					m_maiForm.m_UIGray.SetGrey(true);
					m_maiForm.SetAdTextByState(false);
				}
			}
		}
		else
		{
			if (m_maiForm.m_UIGray != null)
			{
				m_maiForm.m_UIGray.SetGrey(false);
			}
			m_maiForm.SetAdTextByState(true);
		}
	}

	private void OnOpen()
	{
		AddEventHandler();
		bg_Canvas.sortingOrder += 20;
		content_Canvas.sortingOrder += 31;
		m_InfoCanvas.alpha = 0f;
		m_animator.enabled = false;
		StopCenterEffect();
	}

	private void ShowContent()
	{
		m_unLockEffect.SetActive(false);
		m_bg.gameObject.SetActive(false);
		if (m_RoleImage != null)
		{
			m_RoleImage.gameObject.SetActive(false);
		}
		if (m_levelLogicData.LockState <= 0)
		{
			ShowUnLockContent();
		}
		else
		{
			ShowLockContent();
		}
		ShowBg();
		ShowRoleImage();
		bool flag = MonoSingleton<GameTools>.Instacne.IsPreparing(GetLevelId());
		m_PrepareTxt.gameObject.SetActive(flag);
		if (flag)
		{
			m_lockContainer.SetActive(false);
			m_infoContent.SetActive(false);
		}
	}

	private void ShowBg()
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
		string assetName = string.Format("levelbg_{0}", m_currentLevelId);
		Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(assetName), m_assetLoadBgCallBack);
	}

	private void ShowRoleImage()
	{
		string assetName = string.Format("levelrole_{0}", m_currentLevelId);
		Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(assetName), m_assetLoadRoleCallBack);
	}

	private void ShowLevelInfo()
	{
		m_infoContent.SetActive(true);
		GetLevelId();
		PlayerLocalLevelData playerLevelData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(levelData.Id);
		Levels_levelTable levelTableById = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelData.Id);
		int diamonds = levelTableById.Diamonds;
		int crowns = levelTableById.Crowns;
		int num = Mathf.Min(playerLevelData.GetCollectedDiamoundCount(), diamonds);
		m_diamondNumTxt.text = num + "/" + levelData.Diamonds;
		int num2 = Mathf.Min(playerLevelData.GetCollectedCrownCount(), crowns);
		m_crownNumTxt.text = num2 + "/" + levelData.Crowns;
		int maxProgress = playerLevelData.MaxProgress;
		m_progress.text = maxProgress + "%";
		int maxStarLevel = playerLevelData.MaxStarLevel;
		int[] array = new int[3];
		int num3 = 0;
		if (num >= levelData.Diamonds)
		{
			num3++;
		}
		if (maxProgress >= 100)
		{
			num3++;
		}
		if (num2 >= levelData.Crowns)
		{
			num3++;
		}
		for (int i = 0; i < num3; i++)
		{
			array[i] = 1;
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] == 0)
			{
				m_starList[j].transform.Find("crown_up").gameObject.SetActive(false);
				m_starList[j].transform.Find("crown_up_p").gameObject.SetActive(false);
			}
			else if (playerLevelData.IsPerfect == 1)
			{
				m_starList[j].transform.Find("crown_up_p").gameObject.SetActive(true);
				m_starList[j].transform.Find("crown_up").gameObject.SetActive(false);
			}
			else
			{
				m_starList[j].transform.Find("crown_up_p").gameObject.SetActive(false);
				m_starList[j].transform.Find("crown_up").gameObject.SetActive(true);
			}
		}
	}

	private void HideLevelInfo()
	{
		m_infoContent.SetActive(false);
	}

	private void ShowUnLockContent()
	{
		m_unLockContainer.SetActive(true);
		m_lockContainer.SetActive(false);
		ShowLevelInfo();
	}

	private void ShowLockContent()
	{
		m_unLockContainer.SetActive(false);
		m_lockContainer.SetActive(true);
		m_NormalContainer.SetActive(true);
		ShowLevelInfo();
		ShowLockState(m_currentLevelId, (LevelDifficulty)MetaData.DifficultDrgee);
	}

	private void ShowLockState(int levelId, LevelDifficulty degree)
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		dataModule.GetPlayerLevelData(levelId);
		string[] array = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId).UnLockIds.Split('|');
		LevelUnLock_table levelUnLock_table = null;
		List<bool> list = new List<bool>();
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < array.Length; i++)
		{
			levelUnLock_table = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelUnLockById(int.Parse(array[i]));
			if (levelUnLock_table == null)
			{
				continue;
			}
			LevelLockType lockType = (LevelLockType)levelUnLock_table.LockType;
			switch (lockType)
			{
			case LevelLockType.GOODS:
				num = levelUnLock_table.LockTypeId;
				if (num == 6)
				{
					int value = ComputerLevelUnlockKeyNum(MetaData.DifficultDrgee);
					if (m_unLockDic.ContainsKey(6))
					{
						m_unLockDic[6] = value;
					}
					else
					{
						m_unLockDic.Add(6, value);
					}
				}
				break;
			case LevelLockType.CHENGJIU:
			{
				num = levelUnLock_table.LockTypeId;
				int unLockNum = levelUnLock_table.UnLockNum;
				List<int> list2 = MonoSingleton<GameTools>.Instacne.StringToIntList(levelUnLock_table.LinkLevel);
				switch (num)
				{
				case 1:
				{
					int num5 = 0;
					for (int l = 0; l < list2.Count; l++)
					{
						num5 += dataModule.GetLevelMaxDiamond(list2[l]);
					}
					switch (degree)
					{
					case LevelDifficulty.NORMAL:
					{
						int desc = levelUnLock_table.Desc;
						int num6 = 0;
						if (num5 >= unLockNum)
						{
							InfocUtils.Report_rollingsky2_games_Unlock(2, m_currentLevelId);
							num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(lockType, 0, ChengJiuType.DIAMOND);
							if (levelUnLock_table.Relation == 1)
							{
								dataModule.SetLevelLockState(m_currentLevelId, num2);
								ShowUnLockContent();
								return;
							}
						}
						break;
					}
					case LevelDifficulty.Difficulty:
						if (num5 >= unLockNum)
						{
							num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(lockType, 0, ChengJiuType.DIAMOND);
							if (levelUnLock_table.Relation == 1)
							{
								dataModule.SetLevelLockState(m_currentLevelId, num2);
								ShowUnLockContent();
								return;
							}
							if (levelUnLock_table.Relation == 2)
							{
								list.Add(true);
							}
						}
						else if (levelUnLock_table.Relation == 2)
						{
							list.Add(false);
						}
						break;
					}
					break;
				}
				case 3:
				{
					if (degree != LevelDifficulty.Difficulty)
					{
						break;
					}
					int num4 = 0;
					for (int k = 0; k < list2.Count; k++)
					{
						num4 += dataModule.GetLevelMaxProgress(list2[k]);
					}
					if (num4 >= unLockNum)
					{
						if (levelUnLock_table.Relation == 1)
						{
							num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(lockType, 0, ChengJiuType.PROGRESS);
							dataModule.SetLevelLockState(m_currentLevelId, num2);
							ShowUnLockContent();
							return;
						}
						if (levelUnLock_table.Relation == 2)
						{
							list.Add(true);
						}
					}
					else if (levelUnLock_table.Relation == 2)
					{
						list.Add(false);
					}
					break;
				}
				case 2:
				{
					if (degree != LevelDifficulty.Difficulty)
					{
						break;
					}
					int num3 = 0;
					for (int j = 0; j < list2.Count; j++)
					{
						num3 += dataModule.GetLevelMaxCrown(list2[j]);
					}
					if (num3 >= unLockNum)
					{
						if (levelUnLock_table.Relation == 1)
						{
							num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(lockType, 0, ChengJiuType.CROWN);
							dataModule.SetLevelLockState(m_currentLevelId, num2);
							ShowUnLockContent();
							return;
						}
						if (levelUnLock_table.Relation == 2)
						{
							list.Add(true);
						}
					}
					else if (levelUnLock_table.Relation == 2)
					{
						list.Add(false);
					}
					break;
				}
				}
				break;
			}
			}
		}
		bool flag = true;
		for (int m = 0; m < list.Count; m++)
		{
			flag &= list[m];
		}
		if (list.Count > 0 && flag)
		{
			num2 = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(LevelLockType.CHENGJIU);
			dataModule.SetLevelLockState(m_currentLevelId, num2);
			ShowUnLockContent();
		}
	}

	private int GetLevelKeyUnlockNumById(int levelId)
	{
		string[] array = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelTableById(levelId).UnLockIds.Split('|');
		LevelUnLock_table levelUnLock_table = null;
		for (int i = 0; i < array.Length; i++)
		{
			levelUnLock_table = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelUnLockById(int.Parse(array[i]));
			if (levelUnLock_table == null)
			{
				continue;
			}
			LevelLockType lockType = (LevelLockType)levelUnLock_table.LockType;
			if (lockType != 0 && lockType == LevelLockType.GOODS)
			{
				int lockTypeId = levelUnLock_table.LockTypeId;
				if (lockTypeId == 6)
				{
					return levelUnLock_table.UnLockNum;
				}
			}
		}
		return 0;
	}

	private void ComputerKeyUnlockLevelList(int difficulty)
	{
		m_keyUnLockList.Clear();
		List<int> list = new List<int>();
		LevelChapters_table levelChapters_table = Mod.DataTable.Get<LevelChapters_table>()[MetaData.ChapterId];
		if (levelChapters_table != null)
		{
			string[] array = levelChapters_table.Sections.Split('|');
			int result = -1;
			for (int i = 0; i < array.Length; i++)
			{
				if (!int.TryParse(array[i], out result))
				{
					continue;
				}
				LevelSections_table levelSections_table = Mod.DataTable.Get<LevelSections_table>()[result];
				if (levelChapters_table == null)
				{
					continue;
				}
				string[] array2 = levelSections_table.Levels.Split('|');
				int result2 = -1;
				for (int j = 0; j < array2.Length; j++)
				{
					if (int.TryParse(array2[j], out result2) && j == difficulty)
					{
						list.Add(result2);
					}
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			m_keyUnLockList.Add(list[k]);
			if (list[k] == m_currentLevelId)
			{
				break;
			}
		}
	}

	private int ComputerLevelUnlockKeyNum(int difficulty)
	{
		int num = 0;
		ComputerKeyUnlockLevelList(difficulty);
		PlayerLocalLevelData playerLocalLevelData = null;
		for (int i = 0; i < m_keyUnLockList.Count; i++)
		{
			playerLocalLevelData = PlayerDataModule.Instance.GetPlayerLevelData(m_keyUnLockList[i]);
			if (playerLocalLevelData != null && playerLocalLevelData.LockState > 0)
			{
				num += GetLevelKeyUnlockNumById(m_keyUnLockList[i]);
			}
			if (m_keyUnLockList[i] == m_currentLevelId)
			{
				break;
			}
		}
		return num;
	}

	private int ComputerPrice(int levelId, int orinalPrice, int unLocktime)
	{
		return orinalPrice;
	}

	private void AddEventHandler()
	{
		Mod.Event.Subscribe(EventArgs<LevelBundleCheckCompleteEventArgs>.EventId, OnLevelBundleCheckComplete);
		Mod.Event.Subscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnBunldeUpdateAllComplete);
		m_enterLevelBtn.onClick.AddListener(delegate
		{
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_FIRST_LEVEL)
			{
				if (MonoSingleton<GameTools>.Instacne.IsPreparing(GetLevelId()))
				{
					if (prePareTween == null)
					{
						prePareTween = m_PrepareTxt.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 1f), 0.5f, 1);
						prePareTween.SetAutoKill(false);
					}
					else if (!prePareTween.IsPlaying())
					{
						prePareTween.Restart(false);
						prePareTween.Play();
					}
				}
				else if (m_maiForm != null)
				{
					m_maiForm.SwitchLevel(GetLevelId());
				}
			}
		});
		Mod.Event.Subscribe(EventArgs<LevelUnLockEventArgs>.EventId, OnLevelUnLockChange);
	}

	public void EnterLevelHandler()
	{
		if (!BundleReady)
		{
			TryDownloadLevel();
			return;
		}
		if (Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(1) > 0.0 || PlayerDataModule.Instance.PlayerRecordData.IsInNoConsumePowerTime())
		{
			EnterLevelFromMenuById(m_currentLevelId);
			InfocUtils.Report_rollingsky2_games_pageshow(2, 2, 2, m_currentLevelId);
			return;
		}
		TimeOutDialogForm.CurrentLevelId = m_currentLevelId;
		TimeOutDialogForm.ShowData showData = new TimeOutDialogForm.ShowData();
		showData.openFrom = TimeOutOpenFrom.MenuForm;
		Mod.UI.OpenUIForm(UIFormId.TimeOutDialogForm, showData);
	}

	private void OnLevelUnLockChange(object sender, EventArgs e)
	{
		LevelUnLockEventArgs levelUnLockEventArgs = e as LevelUnLockEventArgs;
		if (levelUnLockEventArgs == null)
		{
			return;
		}
		if (levelUnLockEventArgs.LevelId == levelData.Id && !m_isSelected)
		{
			ShowUnLockContent();
		}
		else if (levelData.LockState > 0)
		{
			int value = ComputerLevelUnlockKeyNum(MetaData.DifficultDrgee);
			if (m_unLockDic.ContainsKey(6))
			{
				m_unLockDic[6] = value;
			}
			else
			{
				m_unLockDic.Add(6, value);
			}
		}
	}

	public void AdUnLockkHandler()
	{
		InfocUtils.Report_rollingsky2_games_Unlock(1, m_currentLevelId);
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		if (m_ticketState == 1)
		{
			PlayerLocalLevelData playerLevelData = dataModule.GetPlayerLevelData(m_currentLevelId);
			dataModule.SetLevelTicketNum(m_currentLevelId, playerLevelData.GetTicketNum() - 1);
			EnterLevel(LevelLockData.EnterLevelType.TRY);
		}
		else if (BundleReady)
		{
			if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.LevelLockView))
			{
				InfocUtils.Report_rollingsky2_games_ads(3, 0, 1, m_currentLevelId, 3, 0);
				MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.LevelLockView, OnAddSuccess);
			}
		}
		else
		{
			TryDownloadLevel();
		}
	}

	public void KeyUnLockHandler()
	{
		InfocUtils.Report_rollingsky2_games_pageshow(5, 4, 2);
		PlayerDataModule pleyrDataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		int unLockNum = 0;
		if (!m_unLockDic.TryGetValue(6, out unLockNum))
		{
			return;
		}
		m_isOpenAlert = true;
		CommonAlertData commonAlertData = new CommonAlertData();
		commonAlertData.showType = CommonAlertData.AlertShopType.UNLOCK;
		commonAlertData.lableContent = Mod.Localization.GetInfoById(20);
		commonAlertData.iconid = Mod.DataTable.Get<Goods_goodsTable>()[6].IconId;
		commonAlertData.goodsNum = unLockNum;
		commonAlertData.callBackFunc = delegate
		{
			if (pleyrDataModule.GetPlayGoodsNum(6) >= (double)unLockNum)
			{
				InfocUtils.Report_rollingsky2_games_Unlock(4, m_currentLevelId);
				int state = MonoSingleton<GameTools>.Instacne.GenerateUnLockState(LevelLockType.GOODS, 6);
				pleyrDataModule.ChangePlayerGoodsNum(6, -unLockNum);
				for (int i = 0; i < m_keyUnLockList.Count; i++)
				{
					pleyrDataModule.SetLevelLockState(m_keyUnLockList[i], state);
				}
				UnLockLevelhandler(delegate
				{
					EnterLevel(LevelLockData.EnterLevelType.LOCKED);
				});
				Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).TryLevel = false;
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				m_isOpenAlert = false;
			}
		};
		commonAlertData.closeCallBackFunc = delegate
		{
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
	}

	private void EnterLevel(LevelLockData.EnterLevelType type)
	{
		if (BundleReady)
		{
			EnterLevelFromMenuById(m_currentLevelId);
		}
		else
		{
			TryDownloadLevel();
		}
	}

	private void RemoveEventHandler()
	{
		Mod.Event.Unsubscribe(EventArgs<LevelBundleCheckCompleteEventArgs>.EventId, OnLevelBundleCheckComplete);
		Mod.Event.Unsubscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnBunldeUpdateAllComplete);
		m_enterLevelBtn.onClick.RemoveAllListeners();
		Mod.Event.Unsubscribe(EventArgs<LevelUnLockEventArgs>.EventId, OnLevelUnLockChange);
	}

	private void OnAddSuccess(ADScene scene)
	{
		InfocUtils.Report_rollingsky2_games_ads(3, 0, 1, m_currentLevelId, 4, 0);
		PlayerDataModule.Instance.SetLevelTicketNum(m_currentLevelId, GameCommon.tyrLevelNumPerAd - 1);
		Mod.UI.CloseUIForm(UIFormId.LevelLockForm);
		if (PlayerDataModule.Instance.GetPlayGoodsNum(1) <= 0.0)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(1, 2.0);
		}
		EnterLevel(LevelLockData.EnterLevelType.TRY);
	}

	private Vector3 Vector3WorldToUI(Vector3 sourcePos)
	{
		Vector3 inputPos = Mod.UI.UICamera.WorldToScreenPoint(sourcePos);
		inputPos = MonoSingleton<GameTools>.Instacne.GetValidScreenPos(inputPos);
		return Mod.UI.UICamera.ScreenToWorldPoint(inputPos);
	}

	public void Release()
	{
		Reset();
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		levelData = null;
		m_isRelease = true;
		m_hadPlayUnLockEffect = false;
		if (m_hideAnimation != null)
		{
			m_hideAnimation.Kill();
		}
		if (m_ShowAnimation != null)
		{
			m_ShowAnimation.Kill();
		}
		m_isSelected = false;
		if (prePareTween != null)
		{
			prePareTween.Kill();
		}
		prePareTween = null;
		m_keyUnLockList.Clear();
	}

	public int GetLevelId()
	{
		if (levelData != null)
		{
			return levelData.Id;
		}
		return -1;
	}

	public void UnLockLevelhandler(UnityAction finishAction)
	{
		ShowUnLockContent();
		StartCoroutine(PlayUnLockEffect(1.25f, finishAction));
		MonoSingleton<GameTools>.Instacne.DisableInputForAWhile(1500u);
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

	private void EnterLevelFromMenuById(int levelId)
	{
		EnterLevelOperate(levelId);
		PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(levelId);
		if (playerLevelData != null && playerLevelData.LockState > 0 && !playerLevelData.IsCanFreeEnterLevel())
		{
			PlayerDataModule.Instance.SetLevelAdEnterTime(levelId, GameCommon.adFreeLevelEnterTime);
		}
	}

	private IEnumerator PlayUnLockEffect(float delta, UnityAction fininshAction)
	{
		m_GrayRaycaster.enabled = false;
		m_unLockEffect.SetActive(true);
		yield return new WaitForSeconds(delta);
		if (fininshAction != null)
		{
			fininshAction();
		}
		m_GrayRaycaster.enabled = true;
	}

	private void StopUnLockEffect()
	{
		m_unLockEffect.SetActive(false);
	}

	private void PlayCenterEffect()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		m_normalBgEffect.SetActive(true);
	}

	private IEnumerator RealPlayCenterEffect()
	{
		yield return new WaitForSeconds(0.1f);
		m_normalBgEffect.SetActive(true);
		if (MetaData.DifficultDrgee != 0)
		{
			int difficultDrgee = MetaData.DifficultDrgee;
			int num = 1;
		}
	}

	private void StopCenterEffect()
	{
		m_normalBgEffect.SetActive(false);
	}

	public void SetItemSelected(bool isSelected)
	{
		m_isSelected = isSelected;
		if (isSelected)
		{
			SetAddButtonState();
			TimerHeap.DelTimer(adTimerId);
			adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAddButtonState);
			PlayCenterEffect();
			m_InfoCanvas.DOFade(1f, 0.5f);
			m_animator.enabled = true;
			m_SelectBg.SetActive(false);
			m_SelectedLine.SetActive(true);
			m_UnSelectedLine.SetActive(false);
			base.transform.DOScale(1f, 0.3f);
			int value = 0;
			if (m_unLockDic.TryGetValue(6, out value))
			{
				m_maiForm.SetKeyUnLockNum(value);
			}
		}
		else
		{
			TimerHeap.DelTimer(adTimerId);
			StopCenterEffect();
			m_InfoCanvas.DOFade(0f, 0.5f);
			m_animator.enabled = false;
			m_SelectBg.SetActive(true);
			m_SelectedLine.SetActive(false);
			m_UnSelectedLine.SetActive(true);
			base.transform.DOScale(0.87f, 0.3f);
		}
	}

	public void ChangeAlpha(float alpha)
	{
		m_canvasGroup.alpha = alpha;
	}

	public void PlayContentShowAnimation()
	{
		CanvasGroup component = m_content.GetComponent<CanvasGroup>();
		if (m_hideAnimation != null)
		{
			m_hideAnimation.Kill();
		}
		m_content.SetActive(true);
		m_ShowAnimation = component.DOFade(1f, 0.4f);
	}

	public void PlayContentHideAnimation()
	{
		CanvasGroup component = m_content.GetComponent<CanvasGroup>();
		if (m_ShowAnimation != null)
		{
			m_ShowAnimation.Kill();
		}
		m_hideAnimation = component.DOFade(0f, 0.4f).OnComplete(delegate
		{
			m_content.SetActive(false);
		});
	}

	public float GetContentAlpha()
	{
		return m_canvasGroup.alpha;
	}

	public void Reset()
	{
		RemoveEventHandler();
		StopAllCoroutines();
		StopUnLockEffect();
		TimerHeap.DelTimer(adTimerId);
	}

	private IEnumerator DelayDealTutorial(float deltaTime)
	{
		yield return new WaitForSeconds(deltaTime);
		DealTutorial();
	}

	private void DealTutorial()
	{
		if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_FIRST_LEVEL && GetLevelId() == GameCommon.FIRST_LEVEL)
		{
			CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
			CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
			RectTransform rectTransform = m_maiForm.m_PlayBtn.transform as RectTransform;
			commonTutorialStepData.showContent = false;
			commonTutorialStepData.needBlock = true;
			commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
			commonTutorialStepData.posOffset = new Vector2(0f, 0f);
			commonTutorialStepData.changeRect = true;
			commonTutorialStepData.target = rectTransform;
			commonTutorialStepData.finishTargetActive = false;
			commonTutorialStepData.stepAction = delegate
			{
				EnterLevelFromMenuById(m_currentLevelId);
			};
			commonTutorialData.AddStep(commonTutorialStepData);
			BaseTutorialStep step = new CommonClickTutorialStep(commonTutorialData);
			TutorialManager.Instance.GetCurrentStage().AddStep(step);
			TutorialManager.Instance.GetCurrentStage().Execute();
		}
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

	private Scenes_sceneTable GetSceneData(int level)
	{
		int sceneIdByLevelId = MonoSingleton<GameTools>.Instacne.GetSceneIdByLevelId(level);
		return Mod.DataTable.Get<Scenes_sceneTable>()[sceneIdByLevelId];
	}

	public void RefreshUpdateState()
	{
		m_downloadFlag.gameObject.SetActive(!BundleReady);
	}

	private void OnLevelBundleCheckComplete(object sender, EventArgs args)
	{
		LevelBundleCheckCompleteEventArgs levelBundleCheckCompleteEventArgs = args as LevelBundleCheckCompleteEventArgs;
		if (levelBundleCheckCompleteEventArgs != null && levelBundleCheckCompleteEventArgs.Level == SceneData.Id)
		{
			BundleReady = !levelBundleCheckCompleteEventArgs.NeedUpdate;
			RefreshUpdateState();
			if (m_isSelected)
			{
				SetAddButtonState();
			}
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

	private bool DealLeaveformPluginAd()
	{
		m_leaveFormTotalTime++;
		int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(11);
		ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
		bool flag = false;
		if (screenPluginsAd_table != null)
		{
			flag |= m_leaveFormTotalTime >= screenPluginsAd_table.TriggerNum;
			flag &= MetaData.DifficultDrgee == 1;
			if (flag)
			{
				flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
				if (flag)
				{
					PluginAdData pluginAdData = new PluginAdData();
					pluginAdData.PluginId = 11;
					pluginAdData.EndHandler = delegate
					{
						EnterLevelOperate(m_currentLevelId);
						ClearLeaveFormPluginAdData();
					};
					Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
				}
			}
		}
		ClearLeaveFormPluginAdData();
		return flag;
	}

	private void ClearLeaveFormPluginAdData()
	{
		m_leaveFormTotalTime = 0;
	}
}
