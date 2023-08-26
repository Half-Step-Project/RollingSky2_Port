using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ThiefEndingTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float RoleWaitTime;

			public float CoupleWaitTime;

			public float ChestWaitTime;

			public float CameraWaitTime;

			public bool IfForcePos;

			public Vector3 ForcePos;

			public Vector3 ForceCameraPos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				RoleWaitTime = bytes.GetSingle(ref startIndex);
				CoupleWaitTime = bytes.GetSingle(ref startIndex);
				ChestWaitTime = bytes.GetSingle(ref startIndex);
				CameraWaitTime = bytes.GetSingle(ref startIndex);
				IfForcePos = bytes.GetBoolean(ref startIndex);
				ForcePos = bytes.GetVector3(ref startIndex);
				ForceCameraPos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(RoleWaitTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(CoupleWaitTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ChestWaitTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(CameraWaitTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfForcePos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ForcePos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ForceCameraPos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string NodeForceRolePos = "shuaige";

		public TriggerData data;

		public AnimationClip CameraAnimClip;

		private float curRoleTime;

		private float curCoupleTime;

		private float curChestTime;

		private float curCameraTime;

		private bool ifWaitRole;

		private bool ifWaitCouple;

		private bool ifWaitChest;

		private bool ifWaitCamera;

		private bool ifResetCameraPos;

		private RD_ThiefEndingTrigger_DATA rebirthData;

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
			curRoleTime = 0f;
			curCoupleTime = 0f;
			curChestTime = 0f;
			curCameraTime = 0f;
			ifWaitRole = false;
			ifWaitCouple = false;
			ifWaitChest = false;
			ifWaitCamera = false;
			ifResetCameraPos = false;
			commonState = CommonState.None;
			Animation componentInChildren = GetComponentInChildren<Animation>();
			if ((bool)componentInChildren)
			{
				CameraAnimClip = componentInChildren.clip;
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			curRoleTime = 0f;
			curCoupleTime = 0f;
			curChestTime = 0f;
			curCameraTime = 0f;
			ifWaitRole = false;
			ifWaitCouple = false;
			ifWaitChest = false;
			ifWaitCamera = false;
			ifResetCameraPos = false;
			commonState = CommonState.None;
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.None)
			{
				return;
			}
			if (commonState == CommonState.Begin)
			{
				if (ifWaitRole)
				{
					curRoleTime += Time.deltaTime;
					if (curRoleTime >= data.RoleWaitTime)
					{
						if (data.IfForcePos)
						{
							BaseRole.theBall.transform.position = data.ForcePos;
						}
						BaseRole.theBall.IfWinBeforeFinish = true;
						BaseRole.theBall.TriggerRolePlayAnim(BaseRole.AnimType.SuccessState);
						curRoleTime = 0f;
						ifWaitRole = false;
					}
				}
				if (ifWaitCouple)
				{
					Mod.Event.Fire(this, Mod.Reference.Acquire<ThiefEndEventArgs>().Initialize(ThiefEndEventArgs.eObjCouple, data.RoleWaitTime, data.CoupleWaitTime, data.ChestWaitTime, data.CameraWaitTime));
					ifWaitCouple = false;
				}
				if (ifWaitChest)
				{
					curChestTime += Time.deltaTime;
					if (curChestTime >= data.ChestWaitTime)
					{
						Mod.Event.Fire(this, Mod.Reference.Acquire<ThiefEndEventArgs>().Initialize(ThiefEndEventArgs.eObjChest, data.RoleWaitTime, data.CoupleWaitTime, data.ChestWaitTime, data.CameraWaitTime));
						BaseRole.theBall.ForceShowTreasureChest(false);
						curChestTime = 0f;
						ifWaitChest = false;
					}
				}
				if (ifResetCameraPos)
				{
					Vector3 position = CameraController.theCamera.transform.position;
					position.y = data.ForceCameraPos.y;
					position.z = data.ForceCameraPos.z;
					ifResetCameraPos = false;
				}
				if (ifWaitCamera)
				{
					curCameraTime += Time.deltaTime;
					if (curCameraTime >= data.CameraWaitTime)
					{
						CameraController.theCamera.ChangeStateToWinStatic();
						if ((bool)CameraAnimClip)
						{
							CameraController.theCamera.TriggerPlayAnimClip(CameraAnimClip, CameraAnimClip.name);
						}
						curCameraTime = 0f;
						ifWaitCamera = false;
					}
				}
				if (ifWaitRole && ifWaitCouple && ifWaitChest && ifWaitCamera && ifResetCameraPos)
				{
					commonState = CommonState.End;
				}
			}
			else
			{
				CommonState commonState2 = commonState;
				int num = 5;
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState == CommonState.None)
			{
				ifWaitRole = true;
				ifWaitCouple = true;
				ifWaitChest = true;
				ifWaitCamera = true;
				ifResetCameraPos = true;
				commonState = CommonState.Begin;
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
			Transform transform = base.transform.Find(NodeForceRolePos);
			if ((bool)transform)
			{
				transform.position = data.ForcePos;
			}
		}

		public override string Write()
		{
			Transform transform = base.transform.Find(NodeForceRolePos);
			if ((bool)transform)
			{
				data.ForcePos = transform.position;
			}
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

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_ThiefEndingTrigger_DATA rD_ThiefEndingTrigger_DATA = (rebirthData = JsonUtility.FromJson<RD_ThiefEndingTrigger_DATA>(rd_data as string));
			curRoleTime = rD_ThiefEndingTrigger_DATA.curRoleTime;
			curCoupleTime = rD_ThiefEndingTrigger_DATA.curCoupleTime;
			curChestTime = rD_ThiefEndingTrigger_DATA.curChestTime;
			curCameraTime = rD_ThiefEndingTrigger_DATA.curCameraTime;
			ifWaitRole = rD_ThiefEndingTrigger_DATA.ifWaitRole;
			ifWaitCouple = rD_ThiefEndingTrigger_DATA.ifWaitCouple;
			ifWaitChest = rD_ThiefEndingTrigger_DATA.ifWaitChest;
			ifWaitCamera = rD_ThiefEndingTrigger_DATA.ifWaitCamera;
			ifResetCameraPos = rD_ThiefEndingTrigger_DATA.ifResetCameraPos;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_ThiefEndingTrigger_DATA
			{
				curRoleTime = curRoleTime,
				curCoupleTime = curCoupleTime,
				curChestTime = curChestTime,
				curCameraTime = curCameraTime,
				ifWaitRole = ifWaitRole,
				ifWaitCouple = ifWaitCouple,
				ifWaitChest = ifWaitChest,
				ifWaitCamera = ifWaitCamera,
				ifResetCameraPos = ifResetCameraPos
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_ThiefEndingTrigger_DATA rD_ThiefEndingTrigger_DATA = (rebirthData = Bson.ToObject<RD_ThiefEndingTrigger_DATA>(rd_data));
			curRoleTime = rD_ThiefEndingTrigger_DATA.curRoleTime;
			curCoupleTime = rD_ThiefEndingTrigger_DATA.curCoupleTime;
			curChestTime = rD_ThiefEndingTrigger_DATA.curChestTime;
			curCameraTime = rD_ThiefEndingTrigger_DATA.curCameraTime;
			ifWaitRole = rD_ThiefEndingTrigger_DATA.ifWaitRole;
			ifWaitCouple = rD_ThiefEndingTrigger_DATA.ifWaitCouple;
			ifWaitChest = rD_ThiefEndingTrigger_DATA.ifWaitChest;
			ifWaitCamera = rD_ThiefEndingTrigger_DATA.ifWaitCamera;
			ifResetCameraPos = rD_ThiefEndingTrigger_DATA.ifResetCameraPos;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_ThiefEndingTrigger_DATA
			{
				curRoleTime = curRoleTime,
				curCoupleTime = curCoupleTime,
				curChestTime = curChestTime,
				curCameraTime = curCameraTime,
				ifWaitRole = ifWaitRole,
				ifWaitCouple = ifWaitCouple,
				ifWaitChest = ifWaitChest,
				ifWaitCamera = ifWaitCamera,
				ifResetCameraPos = ifResetCameraPos
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			rebirthData = null;
		}
	}
}
