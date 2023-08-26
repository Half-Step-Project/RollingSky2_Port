using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class InstrumentSpeedUpForm : UGUIForm
	{
		private enum State
		{
			Normal,
			SpeedUp
		}

		private const float INCREASE_TIME = 0.5f;

		private const float FADE_TIME = 0.1f;

		public IncreaseSlider increaseSlider;

		public ImageFade cursorFade;

		public Image timeSlider;

		public Text time;

		public Text buyText;

		public Text times;

		public Text tips;

		public Animator anim;

		public SetUIGrey setUIGrey;

		private State currentState;

		private Color buyTextColor;

		private int preLeftTime;

		private Tweener increaseTweener;

		private void Awake()
		{
			buyTextColor = buyText.color;
			buyText.text = GameCommon.instrumentSpeedUpCostDiamound.ToString();
		}

		protected override bool EnableInputAfterOpen()
		{
			return true;
		}

		private void SetBuyTextColor()
		{
			double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
			int instrumentSpeedUpCostDiamound = GameCommon.instrumentSpeedUpCostDiamound;
			if (playGoodsNum >= (double)instrumentSpeedUpCostDiamound)
			{
				buyText.color = buyTextColor;
			}
			else
			{
				buyText.color = Color.red;
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			times.text = string.Format(Mod.Localization.GetInfoById(243), GameCommon.instrumentAdProductFactor);
			tips.text = string.Format(Mod.Localization.GetInfoById(244), GameCommon.instrumentSpeedUpTime);
			SetBuyTextColor();
			InvokeRepeating("Refresh", 0f, 1f);
			if (PlayerDataModule.Instance.ProductSpeedUpLeftTime() > 0)
			{
				anim.SetTrigger("upOpen");
			}
			else
			{
				anim.SetTrigger("open");
			}
			InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
		}

		private void CheckAds()
		{
			setUIGrey.SetGrey(!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView));
		}

		private void OnPlayerAssetChange(object sender, EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs != null && gameGoodsNumChangeEventArgs.GoodsId == 6)
			{
				SetBuyTextColor();
			}
		}

		private void Refresh()
		{
			int leftTime = PlayerDataModule.Instance.ProductSpeedUpLeftTime();
			Set(leftTime);
		}

		private void Set(int leftTime)
		{
			timeSlider.fillAmount = (float)leftTime / (float)GameCommon.instrumentSpeedUpMaxTime;
			time.gameObject.SetActive(true);
			if (leftTime > 0)
			{
				if (currentState == State.Normal)
				{
					anim.SetTrigger("up");
					currentState = State.SpeedUp;
				}
				time.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormat(leftTime, true);
			}
			else
			{
				if (currentState == State.SpeedUp)
				{
					anim.SetTrigger("idle");
					currentState = State.Normal;
				}
				time.text = "";
			}
			float width = timeSlider.rectTransform.rect.width;
			float x = width * timeSlider.fillAmount - width / 2f;
			time.rectTransform.anchoredPosition = new Vector2(x, time.rectTransform.anchoredPosition.y);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			CancelInvoke();
		}

		public void OnClickClose()
		{
			Mod.UI.CloseUIForm(UIFormId.InstrumentSpeedUpForm);
		}

		public void OnClickBuy()
		{
			double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
			int instrumentSpeedUpCostDiamound = GameCommon.instrumentSpeedUpCostDiamound;
			if (playGoodsNum >= (double)instrumentSpeedUpCostDiamound)
			{
				PlayerDataModule.Instance.ChangePlayerGoodsNum(6, -instrumentSpeedUpCostDiamound);
				AddTime();
			}
		}

		public void OnClickAdFree()
		{
			InfocUtils.Report_rollingsky2_games_ads(26, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.NONE, delegate(ADScene adScen)
			{
				OnAdSuccess(adScen);
				MonoSingleton<GameTools>.Instacne.EnableInput();
				InfocUtils.Report_rollingsky2_games_ads(26, 0, 1, 0, 4, 0);
			});
		}

		private void OnAdSuccess(ADScene adScen = ADScene.NONE)
		{
			AddTime();
		}

		private void AddTime()
		{
			preLeftTime = PlayerDataModule.Instance.ProductSpeedUpLeftTime();
			int num = 0;
			num = ((preLeftTime + GameCommon.instrumentSpeedUpTime <= GameCommon.instrumentSpeedUpMaxTime) ? GameCommon.instrumentSpeedUpTime : (GameCommon.instrumentSpeedUpMaxTime - preLeftTime));
			PlayerDataModule.Instance.AddProductSpeedUpTime(num);
			StartIncrease();
		}

		private void IncreaseSetter(float x)
		{
			time.gameObject.SetActive(true);
			time.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormat((int)x, true);
			float width = timeSlider.rectTransform.rect.width;
			float x2 = width * timeSlider.fillAmount - width / 2f;
			time.rectTransform.anchoredPosition = new Vector2(x2, time.rectTransform.anchoredPosition.y);
		}

		private void StartIncrease()
		{
			CancelInvoke("Refresh");
			if (increaseTweener != null)
			{
				increaseTweener.Kill();
			}
			MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
			PlayerDataModule instance = PlayerDataModule.Instance;
			int num = PlayerDataModule.Instance.ProductSpeedUpLeftTime();
			increaseTweener = DOTween.To(IncreaseSetter, preLeftTime, num, 0.5f).SetEase(Ease.Linear).OnComplete(delegate
			{
				MonoSingleton<GameTools>.Instacne.EnableInput();
				InvokeRepeating("Refresh", 0f, 1f);
			});
			increaseSlider.StartIncrease((float)preLeftTime / (float)GameCommon.instrumentSpeedUpMaxTime, (float)num / (float)GameCommon.instrumentSpeedUpMaxTime, 0.5f, 0.1f);
			if ((bool)cursorFade)
			{
				cursorFade.StartFade(0.5f, 0.1f);
			}
		}

		private void OnDestroy()
		{
			CancelInvoke();
		}
	}
}
