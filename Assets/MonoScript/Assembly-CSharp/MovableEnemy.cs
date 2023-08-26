using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MovableEnemy : BaseEnemy
{
	[Serializable]
	public struct TriggerPointData
	{
		public float beginDistance;

		[HideInInspector]
		public Vector3 beginPos;

		public void ReadBytes(byte[] bytes, ref int startIndex)
		{
			beginDistance = bytes.GetSingle(ref startIndex);
			beginPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(beginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(beginPos.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	[Serializable]
	public struct Data : IReadWriteBytes
	{
		public TriggerPointData activeTriggerData;

		public float moveSpeed;

		public Vector3 destinationPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			activeTriggerData = default(TriggerPointData);
			activeTriggerData.ReadBytes(bytes, ref startIndex);
			moveSpeed = bytes.GetSingle(ref startIndex);
			destinationPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(activeTriggerData.WriteBytes(), ref offset);
				memoryStream.WriteByteArray(moveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(destinationPos.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public Data data;

	public DebugAnimation debugAnimation;

	private Vector3 originalPos;

	private Transform destination;

	private Transform model;

	private Transform cacheParent;

	private bool activeTriggered;

	private bool moveFinished;

	private float movePercent;

	private Transform triggerPoint;

	private Animation anim;

	public DebugAnimation DebugAnimation
	{
		get
		{
			if (debugAnimation == null)
			{
				debugAnimation = new DebugAnimation();
			}
			return debugAnimation;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		model = base.transform.Find("model");
		originalPos = model.position;
		activeTriggered = false;
		moveFinished = false;
		movePercent = 0f;
		anim = GetComponentInChildren<Animation>();
	}

	public override void ResetElement()
	{
		base.ResetElement();
		model.position = originalPos;
		activeTriggered = false;
		moveFinished = false;
		movePercent = 0f;
	}

	private Transform GetParent()
	{
		if (cacheParent == null)
		{
			cacheParent = base.transform.parent;
		}
		return cacheParent;
	}

	private Transform GetActiveTriggerPoint()
	{
		if (triggerPoint == null)
		{
			triggerPoint = base.transform.Find("activeTriggerPoint");
		}
		return triggerPoint;
	}

	private Transform GetDestination()
	{
		if (destination == null)
		{
			destination = base.transform.Find("destination");
		}
		return destination;
	}

	public override void UpdateElement()
	{
		if (!activeTriggered && GetParent().InverseTransformPoint(BaseRole.BallPosition).z - GetParent().InverseTransformPoint(base.transform.position).z >= data.activeTriggerData.beginDistance)
		{
			activeTriggered = true;
		}
		if (!activeTriggered)
		{
			return;
		}
		if ((bool)anim)
		{
			anim.Play();
		}
		if (!moveFinished)
		{
			movePercent += Railway.theRailway.SpeedForward * Time.deltaTime * data.moveSpeed;
			model.position = Vector3.Lerp(originalPos, GetDestination().position, movePercent);
			if ((double)Vector3.Distance(model.position, GetDestination().position) < 0.1)
			{
				moveFinished = true;
			}
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		data = (Data)objs[0];
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<Data>(info);
		GetActiveTriggerPoint().position = data.activeTriggerData.beginPos;
		GetDestination().position = data.destinationPos;
	}

	public override string Write()
	{
		data.activeTriggerData.beginPos = GetActiveTriggerPoint().position;
		data.activeTriggerData.beginDistance = base.transform.parent.InverseTransformPoint(GetActiveTriggerPoint().position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		data.destinationPos = GetDestination().position;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<Data>(bytes);
		GetActiveTriggerPoint().position = data.activeTriggerData.beginPos;
		GetDestination().position = data.destinationPos;
	}

	public override byte[] WriteBytes()
	{
		data.activeTriggerData.beginPos = GetActiveTriggerPoint().position;
		data.activeTriggerData.beginDistance = base.transform.parent.InverseTransformPoint(GetActiveTriggerPoint().position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		data.destinationPos = GetDestination().position;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(base.transform.position, GetActiveTriggerPoint().position);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(GetActiveTriggerPoint().position, new Vector3(1f, 0.1f, 0.1f));
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(GetDestination().position, new Vector3(1f, 0.1f, 0.1f));
		DebugAnimation.OnDrawGizmos(base.transform);
	}
}
