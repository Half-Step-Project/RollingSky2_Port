using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class MusicInstrumentForm : UGUIForm
{
	public static MusicInstrumentForm m_Instance;

	public const int MUSICINSTRUMENTLENGTHLIMIT = 360;

	public const int WAIT_AD_TIME = 15;

	public DOTweenAnimation m_dotweenAnimationShowForm;

	public const int totalItemNum = 20;

	public CustomText m_titleTxt;

	public CustomText m_titleTxt2;

	public string m_titleStr;

	public Text m_lvlTxt;

	public string m_lvlStr;

	public Button m_lvlBtn;

	public Image[] m_btnImgs;

	public int m_lvl;

	public InstrumentItem m_itemPrefab;

	public Text m_wy_CurrentValueText;

	public InstrumentItem m_zhihuibang;

	public List<InstrumentItem> m_items = new List<InstrumentItem>();

	public List<InstrumentData> m_itemDatas = new List<InstrumentData>();

	private int lvlTimesIndex;

	private int[] lvlTimesSet = new int[3] { 1, 10, -1 };

	private bool ifHidingForm;

	private bool ifOpenForm;

	private double m_cachedSpeed;

	private Animator m_anim;

	public bool IfOpenForm
	{
		get
		{
			return ifOpenForm;
		}
	}

	protected override void OnInit(object userData)
	{
		base.OnInit(userData);
		m_Instance = this;
		m_itemDatas.Clear();
		m_items.Clear();
		m_itemPrefab.gameObject.SetActive(false);
		m_items.Add(m_zhihuibang);
		m_zhihuibang.m_musicForm = this;
		for (int i = 0; i < 20; i++)
		{
			InstrumentItem instrumentItem = UnityEngine.Object.Instantiate(m_itemPrefab);
			instrumentItem.transform.SetParent(m_itemPrefab.transform.parent);
			instrumentItem.gameObject.SetActive(true);
			instrumentItem.m_musicForm = this;
			instrumentItem.transform.localPosition = Vector3.zero;
			instrumentItem.transform.localScale = Vector3.one;
			instrumentItem.transform.localRotation = Quaternion.identity;
			m_items.Add(instrumentItem);
		}
		InitTweenValue();
		ifOpenForm = false;
	}

	private void InitTweenValue()
	{
	}

	protected override void OnOpen(object userData)
	{
		base.OnOpen(userData);
		lvlTimesIndex = 0;
		EducationDisplayDirector.Instance.OnMusicalInstrumentUpgrade();
		Refresh();
		StartCoroutine(AutoRefresh());
		if ((bool)m_anim)
		{
			m_anim.SetTrigger("in");
			m_anim.Play("MusicalInstrumentForm_In");
		}
		ifHidingForm = false;
		ifOpenForm = false;
		m_anim = base.gameObject.GetComponentInChildren<Animator>();
	}

	protected override void OnClose(object userData)
	{
		base.OnClose(userData);
		StopAllCoroutines();
	}

	public void HideForm()
	{
		if (!ifHidingForm)
		{
			ifHidingForm = true;
			StartCoroutine(SlowCloseForm());
		}
	}

	private IEnumerator SlowCloseForm()
	{
		if ((bool)m_anim)
		{
			m_anim.SetTrigger("out");
			m_anim.Play("MusicalInstrumentForm_Out");
		}
		GetComponent<Canvas>().sortingOrder = 10001;
		EducationDisplayDirector.Instance.OnMusicalInstrumentUpgradeBack();
		yield return new WaitForSeconds(0.35f);
		Mod.UI.CloseUIForm(UIFormId.MusicalInstrumentForm);
	}

	public void OnLvlBtnClick()
	{
		lvlTimesIndex = (lvlTimesIndex + 1) % 3;
		Refresh();
	}

	public void Refresh()
	{
		DateTime musicInstrumentAdTime = PlayerDataModule.Instance.GetMusicInstrumentAdTime();
		bool flag = (DateTime.Now - musicInstrumentAdTime).TotalSeconds >= (double)GameCommon.showAdInInstrument;
		int index = 0;
		int num = 10086;
		int num2 = 0;
		UpdateLvlTimes();
		m_itemDatas.Clear();
		List<PlayerLocalInstrumentData> allInstrumentDataList = PlayerDataModule.Instance.GetAllInstrumentDataList();
		int num3 = 0;
		for (int i = 0; i < allInstrumentDataList.Count; i++)
		{
			PlayerLocalInstrumentData playerLocalInstrumentData = allInstrumentDataList[i];
			InstrumentData instrumentData = new InstrumentData();
			m_itemDatas.Add(instrumentData);
			instrumentData.m_id = playerLocalInstrumentData.m_Id;
			instrumentData.m_lv = playerLocalInstrumentData.m_Level;
			instrumentData.m_ifLowest = false;
			instrumentData.m_itemOrder = i;
			if (playerLocalInstrumentData.m_lockState <= 0 && num > playerLocalInstrumentData.m_Level)
			{
				index = i;
				num = playerLocalInstrumentData.m_Level;
			}
			num3 += instrumentData.m_lv;
			instrumentData.m_ifLock = playerLocalInstrumentData.m_lockState > 0;
			if (!instrumentData.m_ifLock)
			{
				num2++;
			}
			instrumentData.m_lvlup = lvlTimesSet[lvlTimesIndex];
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
				int num4 = playerLocalInstrumentData.m_Level;
				int level = playerLocalInstrumentData.m_Level;
				double num5 = 0.0;
				double num6 = 0.0;
				do
				{
					level = num4;
					num4++;
					if (num4 > instrumentData.m_instrumentCanUpMaxLevel)
					{
						break;
					}
					num5 = playerLocalInstrumentData.GetUpLevelConsumeCount(PlayerDataModule.Instance.GetPlayerStarLevel(), num4);
					num6 += num5;
				}
				while (!(num6 >= playGoodsNum) && num4 < 360);
				instrumentData.m_lvlup = Mathf.Max(level, playerLocalInstrumentData.m_Level + 1) - playerLocalInstrumentData.m_Level;
			}
			instrumentData.m_lvlup = Mathf.Min(360 - playerLocalInstrumentData.m_Level - 1, instrumentData.m_lvlup);
			instrumentData.m_coin = GetTotalCoinByAddition(instrumentData.m_lvlup, playerLocalInstrumentData);
			instrumentData.m_ifBtnEnable = !instrumentData.m_ifLock && instrumentData.m_coin <= playGoodsNum;
			instrumentData.m_iconId = playerLocalInstrumentData.IconId;
		}
		m_titleTxt.text = string.Format(Mod.Localization.GetInfoById(m_titleTxt.LanguageId), num3);
		m_titleTxt2.text = string.Format(Mod.Localization.GetInfoById(m_titleTxt2.LanguageId), (PlayerDataModule.Instance.InstrumentProductAdditonalPercent() * 100f).ToString("0.##"));
		if (num2 >= 3 && flag)
		{
			m_itemDatas[index].m_ifLowest = true;
		}
		for (int j = 0; j < m_items.Count; j++)
		{
			if (j <= m_itemDatas.Count - 1)
			{
				m_items[j].SetData(m_itemDatas[j]);
			}
			else
			{
				m_items[j].SetData(null);
			}
			m_items[j].RefreshData();
		}
		RefreshSpeed();
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

	public IEnumerator AutoRefresh()
	{
		yield return new WaitForSeconds(0.5f);
		ifOpenForm = true;
		while (true)
		{
			yield return new WaitForSeconds(3f);
			Refresh();
		}
	}

	public void UpdateLvlTimes()
	{
		for (int i = 0; i < m_btnImgs.Length; i++)
		{
			m_btnImgs[i].gameObject.SetActive(i == lvlTimesIndex);
		}
	}

	private void RefreshSpeed()
	{
		double currentProductReputaionSpeed = PlayerDataModule.Instance.GetCurrentProductReputaionSpeed();
		if (m_cachedSpeed != currentProductReputaionSpeed)
		{
			m_cachedSpeed = currentProductReputaionSpeed;
			string arg = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(m_cachedSpeed);
			if ((bool)m_wy_CurrentValueText)
			{
				m_wy_CurrentValueText.text = string.Format("{0}/s", arg);
			}
		}
	}
}
