using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class OfflineAwardForm : UGUIForm
	{
		private const float INCREASE_TIME = 1f;

		public ResultMoveItem goldMoveItem;

		public ResultMoveItem noteMoveItem;

		public RectTransform goldMove;

		public RectTransform noteMove;

		public int goldMoveTargetId;

		public int noteMoveTargetId;

		private UIMoveTarget goldMoveTarget;

		private UIMoveTarget noteMoveTarget;

		public GameObject root;

		public Text time;

		public Text note;

		public Text gold;

		public Text diamoundCount;

		private Color diamoundCountColor;

		public SetUIGrey setUIGrey;

		private int selectMulti;

		private void Awake()
		{
			diamoundCountColor = diamoundCount.color;
			diamoundCount.text = GameCommon.offlineProductCostDiamound.ToString();
		}

		private void OnDestroy()
		{
			CancelInvoke();
		}

		protected override bool EnableInputAfterOpen()
		{
			return true;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			GetMoveTarget();
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			note.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(PlayerDataModule.Instance.OffLineProductReputationNum);
			gold.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(PlayerDataModule.Instance.OffLineProductGoldNum);
			time.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(PlayerDataModule.Instance.OffLineTime);
			PlayerDataModule.Instance.OffLineTime = 0;
			SetDiamoundCountColor();
			InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
		}

		private void CheckAds()
		{
			setUIGrey.SetGrey(!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView));
		}

		private void GetMoveTarget()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("ItemMoveTarget");
			for (int i = 0; i < array.Length; i++)
			{
				UIMoveTarget component = array[i].GetComponent<UIMoveTarget>();
				if (!(component == null))
				{
					if (component.id == goldMoveTargetId)
					{
						goldMoveTarget = component;
					}
					else if (component.id == noteMoveTargetId)
					{
						noteMoveTarget = component;
					}
				}
			}
		}

		private void SetDiamoundCountColor()
		{
			double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
			int offlineProductCostDiamound = GameCommon.offlineProductCostDiamound;
			if (playGoodsNum >= (double)offlineProductCostDiamound)
			{
				diamoundCount.color = diamoundCountColor;
			}
			else
			{
				diamoundCount.color = Color.red;
			}
		}

		private void OnPlayerAssetChange(object sender, EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs != null && gameGoodsNumChangeEventArgs.GoodsId == 6)
			{
				SetDiamoundCountColor();
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
			CancelInvoke();
		}

		public void OnClickClose()
		{
			GetAward(1);
		}

		public void OnClick3Multi()
		{
			double playGoodsNum = PlayerDataModule.Instance.GetPlayGoodsNum(6);
			int offlineProductCostDiamound = GameCommon.offlineProductCostDiamound;
			if (playGoodsNum >= (double)offlineProductCostDiamound)
			{
				GetAward(3);
				PlayerDataModule.Instance.ChangePlayerGoodsNum(6, -offlineProductCostDiamound);
			}
		}

		public void OnClick2Multi()
		{
			InfocUtils.Report_rollingsky2_games_ads(33, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.MainView, delegate(ADScene adScen)
			{
				OnAdSuccess(adScen);
				InfocUtils.Report_rollingsky2_games_ads(33, 0, 1, 0, 4, 0);
			});
		}

		private void OnAdSuccess(ADScene adScen = ADScene.NONE)
		{
			GetAward(2);
		}

		private void GetAward(int multi)
		{
			selectMulti = multi;
			PlayerDataModule instance = PlayerDataModule.Instance;
			if ((bool)goldMoveTarget)
			{
				double playGoodsNum = instance.GetPlayGoodsNum(3);
				goldMoveTarget.SetData(playGoodsNum, playGoodsNum + instance.OffLineProductGoldNum * (double)multi, -1);
			}
			if ((bool)noteMoveTarget)
			{
				double playGoodsNum2 = instance.GetPlayGoodsNum(GameCommon.REPUTATION_ID);
				noteMoveTarget.SetData(playGoodsNum2, playGoodsNum2 + instance.OffLineProductReputationNum * (double)multi, -1);
			}
			instance.ChangePlayerGoodsNum(GameCommon.REPUTATION_ID, instance.OffLineProductReputationNum * (double)multi, AssertChangeType.LEVEL_CONSUME, NetWorkSynType.IMEDIATELY, false);
			instance.ChangePlayerGoodsNum(3, instance.OffLineProductGoldNum * (double)multi, AssertChangeType.LEVEL_CONSUME, NetWorkSynType.IMEDIATELY, false);
			DoGetEffect();
		}

		private void GoldIncreaseSetter(double x)
		{
			gold.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(x);
		}

		private double GoldIncreaseGetter()
		{
			return PlayerDataModule.Instance.OffLineProductGoldNum;
		}

		private void NoteIncreaseSetter(double x)
		{
			note.text = MonoSingleton<GameTools>.Instacne.DoubleToFormatString(x);
		}

		private double NoteIncreaseGetter()
		{
			return PlayerDataModule.Instance.OffLineProductReputationNum;
		}

		private void StartIncreaseMulti()
		{
			PlayerDataModule instance = PlayerDataModule.Instance;
			DOTween.To(GoldIncreaseGetter, GoldIncreaseSetter, instance.OffLineProductGoldNum * (double)selectMulti, 1f);
			DOTween.To(NoteIncreaseGetter, NoteIncreaseSetter, instance.OffLineProductReputationNum * (double)selectMulti, 1f).SetEase(Ease.Linear);
			Mod.Sound.PlayUISound(20019);
		}

		private void DoGetEffect()
		{
			if (selectMulti > 1)
			{
				StartIncreaseMulti();
				Invoke("DoMoveEffect", 1f);
			}
			else
			{
				DoMoveEffect();
			}
		}

		private void DoMoveEffect()
		{
			GetAwardForm form = GetAwardForm.Form;
			if (form != null)
			{
				form.StartMove(goldMove.position, goldMove.sizeDelta, "gold", 3, goldMoveTarget, GetGoldMoveCount(selectMulti));
				form.StartMove(noteMove.position, noteMove.sizeDelta, "note", GameCommon.REPUTATION_ID, noteMoveTarget, GetNoteMoveCount(selectMulti));
			}
		}

		private int GetGoldMoveCount(int multi)
		{
			PlayerDataModule instance = PlayerDataModule.Instance;
			if (instance.OffLineProductGoldNum * (double)multi < 20.0)
			{
				return (int)instance.OffLineProductGoldNum * multi;
			}
			return multi * 8;
		}

		private int GetNoteMoveCount(int multi)
		{
			PlayerDataModule instance = PlayerDataModule.Instance;
			if (instance.OffLineProductReputationNum * (double)multi < 20.0)
			{
				return (int)instance.OffLineProductReputationNum * multi;
			}
			return multi * 8;
		}
	}
}
