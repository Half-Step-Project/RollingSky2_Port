using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeForm : UGUIForm
{
	public static PlayerUpgradeForm m_Instance;

	public PlayerUpgradeItem m_playerUpgradeItemToCopy;

	public Button m_playerUpgradeLevelUpBtn;

	public Button m_playerUpgradeUnlevelUpBtn;

	public List<PlayerUpgradeItem> m_playerUpgradeItemList = new List<PlayerUpgradeItem>();

	public List<PlayerUpgradeData> m_playerUpgradeDataList = new List<PlayerUpgradeData>();

	public Text m_TitleText;

	public Text m_levelText;

	public Text m_coinText;

	public Text m_levelUpText;

	public Text m_unlevelUpText;

	private bool ifHidingForm;

	private bool ifOpenForm;

	public Transform m_PlayerUpgradeContent;

	public Animator m_zhixuan_Anim;

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
		m_playerUpgradeItemList.Clear();
		m_playerUpgradeDataList.Clear();
		m_playerUpgradeItemToCopy.gameObject.SetActive(false);
		for (int i = 0; i < 10; i++)
		{
			PlayerUpgradeItem playerUpgradeItem = Object.Instantiate(m_playerUpgradeItemToCopy);
			playerUpgradeItem.transform.SetParent(m_playerUpgradeItemToCopy.transform.parent);
			playerUpgradeItem.gameObject.SetActive(true);
			playerUpgradeItem.m_playerUpgradeForm = this;
			playerUpgradeItem.transform.localPosition = Vector3.zero;
			playerUpgradeItem.transform.localScale = Vector3.one;
			playerUpgradeItem.transform.localRotation = Quaternion.identity;
			m_playerUpgradeItemList.Add(playerUpgradeItem);
		}
		ifOpenForm = false;
		m_PlayerUpgradeContent = m_playerUpgradeItemToCopy.transform.parent;
	}

	protected override void OnOpen(object userData)
	{
		base.OnOpen(userData);
		base.transform.SetAsFirstSibling();
		GetComponent<Canvas>().sortingOrder = 10002;
		ifHidingForm = false;
		RefreshPlayerUpgradeFormUIELement();
		if ((bool)m_zhixuan_Anim)
		{
			m_zhixuan_Anim.SetTrigger("in");
			m_zhixuan_Anim.Play("PlayerUpgradeForm_In");
		}
		StartCoroutine(AutoRefresh());
		ifOpenForm = false;
		Vector3 localPosition = m_PlayerUpgradeContent.transform.localPosition;
		localPosition.y = (float)Mathf.Max(0, PlayerDataModule.Instance.GetPlayerStarLevel() - 1) * 160f;
		m_PlayerUpgradeContent.transform.localPosition = localPosition;
		EducationDisplayDirector.Instance.OnPlayerUpgradeFormOpen();
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
		EducationDisplayDirector.Instance.OnPlayerUpgradeFormClose();
		if ((bool)m_zhixuan_Anim)
		{
			m_zhixuan_Anim.SetTrigger("out");
			m_zhixuan_Anim.Play("PlayerUpgradeForm_Out");
		}
		GetComponent<Canvas>().sortingOrder = 10002;
		yield return new WaitForSeconds(0.35f);
		Mod.UI.CloseUIForm(UIFormId.PlayerUpgradeForm);
	}

	public void RefreshPlayerUpgradeFormUIELement()
	{
		m_playerUpgradeDataList.Clear();
		for (int i = 0; i < 10; i++)
		{
			PlayerUpgradeData playerUpgradeData = new PlayerUpgradeData();
			playerUpgradeData.m_id = i;
			playerUpgradeData.m_showNum = playerUpgradeData.m_id + 1;
			playerUpgradeData.m_showText = PlayerDataModule.Instance.GetSpecialStarAbilityDesc(playerUpgradeData.m_showNum);
			playerUpgradeData.m_ifUnlock = PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(playerUpgradeData.m_showNum);
			playerUpgradeData.m_showTitle = PlayerDataModule.Instance.GetPlayerTitleByStarLevel(playerUpgradeData.m_showNum);
			playerUpgradeData.m_iconId = Mod.DataTable.Get<PlayerStarLevel_table>()[playerUpgradeData.m_showNum].PlayerIcon;
			m_playerUpgradeDataList.Add(playerUpgradeData);
		}
		for (int j = 0; j < 10; j++)
		{
			if (j < m_playerUpgradeDataList.Count)
			{
				m_playerUpgradeItemList[j].SetData(m_playerUpgradeDataList[j]);
			}
		}
		bool ifHaveEnoughLevel = false;
		bool ifHaveEnoughCoin = false;
		Debug.Log("canStarLevelUp" + PlayerDataModule.Instance.IfCanPlayerParticularStarUpgradeByAbilityLevel(PlayerDataModule.Instance.GetPlayerStarLevel(), ref ifHaveEnoughLevel, ref ifHaveEnoughCoin));
		int level = 0;
		int goodId = 3;
		long goodNum = 0L;
		PlayerDataModule.Instance.GetPlayerParticulerStarUpgradeDataByAbilityLevel(PlayerDataModule.Instance.GetPlayerStarLevel(), ref level, ref goodId, ref goodNum);
		m_TitleText.text = string.Format(Mod.Localization.GetInfoById(269), PlayerDataModule.Instance.GetPlayerTitleByStarLevel(PlayerDataModule.Instance.GetPlayerStarLevel()));
		m_levelText.text = "Lv" + level;
		m_levelText.color = (ifHaveEnoughLevel ? new Color(72f / 85f, 0.7058824f, 0.4f, 1f) : Color.red);
		m_coinText.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(goodNum);
		m_coinText.color = (ifHaveEnoughCoin ? new Color(72f / 85f, 0.7058824f, 0.4f, 1f) : Color.red);
		m_playerUpgradeLevelUpBtn.interactable = ifHaveEnoughLevel;
		m_playerUpgradeLevelUpBtn.gameObject.SetActive(ifHaveEnoughLevel);
		m_playerUpgradeUnlevelUpBtn.interactable = false;
		m_playerUpgradeUnlevelUpBtn.gameObject.SetActive(!ifHaveEnoughLevel);
		m_unlevelUpText.text = string.Format(Mod.Localization.GetInfoById(271), level);
		if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(10))
		{
			m_playerUpgradeLevelUpBtn.gameObject.SetActive(false);
			m_playerUpgradeUnlevelUpBtn.gameObject.SetActive(false);
		}
	}

	public void OnPlayerUpgradeFormCloseClick()
	{
		HideForm();
	}

	public void OnPlayerUpgradeFormStarLevelUpgradeBtnClick()
	{
		NetworkVerifyForm.Verify(delegate
		{
			bool ifHaveEnoughLevel = false;
			bool ifHaveEnoughCoin = false;
			if (PlayerDataModule.Instance.IfCanPlayerParticularStarUpgradeByAbilityLevel(PlayerDataModule.Instance.GetPlayerStarLevel(), ref ifHaveEnoughLevel, ref ifHaveEnoughCoin))
			{
				CommonAlertData userData = new CommonAlertData
				{
					showType = CommonAlertData.AlertShopType.COMMON,
					alertContent = Mod.Localization.GetInfoById(282).Replace("\\n", "\n"),
					lableContent = Mod.Localization.GetInfoById(268),
					cancelButtonText = Mod.Localization.GetInfoById(87),
					needCancelButton = true,
					cancelCallBackFunc = delegate
					{
						Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					},
					callBackFunc = delegate
					{
						Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
						RealOnPlayerUpgradeFormStarLevelUpgradeBtnClick();
					}
				};
				Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, userData);
			}
			else if (ifHaveEnoughLevel && !ifHaveEnoughCoin && !Mod.UI.UIFormIsOpen(UIFormId.BroadCastForm))
			{
				BroadCastData data = new BroadCastData
				{
					Type = BroadCastType.INFO,
					Info = Mod.Localization.GetInfoById(331)
				};
				MonoSingleton<BroadCastManager>.Instacne.BroadCast(data);
				MonoSingleton<GameTools>.Instacne.EnableInput();
			}
			Debug.Log("ifHaveEnoughLevel:" + ifHaveEnoughLevel + "& ifHaveEnoughCoin:" + ifHaveEnoughCoin);
			RefreshPlayerUpgradeFormUIELement();
		});
	}

	private void RealOnPlayerUpgradeFormStarLevelUpgradeBtnClick()
	{
		UpStarLevelResultState num = PlayerDataModule.Instance.UpPlayerStarLevel();
		if (num != 0)
		{
			if (!Mod.UI.UIFormIsOpen(UIFormId.BroadCastForm))
			{
				BroadCastData data = new BroadCastData
				{
					Type = BroadCastType.INFO,
					Info = Mod.Localization.GetInfoById(331)
				};
				MonoSingleton<BroadCastManager>.Instacne.BroadCast(data);
				MonoSingleton<GameTools>.Instacne.EnableInput();
			}
		}
		else
		{
			Mod.Sound.PlaySound(20016);
			HideForm();
			Mod.Event.FireNow(this, PlayerUpgradeEventArg.Make());
			EducationDisplayDirector.Instance.OnPlayerStarUpgrade();
		}
		Debug.Log(num);
	}

	public IEnumerator AutoRefresh()
	{
		yield return new WaitForSeconds(0.5f);
		ifOpenForm = true;
		while (true)
		{
			yield return new WaitForSeconds(3f);
			RefreshPlayerUpgradeFormUIELement();
		}
	}
}
