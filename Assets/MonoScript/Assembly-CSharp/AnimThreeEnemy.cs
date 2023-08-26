using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AnimThreeEnemy : BaseEnemy
{
	public enum EnemyState
	{
		Forward,
		Backward,
		Reforward,
		End
	}

	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public Vector3[] TriggerPoints;

		public float[] BeginDistances;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TriggerPoints = bytes.GetVector3Array(ref startIndex);
			int @int = bytes.GetInt32(ref startIndex);
			BeginDistances = new float[@int];
			for (int i = 0; i < @int; i++)
			{
				BeginDistances[i] = bytes.GetSingle(ref startIndex);
			}
		}

		public byte[] WriteBytes()
		{
			int num = BeginDistances.Length;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(TriggerPoints.GetBytes(), ref offset);
				memoryStream.WriteByteArray(num.GetBytes(), ref offset);
				for (int i = 0; i < num; i++)
				{
					memoryStream.WriteByteArray(BeginDistances[i].GetBytes(), ref offset);
				}
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private static readonly int AnimCount = 3;

	private static readonly string BeginPath = "begin/b_point";

	private static readonly string AnimName = "anim0";

	public EnemyData data;

	private EnemyState currentState;

	private Transform[] percentObjs = new Transform[AnimCount];

	private Animation anim;

	private Color[] percentColor = new Color[AnimCount];

	private string[] animNames = new string[AnimCount];

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
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		for (int i = 0; i < AnimCount; i++)
		{
			animNames[i] = AnimName + (i + 1);
		}
		anim = base.transform.GetComponentInChildren<Animation>();
		BindUtilInfo();
		currentState = EnemyState.Forward;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentState = EnemyState.Forward;
		PlayAnim(anim, animNames[0], false);
	}

	public override void UpdateElement()
	{
		if (currentState == EnemyState.Forward || currentState == EnemyState.Backward || currentState == EnemyState.Reforward)
		{
			float distance = GetDistance(base.groupTransform.InverseTransformPoint(BaseRole.BallPosition));
			if (GetPercent(distance) >= 1f)
			{
				PlayAnim(anim, animNames[currentIndex], true);
				currentState = (EnemyState)Mathf.Min(2, (int)(currentState + 1));
			}
		}
	}

	public override float GetPercent(float distance)
	{
		if (currentState == EnemyState.Forward || currentState == EnemyState.Reforward)
		{
			if (!Railway.theRailway.IfMoveForward)
			{
				return 0f;
			}
		}
		else if (currentState == EnemyState.Backward && Railway.theRailway.IfMoveForward)
		{
			return 0f;
		}
		return distance;
	}

	private float GetDistance(Vector3 roleLocalPos)
	{
		return roleLocalPos.z - base.groupTransform.InverseTransformPoint(data.TriggerPoints[currentIndex]).z + data.BeginDistances[currentIndex];
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
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
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
		for (int i = 0; i < AnimCount; i++)
		{
			if (percentObjs[i] == null)
			{
				string n = BeginPath + i;
				percentObjs[i] = base.transform.Find(n);
			}
		}
	}

	private void SetUtilInfoByData()
	{
		for (int i = 0; i < percentObjs.Length; i++)
		{
			percentObjs[i].transform.position = data.TriggerPoints[i];
		}
	}

	private void SetDataByUtilInfo()
	{
		for (int i = 0; i < percentObjs.Length; i++)
		{
			data.TriggerPoints[i] = percentObjs[i].transform.position;
		}
	}

	private void InitPathColor()
	{
		percentColor[0] = Color.red;
		percentColor[1] = Color.green;
		percentColor[2] = Color.blue;
	}
}
