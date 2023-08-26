using System;
using System.IO;
using Foundation;
using UnityEngine;

public sealed class NormalShipVehicle : BaseVehicle
{
	public enum Status
	{
		eWait,
		eMove,
		eDepart
	}

	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float MoveSpeed;

		public float CameraTargetScaler;

		public float BallSlideSpeed;

		public float InputNormalizeSpeed;

		public float InputSensitivity;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveSpeed = bytes.GetSingle(ref startIndex);
			CameraTargetScaler = bytes.GetSingle(ref startIndex);
			BallSlideSpeed = bytes.GetSingle(ref startIndex);
			InputNormalizeSpeed = bytes.GetSingle(ref startIndex);
			InputSensitivity = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(CameraTargetScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BallSlideSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(InputNormalizeSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(InputSensitivity.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	private Status currentStatus;

	private bool ifIgnoreRoleCollide;

	public override float VehicleRotateSpeed
	{
		get
		{
			return 3f;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.MoveSpeed = (float)objs[0];
			data.CameraTargetScaler = (float)objs[1];
			data.BallSlideSpeed = (float)objs[2];
			data.InputNormalizeSpeed = (float)objs[3];
			data.InputSensitivity = (float)objs[4];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		currentStatus = Status.eWait;
		ifIgnoreRoleCollide = false;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentStatus = Status.eWait;
		ifIgnoreRoleCollide = false;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!ifIgnoreRoleCollide && currentStatus == Status.eWait)
		{
			base.TriggerEnter(ball);
			ResetInputParam();
			AddToBall(ball);
			ball.BeginCloseTrail();
			ball.ChangeToMove();
			ball.TriggerRolePlayAnim(BaseRole.AnimType.JumpOnBoatStandState);
			currentStatus = Status.eMove;
			PlayParticle();
		}
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

	private void ResetInputParam()
	{
		BaseRole.theBall.ResetInputParam(data.BallSlideSpeed);
		CameraController.theCamera.ResetInputParam(data.CameraTargetScaler);
		InputController.instance.ResetInputParam(data.InputNormalizeSpeed, data.InputSensitivity);
	}

	public override void DepartFromBall(BaseRole ball, bool ifDestroy)
	{
		if (currentStatus == Status.eMove)
		{
			base.DepartFromBall(ball, ifDestroy);
			currentStatus = Status.eDepart;
		}
	}

	public override void GiveRebirthTo(BaseRole ball)
	{
		ifIgnoreRoleCollide = true;
		base.TriggerEnter(ball);
		ResetInputParam();
		AddToBall(ball);
		currentStatus = Status.eMove;
	}

	public override void ForceSetTrans()
	{
		base.transform.eulerAngles = Vector3.zero;
		Transform parent = base.transform.parent;
		base.transform.parent = BaseRole.theBall.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.parent = parent;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return "";
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}

	public override void RebirthResetByRole(BaseRole role)
	{
		ifIgnoreRoleCollide = true;
		base.TriggerEnter(role);
		ResetInputParam();
		AddToBall(role);
		currentStatus = Status.eMove;
	}
}
