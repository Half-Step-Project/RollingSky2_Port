using System.Collections.Generic;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CheckAdButton : MonoBehaviour
{
	public List<GameObject> greyObjs = new List<GameObject>();

	public List<GameObject> commonObjs = new List<GameObject>();

	public List<Image> commonImages = new List<Image>();

	public Button adButton;

	private int preState = -1;

	private void Start()
	{
		InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}

	private void CheckAds()
	{
		bool flag = MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView);
		adButton.interactable = flag;
		SetGrey(!flag);
	}

	private void SetGrey(bool isGrey)
	{
		int num = (isGrey ? 1 : 0);
		if (preState == num)
		{
			return;
		}
		preState = num;
		foreach (GameObject greyObj in greyObjs)
		{
			greyObj.SetActive(isGrey);
		}
		foreach (GameObject commonObj in commonObjs)
		{
			commonObj.SetActive(!isGrey);
		}
		foreach (Image commonImage in commonImages)
		{
			commonImage.enabled = !isGrey;
		}
		if ((bool)adButton)
		{
			adButton.interactable = !isGrey;
		}
	}
}
