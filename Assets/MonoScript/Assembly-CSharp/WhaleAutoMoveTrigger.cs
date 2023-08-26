using System;
using System.IO;
using Foundation;
using UnityEngine;

public class WhaleAutoMoveTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public int LerpNum;

		public bool ifAutoSpeed;

		public float MoveSpeed;

		public Vector3[] PathPoints;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			LerpNum = bytes.GetInt32(ref startIndex);
			ifAutoSpeed = bytes.GetBoolean(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			PathPoints = bytes.GetVector3Array(ref startIndex);
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
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Vector3 whaleBeginLocalPos;

	private float moveSpeed;

	private int currentIndex;

	private Transform whaleObject;

	private Vector3[] whaleBezierPoints;

	private Animation anim;

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
		currentIndex = 0;
		moveSpeed = (data.ifAutoSpeed ? data.MoveSpeed : Railway.theRailway.SpeedForward);
		if (whaleObject == null)
		{
			whaleObject = base.transform.Find("model/whaleObject");
			whaleBeginLocalPos = whaleObject.localPosition;
		}
		if (anim == null)
		{
			anim = GetComponentInChildren<Animation>();
		}
		whaleBezierPoints = Bezier.GetPathByPositions(data.PathPoints, data.LerpNum);
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		StopAnim();
		currentIndex = 0;
		whaleObject.position = whaleBeginLocalPos;
		commonState = CommonState.None;
	}

	public override void UpdateElement()
	{
		if (commonState == CommonState.None)
		{
			if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance >= 0f)
			{
				commonState = CommonState.Active;
				PlayAnim();
			}
		}
		else if (commonState == CommonState.Active)
		{
			if (MoveForward())
			{
				StopAnim();
				commonState = CommonState.End;
			}
		}
		else if (commonState != CommonState.InActive)
		{
			CommonState commonState2 = commonState;
			int num = 5;
		}
	}

	private bool MoveForward()
	{
		Vector3 vector = base.groupTransform.InverseTransformPoint(whaleObject.position);
		float num = vector.z + moveSpeed * Time.deltaTime;
		Vector3 nextP = Vector3.zero;
		if (GetNextPoint(whaleBezierPoints, num, ref nextP))
		{
			float num2 = nextP.z - vector.z;
			float num3 = 1f;
			if (num2 > 0f)
			{
				num3 = (num - vector.z) / num2;
				vector = Vector3.Lerp(vector, nextP, num3);
			}
			whaleObject.position = base.groupTransform.TransformPoint(vector);
			return false;
		}
		return true;
	}

	private bool GetNextPoint(Vector3[] bezierPoints, float targetZ, ref Vector3 nextP)
	{
		Vector3 vector = Vector3.zero;
		for (int i = currentIndex; i < bezierPoints.Length; i++)
		{
			vector = base.groupTransform.InverseTransformPoint(base.transform.TransformPoint(bezierPoints[i]));
			if (targetZ <= vector.z)
			{
				currentIndex = i;
				nextP = vector;
				return true;
			}
		}
		currentIndex = bezierPoints.Length - 1;
		nextP = vector;
		return false;
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	private void PlayAnim()
	{
		if ((bool)anim)
		{
			anim.wrapMode = WrapMode.Loop;
			anim["anim01"].normalizedTime = 0f;
			anim.Play();
		}
	}

	private void StopAnim()
	{
		if ((bool)anim)
		{
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_WhaleAutoMoveTrigger_DATA());
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
		return Bson.ToBson(new RD_WhaleAutoMoveTrigger_DATA());
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
