using UnityEngine;
using UnityEngine.UI;

public class RebirthBuffRoot : MonoBehaviour
{
	public Text leftTimeText;

	public GameObject tipsRoot;

	public Text tipsLeftTimeText;

	private bool needUpdateTipsLeftTime;

	public bool IsTipShow { get; private set; }

	protected virtual int GetBuffId()
	{
		return GameCommon.ORIGIN_REBIRTH_FREE;
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
		bool num = PlayerDataModule.Instance.BufferIsEnableForever(GetBuffId());
		bool flag = PlayerDataModule.Instance.BufferIsEnableByTime(GetBuffId());
		if (num)
		{
			leftTimeText.gameObject.SetActive(false);
		}
		else if (flag)
		{
			leftTimeText.gameObject.SetActive(true);
			InvokeRepeating("UpdateLeftTime", 0f, 1f);
		}
	}

	public void Hide()
	{
		CancelInvoke();
		base.gameObject.SetActive(false);
	}

	private void UpdateLeftTime()
	{
		long num = PlayerDataModule.Instance.GetPlayerBufferDataByBufferID(GetBuffId()).LeftBufferTime();
		if (num <= 0)
		{
			Hide();
			return;
		}
		string text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num / 1000);
		leftTimeText.text = text;
		if (needUpdateTipsLeftTime)
		{
			tipsLeftTimeText.text = text;
		}
	}

	public void ShowTips()
	{
		tipsRoot.SetActive(true);
		needUpdateTipsLeftTime = true;
		IsTipShow = true;
	}

	public void HideTips()
	{
		tipsRoot.SetActive(false);
		needUpdateTipsLeftTime = false;
		IsTipShow = false;
	}

	private void Awake()
	{
		tipsRoot.SetActive(false);
		IsTipShow = false;
		needUpdateTipsLeftTime = false;
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}
}
