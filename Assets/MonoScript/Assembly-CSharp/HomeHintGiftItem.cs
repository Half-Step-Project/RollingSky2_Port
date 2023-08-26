using UnityEngine;

public class HomeHintGiftItem : MonoBehaviour
{
	public GameObject root;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
	}

	public void OnClick()
	{
		if (PlayerDataModule.Instance.PlayerGiftPackageData.GetNextGiftId() != -1)
		{
			InfocUtils.Report_rollingsky2_games_pageshow(12, 12, 2);
		}
	}
}
