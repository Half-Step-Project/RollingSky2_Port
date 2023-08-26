using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CoupleMirrorDancer : BaseTriggerBox, IDanceCombine
	{
		public enum DancerState
		{
			None,
			BeginMirror,
			Mirroring,
			LeaveMirror,
			BeginCombine,
			Combined,
			LeaveCombine,
			End
		}

		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float BeginDistance;

			public float CombineDeltaX;

			public int AnimIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginDistance = bytes.GetSingle(ref startIndex);
				CombineDeltaX = bytes.GetSingle(ref startIndex);
				AnimIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(CombineDeltaX.GetBytes(), ref offset);
					memoryStream.WriteByteArray(AnimIndex.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		protected Dictionary<int, System.Action> updateStateList = new Dictionary<int, System.Action>();

		public TriggerData data;

		public DancerState CurrentState;

		private System.Action currentUpdate;

		private Animator coupleAnimator;

		private Dictionary<int, string> animStateNameDic = new Dictionary<int, string>();

		private RD_CoupleMirrorDancer_DATA rebirthData;

		private BaseRole theRole
		{
			get
			{
				return BaseRole.theBall;
			}
		}

		public Transform DancerTrans
		{
			get
			{
				return base.transform;
			}
		}

		public bool IfHaveTreasureChest
		{
			get
			{
				return false;
			}
		}

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			updateStateList[0] = UpdateNone;
			updateStateList[1] = UpdateBeginMirror;
			updateStateList[2] = UpdateMirroring;
			updateStateList[3] = UpdateLeaveMirror;
			updateStateList[4] = UpdateBeginCombine;
			updateStateList[5] = UpdateCombined;
			updateStateList[6] = UpdateLeaveCombine;
			updateStateList[7] = UpdateEnd;
			for (int i = 0; i < 46; i++)
			{
				Dictionary<int, string> dictionary = animStateNameDic;
				int key = i;
				CoupleThiefAnim coupleThiefAnim = (CoupleThiefAnim)i;
				dictionary[key] = coupleThiefAnim.ToString();
			}
			coupleAnimator = GetComponentInChildren<Animator>(true);
			coupleAnimator.gameObject.SetActive(false);
			coupleAnimator.speed = 0f;
			ChangeStateTo(DancerState.None);
		}

		public override void UpdateElement()
		{
			currentUpdate();
		}

		public override void ResetElement()
		{
			base.ResetElement();
			theRole.SeparateDancer(this, true);
			coupleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
			ChangeStateTo(DancerState.None);
		}

		public override void TriggerEnter(BaseRole ball)
		{
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}

		private void ChangeStateTo(DancerState state)
		{
			CurrentState = state;
			currentUpdate = updateStateList[(int)CurrentState];
		}

		private void UpdateNone()
		{
			if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance > 0f)
			{
				coupleAnimator.gameObject.SetActive(true);
				coupleAnimator.speed = Railway.theRailway.SpeedForward / 6f;
				CouplePlayAnim(45);
				ChangeStateTo(DancerState.BeginMirror);
				theRole.CombineDancer(this);
			}
			else if (coupleAnimator.speed != 0f)
			{
				CouplePlayAnim(28);
				coupleAnimator.speed = 0f;
			}
		}

		private void UpdateBeginMirror()
		{
			Vector3 mirrorPosition = GetMirrorPosition(theRole.transform);
			base.transform.position = mirrorPosition;
			ChangeStateTo(DancerState.Mirroring);
		}

		private void UpdateMirroring()
		{
			Vector3 position = theRole.transform.position;
			Vector3 mirrorPosition = GetMirrorPosition(theRole.transform);
			base.transform.position = mirrorPosition;
		}

		private void UpdateLeaveMirror()
		{
			StopElement();
		}

		private void UpdateBeginCombine()
		{
			theRole.CombineDancer(this, true);
			CouplePlayAnim(23, data.AnimIndex);
			ChangeStateTo(DancerState.Combined);
		}

		private void UpdateCombined()
		{
		}

		private void UpdateLeaveCombine()
		{
			StopElement();
		}

		private void UpdateEnd()
		{
		}

		private Vector3 GetMirrorPosition(Transform roleTrans)
		{
			Vector3 position = base.groupTransform.InverseTransformPoint(roleTrans.position);
			position.x = 21f - position.x;
			return base.groupTransform.TransformPoint(position);
		}

		private void CouplePlayAnim(int animType, int index = 0)
		{
			switch (animType)
			{
			case 45:
				coupleAnimator.Play(animStateNameDic[animType]);
				break;
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
				if (IfHaveTreasureChest)
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
			case 41:
			case 42:
			case 43:
			case 44:
				coupleAnimator.Play(animStateNameDic[animType + index]);
				break;
			default:
				Log.Error("Donot play anim state:" + (CoupleThiefAnim)animType);
				break;
			}
		}

		public void OnCombineDancer(Transform parent)
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}

		public void OnSeperateDancer(Transform parent)
		{
			base.transform.parent = base.groupTransform;
		}

		public void OnDanceTogether(int danceType)
		{
			switch (danceType)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			{
				theRole.SeparateDancer(this);
				int index = danceType - 1;
				CouplePlayAnim(19, index);
				ChangeStateTo(DancerState.Mirroring);
				break;
			}
			case 5:
			case 6:
			case 7:
			case 8:
			{
				theRole.CombineDancer(this, true);
				int index2 = danceType - 5;
				CouplePlayAnim(23, index2);
				ChangeStateTo(DancerState.Combined);
				break;
			}
			case 11:
				CouplePlayAnim(41);
				ChangeStateTo(DancerState.Mirroring);
				break;
			case 12:
				CouplePlayAnim(42);
				ChangeStateTo(DancerState.Mirroring);
				break;
			case 13:
				CouplePlayAnim(43);
				ChangeStateTo(DancerState.Combined);
				break;
			case 14:
				CouplePlayAnim(44);
				ChangeStateTo(DancerState.Combined);
				break;
			case 9:
				if (CurrentState != DancerState.Mirroring)
				{
					Log.Error(CurrentState.ToString() + " can not trigger EndMirrorMix");
				}
				theRole.SeparateDancer(this, true);
				ChangeStateTo(DancerState.LeaveMirror);
				break;
			case 10:
				if (CurrentState != DancerState.Combined)
				{
					Log.Error(CurrentState.ToString() + " can not trigger EndCombined");
				}
				theRole.SeparateDancer(this, true);
				ChangeStateTo(DancerState.LeaveCombine);
				break;
			case 0:
				break;
			}
		}

		public int GetDancingState()
		{
			return (int)CurrentState;
		}

		private void StopElement()
		{
			theRole.SeparateDancer(this, true);
			coupleAnimator.speed = 0f;
			ChangeStateTo(DancerState.End);
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_CoupleMirrorDancer_DATA rD_CoupleMirrorDancer_DATA = (rebirthData = JsonUtility.FromJson<RD_CoupleMirrorDancer_DATA>(rd_data as string));
			base.transform.SetTransData(rD_CoupleMirrorDancer_DATA.transformData);
			ChangeStateTo((DancerState)rD_CoupleMirrorDancer_DATA.CurrentState);
			coupleAnimator.gameObject.SetActive(rD_CoupleMirrorDancer_DATA.ifActive);
			coupleAnimator.SetAnimData(rD_CoupleMirrorDancer_DATA.coupleAnimatorData, ProcessState.Pause);
			bool haveParent = rD_CoupleMirrorDancer_DATA.haveParent;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_CoupleMirrorDancer_DATA
			{
				transformData = base.transform.GetTransData(),
				CurrentState = (int)CurrentState,
				coupleAnimatorData = coupleAnimator.GetAnimData(),
				haveParent = (base.transform.parent == BaseRole.theBall.transform),
				ifActive = coupleAnimator.gameObject.activeSelf
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			coupleAnimator.SetAnimData(rebirthData.coupleAnimatorData, ProcessState.UnPause);
			rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_CoupleMirrorDancer_DATA rD_CoupleMirrorDancer_DATA = (rebirthData = Bson.ToObject<RD_CoupleMirrorDancer_DATA>(rd_data));
			base.transform.SetTransData(rD_CoupleMirrorDancer_DATA.transformData);
			ChangeStateTo((DancerState)rD_CoupleMirrorDancer_DATA.CurrentState);
			coupleAnimator.SetAnimData(rD_CoupleMirrorDancer_DATA.coupleAnimatorData, ProcessState.Pause);
			coupleAnimator.gameObject.SetActive(rD_CoupleMirrorDancer_DATA.ifActive);
			bool haveParent = rD_CoupleMirrorDancer_DATA.haveParent;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_CoupleMirrorDancer_DATA
			{
				transformData = base.transform.GetTransData(),
				CurrentState = (int)CurrentState,
				coupleAnimatorData = coupleAnimator.GetAnimData(),
				ifActive = coupleAnimator.gameObject.activeSelf,
				haveParent = (base.transform.parent == BaseRole.theBall)
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			coupleAnimator.SetAnimData(rebirthData.coupleAnimatorData, ProcessState.UnPause);
			rebirthData = null;
		}
	}
}
