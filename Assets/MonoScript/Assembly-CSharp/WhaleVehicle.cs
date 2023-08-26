using System;
using System.IO;
using Foundation;
using UnityEngine;

public class WhaleVehicle : BaseVehicle
{
	public enum Status
	{
		eWait,
		eMove,
		eFollow,
		eEndMove,
		eDepart,
		eStop
	}

	[Serializable]
	public struct VehicleData : IReadWriteBytes
	{
		public float BeginDistance;

		public int LerpNum;

		public bool ifAutoSpeed;

		public float MoveSpeed;

		public Vector3[] PathPoints;

		public float CameraTargetScaler;

		public float BallSlideSpeed;

		public float InputNormalizeSpeed;

		public float InputSensitivity;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			LerpNum = bytes.GetInt32(ref startIndex);
			ifAutoSpeed = bytes.GetBoolean(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			PathPoints = bytes.GetVector3Array(ref startIndex);
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
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(LerpNum.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ifAutoSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
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

	public VehicleData data;

	private float moveSpeed;

	private Transform whaleObject;

	private Status currentStatus;

	private BezierMover bezierMover;

	private Vector3[] whaleBezierPoints;

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

	public override void Initialize()
	{
		base.Initialize();
		moveSpeed = (data.ifAutoSpeed ? data.MoveSpeed : Railway.theRailway.SpeedForward);
		if (whaleObject == null)
		{
			whaleObject = base.transform.Find("model/whaleObject");
		}
		Vector3[] array = new Vector3[data.PathPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = base.transform.TransformPoint(data.PathPoints[i]);
		}
		whaleBezierPoints = Bezier.GetPathByPositions(array, data.LerpNum);
		bezierMover = new BezierMover(whaleBezierPoints);
		currentStatus = Status.eWait;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentStatus = Status.eWait;
		bezierMover.ResetData();
	}

	public override void UpdateElement()
	{
		if (currentStatus == Status.eWait)
		{
			if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance >= 0f)
			{
				currentStatus = Status.eMove;
			}
		}
		else if (currentStatus == Status.eMove || currentStatus == Status.eEndMove)
		{
			float disLocalZ = moveSpeed * Time.deltaTime;
			Vector3 localPosition = base.transform.localPosition;
			Vector3 targetLocPos = Vector3.zero;
			Vector3 moveLocDir = Vector3.forward;
			bool num = bezierMover.MoveForwardByZ(disLocalZ, base.groupTransform, localPosition, ref targetLocPos, ref moveLocDir);
			base.transform.localPosition = targetLocPos;
			if (num)
			{
				StopParticle();
				currentStatus = Status.eStop;
			}
			MoveHorizon();
		}
		else
		{
			Status currentStatus2 = currentStatus;
			int num2 = 2;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (currentStatus == Status.eMove)
		{
			base.TriggerEnter(ball);
			ResetInputParam();
			AddToBall(ball);
			currentStatus = Status.eFollow;
			PlayParticle();
		}
	}

	public override void DepartFromBall(BaseRole ball, bool ifDestroy)
	{
		if (currentStatus == Status.eFollow)
		{
			base.DepartFromBall(ball, ifDestroy);
			base.transform.localEulerAngles = base.transform.localEulerAngles.GetMulti(new Vector3(1f, 0f, 1f));
			currentStatus = Status.eEndMove;
		}
	}

	protected override void ForceResetRoleX(Transform roleModelPart)
	{
		roleModelPart.localEulerAngles = roleModelPart.localEulerAngles.GetMulti(new Vector3(1f, 0f, 1f));
	}

	protected override void ForceSetForward(Vector3 forwardDir)
	{
		base.transform.forward = Vector3.Lerp(base.transform.forward, forwardDir, 0.75f);
	}

	protected override void ForceSetTuringX(float degree, float limitX)
	{
		degree = Mathf.Max(0f - limitX, Mathf.Min(limitX, degree));
		Quaternion b = Quaternion.LookRotation(Quaternion.Euler(new Vector3(0f, degree, 0f)) * new Vector3(0f, 0f, 1f) + base.transform.position - base.transform.position);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, 0.2f);
	}

	private void MoveHorizon()
	{
		Vector3 position = base.groupTransform.InverseTransformPoint(base.transform.position);
		position.x = base.groupTransform.InverseTransformPoint(BaseRole.theBall.transform.position).x;
		base.transform.position = base.groupTransform.TransformPoint(position);
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<VehicleData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<VehicleData>(bytes);
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

	private void PlayAnim()
	{
		if ((bool)defaultAnim)
		{
			defaultAnim.wrapMode = WrapMode.Loop;
			defaultAnim["anim01"].normalizedTime = 0f;
			defaultAnim.Play();
		}
	}

	private void StopAnim()
	{
		if ((bool)defaultAnim)
		{
			defaultAnim.Play();
			defaultAnim[BaseVehicle.AnimDieStr].normalizedTime = 0f;
			defaultAnim.Sample();
			defaultAnim.Stop();
		}
	}

	public override void ForceSetTrans()
	{
		base.transform.eulerAngles = Vector3.zero;
		Transform parent = base.transform.parent;
		base.transform.parent = BaseRole.theBall.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.parent = parent;
	}

	public override void GiveRebirthTo(BaseRole ball)
	{
		base.TriggerEnter(ball);
		ResetInputParam();
		AddToBall(ball);
		currentStatus = Status.eMove;
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (data.PathPoints == null || data.PathPoints.Length == 0)
		{
			return;
		}
		Vector3[] array = new Vector3[data.PathPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = base.transform.TransformPoint(data.PathPoints[i]);
		}
		Vector3[] pathByPositions = Bezier.GetPathByPositions(array, data.LerpNum);
		if (pathByPositions != null && pathByPositions.Length > 1)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			for (int j = 0; j < pathByPositions.Length - 1; j++)
			{
				Gizmos.DrawLine(pathByPositions[j], pathByPositions[j + 1]);
			}
			Gizmos.color = color;
		}
	}
}
