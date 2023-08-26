using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ResultMotivateRoot : MonoBehaviour
{
	private const float EnergyScale = 0.012f;

	public GameObject giftRoot;

	public GameObject maxEffect;

	public GameObject activeGiftRoot;

	public Animator activeGiftAnim;

	public Text leftCount;

	public Image back;

	private PlayerLocalLevelData levelData;

	private bool isOpened;

	private void Awake()
	{
		maxEffect.SetActive(false);
		back.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	public void OnClickBack()
	{
		if (!isOpened)
		{
			isOpened = true;
			activeGiftAnim.SetTrigger("open");
			Mod.Sound.PlayUISound(20010);
		}
	}

	public void OpenFinished()
	{
		ShowMotivateReward();
		Mod.Event.Fire(this, Mod.Reference.Acquire<ResultGetAwardFinishedEventArgs>().Initialize());
	}

	public PlayerLocalLevelData ShowMotivateReward()
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
		levelData = dataModule.GetPlayerLevelData(curLevelId);
		PlayerLocalLevelSeriesData playerLocalLevelSeriesData = PlayerDataModule.Instance.PlayerLocalLevelSeriesData;
		if (playerLocalLevelSeriesData.IsInMotivateAwardCD())
		{
			giftRoot.SetActive(false);
		}
		else
		{
			giftRoot.SetActive(true);
			leftCount.text = string.Format(Mod.Localization.GetInfoById(219), playerLocalLevelSeriesData.GetLeftTimes());
		}
		return levelData;
	}

	public ResultGetGoods.ShowData GetReward()
	{
		levelData.isShowGiftBox = false;
		PlayerLocalLevelSeriesData playerLocalLevelSeriesData = PlayerDataModule.Instance.PlayerLocalLevelSeriesData;
		if (playerLocalLevelSeriesData.IsInMotivateAwardCD())
		{
			return null;
		}
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		ResultPageDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<ResultPageDataModule>(DataNames.ResultPageDataModule);
		ResultGetGoods.ShowData result = null;
		if (dataModule2.Progress >= 100)
		{
			result = dataModule.PlayerMotivateAwardData.GetAwards();
			playerLocalLevelSeriesData.SendSynChangeData();
			levelData.isShowGiftBox = true;
		}
		return result;
	}

	public void ShowAwardBox()
	{
	}

	private void FadeInBack()
	{
		back.gameObject.SetActive(true);
		back.color = Color.clear;
		back.DOFade(0.8f, 0.5f).SetEase(Ease.Linear);
	}

	private void FadeOutBack()
	{
		back.DOFade(0f, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
		{
			activeGiftRoot.SetActive(false);
		});
	}

	public void HideAwardBox()
	{
		FadeOutBack();
	}

	public void ShowMaxEffect()
	{
		activeGiftRoot.SetActive(true);
		activeGiftAnim.SetTrigger("inUp");
		giftRoot.SetActive(false);
		PlayerDataModule.Instance.PlayerLocalLevelSeriesData.AddMotivateAwardTimes();
		Mod.Sound.PlayUISound(20021);
	}

	public void MoveBox()
	{
		activeGiftAnim.SetTrigger("move");
		Mod.Sound.PlayUISound(20022);
		FadeInBack();
	}
}
