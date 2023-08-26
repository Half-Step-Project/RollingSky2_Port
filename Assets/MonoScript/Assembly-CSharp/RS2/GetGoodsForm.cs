using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class GetGoodsForm : UGUIForm
	{
		private GameObject back;

		private Text titleTxt;

		public Text goodsNameTxt;

		public Text goodsDesc;

		public GameObject continueBtn;

		public GameObject closeButton;

		public GameObject actionBtn;

		private Text goodsNumTxt;

		public Image m_awardIcon;

		private GetGoodsData goodsData;

		private AssetLoadCallbacks m_assetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		public RewardItemController m_rewardItem;

		public GameObject m_rewardContent;

		public GameObject m_goodsGetPath;

		public CustomText m_goodsExpound;

		public SetUIGrey setUIGrey;

		public GameObject m_upgradeContent;

		public GameObject m_adBtn;

		public GameObject m_adBtnDisable;

		public Text m_getText;

		public Text m_adText;

		private List<RewardItemController> m_rewardList = new List<RewardItemController>();

		private bool m_release;

		private Button actionButton;

		public RectTransform goldMove;

		public int goldMoveTargetId;

		private UIMoveTarget goldMoveTarget;

		public GameObject root;

		private int selectMulti;

		private uint adTimerId;

		private bool m_IsEffectInited;

		private uint delayShowCloseButtonTimerId;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_release = false;
			if (m_awardIcon != null)
			{
				m_awardIcon.gameObject.SetActive(false);
			}
			m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_release)
				{
					Mod.Resource.UnloadAsset(asset);
				}
				else if (m_awardIcon != null)
				{
					m_awardIcon.gameObject.SetActive(true);
					m_awardIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					loadedAsserts.Add(asset);
					m_awardIcon.gameObject.SetActive(true);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
			InitUI();
			actionButton = actionBtn.GetComponent<Button>();
		}

		protected override bool EnableInputAfterOpen()
		{
			return true;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			base.gameObject.SetActive(true);
			goodsData = userData as GetGoodsData;
			m_goodsGetPath.SetActive(goodsData.ShowGetPath);
			if (m_goodsExpound != null)
			{
				m_goodsExpound.gameObject.SetActive(goodsData.ShowExpound);
				if (goodsData.ExpoundTextId > 0)
				{
					m_goodsExpound.SetContent(goodsData.ExpoundTextId);
				}
			}
			if (!goodsData.Upgrade)
			{
				AddEventListener();
			}
			if (goodsData.GoodsTeam)
			{
				Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsData.GoodsTeamId);
				int num = ((goodsData.GoodsTeamNum < 1) ? 1 : goodsData.GoodsTeamNum);
				if (dictionary.Count == 1)
				{
					goodsData.GoodsTeam = false;
					Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();
					while (enumerator.MoveNext())
					{
						goodsData.GoodsId = enumerator.Current.Key;
						goodsData.GoodsNum = enumerator.Current.Value * num;
					}
				}
			}
			if (goodsData.Buy)
			{
				titleTxt.text = Mod.Localization.GetInfoById(64);
			}
			else
			{
				titleTxt.text = Mod.Localization.GetInfoById(63);
			}
			int num2 = 0;
			if (goodsData.GoodsTeam)
			{
				goodsNumTxt.text = string.Format("x{0}", goodsData.GoodsTeamNum);
				num2 = MonoSingleton<GameTools>.Instacne.GoodsTeamIconId(goodsData.GoodsTeamId);
				goodsNameTxt.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamName(goodsData.GoodsTeamId);
				GameObject gameObject = null;
				RewardItemController rewardItemController = null;
				m_rewardList.Clear();
				Dictionary<int, int>.Enumerator enumerator2 = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsData.GoodsTeamId).GetEnumerator();
				goodsDesc.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamDesc(goodsData.GoodsTeamId);
				int i = 0;
				for (int childCount = m_rewardContent.transform.childCount; i < childCount; i++)
				{
					UnityEngine.Object.Destroy(m_rewardContent.transform.GetChild(i).gameObject);
				}
				while (enumerator2.MoveNext())
				{
					gameObject = UnityEngine.Object.Instantiate(m_rewardItem.gameObject);
					rewardItemController = gameObject.GetComponent<RewardItemController>();
					gameObject.SetActive(true);
					m_rewardList.Add(rewardItemController);
					gameObject.transform.SetParent(m_rewardContent.transform);
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
					gameObject.transform.localScale = Vector3.one;
					rewardItemController.Init();
					rewardItemController.SetGoodsId(enumerator2.Current.Key, enumerator2.Current.Value * goodsData.GoodsTeamNum);
				}
				m_rewardContent.SetActive(true);
				m_awardIcon.gameObject.SetActive(false);
				goodsNumTxt.gameObject.SetActive(false);
			}
			else
			{
				goodsNameTxt.text = MonoSingleton<GameTools>.Instacne.GetGoodsName(goodsData.GoodsId);
				goodsDesc.text = MonoSingleton<GameTools>.Instacne.GetGoodsDesc(goodsData.GoodsId);
				num2 = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(goodsData.GoodsId);
				goodsNumTxt.gameObject.SetActive(true);
				goodsNumTxt.text = "X" + MonoSingleton<GameTools>.Instacne.DoubleToFormatString(goodsData.GoodsNum);
				m_rewardContent.SetActive(false);
				string assetName = num2.ToString();
				Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
			}
			actionBtn.SetActive(goodsData.NeedActionButton);
			ShowCloseButton();
			if (!goodsData.IsAutoPlayEffect)
			{
				Mod.Sound.PlayUISound(10001);
			}
			if (goodsData != null && goodsData.NeedCheckAds && setUIGrey != null && actionButton != null)
			{
				InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
			}
			if (goodsData.Upgrade)
			{
				m_upgradeContent.SetActive(true);
				if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
				{
					m_adBtn.SetActive(true);
				}
				m_getText.text = Mod.Localization.GetInfoById(261);
				titleTxt.text = Mod.Localization.GetInfoById(224);
				SetAdButtonState();
				adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAdButtonState);
				InitEffect();
				if (goodsData.IsAutoPlayEffect)
				{
					PlayEffect(1, true);
				}
			}
			else
			{
				m_upgradeContent.SetActive(false);
			}
		}

		private void PlayEffect(int factor = 1, bool isAuto = false)
		{
			if (isAuto)
			{
				back.SetActive(false);
			}
			selectMulti = factor;
			DoEffect();
		}

		private void InitEffect()
		{
			if (!m_IsEffectInited)
			{
				GetMoveTarget();
				m_IsEffectInited = true;
			}
		}

		private void GetMoveTarget()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("ItemMoveTarget");
			for (int i = 0; i < array.Length; i++)
			{
				UIMoveTarget component = array[i].GetComponent<UIMoveTarget>();
				if (!(component == null) && component.id == goldMoveTargetId)
				{
					goldMoveTarget = component;
				}
			}
		}

		private void DoMoveEffect()
		{
			Vector2 sizeDelta = goldMove.sizeDelta;
			if (goodsData.IsAutoPlayEffect)
			{
				sizeDelta *= 0.6f;
			}
			Vector3 position = goldMove.position;
			if (goodsData.IsAutoPlayEffect && (bool)HomeForm.Form)
			{
				position = HomeForm.Form.upgradeBtn.position;
			}
			float randomStartPosRange = 150f;
			if (goodsData.IsAutoPlayEffect)
			{
				randomStartPosRange = 100f;
			}
			GetAwardForm.Form.StartMove(position, sizeDelta, "gold", 3, goldMoveTarget, GetGoldMoveCount(selectMulti), null, randomStartPosRange);
		}

		private int GetGoldMoveCount(int multi)
		{
			return multi * 8;
		}

		private void CheckAds()
		{
			bool flag = MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView);
			actionButton.interactable = flag;
			setUIGrey.SetGrey(!flag);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			if (goodsData != null && !goodsData.Upgrade)
			{
				RemoveEventListener();
				TimerHeap.DelTimer(delayShowCloseButtonTimerId);
				CancelInvoke();
				Mod.Event.Fire(this, UIPopUpFormCloseEvent.Make(UIFormId.GetGoodsForm));
			}
			goodsData = null;
			m_release = true;
			TimerHeap.DelTimer(adTimerId);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < m_rewardList.Count; i++)
			{
				m_rewardList[i].Release();
			}
			m_rewardList.Clear();
			foreach (object loadedAssert in loadedAsserts)
			{
				Mod.Resource.UnloadAsset(loadedAssert);
			}
			loadedAsserts.Clear();
			m_release = true;
		}

		private void ShowCloseButton()
		{
			closeButton.SetActive(false);
			if (goodsData.NeedCloseButton && goodsData.CloseButtonDelayTime != 0)
			{
				delayShowCloseButtonTimerId = TimerHeap.AddTimer(goodsData.CloseButtonDelayTime, 0u, delegate
				{
					closeButton.SetActive(true);
					TimerHeap.DelTimer(delayShowCloseButtonTimerId);
				});
			}
		}

		private void InitUI()
		{
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			back = dictionary["back"];
			titleTxt = dictionary["titleTxt"].GetComponent<Text>();
			goodsNumTxt = dictionary["goodsNumTxt"].GetComponent<Text>();
		}

		public void AddEventListener()
		{
			if (goodsData.NeedCloseButton)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeButton);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			}
			else
			{
				EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(continueBtn);
				eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			}
		}

		private void OnCloseUIFormSuccess(object sender, Foundation.EventArgs e)
		{
			string assetName = ((UIMod.CloseCompleteEventArgs)e).AssetName;
			string uIFormAsset = AssetUtility.GetUIFormAsset(Mod.DataTable.Get<UIForms_uiformTable>().Get(6).AssetName);
			if (assetName.Equals(uIFormAsset))
			{
				base.gameObject.SetActive(true);
			}
		}

		public void RemoveEventListener()
		{
			if (goodsData != null && goodsData.NeedCloseButton)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeButton);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			}
			else
			{
				EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(continueBtn);
				eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(CloseHandler));
			}
		}

		private void CloseHandler(GameObject go)
		{
			if (goodsData != null && goodsData.CallBackFunc != null)
			{
				goodsData.CallBackFunc();
			}
			if (goodsData != null && goodsData.closeCallback != null)
			{
				goodsData.closeCallback(this);
			}
			MoveEffect();
			Mod.UI.CloseUIForm(UIFormId.GetGoodsForm);
		}

		private void MoveEffect()
		{
			if (goodsData == null || !goodsData.NeedMoveEffect || !GetAwardForm.Form)
			{
				return;
			}
			GetAwardForm form = GetAwardForm.Form;
			if (!goodsData.GoodsTeam)
			{
				form.StartMove(m_awardIcon.GetComponent<RectTransform>(), goodsData.GoodsId, (int)goodsData.GoodsNum, goodsData.moveEffectFinishedCallback);
				return;
			}
			foreach (KeyValuePair<int, int> item in MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsData.GoodsTeamId, goodsData.GoodsTeamNum))
			{
				form.StartMove(m_awardIcon.GetComponent<RectTransform>(), item.Key, item.Value, goodsData.moveEffectFinishedCallback);
			}
		}

		public void OnClickActionButton()
		{
			if (goodsData != null && goodsData.ActionButtonCallback != null)
			{
				goodsData.ActionButtonCallback();
			}
		}

		private void BroadResult(int goodsNum)
		{
			BroadCastData broadCastData = new BroadCastData();
			broadCastData.GoodId = 2;
			broadCastData.Type = BroadCastType.GOODS;
			broadCastData.Info = string.Format("+{0}", goodsNum);
			MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
		}

		public void OnClickNormalButton()
		{
			if (goodsData.Upgrade && goodsData != null && goodsData.NormalButtonCallback != null)
			{
				selectMulti = 1;
				DoEffect();
				goodsData.NormalButtonCallback();
				CloseMe();
			}
		}

		public void OnClickADButton()
		{
			if (!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE) || !goodsData.Upgrade)
			{
				return;
			}
			InfocUtils.Report_rollingsky2_games_ads(25, 0, 1, 0, 3, 0);
			if (goodsData != null && goodsData.ADButtonCallback != null)
			{
				selectMulti = 2;
				goodsData.ADButtonCallback(delegate
				{
					DoEffect();
					CloseMe();
				});
			}
		}

		private void DoEffect()
		{
			DoMoveEffect();
			if ((bool)goldMoveTarget)
			{
				double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(3);
				double end = GetGoldReward(goodsData.GoodsTeamId, selectMulti) + playGoodsNum;
				goldMoveTarget.SetData(playGoodsNum, end, -1);
			}
			if (goodsData.IsAutoPlayEffect)
			{
				CloseMe();
			}
		}

		private void CloseMe()
		{
			if (goodsData != null && goodsData.CallBackFunc != null)
			{
				goodsData.CallBackFunc();
			}
			Mod.UI.CloseUIForm(UIFormId.GetGoodsForm);
		}

		private double GetGoldReward(int groupID, int multiple)
		{
			double num = 0.0;
			Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(groupID).GetEnumerator();
			int num2 = -1;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Key == 3)
				{
					num2 = enumerator.Current.Value * multiple;
					num += (double)num2;
				}
			}
			return num;
		}

		private void SetAdButtonState()
		{
			if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
			{
				m_adBtn.SetActive(true);
				m_adBtnDisable.SetActive(false);
				TimerHeap.DelTimer(adTimerId);
			}
			else
			{
				m_adBtn.SetActive(false);
				m_adBtnDisable.SetActive(true);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(m_adText.transform.parent as RectTransform);
		}

		protected override void OnCover()
		{
			base.OnCover();
			base.gameObject.SetActive(false);
		}

		protected override void OnReveal()
		{
			base.OnReveal();
			base.gameObject.SetActive(true);
		}
	}
}
