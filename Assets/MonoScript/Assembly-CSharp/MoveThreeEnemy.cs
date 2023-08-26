using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MoveThreeEnemy : BaseEnemy
{
	public enum EnemyState
	{
		Forward,
		Backward,
		Reforward
	}

	[Serializable]
	public class EnemyData : IReadWriteBytes
	{
		public Vector3[] PathPoints = new Vector3[PointCount + 1];

		public Vector3[] PercentPoints = new Vector3[PointCount];

		public float[] BeginDistances = new float[PointCount];

		public float[] SpeedScalers = new float[PointCount];

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			PathPoints = bytes.GetVector3Array(ref startIndex);
			PercentPoints = bytes.GetVector3Array(ref startIndex);
			int @int = bytes.GetInt32(ref startIndex);
			BeginDistances = new float[@int];
			for (int i = 0; i < @int; i++)
			{
				BeginDistances[i] = bytes.GetSingle(ref startIndex);
			}
			int int2 = bytes.GetInt32(ref startIndex);
			SpeedScalers = new float[int2];
			for (int j = 0; j < int2; j++)
			{
				SpeedScalers[j] = bytes.GetSingle(ref startIndex);
			}
		}

		public byte[] WriteBytes()
		{
			int num = BeginDistances.Length;
			int num2 = SpeedScalers.Length;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
				memoryStream.WriteByteArray(PercentPoints.GetBytes(), ref offset);
				memoryStream.WriteByteArray(num.GetBytes(), ref offset);
				for (int i = 0; i < num; i++)
				{
					memoryStream.WriteByteArray(BeginDistances[i].GetBytes(), ref offset);
				}
				memoryStream.WriteByteArray(num2.GetBytes(), ref offset);
				for (int j = 0; j < num2; j++)
				{
					memoryStream.WriteByteArray(SpeedScalers[j].GetBytes(), ref offset);
				}
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static bool[] IfShowPoint = new bool[5];

	public static bool[] IfShowPath = new bool[4];

	private static readonly int PointCount = 3;

	public static readonly string PointPath = "path/p_point";

	public static readonly string BeginPath = "begin/b_point";

	public EnemyData data;

	private EnemyState currentState;

	private Transform[] pathObjs = new Transform[PointCount + 1];

	private Transform[] percentObjs = new Transform[PointCount];

	private Color[] pathColors = new Color[PointCount + 1];

	private int currentIndex
	{
		get
		{
			return (int)currentState;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null && data != null)
		{
			data.SpeedScalers[0] = (float)objs[0];
			data.SpeedScalers[1] = (float)objs[1];
			data.SpeedScalers[2] = (float)objs[2];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		BindUtilInfo();
		currentState = EnemyState.Forward;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentState = EnemyState.Forward;
	}

	public override void UpdateElement()
	{
		float distance = GetDistance(base.groupTransform.InverseTransformPoint(BaseRole.BallPosition));
		float percent = GetPercent(distance);
		PlayByPercent(percent);
		if (percent >= 1f)
		{
			currentState = (EnemyState)Mathf.Min(2, (int)(currentState + 1));
		}
	}

	public override void PlayByPercent(float percent)
	{
		if (percent < 1f)
		{
			base.transform.position = GetPositionByPercent(data.PathPoints[currentIndex], data.PathPoints[currentIndex + 1], percent);
		}
		else
		{
			base.transform.position = data.PathPoints[currentIndex + 1];
		}
	}

	public override float GetPercent(float distance)
	{
		float num = 1f;
		if (currentState == EnemyState.Forward || currentState == EnemyState.Reforward)
		{
			num = 1f;
			if (!Railway.theRailway.IfMoveForward)
			{
				return 0f;
			}
		}
		else if (currentState == EnemyState.Backward)
		{
			num = -1f;
			if (Railway.theRailway.IfMoveForward)
			{
				return 0f;
			}
		}
		return distance * data.SpeedScalers[currentIndex] * num;
	}

	private float GetDistance(Vector3 roleLocalPos)
	{
		return roleLocalPos.z - base.groupTransform.InverseTransformPoint(data.PercentPoints[currentIndex]).z + data.BeginDistances[currentIndex];
	}

	private Vector3 GetPositionByPercent(Vector3 beginPos, Vector3 targetPos, float percent, bool ifLimit = true)
	{
		float t = percent;
		if (ifLimit)
		{
			t = Mathf.Clamp(percent, 0f, 1f);
		}
		return Vector3.Lerp(beginPos, targetPos, t);
	}

	public override void Read(string info)
	{
		BindUtilInfo();
		data = JsonUtility.FromJson<EnemyData>(info);
		SetUtilInfoByData();
	}

	public override string Write()
	{
		BindUtilInfo();
		SetDataByUtilInfo();
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		BindUtilInfo();
		data = StructTranslatorUtility.ToClass<EnemyData>(bytes);
		SetUtilInfoByData();
	}

	public override byte[] WriteBytes()
	{
		BindUtilInfo();
		SetDataByUtilInfo();
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
	}

	private void BindUtilInfo()
	{
		for (int i = 0; i < PointCount + 1; i++)
		{
			if (pathObjs[i] == null)
			{
				string n = PointPath + i;
				pathObjs[i] = base.transform.Find(n);
			}
		}
		for (int j = 0; j < PointCount; j++)
		{
			if (percentObjs[j] == null)
			{
				string n2 = BeginPath + j;
				percentObjs[j] = base.transform.Find(n2);
			}
		}
	}

	private void SetUtilInfoByData()
	{
		for (int i = 0; i < pathObjs.Length; i++)
		{
			pathObjs[i].transform.position = data.PathPoints[i];
		}
		for (int j = 0; j < percentObjs.Length; j++)
		{
			percentObjs[j].transform.position = data.PercentPoints[j];
		}
	}

	private void SetDataByUtilInfo()
	{
		for (int i = 0; i < pathObjs.Length; i++)
		{
			data.PathPoints[i] = pathObjs[i].transform.position;
		}
		for (int j = 0; j < percentObjs.Length; j++)
		{
			data.PercentPoints[j] = percentObjs[j].transform.position;
		}
	}

	private void InitPathColor()
	{
		pathColors[0] = Color.red;
		pathColors[1] = Color.green;
		pathColors[2] = Color.blue;
		pathColors[3] = Color.blue;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_MoveThreeEnemy_DATA rD_MoveThreeEnemy_DATA = JsonUtility.FromJson<RD_MoveThreeEnemy_DATA>(rd_data as string);
		currentState = rD_MoveThreeEnemy_DATA.currentState;
		base.transform.SetTransData(rD_MoveThreeEnemy_DATA.transform);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_MoveThreeEnemy_DATA
		{
			currentState = currentState,
			transform = base.transform.GetTransData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_MoveThreeEnemy_DATA rD_MoveThreeEnemy_DATA = Bson.ToObject<RD_MoveThreeEnemy_DATA>(rd_data);
		currentState = rD_MoveThreeEnemy_DATA.currentState;
		base.transform.SetTransData(rD_MoveThreeEnemy_DATA.transform);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_MoveThreeEnemy_DATA
		{
			currentState = currentState,
			transform = base.transform.GetTransData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
