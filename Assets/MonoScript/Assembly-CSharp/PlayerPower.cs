using UnityEngine;
using UnityEngine.UI;

public class PlayerPower : MonoBehaviour
{
	public Text powerText;

	public Text powerTimeText;

	private void Start()
	{
		UpdateState();
	}

	private void UpdateState()
	{
		if (PlayerDataModule.Instance.PlayerRecordData.IsInNoConsumePowerTime())
		{
			powerText.gameObject.SetActive(false);
			powerTimeText.gameObject.SetActive(true);
			InvokeRepeating("UpdatePowerLeftTime", 0f, 1f);
		}
		else
		{
			powerText.gameObject.SetActive(true);
			powerTimeText.gameObject.SetActive(false);
			powerText.text = MonoSingleton<GameTools>.Instacne.ProductsCountToString(PlayerDataModule.Instance.GetPlayGoodsNum(1));
		}
	}

	private void UpdatePowerLeftTime()
	{
		if ((bool)powerTimeText)
		{
			long num = PlayerDataModule.Instance.PlayerRecordData.LeftNoConsumePowerTime() / 1000;
			if (num > 0)
			{
				powerTimeText.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormat(num);
				return;
			}
			CancelInvoke("UpdatePowerLeftTime");
			UpdateState();
		}
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}
}
