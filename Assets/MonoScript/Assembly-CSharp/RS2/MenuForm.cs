using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public sealed class MenuForm : UGUIForm
	{
		public DOTweenAnimation m_tweenAnimationTab;

		public DOTweenAnimation m_tweenAnimationBack;

		public GameObject m_musical;

		public GameObject m_start;

		public GameObject m_upgrade;

		public Button MusicBtn;

		public Button PlayerUpgradeBtn;

		private UIFormId currentOpenFormId;

		private int currentIndex;

		private List<GameObject> tabButtonList = new List<GameObject>();

		private Transform m_homeForm;

		private HomeForm m_homeFormLogic;

		private float m_canvasWidth = 720f;

		private float m_cameraMoveWidth = 1.125f;

		private bool m_isPlayMove;

		private int[] openFlag = new int[3];

		private List<UIFormId> tabFormList = new List<UIFormId>
		{
			UIFormId.HomeForm,
			UIFormId.ShopForm
		};

		public GameObject m_instrumentRedPoint;

		public GameObject m_playerUpgradeRedPoint;

		private int m_tempIndex = -1;

		private const int LeaveSettingFormPluginAdId = 4;

		private static int m_leaveSettingFormTotalTime = 0;

		private const int LevelShopFormPluginAdId = 8;

		private float pluginThroshholdTime = 5f;

		private float pluginDeltyTime;

		public GameObject m_language;

		public GameObject m_StartGame;

		public GameObject m_setting;

		public GameObject m_SelectStartGame;

		public GameObject m_BackToMenu;

		public GameObject m_languageForm;

		public GameObject m_settingForm;

		public static bool enteringLevel = false;

		public static bool isSelectLevel = false;

		public static bool isLoading = true;

		public bool enteringSelect;

		public GameObject BG_return;

		private IEnumerator backMenuCor;

		public static MenuForm Form = null;

		private const float CheckRedPointTime = 5f;

		private float checkRedPointCounter;

		public UIFormId CurrentOpenFormId
		{
			get
			{
				return tabFormList[currentIndex];
			}
		}

		public static MenuFormState State { get; private set; }

		public int TempIndex
		{
			set
			{
				m_tempIndex = value;
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_cameraMoveWidth = Mod.UI.UICamera.aspect * 2f;
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[8];
			if (screenPluginsAd_table != null)
			{
				pluginThroshholdTime = screenPluginsAd_table.TriggerNum;
			}
			InitUI();
		}

		private void InitUI()
		{
			tabButtonList.Add(m_musical);
			tabButtonList.Add(m_start);
			tabButtonList.Add(m_upgrade);
			EventTriggerListener.Get(tabButtonList[0]).onClick = Temp0;
			EventTriggerListener.Get(tabButtonList[1]).onClick = Temp1;
			EventTriggerListener.Get(tabButtonList[2]).onClick = Temp2;
			EventTriggerListener.Get(m_language).onClick = Temp4;
			EventTriggerListener.Get(m_setting).onClick = Temp5;
		}

		private void OnTabItemClickHandler(GameObject obj)
		{
			int num = tabButtonList.FindIndex((GameObject x) => x.name == obj.name);
			if (num == -1)
			{
				Debug.LogError("index == -1 is occurce error click object name:" + obj.name);
			}
			if (currentIndex != num)
			{
				OpenTabItemByIndex(num);
			}
		}

		private void Temp0(GameObject obj)
		{
		}

		public void Temp1(GameObject obj)
		{
			enteringSelect = true;
			if (State == MenuFormState.Education)
			{
				StartCoroutine(DelayGotoSelectLevel(0.15f));
			}
		}

		private IEnumerator DelayStartSelectLevel()
		{
			bool isAction = true;
			int i = 0;
			while (isAction)
			{
				yield return new WaitForSeconds(0.1f);
				i++;
				if (EducationDisplayDirector.Instance._homeForm != null)
				{
					isAction = false;
				}
				if (i > 50)
				{
					isAction = false;
				}
			}
			EducationDisplayDirector.Instance.OnSelectLevel();
			yield return new WaitForSeconds(0.35f);
			BG_return.SetActive(false);
		}

		private IEnumerator DelayGotoSelectLevel(float time)
		{
			yield return new WaitForSeconds(time);
			EducationDisplayDirector.Instance.OnSelectLevel();
		}

		private IEnumerator DelayBackMenu()
		{
			yield return new WaitForEndOfFrame();
			State = MenuFormState.Education;
		}

		private void Temp2(GameObject obj)
		{
		}

		public void Temp3(GameObject obj)
		{
			if (!enteringLevel && TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_HOME_MENU && TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_NEXT_LEVEL)
			{
				EducationDisplayDirector.Instance.OnSelectLevelBack();
			}
		}

		public void Temp4(GameObject obj)
		{
			if (!enteringSelect)
			{
				State = MenuFormState.LanguageSetting;
				m_languageForm.SetActive(true);
			}
		}

		public void CloseLanguageForm()
		{
			m_languageForm.SetActive(false);
			if (backMenuCor != null)
			{
				StopCoroutine(backMenuCor);
			}
			backMenuCor = DelayBackMenu();
			StartCoroutine(backMenuCor);
		}

		public void Temp5(GameObject obj)
		{
			if (!enteringSelect)
			{
				State = MenuFormState.SettingPage;
				m_settingForm.SetActive(true);
				m_settingForm.GetComponent<Setting2Form>().Init();
			}
		}

		public void CloseSettingForm()
		{
			m_settingForm.SetActive(false);
			if (backMenuCor != null)
			{
				StopCoroutine(backMenuCor);
			}
			backMenuCor = DelayBackMenu();
			StartCoroutine(backMenuCor);
		}

		public void EnterGame()
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<EnterLevelArgs>());
		}

		public void BackMenu()
		{
			Temp3(null);
		}

		private void ShowTabByIndex(int index, bool playEffect = true)
		{
			for (int i = 0; i < tabButtonList.Count; i++)
			{
				if (i == index)
				{
					OnTabChangeHandler(tabButtonList[i], true, i, playEffect);
				}
				else
				{
					OnTabChangeHandler(tabButtonList[i], false, i, playEffect);
				}
			}
		}

		private void OpenTabItemByIndex(int index)
		{
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			Form = this;
			m_canvasWidth = MonoSingleton<GameTools>.Instacne.GetGlobalCanvasWith();
			for (int i = 0; i < tabFormList.Count; i++)
			{
				Mod.UI.OpenUIForm(tabFormList[i]);
			}
			PlayerDataModule.Instance.PlayerRecordData.AddOpenMenuFormCount();
			InitTweenParam();
			m_instrumentRedPoint.SetActive(false);
			m_playerUpgradeRedPoint.SetActive(false);
			CheckMusicInstrumentFormRedPoint();
			CheckPlayerUpgradeFormRedPoint();
			AddEventListener();
			m_tempIndex = -1;
			if (isSelectLevel)
			{
				BG_return.SetActive(true);
				StartCoroutine(DelayStartSelectLevel());
			}
			else
			{
				BG_return.SetActive(false);
			}
		}

		private void OnEnable()
		{
			InitTweenParam();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			ShowTabByIndex(-1);
			for (int i = 0; i < tabFormList.Count; i++)
			{
				Mod.UI.CloseUIForm(tabFormList[i]);
			}
			currentIndex = 0;
			m_isPlayMove = false;
			currentOpenFormId = UIFormId.Undefined;
			for (int j = 0; j < openFlag.Length; j++)
			{
				openFlag[j] = 0;
			}
			RemoveEventListener();
			Form = null;
		}

		private void AddEventListener()
		{
			Mod.Event.Subscribe(EventArgs<TrigerTutorialStepEventArgs>.EventId, OnTutorialTrigerHandler);
			Mod.Event.Subscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenFormSuccess);
		}

		private void OnOpenFormSuccess(object sender, Foundation.EventArgs e)
		{
			UIMod.OpenSuccessEventArgs openSuccessEventArgs = (UIMod.OpenSuccessEventArgs)e;
			if (openSuccessEventArgs != null && openSuccessEventArgs.UIForm.Logic is MusicInstrumentForm)
			{
				if (m_tempIndex != 0)
				{
					Mod.UI.CloseUIForm(UIFormId.MusicalInstrumentForm);
				}
			}
			else if (openSuccessEventArgs != null && openSuccessEventArgs.UIForm.Logic is PlayerUpgradeForm && m_tempIndex != 2)
			{
				Mod.UI.CloseUIForm(UIFormId.PlayerUpgradeForm);
			}
		}

		private void OnTutorialTrigerHandler(object sender, Foundation.EventArgs args)
		{
			TrigerTutorialStepEventArgs trigerTutorialStepEventArgs = args as TrigerTutorialStepEventArgs;
			if (trigerTutorialStepEventArgs != null && trigerTutorialStepEventArgs.StageStepId == TutorialStepId.STAGE_HOME_MENU_STEP_1 && TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_HOME_MENU)
			{
				CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				RectTransform rectTransform = m_start.transform as RectTransform;
				commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
				commonTutorialStepData.posOffset = new Vector2(0f, 0f);
				commonTutorialStepData.showContent = true;
				commonTutorialStepData.needBlock = true;
				commonTutorialStepData.changeRect = false;
				commonTutorialStepData.target = rectTransform;
				commonTutorialStepData.stepType = TutorialStepType.CONTENT_AND_FINGER;
				commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(322);
				commonTutorialStepData.stepAction = delegate
				{
					TutorialManager.Instance.ShowUIGroup(default(int));
					Temp1(m_start);
					Mod.Event.Fire(this, Mod.Reference.Acquire<TrigerTutorialStepEventArgs>().Initialize(TutorialStepId.STAGE_HOME_MENU_STEP_2));
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
			}
		}

		private void RemoveEventListener()
		{
			Mod.Event.Unsubscribe(EventArgs<TrigerTutorialStepEventArgs>.EventId, OnTutorialTrigerHandler);
			Mod.Event.Unsubscribe(EventArgs<UIMod.OpenSuccessEventArgs>.EventId, OnOpenFormSuccess);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (currentIndex == 1)
			{
				pluginDeltyTime += elapseSeconds;
			}
			MusicBtn.interactable = !Mod.UI.UIFormIsOpen(UIFormId.MusicalInstrumentForm);
			PlayerUpgradeBtn.interactable = !Mod.UI.UIFormIsOpen(UIFormId.PlayerUpgradeForm);
			checkRedPointCounter += Time.deltaTime;
			if (checkRedPointCounter >= 5f)
			{
				checkRedPointCounter -= 5f;
				CheckMusicInstrumentFormRedPoint();
				CheckPlayerUpgradeFormRedPoint();
			}
		}

		private void CheckMusicInstrumentFormRedPoint()
		{
			DateTime musicInstrumentAdTime = PlayerDataModule.Instance.GetMusicInstrumentAdTime();
			bool flag = (DateTime.Now - musicInstrumentAdTime).TotalSeconds >= (double)GameCommon.showAdInInstrument;
			int index = 0;
			int num = 10086;
			int num2 = 0;
			List<InstrumentData> list = new List<InstrumentData>();
			List<PlayerLocalInstrumentData> allInstrumentDataList = PlayerDataModule.Instance.GetAllInstrumentDataList();
			bool flag2 = false;
			for (int i = 0; i < allInstrumentDataList.Count; i++)
			{
				PlayerLocalInstrumentData playerLocalInstrumentData = allInstrumentDataList[i];
				InstrumentData instrumentData = new InstrumentData();
				list.Add(instrumentData);
				instrumentData.m_id = playerLocalInstrumentData.m_Id;
				instrumentData.m_lv = playerLocalInstrumentData.m_Level;
				instrumentData.m_ifLowest = false;
				if (playerLocalInstrumentData.m_lockState <= 0 && num > playerLocalInstrumentData.m_Level)
				{
					index = i;
					num = playerLocalInstrumentData.m_Level;
				}
				instrumentData.m_ifLock = playerLocalInstrumentData.m_lockState > 0;
				if (!instrumentData.m_ifLock)
				{
					num2++;
				}
				instrumentData.m_lvlup = 1;
				instrumentData.m_output = playerLocalInstrumentData.GetProductReputationBaseNum(PlayerDataModule.Instance.GetPlayerStarLevel(), playerLocalInstrumentData.m_Level);
				instrumentData.m_nextlvl = playerLocalInstrumentData.GetProductReputationBaseNum(PlayerDataModule.Instance.GetPlayerStarLevel(), playerLocalInstrumentData.m_Level + 1);
				double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(3);
				instrumentData.m_desStr = playerLocalInstrumentData.Name();
				instrumentData.m_unlckstarlevel = playerLocalInstrumentData.GetUnLockNeedStarLevel();
				instrumentData.m_unlocklevel = playerLocalInstrumentData.GetUnLockNeedLevel();
				instrumentData.m_instrumentCanUpMaxLevel = PlayerDataModule.Instance.InstrumentCanUpMaxLevel();
				instrumentData.m_instrumentCanUpMaxLevelShow = PlayerDataModule.Instance.GetPlayerLevel() + 1;
				if (instrumentData.m_lvlup < 0)
				{
					int num3 = playerLocalInstrumentData.m_Level;
					int level = playerLocalInstrumentData.m_Level;
					double num4 = 0.0;
					double num5 = 0.0;
					do
					{
						level = num3;
						num3++;
						if (num3 > instrumentData.m_instrumentCanUpMaxLevel)
						{
							break;
						}
						num4 = playerLocalInstrumentData.GetUpLevelConsumeCount(PlayerDataModule.Instance.GetPlayerStarLevel(), num3);
						num5 += num4;
					}
					while (!(num5 >= playGoodsNum) && num3 < 360);
					instrumentData.m_lvlup = Mathf.Max(level, playerLocalInstrumentData.m_Level + 1) - playerLocalInstrumentData.m_Level;
				}
				instrumentData.m_lvlup = Mathf.Min(360 - playerLocalInstrumentData.m_Level - 1, instrumentData.m_lvlup);
				instrumentData.m_coin = GetTotalCoinByAddition(instrumentData.m_lvlup, playerLocalInstrumentData);
				instrumentData.m_ifBtnEnable = !instrumentData.m_ifLock && instrumentData.m_coin <= playGoodsNum;
				flag2 = (instrumentData.m_ifBtnEnable && instrumentData.m_lv + instrumentData.m_lvlup <= PlayerDataModule.Instance.InstrumentCanUpMaxLevel()) || flag2;
			}
			if (num2 >= 3 && flag)
			{
				list[index].m_ifLowest = true;
				if (list[index].m_nextlvl <= (double)list[index].m_instrumentCanUpMaxLevel)
				{
					m_instrumentRedPoint.SetActive(true);
				}
				else
				{
					m_instrumentRedPoint.SetActive(false);
				}
			}
			else
			{
				m_instrumentRedPoint.SetActive(false);
			}
			if (flag2)
			{
				m_instrumentRedPoint.SetActive(true);
			}
		}

		private double GetTotalCoinByAddition(int addition, PlayerLocalInstrumentData ii)
		{
			double num = 0.0;
			for (int i = 1; i < addition + 1 && ii.m_Level + i < 360; i++)
			{
				num += ii.GetUpLevelConsumeCount(PlayerDataModule.Instance.GetPlayerStarLevel(), checked(ii.m_Level + i));
			}
			return num;
		}

		private void CheckPlayerUpgradeFormRedPoint()
		{
			bool ifHaveEnoughLevel = false;
			bool ifHaveEnoughCoin = false;
			bool flag = PlayerDataModule.Instance.IfCanPlayerParticularStarUpgradeByAbilityLevel(PlayerDataModule.Instance.GetPlayerStarLevel(), ref ifHaveEnoughLevel, ref ifHaveEnoughCoin);
			m_playerUpgradeRedPoint.SetActive(flag || ifHaveEnoughLevel);
		}

		private void OnTabChangeHandler(GameObject obj, bool isSelect, int index, bool isPlayEffect = true)
		{
			CheckMusicInstrumentFormRedPoint();
			CheckPlayerUpgradeFormRedPoint();
			if (openFlag[index] == 0)
			{
				return;
			}
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(obj);
			GameObject gameObject = dictionary["select"];
			GameObject gameObject2 = dictionary["icon"];
			if (isSelect)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
				if (!isPlayEffect)
				{
					return;
				}
				GameObject effect = dictionary["effect"];
				Image effectImage = effect.GetComponent<Image>();
				if (effect != null)
				{
					effect.SetActive(true);
					effectImage.color = new Color(1f, 1f, 1f, 1f);
					effect.transform.localScale = Vector3.one;
					effect.transform.DOScale(new Vector3(1.25f, 1.25f, 1f), 0.5f).OnComplete(delegate
					{
						effect.transform.localScale = Vector3.one;
					});
					effectImage.DOFade(0f, 0.5f).OnComplete(delegate
					{
						effectImage.color = new Color(1f, 1f, 1f, 1f);
						effect.SetActive(false);
					});
				}
			}
			else
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
				if (gameObject2 != null)
				{
					gameObject2.SetActive(true);
				}
			}
		}

		private void InitTweenParam()
		{
			m_tweenAnimationTab.endValueV3 = new Vector3(0f, -713f, 0f);
			m_tweenAnimationBack.endValueV3 = new Vector3(0f, -577f, 0f);
			m_tweenAnimationTab.CreateTween();
			m_tweenAnimationBack.CreateTween();
		}

		public void SetState(MenuFormState state)
		{
			State = state;
			CanvasGroup BGCanvasGroup = base.gameObject.transform.Find("BG").GetComponent<CanvasGroup>();
			CanvasGroup TapCanvasGroup = m_tweenAnimationTab.gameObject.GetComponent<CanvasGroup>();
			CanvasGroup BackCanvasGroup = m_tweenAnimationBack.gameObject.GetComponent<CanvasGroup>();
			float myFloat;
			switch (state)
			{
			case MenuFormState.Education:
				myFloat = BGCanvasGroup.alpha;
				TapCanvasGroup.blocksRaycasts = true;
				BackCanvasGroup.blocksRaycasts = false;
				DOTween.To(() => myFloat, delegate(float x)
				{
					myFloat = x;
				}, 1f, 0.5f).OnUpdate(delegate
				{
					BGCanvasGroup.alpha = myFloat;
					TapCanvasGroup.alpha = myFloat;
					BackCanvasGroup.alpha = 1f - myFloat;
				});
				enteringSelect = false;
				isSelectLevel = false;
				break;
			case MenuFormState.SelectLevel:
				myFloat = BGCanvasGroup.alpha;
				TapCanvasGroup.blocksRaycasts = false;
				BackCanvasGroup.blocksRaycasts = true;
				DOTween.To(() => myFloat, delegate(float x)
				{
					myFloat = x;
				}, 0f, 0.5f).OnUpdate(delegate
				{
					BGCanvasGroup.alpha = myFloat;
					TapCanvasGroup.alpha = myFloat;
				});
				DOTween.To(() => myFloat, delegate(float x)
				{
					myFloat = x;
				}, 0f, 0.5f).OnUpdate(delegate
				{
					if (State == MenuFormState.SelectLevel)
					{
						BackCanvasGroup.alpha = 1f - myFloat;
					}
				}).SetDelay(1.2f);
				enteringSelect = false;
				isSelectLevel = true;
				break;
			}
		}

		private bool DealSettingformPluginAd()
		{
			m_leaveSettingFormTotalTime++;
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(4);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = false;
			if (screenPluginsAd_table != null)
			{
				flag |= m_leaveSettingFormTotalTime >= screenPluginsAd_table.TriggerNum;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 4;
						pluginAdData.EndHandler = delegate
						{
							ClearSettingFormPluginAdData();
						};
						Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			return flag;
		}

		private void ClearSettingFormPluginAdData()
		{
			m_leaveSettingFormTotalTime = 0;
		}

		private bool DealShopingformPluginAd()
		{
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(8);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = true;
			if (screenPluginsAd_table != null)
			{
				flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
				if (flag)
				{
					PluginAdData pluginAdData = new PluginAdData();
					pluginAdData.PluginId = 8;
					pluginAdData.EndHandler = delegate
					{
						ClearShopingFormPluginAdData();
					};
					Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
				}
			}
			return flag;
		}

		private void ClearShopingFormPluginAdData()
		{
			pluginDeltyTime = 0f;
			PlayerDataModule.Instance.PluginAdController.CommonBuyRecord.RemoveAll((UIFormId x) => x == UIFormId.ShopForm);
		}

		public void OnMusicalBtnClick()
		{
			m_tempIndex = 0;
			Mod.UI.OpenUIForm(UIFormId.MusicalInstrumentForm, this);
			Mod.UI.CloseUIForm(UIFormId.PlayerUpgradeForm);
		}

		public void OnUpgradeBtnClick()
		{
		}

		private void SetNSButton(object sender, Foundation.EventArgs e)
		{
			List<RectTransform> list = new List<RectTransform>();
			float preferredWidth = m_start.transform.Find("Text").GetComponent<CustomText>().preferredWidth;
			float preferredWidth2 = m_language.transform.Find("Text").GetComponent<CustomText>().preferredWidth;
			float x = m_start.transform.Find("Image").GetComponent<RectTransform>().sizeDelta.x;
			list.Add(m_start.transform.Find("Text").GetComponent<RectTransform>());
			list.Add(m_language.transform.Find("Text").GetComponent<RectTransform>());
			float num = ((preferredWidth > preferredWidth2) ? preferredWidth : preferredWidth2);
			float num2 = (num + x) * 1.3f;
			float space = (num2 - (num + x)) / 3f;
			SetButtonGroup(list, num, num2, space, x);
			preferredWidth = m_StartGame.transform.Find("Text").GetComponent<CustomText>().preferredWidth;
			preferredWidth2 = m_BackToMenu.transform.Find("Text").GetComponent<CustomText>().preferredWidth;
			x = m_StartGame.transform.Find("Image").GetComponent<RectTransform>().sizeDelta.x;
			list = new List<RectTransform>();
			list.Add(m_StartGame.transform.Find("Text").GetComponent<RectTransform>());
			list.Add(m_BackToMenu.transform.Find("Text").GetComponent<RectTransform>());
			num = ((preferredWidth > preferredWidth2) ? preferredWidth : preferredWidth2);
			num2 = (num + x) * 1.3f;
			space = (num2 - (num + x)) / 3f;
			SetButtonGroup(list, num, num2, space, x);
		}

		private void SetButtonGroup(List<RectTransform> nsBtns, float textWidth, float buttonWidth, float space, float NSIconWidth)
		{
			foreach (RectTransform nsBtn in nsBtns)
			{
				nsBtn.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonWidth, nsBtn.parent.GetComponent<RectTransform>().sizeDelta.y);
				nsBtn.sizeDelta = new Vector2(textWidth, 108f);
				nsBtn.anchorMin = new Vector2(0f, 0.5f);
				nsBtn.anchorMax = new Vector2(0f, 0.5f);
				nsBtn.pivot = new Vector2(0f, 0.5f);
				nsBtn.GetComponent<CustomText>().alignment = TextAnchor.MiddleLeft;
				nsBtn.anchoredPosition = new Vector2(NSIconWidth + space * 2f, nsBtn.anchoredPosition.y);
				nsBtn.parent.Find("Image").GetComponent<RectTransform>().anchoredPosition = new Vector2(space, 0f);
			}
		}
	}
}
