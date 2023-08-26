using System;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LuckyTurntableForm : UGUIForm
	{
		private enum State
		{
			NORMAL,
			NOAD,
			NOCOUNT
		}

		[Serializable]
		public class Slot
		{
			private const int JINBI_ID = 3;

			private const int SHENGWANG_ID = 32;

			public GameObject activeGo;

			public Text nameText;

			public Image iconImg;

			private int _count;

			private int _weigth;

			private int _itemId;

			private bool _released;

			private object _iconAsset;

			public int Weigth
			{
				get
				{
					return _weigth;
				}
			}

			public int ItemId
			{
				get
				{
					return _itemId;
				}
			}

			public double AwardNum { get; private set; }

			public void SetActive(bool active)
			{
				activeGo.SetActive(active);
			}

			public void Init(LuckyTurntable_table table)
			{
				activeGo.SetActive(false);
				_itemId = table.GoodsId;
				_count = table.GoodsTime;
				if (_count <= 0)
				{
					_count = table.GoodsCount;
				}
				_weigth = table.WeightNess;
				_released = false;
				nameText.text = Mod.Localization.GetInfoById(table.Desc);
				int goodsIconIdByGoodsId = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(table.GoodsId);
				Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(goodsIconIdByGoodsId.ToString()), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
				{
					if (_released)
					{
						Mod.Resource.UnloadAsset(asset);
					}
					else
					{
						_iconAsset = asset;
						iconImg.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					}
				}, delegate(string assetName, string errorMessage, object data2)
				{
					Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
				}));
			}

			public void Award()
			{
				switch (_itemId)
				{
				case 3:
					AwardNum = PlayerDataModule.Instance.GetOffLineProductionGoldByTime(_count * 1000);
					PlayerDataModule.Instance.ChangePlayerGoodsNum(_itemId, AwardNum);
					break;
				case 32:
					AwardNum = PlayerDataModule.Instance.GetCurrentProductReputaionSpeed() * (double)_count;
					PlayerDataModule.Instance.ChangePlayerGoodsNum(_itemId, AwardNum);
					break;
				default:
					AwardNum = _count;
					PlayerDataModule.Instance.ChangePlayerGoodsNum(_itemId, AwardNum);
					break;
				}
			}

			public void Release()
			{
				_released = true;
				if (_iconAsset != null)
				{
					Mod.Resource.UnloadAsset(_iconAsset);
				}
			}
		}

		public static int FREE_TOTAL_COUNT = 3;

		private const int STEP_ANGLE = 45;

		[Space(2f)]
		public Transform turntable;

		public Animator animator;

		public Text freeCountText;

		public Button withAdBtn;

		public GameObject normalStat;

		public GameObject noAdState;

		public GameObject noTimeState;

		public Button withGemBtn;

		public Text needGemCountText;

		public Button closeBtn;

		public Button helpBtn;

		public Slot[] slots;

		[Range(0f, 10f)]
		public int cyclesNum = 5;

		[Space(2f)]
		public float totalTime = 3f;

		[Range(0.1f, 0.9f)]
		public float percentOfTimeForMaxSpeed = 0.6f;

		[Range(0.1f, 10f)]
		public float openAwardUIDelayDuration = 0.5f;

		private PlayerLuckTurnTableLocalData _localData;

		private int _slotsNum;

		private double _totalAngle = 360.0;

		private double _maxSpeed;

		private double _maxSpeedTime;

		private double _maxSpeedAngle;

		private double _latestAngle;

		private double _initTime;

		private double _acceleration;

		private bool _isStartSpin;

		private bool _isNagativeAcceleration;

		private int _selectIndex;

		private float _originRotation;

		private int _totalWeight;

		private bool _beginSpin;

		private bool _endSpin;

		private Color m_sourceColor;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			_localData = PlayerDataModule.Instance.LuckTurnLocalData;
			UpdateFreeCountText();
			needGemCountText.text = GameCommon.luckyTurnTableSpentGemsNum.ToString();
			m_sourceColor = needGemCountText.color;
			_totalWeight = 0;
			_slotsNum = slots.Length;
			IDataTable<LuckyTurntable_table> dataTable = Mod.DataTable.Get<LuckyTurntable_table>();
			if (dataTable.Count < _slotsNum)
			{
				_slotsNum = dataTable.Count;
			}
			for (int i = 0; i < _slotsNum; i++)
			{
				slots[i].Init(dataTable[i + 1]);
				_totalWeight += slots[i].Weigth;
			}
		}

		private State ComputerState()
		{
			if (_localData.FreeCount > 0)
			{
				if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView))
				{
					return State.NORMAL;
				}
				return State.NOAD;
			}
			return State.NOCOUNT;
		}

		private void SetUIByState(State state)
		{
			switch (state)
			{
			case State.NORMAL:
				normalStat.SetActive(true);
				noAdState.SetActive(false);
				noTimeState.SetActive(false);
				break;
			case State.NOAD:
				normalStat.SetActive(false);
				noAdState.SetActive(true);
				noTimeState.SetActive(false);
				break;
			case State.NOCOUNT:
				normalStat.SetActive(false);
				noAdState.SetActive(false);
				noTimeState.SetActive(true);
				break;
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			_originRotation = turntable.localRotation.eulerAngles.z;
			AddEventListener();
			State state = ComputerState();
			SetUIByState(state);
			if (state == State.NOAD)
			{
				InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
			}
			if (PlayerDataModule.Instance.GetPlayGoodsNum(6) < (double)GameCommon.luckyTurnTableSpentGemsNum)
			{
				needGemCountText.color = GameCommon.COMMON_RED;
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			CancelInvoke();
			RemoveEventListener();
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			Spin();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < _slotsNum; i++)
			{
				slots[i].Release();
			}
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(withAdBtn.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickWithAdBtn));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(withGemBtn.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickWithGemBtn));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(helpBtn.gameObject);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnClickHelpBtn));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(closeBtn.gameObject);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseBtn));
			Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(withAdBtn.gameObject);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickWithAdBtn));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(withGemBtn.gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickWithGemBtn));
			EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(helpBtn.gameObject);
			eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnClickHelpBtn));
			EventTriggerListener eventTriggerListener4 = EventTriggerListener.Get(closeBtn.gameObject);
			eventTriggerListener4.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener4.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseBtn));
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		private void OnPlayerAssetChange(object sender, Foundation.EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs != null && gameGoodsNumChangeEventArgs.GoodsId == 6)
			{
				if (PlayerDataModule.Instance.GetPlayGoodsNum(6) < (double)GameCommon.luckyTurnTableSpentGemsNum)
				{
					needGemCountText.color = GameCommon.COMMON_RED;
				}
				else
				{
					needGemCountText.color = m_sourceColor;
				}
			}
		}

		private void OnClickWithAdBtn(GameObject go)
		{
			if (ComputerState() != 0)
			{
				return;
			}
			InfocUtils.Report_rollingsky2_games_ads(27, 0, 1, 0, 3, 0);
			_localData.FreeCount--;
			UpdateFreeCountText();
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.MainView, delegate
			{
				if (!_isStartSpin && StartSpin())
				{
					InfocUtils.Report_rollingsky2_games_ads(27, 0, 1, 0, 4, 0);
					UpdateFreeCountText();
					State uIByState = ComputerState();
					SetUIByState(uIByState);
				}
				MonoSingleton<GameTools>.Instacne.EnableInput();
			});
		}

		private void OnClickWithGemBtn(GameObject go)
		{
			if (!_isStartSpin)
			{
				int luckyTurnTableSpentGemsNum = GameCommon.luckyTurnTableSpentGemsNum;
				if (!(PlayerDataModule.Instance.GetPlayGoodsNum(6) < (double)luckyTurnTableSpentGemsNum) && StartSpin())
				{
					PlayerDataModule.Instance.ChangePlayerGoodsNum(6, -luckyTurnTableSpentGemsNum);
				}
			}
		}

		private void OnClickHelpBtn(GameObject go)
		{
			Mod.UI.OpenUIForm(UIFormId.LuckyTurntableInfoForm);
		}

		private void OnClickCloseBtn(GameObject go)
		{
			Mod.UI.CloseUIForm(UIFormId.LuckyTurntableForm);
		}

		private void CheckAds()
		{
			MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView);
			State uIByState = ComputerState();
			SetUIByState(uIByState);
		}

		private bool RandomItem()
		{
			int num = UnityEngine.Random.Range(0, _totalWeight);
			int num2 = 0;
			for (int i = 0; i < _slotsNum; i++)
			{
				int num3 = num2 + slots[i].Weigth;
				if (num >= num2 && num < num3)
				{
					_selectIndex = i;
					_totalAngle = (double)(cyclesNum * 360) + (360.0 - ((double)_selectIndex + 0.5) * 45.0);
					return true;
				}
				num2 += slots[i].Weigth;
			}
			Log.Error("LuckyTurntable: random award failed.");
			return false;
		}

		private bool StartSpin()
		{
			if (_isStartSpin)
			{
				return false;
			}
			closeBtn.gameObject.SetActive(false);
			helpBtn.gameObject.SetActive(false);
			withAdBtn.gameObject.SetActive(false);
			withGemBtn.gameObject.SetActive(false);
			if (!RandomItem())
			{
				return false;
			}
			_maxSpeed = 2.0 * _totalAngle / (double)totalTime;
			_maxSpeedTime = totalTime * percentOfTimeForMaxSpeed;
			_maxSpeedAngle = 0.0;
			_latestAngle = 0.0;
			_initTime = Time.time;
			_acceleration = _maxSpeed / _maxSpeedTime;
			_beginSpin = false;
			_endSpin = false;
			_isStartSpin = true;
			return true;
		}

		private void Spin()
		{
			if (!_isStartSpin)
			{
				return;
			}
			if (!_beginSpin)
			{
				_beginSpin = true;
				animator.SetBool("Spin", true);
			}
			double num = (double)Time.time - _initTime;
			if (_isNagativeAcceleration && _maxSpeedTime + num > (double)totalTime)
			{
				if (!_endSpin)
				{
					_endSpin = true;
					animator.SetBool("Win", true);
					SpinFinished();
				}
				return;
			}
			if (!_isNagativeAcceleration && num > _maxSpeedTime)
			{
				num -= _maxSpeedTime;
				_isNagativeAcceleration = true;
				_initTime += _maxSpeedTime;
				_maxSpeedAngle = 0.5 * _acceleration * _maxSpeedTime * _maxSpeedTime;
				double num2 = (double)totalTime - _maxSpeedTime;
				_acceleration = (0.0 - _maxSpeed) / num2;
			}
			_latestAngle = (_isNagativeAcceleration ? (_maxSpeedAngle + _maxSpeed * num + 0.5 * _acceleration * num * num) : (0.5 * _acceleration * num * num));
			turntable.localRotation = Quaternion.Euler(0f, 0f, (float)((double)_originRotation - _latestAngle));
		}

		private void SpinFinished()
		{
			slots[_selectIndex].Award();
			slots[_selectIndex].activeGo.SetActive(true);
			Invoke("OpenAwardUI", openAwardUIDelayDuration);
		}

		private void OpenAwardUI()
		{
			CancelInvoke("OpenAwardUI");
			if (_selectIndex >= 0 && _selectIndex < _slotsNum)
			{
				Slot slot = slots[_selectIndex];
				GetGoodsData getGoodsData = new GetGoodsData();
				getGoodsData.GoodsTeam = false;
				getGoodsData.GoodsId = slot.ItemId;
				getGoodsData.GoodsNum = slot.AwardNum;
				Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
				slot.SetActive(false);
				_selectIndex = -1;
			}
			closeBtn.gameObject.SetActive(true);
			helpBtn.gameObject.SetActive(true);
			withGemBtn.gameObject.SetActive(true);
			withAdBtn.gameObject.SetActive(true);
			turntable.localRotation = Quaternion.Euler(0f, 0f, _originRotation);
			_isStartSpin = false;
			_isNagativeAcceleration = false;
		}

		private void UpdateFreeCountText()
		{
			string infoById = Mod.Localization.GetInfoById(305);
			freeCountText.text = string.Format(infoById, _localData.FreeCount.ToString(), FREE_TOTAL_COUNT.ToString());
		}
	}
}
