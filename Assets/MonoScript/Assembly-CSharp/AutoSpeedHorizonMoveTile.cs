using System;
using Foundation;
using UnityEngine;

public class AutoSpeedHorizonMoveTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float CycleDistance;

		public Vector3 BeginPos;

		public Vector3 EndPos;

		[HideInInspector]
		public Vector3 TargetPos;

		[Range(0f, 1f)]
		public float targetPercent;

		public AnimationCurve SpeedCurve;

		public bool IfRoot;

		public void ReadBytes(byte[] bytes)
		{
			TileData tileData = (this = JsonUtility.FromJson<TileData>(bytes.GetString()));
		}

		public byte[] WriteBytes()
		{
			return JsonUtility.ToJson(this).GetBytes();
		}
	}

	public static readonly string NodeBeginPoint = "path/beginPoint";

	public static readonly string NodeEndPoint = "path/endPoint";

	public TileData data;

	private Transform beginNode;

	private Transform endNode;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null && objs.Length != 0)
		{
			data.CycleDistance = (float)objs[0];
			data.targetPercent = (float)objs[1];
			beginNode = base.transform.Find(NodeBeginPoint);
			endNode = base.transform.Find(NodeEndPoint);
			if (beginNode != null && endNode != null)
			{
				beginNode.localPosition = new Vector3(-2.5f, 0f, 0f);
				endNode.localPosition = new Vector3(2.5f, 0f, 0f);
			}
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		beginNode = base.transform.Find(NodeBeginPoint);
		endNode = base.transform.Find(NodeEndPoint);
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
	}

	public override void UpdateElement()
	{
		float num = (base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.transform.parent.InverseTransformPoint(data.TargetPos).z) / data.CycleDistance + data.targetPercent * 0.5f;
		int num2 = Mathf.FloorToInt(num);
		float num3 = num - (float)num2;
		float num4 = 0f;
		num4 = ((num3 < -0.5f) ? ((1f + num3) * 2f) : ((num3 <= 0f) ? ((0f - num3) * 2f) : ((!(num3 <= 0.5f)) ? ((1f - num3) * 2f) : (num3 * 2f))));
		float t = data.SpeedCurve.Evaluate(num4);
		Vector3 position = Vector3.Lerp(data.BeginPos, data.EndPos, t);
		base.transform.position = position;
	}

	public void OnDrawGizmos()
	{
		if (beginNode == null)
		{
			beginNode = base.transform.Find(NodeBeginPoint);
		}
		if (endNode == null)
		{
			endNode = base.transform.Find(NodeEndPoint);
		}
		if (!(beginNode == null) && !(endNode == null))
		{
			Gizmos.DrawCube(beginNode.position, new Vector3(0.3f, 0.3f, 0.3f));
			Gizmos.DrawCube(endNode.position, new Vector3(0.3f, 0.3f, 0.3f));
			BaseElement.DrawWorldPath(new Vector3[2] { beginNode.position, endNode.position }, Color.red);
			if (data.SpeedCurve != null)
			{
				float t = data.SpeedCurve.Evaluate(data.targetPercent);
				Gizmos.DrawSphere(Vector3.Lerp(beginNode.position, endNode.position, t), 0.3f);
			}
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
		if (beginNode == null)
		{
			beginNode = base.transform.Find(NodeBeginPoint);
		}
		if (endNode == null)
		{
			endNode = base.transform.Find(NodeEndPoint);
		}
		beginNode.position = data.BeginPos;
		endNode.position = data.EndPos;
	}

	public override string Write()
	{
		if (beginNode == null)
		{
			beginNode = base.transform.Find(NodeBeginPoint);
		}
		if (endNode == null)
		{
			endNode = base.transform.Find(NodeEndPoint);
		}
		data.BeginPos = beginNode.position;
		data.EndPos = endNode.position;
		float t = data.SpeedCurve.Evaluate(data.targetPercent);
		data.TargetPos = Vector3.Lerp(beginNode.position, endNode.position, t);
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		if (beginNode == null)
		{
			beginNode = base.transform.Find(NodeBeginPoint);
		}
		if (endNode == null)
		{
			endNode = base.transform.Find(NodeEndPoint);
		}
		beginNode.position = data.BeginPos;
		endNode.position = data.EndPos;
	}

	public override byte[] WriteBytes()
	{
		if (beginNode == null)
		{
			beginNode = base.transform.Find(NodeBeginPoint);
		}
		if (endNode == null)
		{
			endNode = base.transform.Find(NodeEndPoint);
		}
		data.BeginPos = beginNode.position;
		data.EndPos = endNode.position;
		float t = data.SpeedCurve.Evaluate(data.targetPercent);
		data.TargetPos = Vector3.Lerp(beginNode.position, endNode.position, t);
		return StructTranslatorUtility.ToByteArray(data);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_AutoSpeedHorizonMoveTile_DATA rD_AutoSpeedHorizonMoveTile_DATA = JsonUtility.FromJson<RD_AutoSpeedHorizonMoveTile_DATA>(rd_data as string);
		base.transform.position = rD_AutoSpeedHorizonMoveTile_DATA.CurrentPos;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AutoSpeedHorizonMoveTile_DATA
		{
			CurrentPos = base.transform.position
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_AutoSpeedHorizonMoveTile_DATA rD_AutoSpeedHorizonMoveTile_DATA = Bson.ToObject<RD_AutoSpeedHorizonMoveTile_DATA>(rd_data);
		base.transform.position = rD_AutoSpeedHorizonMoveTile_DATA.CurrentPos;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AutoSpeedHorizonMoveTile_DATA
		{
			CurrentPos = base.transform.position
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
