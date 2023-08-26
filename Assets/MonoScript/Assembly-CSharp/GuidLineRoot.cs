using System.Collections;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class GuidLineRoot : MonoBehaviour
{
	public enum Type
	{
		None,
		GuideLine,
		Shield,
		Rebirth
	}

	private const int GUILDLINE_SHOP_ITEM_ID = 60010;

	private const int SHIELD_SHOP_ITEM_ID = 60011;

	private const int REBIRTH_SHOP_ITEM_ID = 60012;

	public Type type;

	public GameObject permanentRoot;

	public GameObject usedRoot;

	public GameObject numRoot;

	public GameObject buyRoot;

	public Text count;

	public Text usedCount;

	public Transform root;

	public GameObject selectEffect;

	private bool used;

	private bool isDestroyed;

	private uint timerId;

	private void Awake()
	{
		Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Subscribe(EventArgs<SelectSkillLightupEventArgs>.EventId, OnSelectSkillLightup);
		Mod.Event.Subscribe(EventArgs<InstantPropsEventArgs>.EventId, OnInstantPropsHandler);
		Mod.Event.Subscribe(EventArgs<ChangeTempGoodsEvent>.EventId, OnTempGoodsChanged);
		selectEffect.SetActive(false);
		isDestroyed = false;
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		Mod.Event.Unsubscribe(EventArgs<SelectSkillLightupEventArgs>.EventId, OnSelectSkillLightup);
		Mod.Event.Unsubscribe(EventArgs<InstantPropsEventArgs>.EventId, OnInstantPropsHandler);
		Mod.Event.Unsubscribe(EventArgs<ChangeTempGoodsEvent>.EventId, OnTempGoodsChanged);
		CancelInvoke();
		StopAllCoroutines();
		TimerHeap.DelTimer(timerId);
		isDestroyed = true;
	}

	private void OnEnable()
	{
		UpdateStatus();
	}

	private void Start()
	{
	}

	public void UpdateStatus()
	{
		double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(GetItemIdByType());
		if (IsPermanent())
		{
			permanentRoot.SetActive(true);
			usedRoot.SetActive(false);
			numRoot.SetActive(false);
			buyRoot.SetActive(false);
		}
		else if (IsTimeLimit())
		{
			permanentRoot.SetActive(false);
			usedRoot.SetActive(true);
			usedCount.text = playGoodsNum.ToString();
			numRoot.SetActive(false);
			buyRoot.SetActive(false);
			if (timerId != 0)
			{
				TimerHeap.DelTimer(timerId);
			}
			timerId = TimerHeap.AddTimer(0u, 1000u, delegate
			{
				if (GetLeftTime() <= 0)
				{
					TimerHeap.DelTimer(timerId);
					UpdateStatus();
				}
			});
		}
		else if (used)
		{
			permanentRoot.SetActive(false);
			usedRoot.SetActive(true);
			numRoot.SetActive(false);
			buyRoot.SetActive(false);
			usedCount.text = (playGoodsNum - 1.0).ToString();
		}
		else if (playGoodsNum > 0.0)
		{
			permanentRoot.SetActive(false);
			usedRoot.SetActive(false);
			numRoot.SetActive(true);
			buyRoot.SetActive(false);
			count.text = playGoodsNum.ToString();
		}
		else
		{
			permanentRoot.SetActive(false);
			usedRoot.SetActive(false);
			numRoot.SetActive(false);
			buyRoot.SetActive(true);
		}
	}

	private void CheckAdSeconds()
	{
		if (!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			UpdateStatus();
			CancelInvoke();
		}
	}

	public void OnClickUse()
	{
		OnClickUserHandle();
	}

	private void OnClickUserHandle()
	{
		used = true;
		UpdateStatus();
		int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
		int button = 0;
		if (type == Type.GuideLine)
		{
			button = 16;
		}
		else if (type == Type.Shield)
		{
			button = 17;
		}
		else if (type == Type.Rebirth)
		{
			button = 18;
		}
		InfocUtils.Report_rollingsky2_games_pageshow(3, button, 2, curLevelId);
	}

	private IEnumerator ToClickUserHandle()
	{
		yield return new WaitForSeconds(0.5f);
		OnClickUserHandle();
		selectEffect.SetActive(false);
	}

	private PropsName GetPropsNameByType(Type type)
	{
		PropsName result = PropsName.NULL;
		switch (type)
		{
		case Type.GuideLine:
			result = PropsName.PATHGUIDE;
			break;
		case Type.Shield:
			result = PropsName.SHIELD;
			break;
		case Type.Rebirth:
			result = PropsName.REBIRTH;
			break;
		}
		return result;
	}

	private FairySkillsName GetSkillsNameType(Type type)
	{
		FairySkillsName result = FairySkillsName.NULL;
		switch (type)
		{
		case Type.GuideLine:
			result = FairySkillsName.PATHGUIDE;
			break;
		case Type.Shield:
			result = FairySkillsName.SHIELD;
			break;
		case Type.Rebirth:
			result = FairySkillsName.REBIRTH;
			break;
		}
		return result;
	}

	private Type GetTypebySkillsName(FairySkillsName name)
	{
		Type result = Type.None;
		switch (name)
		{
		case FairySkillsName.SHIELD:
			result = Type.Shield;
			break;
		case FairySkillsName.REBIRTH:
			result = Type.Rebirth;
			break;
		case FairySkillsName.PATHGUIDE:
			result = Type.GuideLine;
			break;
		}
		return result;
	}

	public void OnClickUnuse()
	{
		BroadCastData broadCastData = new BroadCastData();
		broadCastData.Type = BroadCastType.INFO;
		broadCastData.Info = string.Format(Mod.Localization.GetInfoById(306), GetName());
		MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
	}

	public void OnClickBuy()
	{
		int shopItemId = GetShopItemIdByType();
		int itemId = GetItemIdByType();
		CommonAlertData commonAlertData = new CommonAlertData();
		commonAlertData.showType = CommonAlertData.AlertShopType.BUY_OR_AD;
		commonAlertData.shopItemId = shopItemId;
		commonAlertData.callBackFunc = delegate
		{
			InfocUtils.Report_rollingsky2_games_neigou(shopItemId, 2, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
			int num = int.Parse(Mod.DataTable.Get<Shops_shopTable>()[shopItemId].Price);
			if (PlayerDataModule.Instance.GetPlayGoodsNum(6) >= (double)num)
			{
				InfocUtils.Report_rollingsky2_games_neigou(shopItemId, 3, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
				PlayerDataModule.Instance.ChangePlayerGoodsNum(6, -num);
				PlayerDataModule.Instance.ChangePlayerGoodsNum(itemId, 1.0);
				OnClickUse();
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				InfocUtils.Report_rollingsky2_games_currency(itemId, 2, 16);
			}
			else
			{
				InfocUtils.Report_rollingsky2_games_neigou(shopItemId, 4, Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
			}
		};
		commonAlertData.startADCallBackFunc = delegate
		{
			OnClickAd();
		};
		Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
	}

	public void OnClickAd()
	{
		int adsId = 0;
		if (type == Type.GuideLine)
		{
			adsId = 14;
		}
		else if (type == Type.Shield)
		{
			adsId = 15;
		}
		else if (type == Type.Rebirth)
		{
			adsId = 16;
		}
		InfocUtils.Report_rollingsky2_games_ads(adsId, 0, 1, 0, 3, 0);
		MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.NONE, delegate
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).RestartCount = 0;
			PlayerDataModule.Instance.ChangePlayerGoodsNum(GetItemIdByType(), 1.0);
			if (!isDestroyed && (bool)base.gameObject)
			{
				used = true;
				UpdateStatus();
			}
			Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
			InfocUtils.Report_rollingsky2_games_ads(adsId, 0, 1, 0, 4, 0);
		});
	}

	private bool CanAdAward()
	{
		int num = (int)PlayerDataModule.Instance.GetPlayGoodsNum(GetItemIdByType());
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		if (num <= GameCommon.AdAwardItemCount && dataModule.RestartCount >= GameCommon.AdAwardRestartTimes)
		{
			return MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE);
		}
		return false;
	}

	private int GetItemIdByType()
	{
		if (type == Type.Shield)
		{
			return GameCommon.SHIELD_ITEM;
		}
		if (type == Type.Rebirth)
		{
			return GameCommon.REBIRTH_FREE_ITEM;
		}
		if (type == Type.GuideLine)
		{
			return GameCommon.GuideLine;
		}
		return 0;
	}

	private int GetShopItemIdByType()
	{
		if (type == Type.Shield)
		{
			return 60011;
		}
		if (type == Type.Rebirth)
		{
			return 60012;
		}
		if (type == Type.GuideLine)
		{
			return 60010;
		}
		return 0;
	}

	private void OnTempGoodsChanged(object sender, EventArgs e)
	{
		ChangeTempGoodsEvent changeTempGoodsEvent = e as ChangeTempGoodsEvent;
		if (changeTempGoodsEvent != null)
		{
			used = changeTempGoodsEvent.isAdd;
			UpdateStatus();
		}
	}

	private void OnSelectSkillLightup(object sender, EventArgs e)
	{
		SelectSkillLightupEventArgs selectSkillLightupEventArgs = e as SelectSkillLightupEventArgs;
		if (selectSkillLightupEventArgs == null || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		for (int i = 0; i < selectSkillLightupEventArgs.mSkillNames.Length; i++)
		{
			if (GetTypebySkillsName(selectSkillLightupEventArgs.mSkillNames[i]) == type)
			{
				selectEffect.SetActive(true);
				StartCoroutine(ToClickUserHandle());
				break;
			}
		}
	}

	private void OnPlayerAssetChange(object sender, EventArgs e)
	{
		GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
		if (gameGoodsNumChangeEventArgs != null)
		{
			if (gameGoodsNumChangeEventArgs.GoodsId == 6)
			{
				Mod.UI.CloseUIForm(UIFormId.MoneyShopForm);
			}
			UpdateStatus();
		}
	}

	private void OnInstantPropsHandler(object sender, EventArgs e)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		if (type == Type.GuideLine)
		{
			if (IsTimeLimit())
			{
				dataModule.GuideLine = true;
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.PATHGUIDE));
			}
			else if (used)
			{
				dataModule.GuideLine = true;
				PlayerDataModule.Instance.ChangePlayerGoodsNum(GameCommon.GuideLine, -1.0);
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.PATHGUIDE));
				used = false;
				GameController.Instance.StartGuideLineTimer();
			}
			else
			{
				dataModule.GuideLine = false;
			}
		}
		else if (type == Type.Shield)
		{
			if (IsTimeLimit() || IsPermanent())
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.SHIELD));
			}
			else if (used)
			{
				PlayerDataModule.Instance.ChangePlayerGoodsNum(GameCommon.SHIELD_ITEM, -1.0);
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.SHIELD));
				used = false;
			}
		}
		else if (type == Type.Rebirth)
		{
			if (IsTimeLimit() || IsPermanent())
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.REBIRTH));
			}
			else if (used)
			{
				PlayerDataModule.Instance.ChangePlayerGoodsNum(GameCommon.REBIRTH_FREE_ITEM, -1.0);
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.REBIRTH));
				dataModule.HasUseRebirthItem = true;
				used = false;
			}
		}
	}

	private IEnumerator ToStartGame()
	{
		yield return new WaitForSeconds(0.5f);
		Mod.Event.Fire(this, Mod.Reference.Acquire<ToClickGameStartEventArgs>().Initialize());
	}

	private bool IsPermanent()
	{
		if (type == Type.Shield)
		{
			return PlayerDataModule.Instance.BufferIsEnableForever(GameCommon.START_FREE_SHIELD);
		}
		if (type == Type.Rebirth)
		{
			return PlayerDataModule.Instance.BufferIsEnableForever(GameCommon.ORIGIN_REBIRTH_FREE);
		}
		return false;
	}

	private bool IsTimeLimit()
	{
		if (type == Type.Shield)
		{
			return PlayerDataModule.Instance.BufferIsEnableByTime(GameCommon.START_FREE_SHIELD);
		}
		if (type == Type.Rebirth)
		{
			return PlayerDataModule.Instance.BufferIsEnableByTime(GameCommon.ORIGIN_REBIRTH_FREE);
		}
		if (type == Type.GuideLine)
		{
			return PlayerDataModule.Instance.BufferIsEnableByTime(GameCommon.GuideLine);
		}
		return false;
	}

	private long GetLeftTime()
	{
		if (type == Type.Shield)
		{
			return PlayerDataModule.Instance.GetPlayerBufferDataByBufferID(GameCommon.START_FREE_SHIELD).LeftBufferTime();
		}
		if (type == Type.Rebirth)
		{
			return PlayerDataModule.Instance.GetPlayerBufferDataByBufferID(GameCommon.ORIGIN_REBIRTH_FREE).LeftBufferTime();
		}
		if (type == Type.GuideLine)
		{
			return PlayerDataModule.Instance.GetPlayerBufferDataByBufferID(GameCommon.GuideLine).LeftBufferTime();
		}
		return 0L;
	}

	private string GetName()
	{
		if (type == Type.Shield)
		{
			return MonoSingleton<GameTools>.Instacne.GetGoodsName(4);
		}
		if (type == Type.Rebirth)
		{
			return MonoSingleton<GameTools>.Instacne.GetGoodsName(GameCommon.REBIRTH_FREE_ITEM);
		}
		if (type == Type.GuideLine)
		{
			return MonoSingleton<GameTools>.Instacne.GetGoodsName(GameCommon.GuideLine);
		}
		return "";
	}
}
