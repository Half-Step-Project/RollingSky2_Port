using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AutoMoveJumpTile : BaseTile
{
	public enum AutoMoveState
	{
		Null,
		Wait,
		Move,
		End
	}

	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public int LerpNum;

		public float MoveSpeed;

		public int PartNum;

		public int PointNum;

		public Vector3[] PathPoints;

		public Vector3[] ColliderInfo;

		public Vector3[] JumpInfo;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			LerpNum = bytes.GetInt32(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			PartNum = bytes.GetInt32(ref startIndex);
			PointNum = bytes.GetInt32(ref startIndex);
			PathPoints = bytes.GetVector3Array(ref startIndex);
			ColliderInfo = bytes.GetVector3Array(ref startIndex);
			JumpInfo = bytes.GetVector3Array(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(LerpNum.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(PartNum.GetBytes(), ref offset);
				memoryStream.WriteByteArray(PointNum.GetBytes(), ref offset);
				memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ColliderInfo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpInfo.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Transform tileObject;

	private BoxCollider[] colliders;

	private Vector3[,] bezierWorldPoints;

	private Vector3[,] bezierLocalPoints;

	private AutoMoveState currentState;

	private int currentPartIndex;

	private int currentPointIndex;

	public override bool CanRecycle
	{
		get
		{
			return false;
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
		data.LerpNum = (int)objs[0];
		data.MoveSpeed = (float)objs[1];
		data.PartNum = (int)objs[2];
		data.PointNum = (int)objs[3];
		data.JumpInfo = (Vector3[])objs[4];
	}

	public override void Initialize()
	{
		base.Initialize();
		if (tileObject == null)
		{
			tileObject = base.transform.Find("model");
		}
		if (colliders == null)
		{
			colliders = new BoxCollider[data.PartNum];
			for (int i = 0; i < colliders.Length; i++)
			{
				colliders[i] = base.transform.Find("collider" + i).GetComponent<BoxCollider>();
			}
		}
		for (int j = 0; j < colliders.Length; j++)
		{
			if (j == 0)
			{
				colliders[j].gameObject.SetActive(true);
			}
			else
			{
				colliders[j].gameObject.SetActive(false);
			}
		}
		tileObject.position = data.PathPoints[0];
		currentState = AutoMoveState.Null;
		currentPartIndex = 0;
		currentPointIndex = 0;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentState = AutoMoveState.Null;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		Debug.Log("Enter");
		if (currentState == AutoMoveState.Null)
		{
			currentState = AutoMoveState.Move;
		}
		else if (currentState == AutoMoveState.Wait)
		{
			currentState = AutoMoveState.Move;
			TriggerJump(ball);
		}
		else if (currentState == AutoMoveState.Move || currentState == AutoMoveState.End)
		{
			Debug.LogError("Role should not collide with the AutoMoveJumpTile :" + currentState);
		}
	}

	public override void UpdateElement()
	{
		if (currentState == AutoMoveState.Null || currentState == AutoMoveState.Wait)
		{
			return;
		}
		if (currentState == AutoMoveState.Move)
		{
			bool ifMoveFinish = false;
			bool ifAllFinish = false;
			MoveForward(ref ifMoveFinish, ref ifAllFinish);
			if (ifMoveFinish)
			{
				if (ifAllFinish)
				{
					currentState = AutoMoveState.End;
					return;
				}
				currentPointIndex = 0;
				currentPartIndex++;
				colliders[currentPartIndex].gameObject.SetActive(true);
				currentState = AutoMoveState.Wait;
			}
		}
		else
		{
			AutoMoveState currentState2 = currentState;
			int num = 3;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
		Transform transform = base.transform.Find("path");
		bezierWorldPoints = new Vector3[data.PartNum, data.PointNum * data.LerpNum + 1];
		bezierLocalPoints = new Vector3[data.PartNum, data.PointNum * data.LerpNum + 1];
		for (int i = 0; i < data.PartNum; i++)
		{
			Vector3[] array = new Vector3[data.PointNum];
			Vector3[] array2 = new Vector3[data.PointNum * data.LerpNum + 1];
			Transform transform2 = transform.Find("part" + i);
			for (int j = 0; j < data.PointNum; j++)
			{
				Transform transform3 = transform2.Find("item" + j);
				if ((bool)transform3)
				{
					transform3.position = data.PathPoints[i * data.PartNum + j];
					array[j] = data.PathPoints[i * data.PartNum + j];
					continue;
				}
				Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
			}
			array2 = Bezier.GetPathByPositions(array, data.LerpNum);
			for (int k = 0; k < array2.Length; k++)
			{
				bezierWorldPoints[i, k] = array2[k];
				if (base.groupTransform != null)
				{
					bezierLocalPoints[i, k] = base.groupTransform.InverseTransformPoint(bezierWorldPoints[i, k]);
				}
			}
			Transform transform4 = base.transform.Find("collider" + i);
			if ((bool)transform4)
			{
				Vector3[] colliderInfo = data.ColliderInfo;
				TileData datum = data;
				transform4.localPosition = colliderInfo[0 + i];
				BoxCollider component = transform4.GetComponent<BoxCollider>();
				component.center = data.ColliderInfo[data.PartNum + i];
				component.size = data.ColliderInfo[2 * data.PartNum + i];
			}
		}
	}

	public override string Write()
	{
		if (data.PartNum == 0 || data.PointNum == 0)
		{
			Debug.LogError("PathNum &  PointNum should not be 0!");
		}
		Vector3[] array = new Vector3[data.PartNum * data.PointNum];
		Vector3[] array2 = new Vector3[3 * data.PartNum];
		Transform transform = base.transform.Find("path");
		for (int i = 0; i < data.PartNum; i++)
		{
			Transform transform2 = transform.Find("part" + i);
			for (int j = 0; j < data.PointNum; j++)
			{
				Transform transform3 = transform2.Find("item" + j);
				if ((bool)transform3)
				{
					array[i * data.PartNum + j] = transform3.position;
					continue;
				}
				Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
			}
			Transform transform4 = base.transform.Find("collider" + i);
			if ((bool)transform4)
			{
				TileData datum = data;
				array2[0 + i] = transform4.localPosition;
				BoxCollider component = transform4.GetComponent<BoxCollider>();
				array2[data.PartNum + i] = component.center;
				array2[2 * data.PartNum + i] = component.size;
			}
		}
		data.PathPoints = array;
		data.ColliderInfo = array2;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		Transform transform = base.transform.Find("path");
		bezierWorldPoints = new Vector3[data.PartNum, data.PointNum * data.LerpNum + 1];
		bezierLocalPoints = new Vector3[data.PartNum, data.PointNum * data.LerpNum + 1];
		for (int i = 0; i < data.PartNum; i++)
		{
			Vector3[] array = new Vector3[data.PointNum];
			Vector3[] array2 = new Vector3[data.PointNum * data.LerpNum + 1];
			Transform transform2 = transform.Find("part" + i);
			for (int j = 0; j < data.PointNum; j++)
			{
				Transform transform3 = transform2.Find("item" + j);
				if ((bool)transform3)
				{
					transform3.position = data.PathPoints[i * data.PartNum + j];
					array[j] = data.PathPoints[i * data.PartNum + j];
					continue;
				}
				Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
			}
			array2 = Bezier.GetPathByPositions(array, data.LerpNum);
			for (int k = 0; k < array2.Length; k++)
			{
				bezierWorldPoints[i, k] = array2[k];
				if (base.groupTransform != null)
				{
					bezierLocalPoints[i, k] = base.groupTransform.InverseTransformPoint(bezierWorldPoints[i, k]);
				}
			}
			Transform transform4 = base.transform.Find("collider" + i);
			if ((bool)transform4)
			{
				Vector3[] colliderInfo = data.ColliderInfo;
				TileData datum = data;
				transform4.localPosition = colliderInfo[0 + i];
				BoxCollider component = transform4.GetComponent<BoxCollider>();
				component.center = data.ColliderInfo[data.PartNum + i];
				component.size = data.ColliderInfo[2 * data.PartNum + i];
			}
		}
	}

	public override byte[] WriteBytes()
	{
		if (data.PartNum == 0 || data.PointNum == 0)
		{
			Debug.LogError("PathNum &  PointNum should not be 0!");
		}
		Vector3[] array = new Vector3[data.PartNum * data.PointNum];
		Vector3[] array2 = new Vector3[3 * data.PartNum];
		Transform transform = base.transform.Find("path");
		for (int i = 0; i < data.PartNum; i++)
		{
			Transform transform2 = transform.Find("part" + i);
			for (int j = 0; j < data.PointNum; j++)
			{
				Transform transform3 = transform2.Find("item" + j);
				if ((bool)transform3)
				{
					array[i * data.PartNum + j] = transform3.position;
					continue;
				}
				Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
			}
			Transform transform4 = base.transform.Find("collider" + i);
			if ((bool)transform4)
			{
				TileData datum = data;
				array2[0 + i] = transform4.localPosition;
				BoxCollider component = transform4.GetComponent<BoxCollider>();
				array2[data.PartNum + i] = component.center;
				array2[2 * data.PartNum + i] = component.size;
			}
		}
		data.PathPoints = array;
		data.ColliderInfo = array2;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public void OnDrawGizmos()
	{
		if (data.PartNum == 0 || data.PointNum == 0)
		{
			return;
		}
		Vector3[,] array = new Vector3[data.PartNum, data.PointNum];
		Transform transform = base.transform.Find("path");
		for (int i = 0; i < data.PartNum; i++)
		{
			Vector3[] array2 = new Vector3[data.PartNum];
			Transform transform2 = transform.Find("part" + i);
			for (int j = 0; j < data.PointNum; j++)
			{
				Transform transform3 = transform2.Find("item" + j);
				if ((bool)transform3)
				{
					array[i, j] = transform3.position;
					array2[j] = transform3.position;
					Color color = Gizmos.color;
					Gizmos.color = Color.green;
					Gizmos.DrawCube(transform3.position, new Vector3(0.2f, 0.2f, 0.2f));
					Gizmos.color = color;
				}
				else
				{
					Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
				}
			}
			Vector3[] pathByPositions = Bezier.GetPathByPositions(array2, data.LerpNum);
			for (int k = 0; k < pathByPositions.Length - 1; k++)
			{
				Gizmos.DrawLine(pathByPositions[k], pathByPositions[k + 1]);
			}
		}
	}

	private void MoveForward(ref bool ifMoveFinish, ref bool ifAllFinish)
	{
		Vector3 a = base.groupTransform.InverseTransformPoint(tileObject.position);
		float num = a.z + data.MoveSpeed * Time.deltaTime;
		Vector3 nextP = Vector3.zero;
		bool ifNext = false;
		GetNextPoint(num, ref nextP, ref ifNext, ref ifAllFinish);
		ifMoveFinish = !ifNext;
		float num2 = nextP.z - a.z;
		float num3 = 1f;
		if (num2 > 0f)
		{
			num3 = (num - a.z) / num2;
			a = Vector3.Lerp(a, nextP, num3);
			tileObject.position = base.groupTransform.TransformPoint(a);
		}
	}

	private void GetNextPoint(float targetZ, ref Vector3 nextP, ref bool ifNext, ref bool ifMoveFinish)
	{
		ifNext = false;
		ifMoveFinish = false;
		for (int i = currentPointIndex; i < bezierLocalPoints.GetLength(1); i++)
		{
			if (targetZ < bezierLocalPoints[currentPartIndex, i].z)
			{
				currentPointIndex = i;
				nextP = bezierLocalPoints[currentPartIndex, currentPointIndex];
				ifNext = true;
				ifMoveFinish = false;
				return;
			}
		}
		currentPointIndex = bezierLocalPoints.GetLength(1) - 1;
		nextP = bezierLocalPoints[currentPartIndex, currentPointIndex];
		if (currentPartIndex >= bezierLocalPoints.GetLength(0) - 1)
		{
			ifMoveFinish = true;
		}
		else
		{
			ifMoveFinish = false;
		}
	}

	private void TriggerJump(BaseRole ball)
	{
		ball.OnTileEnter(this);
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(tileObject.position.y);
			ball.CallBeginJump(tileObject.position, tileObject.position + data.JumpInfo[currentPartIndex - 1].x * ball.transform.forward, ball.transform.forward, data.JumpInfo[currentPartIndex - 1].y, BaseRole.JumpType.Super);
			return;
		}
		if (ball.IfDropping)
		{
			ball.CallEndDrop(tileObject.position.y);
			ball.CallBeginJump(tileObject.position, tileObject.position + data.JumpInfo[currentPartIndex - 1].x * ball.transform.forward, ball.transform.forward, data.JumpInfo[currentPartIndex - 1].y, BaseRole.JumpType.Super);
		}
		if (!ball.IfJumping)
		{
			ball.CallBeginJump(tileObject.position, tileObject.position + data.JumpInfo[currentPartIndex - 1].x * ball.transform.forward, ball.transform.forward, data.JumpInfo[currentPartIndex - 1].y, BaseRole.JumpType.Super);
		}
	}
}
