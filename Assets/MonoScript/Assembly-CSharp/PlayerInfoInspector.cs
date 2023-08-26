using System;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInfoInspector : MonoBehaviour
{
	[SerializeField]
	private Text m_nameTex;

	[SerializeField]
	private Text m_levelTex;

	[SerializeField]
	private Text m_expInfo;

	[SerializeField]
	private Text m_messionInfo;

	[SerializeField]
	private Image m_CoinIcon;

	[SerializeField]
	private Text m_CoinTex;

	[SerializeField]
	private IncreaseSlider m_increaseSlider;

	[SerializeField]
	private ImageFade m_cursorFade;

	[SerializeField]
	private Button m_upgradeBtn;

	[SerializeField]
	private GameObject m_messionContent;

	[SerializeField]
	private GameObject m_noteEffect1;

	[SerializeField]
	private GameObject m_noteEffect2;

	[SerializeField]
	private Text m_RiseTex;

	[SerializeField]
	private Text m_SpeedTex;

	[SerializeField]
	private List<Image> m_starts;

	[SerializeField]
	private Image m_playerIcon;

	[SerializeField]
	private UIPersonalAssetsList m_PlayerAssetList;

	[Header("Color")]
	[SerializeField]
	private Color m_expCurrentNormal;

	[SerializeField]
	private Color m_expCurrentUpgrade;

	[SerializeField]
	private Color m_messionNormal;

	[SerializeField]
	private Color m_messionFinish;

	private double m_cacheSpeed;

	private object m_cacheIcon;

	private float m_cacheFill;

	private bool canClickUpgrade = true;

	private uint m_timerId;

	private int m_level
	{
		get
		{
			return m_playerData.GetPlayerLevel();
		}
	}

	private int m_expGoodsId
	{
		get
		{
			return m_playerData.GetPlayerUpLevelNeedGoodsId();
		}
	}

	private PlayerDataModule m_playerData
	{
		get
		{
			return PlayerDataModule.Instance;
		}
	}

	private void Start()
	{
		m_RiseTex.CreatePool(10);
		m_noteEffect1.CreatePool(30);
		m_noteEffect2.CreatePool(100);
		AddEvents();
		RefreshTitle();
		LoadAndApplyPlayerIcon();
		RefreshStar();
		RefreshLevel();
		RefreshExp();
		RefreshMession();
		RefreshGold();
		InvokeRepeating("RefreshSpeed", 0f, GameCommon.instrumentProductFrequency);
		m_PlayerAssetList.OnInit();
		m_PlayerAssetList.OnOpen(UIPersonalAssetsList.ParentType.Home);
	}

	private void Update()
	{
		m_PlayerAssetList.OnUpdate();
	}

	private void AddEvents()
	{
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnGoodsChange);
		Mod.Event.Subscribe(EventArgs<MessionChangeEventArgs>.EventId, OnMessionChange);
		Mod.Event.Subscribe(EventArgs<MusicalInstrumentClickEventArgs>.EventId, OnMusicalInstrumentClick);
		Mod.Event.Subscribe(EventArgs<PlayerUpgradeEventArg>.EventId, OnPlayerStarUpgrade);
		if (m_CoinIcon != null)
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_CoinIcon.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickGoldButton));
		}
	}

	private void RemoveEvents()
	{
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnGoodsChange);
		Mod.Event.Unsubscribe(EventArgs<MessionChangeEventArgs>.EventId, OnMessionChange);
		Mod.Event.Unsubscribe(EventArgs<MusicalInstrumentClickEventArgs>.EventId, OnMusicalInstrumentClick);
		Mod.Event.Unsubscribe(EventArgs<PlayerUpgradeEventArg>.EventId, OnPlayerStarUpgrade);
		if (m_CoinIcon != null)
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_CoinIcon.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickGoldButton));
		}
	}

	private void OnGoodsChange(object sender, Foundation.EventArgs args)
	{
		GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = args as GameGoodsNumChangeEventArgs;
		if (gameGoodsNumChangeEventArgs != null)
		{
			if (gameGoodsNumChangeEventArgs.GoodsId == m_expGoodsId && gameGoodsNumChangeEventArgs.ChangeType != AssertChangeType.INSTRUMENT_PLAY)
			{
				RefreshExp();
				RiseExp(gameGoodsNumChangeEventArgs.ChangeNum);
			}
			else if (gameGoodsNumChangeEventArgs.GoodsId == 3 && gameGoodsNumChangeEventArgs.ChangeType != AssertChangeType.PLAYER_UPGRADE)
			{
				RefreshGold();
			}
		}
	}

	private void RiseExp(double num)
	{
		string arg = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(Math.Abs(num));
		string text = ((!(num >= 0.0)) ? string.Format("-{0}", arg) : string.Format("+{0}", arg));
		RiseText(text);
	}

	private void OnClickGoldButton(GameObject obj)
	{
	}

	private void OnMessionChange(object sender, Foundation.EventArgs args)
	{
		if (args is MessionChangeEventArgs)
		{
			RefreshMession();
		}
	}

	private void RefreshTitle()
	{
		m_nameTex.text = m_playerData.GetPlayerTitleByStarLevel(PlayerDataModule.Instance.GetPlayerStarLevel());
	}

	private void RefreshStar()
	{
		int playerStarLevel = m_playerData.GetPlayerStarLevel();
		for (int i = 0; i < m_starts.Count; i++)
		{
			if (i >= playerStarLevel)
			{
				m_starts[i].gameObject.SetActive(false);
			}
			else
			{
				m_starts[i].gameObject.SetActive(true);
			}
		}
	}

	private void RefreshLevel()
	{
		m_levelTex.text = string.Format("Lv:{0}", m_level);
	}

	private void RefreshExp()
	{
		double playerUpLevelNeedGoodsNum = m_playerData.GetPlayerUpLevelNeedGoodsNum(m_level);
		double playGoodsNum = m_playerData.GetPlayGoodsNum(m_expGoodsId);
		string arg = ((!(playGoodsNum < playerUpLevelNeedGoodsNum)) ? string.Format("<color=#{0}>{1}</color>", ColorToHex(m_expCurrentUpgrade), MonoSingleton<GameTools>.Instacne.DoubleToFormatString(playGoodsNum)) : string.Format("<color=#{0}>{1}</color>", ColorToHex(m_expCurrentNormal), MonoSingleton<GameTools>.Instacne.DoubleToFormatString(playGoodsNum)));
		string text = string.Format("{0}/{1}", arg, MonoSingleton<GameTools>.Instacne.DoubleToFormatString(playerUpLevelNeedGoodsNum));
		m_expInfo.text = text;
		double num = playGoodsNum / playerUpLevelNeedGoodsNum;
		if (num > 1.0)
		{
			num = 1.0;
			m_upgradeBtn.gameObject.SetActive(true);
		}
		else
		{
			m_upgradeBtn.gameObject.SetActive(false);
		}
		StartIncrease(m_cacheFill, (float)num);
		m_cacheFill = (float)num;
	}

	private void StartIncrease(float start, float end)
	{
		m_increaseSlider.StartIncrease(start, end, 0.1f, 0.1f);
		m_cursorFade.StartFade(0.5f, 0.1f);
	}

	private void RefreshMession()
	{
		int playerUpLevelNeedFinishiMessionId = m_playerData.GetPlayerUpLevelNeedFinishiMessionId(m_level);
		if (playerUpLevelNeedFinishiMessionId != -1)
		{
			m_messionContent.SetActive(true);
			MessionProgressData messionPrgressPartList = m_playerData.GetMessionPrgressPartList(playerUpLevelNeedFinishiMessionId);
			Color color = ((!m_playerData.MessionIsFinished(playerUpLevelNeedFinishiMessionId)) ? m_messionNormal : m_messionFinish);
			string text = "";
			if (PlayerDataModule.Instance.GetMessionTypeById(playerUpLevelNeedFinishiMessionId) == MessionType.SPECIAL_LEVEL_PROGRESS)
			{
				text = "%";
			}
			string arg = string.Format("<color=#{0}>({1}/{2}{3})</color>", ColorToHex(color), messionPrgressPartList.Member_One, messionPrgressPartList.Member_Two, text);
			string text2 = string.Format("{0}{1}", m_playerData.GetMessionDesc(playerUpLevelNeedFinishiMessionId), arg);
			m_messionInfo.text = text2;
		}
		else
		{
			int targetLevel = -1;
			int nextEffectMessionId = PlayerDataModule.Instance.GetNextEffectMessionId(m_level, ref targetLevel);
			if (nextEffectMessionId > 0)
			{
				m_messionContent.SetActive(true);
				string text3 = string.Format(Mod.Localization.GetInfoById(314), targetLevel, m_playerData.GetMessionDesc(nextEffectMessionId));
				m_messionInfo.text = text3;
			}
			else
			{
				m_messionInfo.text = "";
				m_messionContent.SetActive(false);
			}
		}
	}

	private void RefreshGold()
	{
		string text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_playerData.GetPlayGoodsNum(3));
		m_CoinTex.text = text;
	}

	private void RefreshSpeed()
	{
		double currentProductReputaionSpeed = m_playerData.GetCurrentProductReputaionSpeed();
		if (m_cacheSpeed != currentProductReputaionSpeed)
		{
			m_cacheSpeed = currentProductReputaionSpeed;
			string arg = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_cacheSpeed);
			m_SpeedTex.text = string.Format("{0}/s", arg);
		}
	}

	public void OnUpgradeClick()
	{
		if (canClickUpgrade && !Mod.UI.UIFormIsOpen(UIFormId.GetGoodsForm))
		{
			int playerLevel = PlayerDataModule.Instance.GetPlayerLevel();
			bool isHadMession = PlayerDataModule.Instance.GetPlayerUpLevelNeedFinishiMessionId(playerLevel) > 0;
			int rewardId;
			ResultState state = m_playerData.UpPlayerLevel(out rewardId);
			HandleUpgrade(state, rewardId, isHadMession);
			canClickUpgrade = false;
			TimerHeap.DelTimer(m_timerId);
			m_timerId = TimerHeap.AddTimer(400u, 0u, delegate
			{
				canClickUpgrade = true;
			});
		}
	}

	public void OnPlayerIconClick()
	{
	}

	private void OnMusicalInstrumentClick(object sender, Foundation.EventArgs args)
	{
		if (EducationDisplayDirector.InEducation)
		{
			MusicalInstrumentClickEventArgs musicalInstrumentClickEventArgs = args as MusicalInstrumentClickEventArgs;
			FlyNoteEffect(musicalInstrumentClickEventArgs.Type, musicalInstrumentClickEventArgs.Postion, musicalInstrumentClickEventArgs.Product);
		}
	}

	private void OnPlayerStarUpgrade(object sender, Foundation.EventArgs args)
	{
		RefreshTitle();
		LoadAndApplyPlayerIcon();
		RefreshStar();
		RefreshLevel();
		RefreshExp();
		RefreshMession();
	}

	public void FlyNoteEffect(int noteType, Vector3 position, double product)
	{
		GameObject obj;
		if (noteType == 1)
		{
			obj = m_noteEffect1.Spawn();
		}
		else
		{
			obj = m_noteEffect2.Spawn();
		}
		Vector3 position2 = Camera.main.WorldToScreenPoint(position);
		Vector3 vector = Mod.UI.UICamera.ScreenToWorldPoint(position2);
		obj.transform.SetParent(base.transform);
		obj.transform.position = new Vector3(vector.x, vector.y, m_noteEffect1.transform.position.z);
		if (noteType == 1)
		{
			float x = UnityEngine.Random.Range(-0.1f, 0.1f);
			Vector3 endValue = m_increaseSlider.transform.position + new Vector3(x, 0f, 0f);
			endValue.z = m_noteEffect1.transform.position.z;
			obj.transform.DOJump(endValue, 0.2f, 1, 1f).OnComplete(delegate
			{
				RefreshExp();
				RiseExp(product);
				obj.Recycle();
			});
			return;
		}
		string arg = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(product);
		string text2 = string.Format("+{0}", arg);
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = obj.transform.localPosition + new Vector3(50f, 30f, 0f);
		Text text = obj.GetComponent<Text>();
		text.text = text2;
		Vector3 endValue2 = text.transform.localPosition + new Vector3(0f, 70f, 0f);
		PlayerDataModule.Instance.IsProductSpeedUpGoing();
		float duration = 1.1f;
		text.transform.DOLocalMove(endValue2, duration).OnComplete(delegate
		{
			obj.Recycle();
		}).SetEase(Ease.Unset);
		text.DOFade(1f, duration / 2f).OnComplete(delegate
		{
			text.DOFade(0f, duration / 2f);
		});
		Image image = obj.GetComponentInChildren<Image>();
		image.DOFade(1f, duration / 2f).OnComplete(delegate
		{
			image.DOFade(0f, duration / 2f);
		});
	}

	private void RiseText(string text)
	{
		if (EducationDisplayDirector.InEducation)
		{
			Text textObj = m_RiseTex.Spawn();
			textObj.text = text;
			textObj.gameObject.SetActive(true);
			textObj.transform.SetParent(m_RiseTex.transform.parent);
			textObj.transform.localScale = Vector3.one;
			textObj.transform.localPosition = m_RiseTex.transform.localPosition;
			float endValue = m_RiseTex.transform.localPosition.y + 30f;
			textObj.transform.DOLocalMoveY(endValue, 1f).OnComplete(delegate
			{
				textObj.Recycle();
			}).SetEase(Ease.Unset);
			textObj.DOFade(1f, 0.25f).OnComplete(delegate
			{
				textObj.DOFade(0f, 0.75f);
			});
		}
	}

	private void HandleUpgrade(ResultState state, int groupID, bool isHadMession)
	{
		switch (state)
		{
		case ResultState.SUCCESS:
			OpenGetGoodsForm(groupID, isHadMession);
			Mod.Sound.PlayUISound(20015);
			EducationDisplayDirector.Instance.OnPlayerUpgrade();
			break;
		case ResultState.MESSION_NOT_FINISH:
			CreateMessionAlert(m_playerData.IsUpLevelShowAd());
			break;
		}
		RefreshLevel();
		RefreshExp();
		RefreshMession();
	}

	private void OpenGetGoodsForm(int groupID, bool isHandMession)
	{
		GetGoodsData getGoodsData = new GetGoodsData();
		getGoodsData.GoodsTeamId = groupID;
		getGoodsData.GoodsTeamNum = 1;
		getGoodsData.GoodsTeam = true;
		getGoodsData.ShowGetPath = false;
		getGoodsData.Upgrade = true;
		if (!isHandMession)
		{
			getGoodsData.IsAutoPlayEffect = true;
			getGoodsData.CallBackFunc = delegate
			{
				Reward(groupID, 1);
			};
		}
		getGoodsData.NormalButtonCallback = delegate
		{
			Reward(groupID, 1);
		};
		getGoodsData.ADButtonCallback = delegate(UnityAction x)
		{
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.MainView, delegate
			{
				MonoSingleton<GameTools>.Instacne.EnableInput();
				Reward(groupID, 2);
				InfocUtils.Report_rollingsky2_games_ads(25, 0, 1, 0, 4, 0);
				x();
			});
		};
		Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
	}

	private void Reward(int groupID, int multiple)
	{
		Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(groupID).GetEnumerator();
		int num = -1;
		int num2 = -1;
		while (enumerator.MoveNext())
		{
			num = enumerator.Current.Key;
			num2 = enumerator.Current.Value * multiple;
			PlayerDataModule.Instance.ChangePlayerGoodsNum(num, num2, AssertChangeType.PLAYER_UPGRADE);
		}
	}

	private void CreateMessionAlert(bool isShowAD)
	{
		CommonAlertData commonAlertData = new CommonAlertData();
		commonAlertData.showType = CommonAlertData.AlertShopType.MESSION;
		commonAlertData.isADBtnShow = isShowAD;
		string infoById = Mod.Localization.GetInfoById(259);
		int playerUpLevelNeedFinishiMessionId = m_playerData.GetPlayerUpLevelNeedFinishiMessionId(m_level);
		MessionProgressData messionPrgressPartList = m_playerData.GetMessionPrgressPartList(playerUpLevelNeedFinishiMessionId);
		string arg = string.Format("<color=#{0}>({1}/{2})</color>", ColorToHex(m_messionNormal), messionPrgressPartList.Member_One, messionPrgressPartList.Member_Two);
		string messionDesc = m_playerData.GetMessionDesc(playerUpLevelNeedFinishiMessionId);
		string alertContent = string.Format(infoById, arg, messionDesc).Replace("\\n", "\n");
		bool islevel = m_playerData.IsLevelMession(playerUpLevelNeedFinishiMessionId);
		int id = (islevel ? 281 : 146);
		int levelID = -1;
		if (islevel)
		{
			levelID = m_playerData.GetMessionTypeId(playerUpLevelNeedFinishiMessionId);
		}
		commonAlertData.alertContent = alertContent;
		commonAlertData.lableContent = Mod.Localization.GetInfoById(id);
		commonAlertData.callBackFunc = delegate
		{
			if (islevel)
			{
				if (levelID != -1)
				{
					(Mod.UI.GetUIForm(UIFormId.HomeForm) as HomeForm).GoToLevel(levelID);
				}
				else
				{
					EducationDisplayDirector.Instance.OnSelectLevel();
				}
			}
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
		};
		if (isShowAD)
		{
			commonAlertData.adContent = Mod.Localization.GetInfoById(288);
			commonAlertData.adCallBackFunc = delegate
			{
				int playerUpLevelNeedFinishiMessionId2 = m_playerData.GetPlayerUpLevelNeedFinishiMessionId(m_level);
				m_playerData.FinishMession(new List<int> { playerUpLevelNeedFinishiMessionId2 });
				int playerLevel = PlayerDataModule.Instance.GetPlayerLevel();
				bool isHadMession = PlayerDataModule.Instance.GetPlayerUpLevelNeedFinishiMessionId(playerLevel) > 0;
				int rewardId;
				ResultState state = m_playerData.UpPlayerLevel(out rewardId);
				HandleUpgrade(state, playerUpLevelNeedFinishiMessionId2, isHadMession);
				InfocUtils.Report_rollingsky2_games_ads(30, 0, 1, 0, 4, 0);
			};
			commonAlertData.closeCallBackFunc = delegate
			{
				m_playerData.ResetUpLevelTime();
			};
		}
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
	}

	private void LoadAndApplyPlayerIcon()
	{
		string gameIconAsset = AssetUtility.GetGameIconAsset(PlayerDataModule.Instance.GetPlayerHeadIcon());
		Mod.Resource.LoadAsset(gameIconAsset, new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			m_playerIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
			if (m_cacheIcon != null)
			{
				Mod.Resource.UnloadAsset(m_cacheIcon);
			}
			m_cacheIcon = asset;
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		}));
	}

	public static string ColorToHex(Color color)
	{
		int num = Mathf.RoundToInt(color.r * 255f);
		int num2 = Mathf.RoundToInt(color.g * 255f);
		int num3 = Mathf.RoundToInt(color.b * 255f);
		int num4 = Mathf.RoundToInt(color.a * 255f);
		return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", num, num2, num3, num4);
	}

	private void OnDestroy()
	{
		CancelInvoke("RefreshSpeed");
		RemoveEvents();
		m_PlayerAssetList.OnClose();
		TimerHeap.DelTimer(m_timerId);
	}
}
