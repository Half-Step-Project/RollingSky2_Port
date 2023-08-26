using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class VideoAwardRoot : MonoBehaviour
{
	public HomeHintVideoAwardItem videoAwardItem;

	[HideInInspector]
	public SetRenderDepthGroup setRenderDepthGroup;

	[HideInInspector]
	public Canvas renderDepthTargetCanvas;

	public static void Add(List<object> loadedAsserts, Transform parent, Canvas targetCanvas, int delayShowCloseButtonTime = 1, System.Action<VideoAwardRoot> successCallback = null)
	{
		string videoAwardItemName = "videoAwardRoot";
		Mod.Resource.LoadAsset(AssetUtility.GetGameUIItemAsset(videoAwardItemName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (asset != null && !MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(targetCanvas))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(asset as GameObject);
				if (!MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(gameObject))
				{
					gameObject.SetActive(true);
					gameObject.transform.SetParent(parent, false);
					VideoAwardRoot component = gameObject.GetComponent<VideoAwardRoot>();
					if (!MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(component))
					{
						component.renderDepthTargetCanvas = targetCanvas;
						if (!MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(component.videoAwardItem))
						{
							component.videoAwardItem.delayShowCloseButtonTime = delayShowCloseButtonTime;
							loadedAsserts.Add(asset);
							if (successCallback != null)
							{
								successCallback(component);
							}
						}
					}
				}
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message '{2}'.", videoAwardItemName, assetName, errorMessage));
		}));
	}

	private void Awake()
	{
		setRenderDepthGroup = GetComponent<SetRenderDepthGroup>();
		videoAwardItem.gameObject.SetActive(false);
		Mod.Event.Subscribe(EventArgs<GetVideoAwardEvent>.EventId, OnGetVideoAward);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<GetVideoAwardEvent>.EventId, OnGetVideoAward);
		StopAllCoroutines();
		CancelInvoke();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		CancelInvoke();
	}

	private void OnEnable()
	{
		InvokeRepeating("UpdateSecond", 0f, 1f);
	}

	private void Start()
	{
		InitVideoAward();
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(ToSetRenderDepth());
		}
	}

	private IEnumerator ToSetRenderDepth()
	{
		yield return new WaitForEndOfFrame();
		Canvas component = GetComponent<Canvas>();
		if (!MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(renderDepthTargetCanvas) && !MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(component))
		{
			if (component.sortingOrder < renderDepthTargetCanvas.sortingOrder)
			{
				component.sortingOrder += renderDepthTargetCanvas.sortingOrder;
			}
			if (!MonoSingleton<GameTools>.Instacne.IsNullAndNoLog(setRenderDepthGroup))
			{
				setRenderDepthGroup.SetTargetCanvas(renderDepthTargetCanvas);
			}
		}
	}

	private void InitVideoAward()
	{
		if (PlayerDataModule.Instance.PlayerGiftPackageData.VideoAwardTable != null)
		{
			videoAwardItem.gameObject.SetActive(true);
			videoAwardItem.StartWork();
		}
	}

	private void OnGetVideoAward(object sender, Foundation.EventArgs e)
	{
		videoAwardItem.gameObject.SetActive(false);
	}

	private void UpdateSecond()
	{
		UpdateVideoAward();
	}

	private void UpdateVideoAward()
	{
		if (PlayerDataModule.Instance.IsInNewPlayerProtectedStage() || !TutorialManager.Instance.IsTutorialStageFinish(TutorialStageId.STAGE_NEXT_LEVEL))
		{
			videoAwardItem.gameObject.SetActive(false);
			return;
		}
		PlayerLocalGiftpackageData playerGiftPackageData = PlayerDataModule.Instance.PlayerGiftPackageData;
		VideoAward_videoAwardTable videoAwardTableByPlayerType = playerGiftPackageData.GetVideoAwardTableByPlayerType();
		PlayerLocalVideoAwardData playerLocalVideoAwardData = PlayerDataModule.Instance.PlayerLocalVideoAwardData;
		playerLocalVideoAwardData.CheckNextDay();
		if (playerLocalVideoAwardData.getCounts >= videoAwardTableByPlayerType.Limit)
		{
			videoAwardItem.gameObject.SetActive(false);
		}
		else if ((DateTime.Now - TimeTools.StringToDatetime(playerLocalVideoAwardData.lastGetAwardTime)).TotalSeconds > (double)videoAwardTableByPlayerType.Cd)
		{
			if (playerGiftPackageData.VideoAwardTable == null)
			{
				playerGiftPackageData.NewVideoAwardId();
			}
			if (!videoAwardItem.gameObject.activeInHierarchy)
			{
				videoAwardItem.gameObject.SetActive(true);
				videoAwardItem.StartWork();
			}
		}
		else
		{
			videoAwardItem.gameObject.SetActive(false);
		}
	}
}
