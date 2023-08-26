using System;
using System.IO;
using Foundation;
using UnityEngine;

public class PathToMoveEffect : BaseEnemy
{
	[Serializable]
	public struct PathToMoveData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public bool IfParticleAutoStop;

		public bool IfAutoSpeed;

		public float MoveSpeed;

		public Vector3[] PathPoints;

		public int m_smooth;

		public bool m_isLookAt;

		public Vector3[] BezierPoints;

		public bool IfRotate;

		public float RotateDistance;

		public Vector3 BeginLocalEular;

		public Vector3 EndLocalEular;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			IfParticleAutoStop = bytes.GetBoolean(ref startIndex);
			IfAutoSpeed = bytes.GetBoolean(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			PathPoints = bytes.GetVector3Array(ref startIndex);
			m_smooth = bytes.GetInt32(ref startIndex);
			m_isLookAt = bytes.GetBoolean(ref startIndex);
			BezierPoints = bytes.GetVector3Array(ref startIndex);
			IfRotate = bytes.GetBoolean(ref startIndex);
			RotateDistance = bytes.GetSingle(ref startIndex);
			BeginLocalEular = bytes.GetVector3(ref startIndex);
			EndLocalEular = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfParticleAutoStop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfAutoSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_smooth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isLookAt.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BezierPoints.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfRotate.GetBytes(), ref offset);
				memoryStream.WriteByteArray(RotateDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginLocalEular.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EndLocalEular.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly float DefaultTrailTime = 1.1f;

	private static readonly string DebugRootName = "debugRoot";

	private static readonly string DebugChildName = "debugChild";

	public PathToMoveData data;

	[Range(0f, 1f)]
	public float DebugPercent;

	private float cachedDebugPercent = -1f;

	private Transform model;

	private Vector3 StartModelEular;

	private ParticleSystem particleState1;

	private TrailRenderer trailRender;

	private float moveSpeed;

	private BezierMover bezierMover;

	private RD_PathToMoveEffect_DATA m_rebirthData;

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
		commonState = CommonState.None;
		model = base.transform.Find("model");
		if ((bool)model)
		{
			StartModelEular = model.localEulerAngles;
		}
		particleState1 = base.gameObject.transform.Find("effect/state1").GetComponent<ParticleSystem>();
		moveSpeed = (data.IfAutoSpeed ? data.MoveSpeed : Railway.theRailway.SpeedForward);
		if ((bool)particleState1)
		{
			trailRender = particleState1.transform.GetComponentInChildren<TrailRenderer>();
			particleState1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		Vector3[] array = new Vector3[data.PathPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = base.transform.TransformPoint(data.PathPoints[i]);
		}
		bezierMover = new BezierMover(data.BezierPoints);
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				if ((bool)trailRender)
				{
					trailRender.time = DefaultTrailTime;
				}
				PlayParticle(particles, true);
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active)
		{
			OnTriggerPlay();
			if (data.IfRotate && model != null)
			{
				float percent = (num - data.BeginDistance) / data.RotateDistance;
				RotateByPercent(model, percent);
			}
			if (num >= data.ResetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}
	}

	private void RotateByPercent(Transform rotTrans, float percent)
	{
		rotTrans.localEulerAngles = Vector3.Lerp(data.BeginLocalEular, data.EndLocalEular, percent);
	}

	public override void OnTriggerPlay()
	{
		float distance = moveSpeed * Time.deltaTime;
		Vector3 position = base.transform.position;
		Vector3 targetPos = Vector3.zero;
		Vector3 moveDir = Vector3.forward;
		bool num = bezierMover.MoveForwardByDis(distance, position, ref targetPos, ref moveDir);
		if (data.m_isLookAt)
		{
			base.transform.forward = moveDir;
		}
		base.transform.position = targetPos;
		if (num && data.IfParticleAutoStop)
		{
			particleState1.Stop();
			commonState = CommonState.End;
		}
	}

	public override void OnTriggerStop()
	{
		if (particleState1.isPlaying)
		{
			particleState1.Stop();
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		bezierMover.ResetData();
		commonState = CommonState.None;
		if ((bool)particleState1)
		{
			particleState1.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		if ((bool)trailRender)
		{
			trailRender.time = 0f;
		}
		if ((bool)model)
		{
			model.localEulerAngles = StartModelEular;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		data = (PathToMoveData)objs[0];
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<PathToMoveData>(info);
	}

	public override string Write()
	{
		Vector3[] array = new Vector3[data.PathPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = base.transform.TransformPoint(data.PathPoints[i]);
		}
		data.BezierPoints = Bezier.GetPathByPositions(array, data.m_smooth);
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<PathToMoveData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		Vector3[] array = new Vector3[data.PathPoints.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = base.transform.TransformPoint(data.PathPoints[i]);
		}
		data.BezierPoints = Bezier.GetPathByPositions(array, data.m_smooth);
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (data.PathPoints != null && data.PathPoints.Length != 0)
		{
			Vector3[] array = new Vector3[data.PathPoints.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.transform.TransformPoint(data.PathPoints[i]);
			}
			Vector3[] pathByPositions = Bezier.GetPathByPositions(array, data.m_smooth);
			Gizmos.color = Color.yellow;
			if (pathByPositions != null && pathByPositions.Length > 1)
			{
				Gizmos.color = Color.yellow;
				for (int j = 0; j < pathByPositions.Length - 1; j++)
				{
					Gizmos.DrawLine(pathByPositions[j], pathByPositions[j + 1]);
				}
				DebugDrawByPercent(pathByPositions, DebugPercent);
			}
		}
		Vector3 position = base.gameObject.transform.position;
		Vector3 vector = base.gameObject.transform.position + new Vector3(0f, 0f, data.BeginDistance);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(vector, position);
		Gizmos.DrawCube(vector, new Vector3(1f, 0.2f, 0.2f));
		Gizmos.color = Color.white;
	}

	private void DebugDrawByPercent(Vector3[] points, float percent)
	{
		Transform transform = base.transform.Find(DebugRootName);
		if (transform == null)
		{
			return;
		}
		Transform transform2 = transform.Find(DebugChildName);
		if (transform2 == null)
		{
			return;
		}
		Vector3 position = Vector3.zero;
		Vector3 zero = Vector3.zero;
		Vector3 forward = Vector3.forward;
		if (cachedDebugPercent != percent)
		{
			cachedDebugPercent = percent;
			float z = points[0].z;
			float z2 = points[points.Length - 1].z;
			float num = Mathf.Lerp(z, z2, percent);
			for (int i = 0; i < points.Length - 1; i++)
			{
				if (num >= points[i].z && num <= points[i + 1].z)
				{
					position = Vector3.Lerp(points[i], points[i + 1], (num - points[i].z) / (points[i + 1].z - points[i].z));
					forward = points[i + 1] - points[i];
					break;
				}
			}
			zero = Vector3.Lerp(data.BeginLocalEular, data.EndLocalEular, percent);
			transform.transform.position = position;
			transform.forward = forward;
			transform2.transform.localEulerAngles = zero;
		}
		Color color = Gizmos.color;
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.color = Color.yellow;
		Gizmos.matrix = transform2.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		Gizmos.matrix = matrix;
		Gizmos.color = color;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_PathToMoveEffect_DATA>(rd_data as string);
		base.RebirthReadData((object)m_rebirthData.baseData);
		particleState1.SetParticleData(m_rebirthData.particleData, ProcessState.Pause);
		trailRender.SetTrailData(m_rebirthData.trailData, ProcessState.Pause);
		bezierMover.SetBezierData(m_rebirthData.bezierData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_PathToMoveEffect_DATA
		{
			baseData = (base.RebirthWriteData() as string),
			particleData = particleState1.GetParticleData(),
			trailData = trailRender.GetTrailData(),
			bezierData = bezierMover.GetBezierData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		particleState1.SetParticleData(m_rebirthData.particleData, ProcessState.UnPause);
		trailRender.SetTrailData(m_rebirthData.trailData, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_PathToMoveEffect_DATA>(rd_data);
		base.RebirthReadByteData(m_rebirthData.baseBytesData);
		particleState1.SetParticleData(m_rebirthData.particleData, ProcessState.Pause);
		trailRender.SetTrailData(m_rebirthData.trailData, ProcessState.Pause);
		bezierMover.SetBezierData(m_rebirthData.bezierData);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_PathToMoveEffect_DATA
		{
			baseBytesData = base.RebirthWriteByteData(),
			particleData = particleState1.GetParticleData(),
			trailData = trailRender.GetTrailData(),
			bezierData = bezierMover.GetBezierData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		particleState1.SetParticleData(m_rebirthData.particleData, ProcessState.UnPause);
		trailRender.SetTrailData(m_rebirthData.trailData, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
