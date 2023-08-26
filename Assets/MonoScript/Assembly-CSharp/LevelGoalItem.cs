using RS2;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoalItem : UILoopItem
{
	private LevelGoalItemData itemData;

	private Text targetDec;

	private Text rewardNumTxt;

	private Image goodsIcon;

	private Image goldIcon;

	private Button refreshBtn;

	private ParticleSystem refreshEffect;

	public override void Data(object data)
	{
		Init();
		itemData = data as LevelGoalItemData;
		UpdateContent();
	}

	private void Init()
	{
		if (targetDec == null)
		{
			targetDec = base.transform.Find("targetDec").GetComponent<Text>();
		}
		if (rewardNumTxt == null)
		{
			rewardNumTxt = base.transform.Find("rewareContent/rewardNumTxt").GetComponent<Text>();
		}
		if (goodsIcon == null)
		{
			goodsIcon = base.transform.Find("rewareContent/goodsIcon").GetComponent<Image>();
		}
		if (goldIcon == null)
		{
			goldIcon = base.transform.Find("rewareContent/goldIcon").GetComponent<Image>();
		}
		if (refreshBtn == null)
		{
			refreshBtn = base.transform.Find("refreshBtn").GetComponent<Button>();
			refreshBtn.gameObject.SetActive(false);
		}
		if (refreshEffect == null)
		{
			refreshEffect = base.transform.Find("Effect_JM_glow01/effect/UI_glow_047").GetComponent<ParticleSystem>();
			refreshEffect.gameObject.SetActive(false);
			refreshEffect.Stop();
		}
		EventTriggerListener.Get(refreshBtn.gameObject).onClick = RefreshBtnHandle;
	}

	private void UpdateContent()
	{
		GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		Award_awardTable award_awardTable = null;
		targetDec.text = itemData.targetDesc;
		int i = 0;
		for (int count = itemData.awardList.Count; i < count; i++)
		{
			award_awardTable = dataModule.GetAwardTableById(itemData.awardList[i]);
			if (award_awardTable != null)
			{
				if (award_awardTable.GoodsID == 3)
				{
					goldIcon.gameObject.SetActive(true);
					goodsIcon.gameObject.SetActive(false);
					rewardNumTxt.text = award_awardTable.Count.ToString();
				}
				else
				{
					goldIcon.gameObject.SetActive(false);
					goodsIcon.gameObject.SetActive(true);
					rewardNumTxt.text = award_awardTable.Count.ToString();
				}
			}
		}
	}

	private void RefreshBtnHandle(GameObject obj)
	{
		int num = 0;
		if (itemData.levelId == 3 || itemData.levelId == 4 || itemData.levelId == 5)
		{
			num = itemData.levelId;
		}
		string info = string.Format("scenes=4&scenes_source=0&ad_source=1&action=1&source=0&level={0}", num);
		InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
		MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.LevelTargetRefresh, OnAddSuccess);
	}

	private void OnAddSuccess(ADScene adScene)
	{
		RefreshTarget();
		PlayRefreshEffect();
	}

	private void RefreshTarget()
	{
	}

	private void PlayRefreshEffect()
	{
		refreshEffect.gameObject.SetActive(true);
		refreshEffect.Play();
	}

	public void SetRefreshBtnVisiable(bool visiable)
	{
		refreshBtn.gameObject.SetActive(visiable);
	}

	public override object GetData()
	{
		return itemData;
	}

	public override void SetSelected(bool selected)
	{
	}

	public override void OnRelease()
	{
		refreshEffect.gameObject.SetActive(false);
		refreshEffect.Stop();
	}
}
