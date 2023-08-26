using System;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSeriesController : MonoBehaviour
{
	private class WaitForm
	{
		public UIFormId formId;

		public Func<bool> checkFunc;

		public bool needWait;

		public WaitForm(UIFormId formId, Func<bool> checkFunc)
		{
			this.formId = formId;
			this.checkFunc = checkFunc;
			needWait = false;
		}

		public bool Check()
		{
			if (checkFunc == null)
			{
				return false;
			}
			needWait = checkFunc();
			return needWait;
		}
	}

	private enum WaitToPlay
	{
		None,
		AllStar,
		Perfect
	}

	private const int emptyLevelSeriesId = 7;

	private LevelSeries_table levelData;

	private bool isInited;

	private GameObject modelObj;

	public bool isPlayed;

	private Vector3 titlePos = Vector3.zero;

	private List<object> loadedAsserts = new List<object>();

	public Image m_title;

	public Text m_SeriesName;

	public Transform moveAllStar;

	public GameObject perfectAllStarGo;

	public GameObject perfectAllStarEffectGo;

	public GameObject moveCompleteEffect;

	public LevelListController m_LevelListController;

	private bool m_isUpdateUnLockTime;

	private HomeForm m_HomeForm;

	private BoxCollider m_boxCollider;

	public GameObject commonSoon;

	public GameObject m_openContainer;

	public GameObject m_lockContainer;

	public Text m_lockInfo;

	public CanvasGroup m_CanvasGroup;

	public GameObject m_starGameObj;

	private List<LevelItemMaterialData> m_bgMaterialDataList;

	private bool m_isBgLoaded;

	private bool m_isRelease;

	private bool m_isShow;

	private bool m_isShowBg;

	private int m_index = -1;

	private Vector3 targetPosition = Vector3.zero;

	private Vector3 leftPosition = Vector3.zero;

	private Vector3 rightPosition = Vector3.zero;

	private Vector3 hidePosition = Vector3.zero;

	private Vector3 upPosition = Vector3.zero;

	private bool m_isBuiltin;

	private LevelUpdateData m_updateData;

	private bool EnterLevelIfPossible;

	private bool m_hadPlayUnLockEffect;

	private bool isSetPos;

	private bool m_isLock;

	private static List<WaitForm> waitForms = new List<WaitForm>();

	private int currentStars;

	private int maxStars;

	private bool isAllPerfect;

	private WaitToPlay waitToPlay;

	private uint timerId;

	public bool BundleReady { get; private set; }

	public bool BgLoaded
	{
		get
		{
			return m_isBgLoaded;
		}
	}

	public int Index
	{
		get
		{
			return m_index;
		}
	}

	private void Awake()
	{
		AddEventListener();
	}

	private void OnDestroy()
	{
		moveAllStar.DOKill();
		CancelInvoke();
	}

	private void OnGetAllStar(object sender, Foundation.EventArgs e)
	{
		GetAllStarEventArgs getAllStarEventArgs = e as GetAllStarEventArgs;
		if (getAllStarEventArgs != null && getAllStarEventArgs.LevelSeriesId == levelData.Id)
		{
			moveAllStar.gameObject.SetActive(true);
		}
	}

	private void ToPlayPerfect()
	{
		perfectAllStarEffectGo.SetActive(true);
		perfectAllStarGo.SetActive(true);
	}

	public void SetData(LevelSeries_table data, HomeForm homeForm, int index)
	{
		m_index = index;
		m_isRelease = false;
		m_HomeForm = homeForm;
		levelData = data;
		EnterLevelIfPossible = false;
		moveAllStar.gameObject.SetActive(false);
		perfectAllStarEffectGo.SetActive(false);
		moveCompleteEffect.SetActive(false);
		Init2DUI();
		Init();
		LevelMetaTableData levelMetaTableData = new LevelMetaTableData();
		levelMetaTableData.SeriesId = levelData.Id;
		m_LevelListController.Init(levelMetaTableData);
		m_openContainer.transform.Find("enterBtn").gameObject.SetActive(false);
	}

	private void Init()
	{
		if (isInited)
		{
			return;
		}
		string spriteName = string.Format("Series_{0}", levelData.Id);
		Mod.Resource.LoadAsset(AssetUtility.GetGameUIItemAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isRelease || m_isBgLoaded)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else
			{
				modelObj = UnityEngine.Object.Instantiate(asset as GameObject);
				loadedAsserts.Add(asset);
				Init3DUI();
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message '{2}'.", spriteName, assetName, errorMessage));
		}));
		isInited = true;
	}

	private void Init3DUI()
	{
		m_HomeForm.SetLevelBgContainer(modelObj.transform);
		float[] levelBgTransformInfo = GetLevelBgTransformInfo(GetSeriesId());
		if (levelBgTransformInfo != null && levelBgTransformInfo.Length != 0)
		{
			targetPosition = new Vector3(levelBgTransformInfo[2], levelBgTransformInfo[3], m_index * 10);
			modelObj.transform.localScale = new Vector3(levelBgTransformInfo[0], levelBgTransformInfo[1], 1f);
			print("BG 3D Size : CMInfo");
		}
		else
		{
			targetPosition = new Vector3(0f, 0f, m_index * 10);
			float num = 720f;
			float num2 = 1280f;
			float num3 = num / num2;
			float num4 = (float)Screen.width * 1f;
			float num5 = (float)Screen.height * 1f;
			float num6 = 1f;
			float num7 = num4 / num5;
			float num8 = 605f * num2 / (num * 1024f);
			if (num7 > num3)
			{
				num6 = num4 * num2 / (num * num5);
				num6 *= num8;
			}
			float num9 = num6;
			modelObj.transform.localScale = new Vector3(num9, num9, 1f);
			print("BG 3D Size : RTAdapt");
		}
		modelObj.transform.localPosition = targetPosition;
		rightPosition = new Vector3(1f, modelObj.transform.localPosition.y, m_index * 10);
		leftPosition = new Vector3(-1f, modelObj.transform.localPosition.y, m_index * 10);
		hidePosition = new Vector3(0f, modelObj.transform.localPosition.y, (float)(m_index * 10) * -1f);
		upPosition = new Vector3(0f, modelObj.transform.localPosition.y, 5f);
		float x = modelObj.transform.localScale.x;
		modelObj.SetActive(m_isShow);
		modelObj.transform.localRotation = Quaternion.identity;
		modelObj.name = "Series_" + levelData.Id;
		if (m_bgMaterialDataList == null)
		{
			m_bgMaterialDataList = new List<LevelItemMaterialData>();
		}
		string text = "";
		Renderer renderer = null;
		Transform transform = null;
		Material[] array = null;
		LevelItemMaterialData levelItemMaterialData = null;
		int seriesId = GetSeriesId();
		for (int i = 0; i <= 15; i++)
		{
			if (i == 0)
			{
				text = string.Format("animation/anim01/Level_{0}_{1}", seriesId, "BG");
				transform = modelObj.transform.Find(text);
				if (transform != null)
				{
					transform.transform.localScale = new Vector3(transform.transform.localScale.x, transform.transform.localScale.y, transform.transform.localScale.z * -1f);
				}
			}
			else
			{
				text = string.Format("Level_{0}_{1}", seriesId, i);
				transform = MonoSingleton<GameTools>.Instacne.RecursiveFindChild(modelObj.transform, text);
			}
			if (transform != null)
			{
				renderer = transform.GetComponent<Renderer>();
			}
			if (!(renderer != null))
			{
				continue;
			}
			array = ((!Mod.Core.EditorMode) ? renderer.sharedMaterials : renderer.materials);
			int j = 0;
			for (int num10 = array.Length; j < num10; j++)
			{
				if (array[j] != null)
				{
					levelItemMaterialData = new LevelItemMaterialData(array[j]);
					m_bgMaterialDataList.Add(levelItemMaterialData);
				}
			}
		}
		if (modelObj != null)
		{
			ChangeBoxCollider(x);
		}
		m_isBgLoaded = true;
	}

	private void ChangeBoxCollider(float scale)
	{
		m_boxCollider = modelObj.GetComponent<BoxCollider>();
		float x = Mod.UI.GetUIGroup("First").transform.localScale.x;
		x = x * 350f / scale;
		if ((bool)m_boxCollider)
		{
			m_boxCollider.size = new Vector3(x, scale * 0.8f, 1f);
		}
	}

	private void OnDrawGizmos()
	{
		if (!(modelObj == null))
		{
			Bounds bounds = modelObj.transform.GetComponentInChildren<MeshRenderer>().bounds;
			Vector3 center = bounds.center;
			float magnitude = bounds.extents.magnitude;
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(center, magnitude);
		}
	}

	private void SetTilteName()
	{
		m_SeriesName.gameObject.SetActive(false);
		m_SeriesName.gameObject.SetActive(true);
		m_SeriesName.text = Mod.Localization.GetInfoById(levelData.Name);
	}

	private void Init2DUI()
	{
		int seriesId = GetSeriesId();
		SetTilteName();
		commonSoon.SetActive(false);
		if (seriesId == 7)
		{
			m_openContainer.SetActive(false);
			m_lockContainer.SetActive(false);
		}
		else
		{
			m_openContainer.SetActive(true);
		}
	}

	public void PlayeCommonSoonEffect()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(commonSoon.transform.DOScale(1.2f, 0.5f));
		sequence.Append(commonSoon.transform.DOScale(1f, 0.5f));
		sequence.Play();
	}

	private void LevelTargetHandler(GameObject obj)
	{
		Mod.UI.OpenUIForm(UIFormId.LevelTargetForm, levelData.Id);
	}

	private void AdapterScreen()
	{
	}

	private Vector3 Vector3WorldToUI(Vector3 sourcePos)
	{
		Vector3 position = Mod.UI.UICamera.WorldToScreenPoint(sourcePos);
		return Mod.UI.UICamera.ScreenToWorldPoint(position);
	}

	public void Release()
	{
		m_LevelListController.Release();
		StopAllCoroutines();
		if (modelObj != null)
		{
			UnityEngine.Object.Destroy(modelObj);
			modelObj = null;
		}
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		for (int j = 0; j < m_bgMaterialDataList.Count; j++)
		{
			m_bgMaterialDataList[j].Release();
		}
		m_bgMaterialDataList.Clear();
		m_isBgLoaded = false;
		isInited = false;
		levelData = null;
		m_isShow = false;
		m_isRelease = true;
		m_hadPlayUnLockEffect = false;
	}

	public int GetSeriesId()
	{
		if (levelData != null)
		{
			return levelData.Id;
		}
		return -1;
	}

	private void FadeBgMaterial(float targetAlpha, float time)
	{
		for (int i = 0; i < m_bgMaterialDataList.Count; i++)
		{
			m_bgMaterialDataList[i].DoFadeAlpha(targetAlpha, time);
		}
	}

	public void ShowCurrentLevel(MoveDirection direction)
	{
		m_HomeForm.ChangeBgColorById(true, GetSeriesId());
		base.gameObject.SetActive(true);
		m_isShow = true;
		if (modelObj != null)
		{
			modelObj.SetActive(m_isShow);
		}
		if (direction != MoveDirection.NONE && modelObj != null)
		{
			m_HomeForm.ShowPreIemBg();
			m_HomeForm.PlayingTweenAniation = true;
			SetBgMaterialAlpha(0.2f);
			switch (direction)
			{
			case MoveDirection.NONE:
				modelObj.transform.localPosition = targetPosition;
				break;
			case MoveDirection.LEFT:
				modelObj.transform.localPosition = leftPosition;
				break;
			case MoveDirection.RIGHT:
				modelObj.transform.localPosition = rightPosition;
				break;
			}
			FadeBgMaterial(1f, 0.3f);
			modelObj.transform.DOLocalMove(targetPosition, 0.4f).OnComplete(delegate
			{
				m_CanvasGroup.DOFade(1f, 0.5f);
				m_HomeForm.PlayingTweenAniation = false;
				m_HomeForm.HidePreIemBg();
			});
			m_CanvasGroup.alpha = 0f;
		}
		else
		{
			m_HomeForm.PlayingTweenAniation = false;
			if (modelObj != null)
			{
				modelObj.transform.localPosition = new Vector3(0f, modelObj.transform.localPosition.y, 5f);
			}
			m_CanvasGroup.alpha = 1f;
		}
	}

	public void HideCurrentLevel(MoveDirection direction)
	{
		m_CanvasGroup.DOFade(0f, 0.4f).OnComplete(delegate
		{
			base.gameObject.SetActive(false);
		});
		m_isShow = false;
		Vector3 endValue = Vector3.zero;
		int index = m_index;
		if (direction != MoveDirection.NONE && modelObj != null)
		{
			switch (direction)
			{
			case MoveDirection.NONE:
				endValue = targetPosition;
				break;
			case MoveDirection.LEFT:
				modelObj.transform.localPosition = targetPosition;
				endValue = new Vector3(targetPosition.x - 0.1f, modelObj.transform.localPosition.y, rightPosition.z);
				break;
			case MoveDirection.RIGHT:
				index += 2;
				modelObj.transform.localPosition = new Vector3(targetPosition.x, targetPosition.y, index * 10);
				endValue = new Vector3(targetPosition.x + 0.1f, modelObj.transform.localPosition.y, index * 10);
				break;
			}
			SetBgMaterialAlpha(1f);
			modelObj.transform.DOLocalMove(endValue, 0.4f);
			FadeBgMaterial(0f, 0.32f);
		}
	}

	private void SetBgMaterialAlpha(float alpha)
	{
		for (int i = 0; i < m_bgMaterialDataList.Count; i++)
		{
			m_bgMaterialDataList[i].SetMaterialAlpha(alpha);
		}
	}

	public void SetBgShow(bool isShow)
	{
		m_isShowBg = isShow;
		if (modelObj != null)
		{
			modelObj.SetActive(m_isShowBg);
			SetBgMaterialAlpha(1f);
			if (!m_isShowBg)
			{
				modelObj.transform.localPosition = targetPosition;
			}
		}
	}

	public void OnOpen()
	{
		if (modelObj != null)
		{
			modelObj.SetActive(m_isShow);
		}
	}

	public void Reset()
	{
		StopAllCoroutines();
		m_isShow = false;
		if (modelObj != null)
		{
			modelObj.SetActive(false);
		}
		TimerHeap.DelTimer(timerId);
		RemoveEventListener();
	}

	private void CheckNewLevelLock()
	{
		bool flag = false;
		bool flag2 = false;
		PlayerLocalLevelData playerLocalLevelData = null;
		LevelOrder_levelOrderTable[] records = Mod.DataTable.Get<LevelOrder_levelOrderTable>().Records;
		for (int i = 0; i < records.Length; i++)
		{
			if (records[i].LevelSeriesId == levelData.Id)
			{
				flag = true;
			}
			else if (flag)
			{
				playerLocalLevelData = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayerLevelData(records[i].LevelSeriesId);
				if (playerLocalLevelData != null && playerLocalLevelData.IsNewUnLock())
				{
					flag2 = true;
					break;
				}
			}
		}
		if (flag2)
		{
			m_HomeForm.PlayHadNewUnLockEffect();
		}
		else
		{
			m_HomeForm.StopHadNewUnLockEffect();
		}
	}

	private float[] GetLevelBgTransformInfo(int levelId)
	{
		LevelsResolution_table levelsResolution_table = Mod.DataTable.Get<LevelsResolution_table>()[levelId];
		if (levelsResolution_table == null)
		{
			return null;
		}
		float num = (float)Screen.width * 1f;
		float num2 = (float)Screen.height * 1f;
		float num3 = num / num2;
		string text = num3.ToString("0.00");
		string value = (2f / 3f).ToString("0.00");
		string value2 = 0.75f.ToString("0.00");
		string value3 = 0.5625f.ToString("0.00");
		float num4 = 375f / 812f;
		string value4 = num4.ToString("0.00");
		float[] array = null;
		string text2 = "";
		if (text.Equals(value))
		{
			text2 = levelsResolution_table.Type_1;
		}
		else if (text.Equals(value2))
		{
			text2 = levelsResolution_table.Type_2;
		}
		else if (text.Equals(value3))
		{
			text2 = levelsResolution_table.Type_3;
		}
		else if (text.Equals(value4))
		{
			text2 = levelsResolution_table.Type_4;
		}
		else if (num3 < num4)
		{
			text2 = levelsResolution_table.Type_4;
		}
		if (!string.IsNullOrEmpty(text2))
		{
			CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
			cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
			string[] array2 = text2.Split('|');
			array = new float[5] { 1f, 1f, 0f, 0f, 0f };
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = float.Parse(array2[i], NumberStyles.Any, cultureInfo);
			}
		}
		return array;
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
			Mod.Resource.StartUpdateLevelBundle(levelData.Id);
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
			Mod.Resource.StartUpdateLevelBundle(levelData.Id);
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData2);
	}

	public void EnterLevel()
	{
		if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_NEXT_LEVEL && m_HomeForm.CanEnterLevel && m_HomeForm.State != 0 && !m_isLock)
		{
			m_LevelListController.EnterLevel();
		}
	}

	private bool CheckSeriesLock()
	{
		bool flag = false;
		int unlockStar = levelData.UnlockStar;
		int unlockLevel = levelData.UnlockLevel;
		int playerStarLevel = PlayerDataModule.Instance.GetPlayerStarLevel();
		int playerLevel = PlayerDataModule.Instance.GetPlayerLevel();
		if (unlockStar > playerStarLevel)
		{
			flag = true;
		}
		else if (unlockStar == playerStarLevel && unlockLevel > playerLevel)
		{
			flag = true;
		}
		if (flag && m_LevelListController.CompatibleOldVersionUnLock())
		{
			Log.Info(string.Format("{0} 章节,兼容老用户打开", levelData.Id));
			return false;
		}
		return flag;
	}

	private void RefreshLockInfo()
	{
		int unlockStar = levelData.UnlockStar;
		string text = ((levelData.UnlockStar != 0) ? string.Format(Mod.Localization.GetInfoById(256), levelData.UnlockStar, levelData.UnlockLevel) : string.Format(Mod.Localization.GetInfoById(257), levelData.UnlockLevel));
		m_lockInfo.text = text;
	}

	public void OnShow()
	{
		if (GetSeriesId() != 7)
		{
			m_isLock = false;
			if (m_isLock)
			{
				m_lockContainer.SetActive(true);
				m_openContainer.SetActive(false);
				RefreshLockInfo();
			}
			else
			{
				m_lockContainer.SetActive(false);
				m_openContainer.SetActive(true);
			}
			m_LevelListController.OnShow();
		}
	}

	public void OnHide()
	{
		m_LevelListController.OnHide();
	}

	private void AddEventListener()
	{
		Mod.Event.Subscribe(EventArgs<TrigerTutorialStepEventArgs>.EventId, OnTutorialTrigerHandler);
		Mod.Event.Subscribe(EventArgs<ChangeLanguageArgs>.EventId, ChangeLanguage);
		Mod.Event.Subscribe(EventArgs<EnterLevelArgs>.EventId, EnterLevelGame);
	}

	private void RemoveEventListener()
	{
		Mod.Event.Unsubscribe(EventArgs<TrigerTutorialStepEventArgs>.EventId, OnTutorialTrigerHandler);
		Mod.Event.Unsubscribe(EventArgs<ChangeLanguageArgs>.EventId, ChangeLanguage);
		Mod.Event.Unsubscribe(EventArgs<EnterLevelArgs>.EventId, EnterLevelGame);
	}

	private void ChangeLanguage(object sender, Foundation.EventArgs e)
	{
		m_SeriesName.text = Mod.Localization.GetInfoById(levelData.Name);
	}

	private void EnterLevelGame(object sender, Foundation.EventArgs e)
	{
		if (base.gameObject.activeSelf)
		{
			EnterLevel();
		}
	}

	private void OnTutorialTrigerHandler(object sender, Foundation.EventArgs args)
	{
		TrigerTutorialStepEventArgs trigerTutorialStepEventArgs = args as TrigerTutorialStepEventArgs;
		if (trigerTutorialStepEventArgs != null && trigerTutorialStepEventArgs.StageStepId == TutorialStepId.STAGE_HOME_MENU_STEP_2 && GetSeriesId() == GameCommon.FIRST_SERIES)
		{
			DelayInvoke(1000u, DealTutorial);
		}
	}

	private void DelayInvoke(uint delayTime, UnityAction action)
	{
		TimerHeap.DelTimer(timerId);
		timerId = TimerHeap.AddTimer(delayTime, 0u, delegate
		{
			action();
		});
	}

	private void DealTutorial()
	{
		if (GetSeriesId() == GameCommon.FIRST_SERIES && TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_HOME_MENU)
		{
			CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
			CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
			RectTransform rectTransform = m_starGameObj.transform as RectTransform;
			commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
			commonTutorialStepData.posOffset = new Vector2(0f, 0f);
			commonTutorialStepData.needBlock = true;
			commonTutorialStepData.changeRect = false;
			commonTutorialStepData.target = rectTransform;
			commonTutorialStepData.stepType = TutorialStepType.CONTENT_AND_FINGER;
			commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(323);
			commonTutorialStepData.stepAction = delegate
			{
				EnterLevel();
			};
			commonTutorialData.AddStep(commonTutorialStepData);
			BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
		}
	}
}
