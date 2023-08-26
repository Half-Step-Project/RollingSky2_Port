using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

public class AutoMoveLight : BaseTriggerBox
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public int LerpNum;

		public bool ifAutoSpeed;

		public float MoveSpeed;

		public Vector3[] PathPoints;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
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

	private float moveSpeed;

	private Vector3[] bezierWorldPoints;

	private Vector3[] bezierGridPoints;

	private int currentIndex;

	private Transform lightObject;

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
		commonState = CommonState.None;
		if (lightObject == null)
		{
			lightObject = base.transform.Find("light");
		}
		lightObject.gameObject.SetActive(false);
		lightObject.position = data.PathPoints[0];
		if (data.ifAutoSpeed)
		{
			moveSpeed = Railway.theRailway.SpeedForward;
		}
		else
		{
			moveSpeed = data.MoveSpeed;
		}
		bezierGridPoints = new Vector3[bezierWorldPoints.Length];
		for (int i = 0; i < bezierWorldPoints.Length; i++)
		{
			bezierGridPoints[i] = base.groupTransform.InverseTransformPoint(bezierWorldPoints[i]);
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentIndex = 0;
	}

	public override void UpdateElement()
	{
		if (commonState == CommonState.None)
		{
			return;
		}
		if (commonState == CommonState.Active)
		{
			if (MoveForward())
			{
				lightObject.gameObject.SetActive(false);
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
			lightObject.gameObject.SetActive(true);
			commonState = CommonState.Active;
		}
	}

	private bool MoveForward()
	{
		Vector3 vector = base.groupTransform.InverseTransformPoint(lightObject.position);
		float num = vector.z + moveSpeed * Time.deltaTime;
		Vector3 nextP = Vector3.zero;
		if (GetNextPoint(num, ref nextP))
		{
			float num2 = nextP.z - vector.z;
			float num3 = 1f;
			if (num2 > 0f)
			{
				num3 = (num - vector.z) / num2;
				vector = Vector3.Lerp(vector, nextP, num3);
			}
			lightObject.position = base.groupTransform.TransformPoint(vector);
			return false;
		}
		return true;
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
		Transform transform = base.transform.Find("path");
		if ((bool)transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform transform2 = transform.Find("pathItem" + i);
				if ((bool)transform2)
				{
					transform2.transform.position = data.PathPoints[i];
				}
				else
				{
					Debug.LogError("Not Contains pathPoint" + i);
				}
			}
		}
		else
		{
			Debug.LogError("Can't find the pathRoot named \"path\"");
		}
		bezierWorldPoints = Bezier.GetPathByPositions(data.PathPoints, data.LerpNum);
	}

	public override string Write()
	{
		List<Vector3> list = new List<Vector3>();
		Transform transform = base.transform.Find("path");
		if ((bool)transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform transform2 = transform.Find("pathItem" + i);
				if ((bool)transform2)
				{
					list.Add(transform2.position);
				}
				else
				{
					Debug.LogError("Not Contains pathPoint" + i);
				}
			}
			data.PathPoints = list.ToArray();
		}
		else
		{
			Debug.LogError("Can't find the pathRoot named \"path\"");
		}
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		Transform transform = base.transform.Find("path");
		if ((bool)transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform transform2 = transform.Find("pathItem" + i);
				if ((bool)transform2)
				{
					transform2.transform.position = data.PathPoints[i];
				}
				else
				{
					Debug.LogError("Not Contains pathPoint" + i);
				}
			}
		}
		else
		{
			Debug.LogError("Can't find the pathRoot named \"path\"");
		}
		bezierWorldPoints = Bezier.GetPathByPositions(data.PathPoints, data.LerpNum);
	}

	public override byte[] WriteBytes()
	{
		List<Vector3> list = new List<Vector3>();
		Transform transform = base.transform.Find("path");
		if ((bool)transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform transform2 = transform.Find("pathItem" + i);
				if ((bool)transform2)
				{
					list.Add(transform2.position);
				}
				else
				{
					Debug.LogError("Not Contains pathPoint" + i);
				}
			}
			data.PathPoints = list.ToArray();
		}
		else
		{
			Debug.LogError("Can't find the pathRoot named \"path\"");
		}
		return StructTranslatorUtility.ToByteArray(data);
	}

	private bool GetNextPoint(float targetZ, ref Vector3 nextP)
	{
		for (int i = currentIndex; i < bezierGridPoints.Length; i++)
		{
			if (targetZ <= bezierGridPoints[i].z)
			{
				currentIndex = i;
				nextP = bezierGridPoints[currentIndex];
				return true;
			}
		}
		currentIndex = bezierGridPoints.Length - 1;
		nextP = bezierGridPoints[currentIndex];
		return false;
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		List<Vector3> list = new List<Vector3>();
		Transform transform = base.transform.Find("path");
		if ((bool)transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform transform2 = transform.Find("pathItem" + i);
				if ((bool)transform2)
				{
					list.Add(transform2.position);
					Color color = Gizmos.color;
					Gizmos.color = Color.green;
					Gizmos.DrawCube(transform2.position, new Vector3(0.2f, 0.2f, 0.2f));
					Gizmos.color = color;
				}
				else
				{
					Debug.LogError("Not Contains pathPoint" + i);
				}
			}
			Vector3[] pathByPositions = Bezier.GetPathByPositions(list.ToArray(), data.LerpNum);
			for (int j = 0; j < pathByPositions.Length - 1; j++)
			{
				Gizmos.DrawLine(pathByPositions[j], pathByPositions[j + 1]);
			}
		}
		else
		{
			Debug.LogError("Can't find the pathRoot named \"path\"");
		}
	}
}
