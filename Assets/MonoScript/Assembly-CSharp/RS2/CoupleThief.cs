using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CoupleThief : BaseCouple
	{
		public static readonly string NodeTreasureBox = "modelPoint/role/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand/Thief_baoxiang_a_001";

		private CouplePathMoveData currentPathData;

		private BezierMover currentBezierMover = new BezierMover();

		private Railway theRailway;

		private AnimationCurve currentSpeedCurve;

		private Vector3 bezierBegin;

		private Vector3 bezierEnd;

		private float pathDeltaPos;

		private Animator coupleAnimator;

		private GameObject treasureElement;

		private Dictionary<int, string> animStateNameDic = new Dictionary<int, string>();

		private float dizzinessCounter;

		private float newRoundCounter;

		private bool ifSlowPath;

		private BaseRole theRole
		{
			get
			{
				return BaseRole.theBall;
			}
		}

		public override bool IsRecordOriginRebirth
		{
			get
			{
				return true;
			}
		}

		public override void Initialize(Vector3 startPos)
		{
			base.Initialize(startPos);
			theRailway = Railway.theRailway;
			for (int i = 0; i < 46; i++)
			{
				Dictionary<int, string> dictionary = animStateNameDic;
				int key = i;
				CoupleThiefAnim coupleThiefAnim = (CoupleThiefAnim)i;
				dictionary[key] = coupleThiefAnim.ToString();
			}
			coupleAnimator = GetComponentInChildren<Animator>();
			coupleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
			CouplePlayAnim(0);
			base.IfHaveTreasureChest = true;
			base.IfDizziness = false;
			dizzinessCounter = 0f;
			base.IfNewRound = false;
			newRoundCounter = 0f;
			ifSlowPath = !base.IfHaveTreasureChest;
			Transform transform = base.transform.Find(NodeTreasureBox);
			if ((bool)transform)
			{
				treasureElement = transform.gameObject;
				treasureElement.SetActive(false);
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<CoupleThiefAnimEventArgs>.EventId, OnCallAnimPlay);
			Mod.Event.Subscribe(EventArgs<ThiefGrabTreasureEventArgs>.EventId, OnCallGrabTreasure);
			Mod.Event.Subscribe(EventArgs<GameFailEventArgs>.EventId, OnRoleDie);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<CoupleThiefAnimEventArgs>.EventId, OnCallAnimPlay);
			Mod.Event.Unsubscribe(EventArgs<ThiefGrabTreasureEventArgs>.EventId, OnCallGrabTreasure);
			Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnRoleDie);
		}

		public override void ChangeToStartAnim()
		{
			coupleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
			CouplePlayAnim(1);
		}

		public override void ChangeToCutScene()
		{
		}

		public override void SetPathMoveData(CouplePathMoveData pathData)
		{
			ifSlowPath = !base.IfHaveTreasureChest;
			if (currentPathData != null)
			{
				if (currentPathData.BelongType == TreasureBelongType.Role)
				{
					if (base.IfHaveTreasureChest)
					{
						GainTreasureChest(false);
						theRole.GainTreasureChest(true);
					}
				}
				else if (currentPathData.BelongType == TreasureBelongType.Couple && !base.IfHaveTreasureChest)
				{
					GainTreasureChest(true);
					ifSlowPath = true;
					theRole.GainTreasureChest(false);
				}
			}
			currentPathData = pathData;
			bezierBegin = pathData.BezierPoints[0];
			bezierEnd = pathData.BezierPoints[pathData.BezierPoints.Length - 1];
			pathDeltaPos = bezierEnd.z - bezierBegin.z;
			if (pathData.BezierSlowPoints.Length == 0)
			{
				currentBezierMover.ResetData(pathData.BezierPoints);
			}
			else
			{
				currentBezierMover.ResetData(base.IfHaveTreasureChest ? pathData.BezierPoints : pathData.BezierSlowPoints);
			}
			currentSpeedCurve = (base.IfHaveTreasureChest ? currentPathData.SpeedQuickCurve : currentPathData.SpeedSlowCurve);
			base.IfNewRound = true;
			newRoundCounter = NewRoundTime;
		}

		private void SetPathMoveDataRebirth(CouplePathMoveData pathData)
		{
			currentPathData = pathData;
			bezierBegin = pathData.BezierPoints[0];
			bezierEnd = pathData.BezierPoints[pathData.BezierPoints.Length - 1];
			pathDeltaPos = bezierEnd.z - bezierBegin.z;
			if (pathData.BezierSlowPoints.Length == 0)
			{
				currentBezierMover.ResetData(pathData.BezierPoints);
			}
			else
			{
				currentBezierMover.ResetData(base.IfHaveTreasureChest ? pathData.BezierPoints : pathData.BezierSlowPoints);
			}
			currentSpeedCurve = (base.IfHaveTreasureChest ? currentPathData.SpeedQuickCurve : currentPathData.SpeedSlowCurve);
			base.IfNewRound = true;
			newRoundCounter = NewRoundTime;
		}

		public override void UpdateCouple()
		{
			if (base.IfDizziness)
			{
				dizzinessCounter -= Time.deltaTime;
				if (dizzinessCounter <= 0f)
				{
					dizzinessCounter = 0f;
					base.IfDizziness = false;
				}
			}
			if (base.IfNewRound)
			{
				newRoundCounter -= Time.deltaTime;
				if (newRoundCounter <= 0f)
				{
					newRoundCounter = 0f;
					base.IfNewRound = false;
				}
			}
			Vector3 position = theRailway.transform.position;
			Vector3 targetPos = base.transform.position;
			if (currentPathData != null)
			{
				float time = (position.z - bezierBegin.z) / (bezierEnd.z - bezierBegin.z);
				float num = currentSpeedCurve.Evaluate(time);
				targetPos.z = bezierBegin.z + pathDeltaPos * num;
				Vector3 moveDir = Vector3.forward;
				currentBezierMover.MoveToPosZ(ref targetPos, ref moveDir);
				base.transform.position = targetPos;
				if (currentPathData.IfFollowDir)
				{
					moveDir.y = 0f;
					base.transform.forward = moveDir;
				}
			}
		}

		public override void ResetCouple()
		{
			base.ResetCouple();
			currentPathData = null;
			currentBezierMover.ResetData();
			currentSpeedCurve = null;
			base.IfHaveTreasureChest = true;
			ifSlowPath = !base.IfHaveTreasureChest;
			if ((bool)treasureElement)
			{
				treasureElement.SetActive(false);
			}
			base.IfDizziness = false;
			dizzinessCounter = 0f;
			base.IfNewRound = false;
			newRoundCounter = 0f;
			coupleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
			CouplePlayAnim(0);
		}

		private void CouplePlayAnim(int animType, int index = 0)
		{
			switch (animType)
			{
			case 0:
			case 1:
			case 2:
			case 27:
			case 28:
			case 29:
				coupleAnimator.Play(animStateNameDic[animType]);
				break;
			case 3:
			case 5:
			case 9:
			case 11:
			case 13:
			case 15:
			case 17:
				if (base.IfHaveTreasureChest)
				{
					animType++;
				}
				coupleAnimator.Play(animStateNameDic[animType]);
				break;
			case 4:
			case 6:
			case 7:
			case 8:
			case 10:
			case 12:
			case 14:
			case 16:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 30:
			case 31:
			case 32:
			case 33:
			case 41:
			case 42:
			case 43:
			case 44:
				coupleAnimator.Play(animStateNameDic[animType + index]);
				break;
			default:
				Log.Error("Donot play anim state:{0}", (CoupleThiefAnim)animType);
				break;
			}
		}

		private void OnCallAnimPlay(object sender, EventArgs e)
		{
			CoupleThiefAnimEventArgs coupleThiefAnimEventArgs = e as CoupleThiefAnimEventArgs;
			if (coupleThiefAnimEventArgs != null && coupleThiefAnimEventArgs.Receiver == CoupleAnimReceiverType.eDefault)
			{
				CouplePlayAnim(coupleThiefAnimEventArgs.AnimType);
			}
		}

		private void OnCallGrabTreasure(object sender, EventArgs e)
		{
			ThiefGrabTreasureEventArgs thiefGrabTreasureEventArgs = e as ThiefGrabTreasureEventArgs;
			if (thiefGrabTreasureEventArgs != null)
			{
				bool ifGrab = thiefGrabTreasureEventArgs.IfGrab;
				treasureElement.SetActive(ifGrab);
			}
		}

		private void OnRoleDie(object sender, EventArgs e)
		{
			if (e is GameFailEventArgs)
			{
				coupleAnimator.speed = 0f;
			}
		}

		public override void GainTreasureChest(bool ifGain)
		{
			base.GainTreasureChest(ifGain);
			if (ifGain)
			{
				if ((bool)treasureElement)
				{
					treasureElement.SetActive(true);
				}
				return;
			}
			CouplePlayAnim(27);
			base.IfDizziness = true;
			dizzinessCounter = DizzinessTime;
			if ((bool)treasureElement)
			{
				treasureElement.SetActive(false);
			}
		}

		protected override void OnTriggerEnter(Collider collider)
		{
			if (currentSpeedCurve == null)
			{
				return;
			}
			bool flag = true;
			if (currentPathData != null)
			{
				if (currentPathData.BelongType == TreasureBelongType.Role)
				{
					if (!base.IfHaveTreasureChest)
					{
						flag = false;
					}
				}
				else if (currentPathData.BelongType == TreasureBelongType.Couple && base.IfHaveTreasureChest)
				{
					flag = false;
				}
			}
			if (flag && !base.IfNewRound && !base.IfDizziness)
			{
				BaseRole componentInParent = collider.transform.GetComponentInParent<BaseRole>();
				if (componentInParent != null)
				{
					if (!componentInParent.IfDizziness)
					{
						bool ifHaveTreasureChest = componentInParent.IfHaveTreasureChest;
						GainTreasureChest(ifHaveTreasureChest);
						componentInParent.GainTreasureChest(!ifHaveTreasureChest);
					}
					return;
				}
			}
			BaseElement gameComponent = collider.gameObject.GetGameComponent<BaseElement>();
			if ((bool)gameComponent)
			{
				gameComponent.CoupleTriggerEnter(this, collider);
			}
		}

		public override object GetOriginRebirthData(object obj = null)
		{
			string empty = string.Empty;
			RD_CoupleThief_DATA rD_CoupleThief_DATA = new RD_CoupleThief_DATA();
			rD_CoupleThief_DATA.transformData = base.transform.GetTransData();
			rD_CoupleThief_DATA.IfHaveTreasureChest = base.IfHaveTreasureChest;
			rD_CoupleThief_DATA.ifSlowPath = ifSlowPath;
			rD_CoupleThief_DATA.treasureElementShow = (bool)treasureElement && treasureElement.activeSelf;
			rD_CoupleThief_DATA.IfDizziness = base.IfDizziness;
			rD_CoupleThief_DATA.dizzinessCounter = dizzinessCounter;
			rD_CoupleThief_DATA.newRoundCounter = newRoundCounter;
			rD_CoupleThief_DATA.coupleAnimator = coupleAnimator.GetAnimData();
			if (currentPathData != null)
			{
				rD_CoupleThief_DATA.pathTriggerUuid = currentPathData.triggerUuid;
			}
			rD_CoupleThief_DATA.currentBezierMoverData = currentBezierMover.GetBezierData();
			return JsonUtility.ToJson(rD_CoupleThief_DATA);
		}

		public override void SetOriginRebirthData(object dataInfo)
		{
			RD_CoupleThief_DATA rD_CoupleThief_DATA = JsonUtility.FromJson<RD_CoupleThief_DATA>(dataInfo as string);
			base.transform.SetTransData(rD_CoupleThief_DATA.transformData);
			base.IfHaveTreasureChest = rD_CoupleThief_DATA.IfHaveTreasureChest;
			if ((bool)treasureElement)
			{
				treasureElement.SetActive(rD_CoupleThief_DATA.treasureElementShow);
			}
			base.IfDizziness = rD_CoupleThief_DATA.IfDizziness;
			dizzinessCounter = rD_CoupleThief_DATA.dizzinessCounter;
			newRoundCounter = rD_CoupleThief_DATA.newRoundCounter;
			ifSlowPath = rD_CoupleThief_DATA.ifSlowPath;
			coupleAnimator.SetAnimData(rD_CoupleThief_DATA.coupleAnimator, ProcessState.Pause);
			if (rD_CoupleThief_DATA.pathTriggerUuid <= 0)
			{
				return;
			}
			BaseElement element;
			MapController.Instance.TryGetElementByUUID<BaseElement>(rD_CoupleThief_DATA.pathTriggerUuid, out element);
			if ((bool)element)
			{
				ThiefMovePathTrigger thiefMovePathTrigger = element as ThiefMovePathTrigger;
				if ((bool)thiefMovePathTrigger)
				{
					CouplePathMoveData couplePathMoveData = thiefMovePathTrigger.GetCouplePathMoveData();
					SetPathMoveDataRebirth(couplePathMoveData);
					currentBezierMover.SetBezierData(rD_CoupleThief_DATA.currentBezierMoverData);
				}
			}
		}

		public override void StartRunByOriginRebirthData(object dataInfo)
		{
			RD_CoupleThief_DATA rD_CoupleThief_DATA = JsonUtility.FromJson<RD_CoupleThief_DATA>(dataInfo as string);
			coupleAnimator.SetAnimData(rD_CoupleThief_DATA.coupleAnimator, ProcessState.UnPause);
		}

		public override byte[] GetOriginRebirthBsonData(object obj = null)
		{
			RD_CoupleThief_DATA rD_CoupleThief_DATA = new RD_CoupleThief_DATA();
			rD_CoupleThief_DATA.transformData = base.transform.GetTransData();
			rD_CoupleThief_DATA.IfHaveTreasureChest = base.IfHaveTreasureChest;
			rD_CoupleThief_DATA.ifSlowPath = ifSlowPath;
			rD_CoupleThief_DATA.treasureElementShow = (bool)treasureElement && treasureElement.activeSelf;
			rD_CoupleThief_DATA.IfDizziness = base.IfDizziness;
			rD_CoupleThief_DATA.dizzinessCounter = dizzinessCounter;
			rD_CoupleThief_DATA.newRoundCounter = newRoundCounter;
			rD_CoupleThief_DATA.coupleAnimator = coupleAnimator.GetAnimData();
			if (currentPathData != null)
			{
				rD_CoupleThief_DATA.pathTriggerUuid = currentPathData.triggerUuid;
			}
			rD_CoupleThief_DATA.currentBezierMoverData = currentBezierMover.GetBezierData();
			return Bson.ToBson(rD_CoupleThief_DATA);
		}

		public override void SetOriginRebirthBsonData(byte[] dataInfo)
		{
			RD_CoupleThief_DATA rD_CoupleThief_DATA = Bson.ToObject<RD_CoupleThief_DATA>(dataInfo);
			base.transform.SetTransData(rD_CoupleThief_DATA.transformData);
			base.IfHaveTreasureChest = rD_CoupleThief_DATA.IfHaveTreasureChest;
			if ((bool)treasureElement)
			{
				treasureElement.SetActive(rD_CoupleThief_DATA.treasureElementShow);
			}
			base.IfDizziness = rD_CoupleThief_DATA.IfDizziness;
			dizzinessCounter = rD_CoupleThief_DATA.dizzinessCounter;
			newRoundCounter = rD_CoupleThief_DATA.newRoundCounter;
			ifSlowPath = rD_CoupleThief_DATA.ifSlowPath;
			coupleAnimator.SetAnimData(rD_CoupleThief_DATA.coupleAnimator, ProcessState.Pause);
			if (rD_CoupleThief_DATA.pathTriggerUuid <= 0)
			{
				return;
			}
			BaseElement element;
			MapController.Instance.TryGetElementByUUID<BaseElement>(rD_CoupleThief_DATA.pathTriggerUuid, out element);
			if ((bool)element)
			{
				ThiefMovePathTrigger thiefMovePathTrigger = element as ThiefMovePathTrigger;
				if ((bool)thiefMovePathTrigger)
				{
					CouplePathMoveData couplePathMoveData = thiefMovePathTrigger.GetCouplePathMoveData();
					SetPathMoveDataRebirth(couplePathMoveData);
					currentBezierMover.SetBezierData(rD_CoupleThief_DATA.currentBezierMoverData);
				}
			}
		}

		public override void StartRunByOriginRebirthBsonData(byte[] dataInfo)
		{
			RD_CoupleThief_DATA rD_CoupleThief_DATA = Bson.ToObject<RD_CoupleThief_DATA>(dataInfo);
			coupleAnimator.SetAnimData(rD_CoupleThief_DATA.coupleAnimator, ProcessState.UnPause);
		}
	}
}
