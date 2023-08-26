using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AutoMoveJumpTrigger : BaseTriggerBox
{
	public enum AutoMoveState
	{
		Null,
		Wait,
		Move,
		End
	}

	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public int LerpNum;

		public float MoveSpeed;

		public int PartNum;

		public int PointNum;

		public Vector3[] PathPoints;

		public Vector3[] ColliderInfo;

		public Vector2[] JumpInfo;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			LerpNum = bytes.GetInt32(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			PartNum = bytes.GetInt32(ref startIndex);
			PointNum = bytes.GetInt32(ref startIndex);
			PathPoints = bytes.GetVector3Array(ref startIndex);
			ColliderInfo = bytes.GetVector3Array(ref startIndex);
			JumpInfo = bytes.GetVector2Array(ref startIndex);
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

	public TriggerData data;

	private Transform tileObject;

	private BoxCollider[] colliders;

	private Vector3[,] bezierWorldPoints;

	private Vector3[,] bezierLocalPoints;

	private AutoMoveState currentState;

	private int currentPartIndex;

	private int currentPointIndex;

	private int partCount;

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
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		data.LerpNum = (int)objs[0];
		data.MoveSpeed = (float)objs[1];
		data.PartNum = (int)objs[2];
		data.PointNum = (int)objs[3];
		data.JumpInfo = (Vector2[])objs[4];
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
		partCount = data.PartNum;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentState = AutoMoveState.Null;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (currentState == AutoMoveState.Null)
		{
			currentState = AutoMoveState.Move;
			PlaySoundEffect();
			TriggerJump(ball);
		}
		else if (currentState == AutoMoveState.Wait)
		{
			currentState = AutoMoveState.Move;
			PlaySoundEffect();
			TriggerJump(ball);
		}
		else if (currentState == AutoMoveState.Move || currentState == AutoMoveState.End)
		{
			Debug.LogWarning(string.Concat("Role should not collide with the AutoMoveJumpTile :", currentState, base.name));
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
		data = JsonUtility.FromJson<TriggerData>(info);
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
					transform3.position = data.PathPoints[i * data.PointNum + j];
					array[j] = data.PathPoints[i * data.PointNum + j];
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
				TriggerData datum = data;
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
					array[i * data.PointNum + j] = transform3.position;
					continue;
				}
				Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
			}
			Transform transform4 = base.transform.Find("collider" + i);
			if ((bool)transform4)
			{
				TriggerData datum = data;
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
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
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
					transform3.position = data.PathPoints[i * data.PointNum + j];
					array[j] = data.PathPoints[i * data.PointNum + j];
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
				TriggerData datum = data;
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
					array[i * data.PointNum + j] = transform3.position;
					continue;
				}
				Debug.LogError("Not Contains pathPoint[" + i + "," + j + "]");
			}
			Transform transform4 = base.transform.Find("collider" + i);
			if ((bool)transform4)
			{
				TriggerData datum = data;
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

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (data.PartNum == 0 || data.PointNum == 0)
		{
			return;
		}
		Vector3[,] array = new Vector3[data.PartNum, data.PointNum];
		Transform transform = base.transform.Find("path");
		for (int i = 0; i < data.PartNum; i++)
		{
			Vector3[] array2 = new Vector3[data.PointNum];
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
		if (data.JumpInfo == null || data.JumpInfo.Length == 0)
		{
			return;
		}
		for (int l = 0; l < data.JumpInfo.Length; l++)
		{
			Transform transform4 = base.transform.Find("collider" + l);
			if ((bool)transform4)
			{
				Vector3 position = transform4.position;
				Vector3 forward = transform4.forward;
				float y = data.JumpInfo[l].y;
				Vector3 vector = position;
				int num = Mathf.Max(3, Mathf.CeilToInt(data.JumpInfo[l].x * 2.5f));
				float num2 = 0.5f;
				Vector3[] array3 = new Vector3[num + 1];
				array3[0] = vector;
				Color color2 = Gizmos.color;
				Gizmos.color = Color.yellow;
				for (int m = 0; m < num; m++)
				{
					Vector3 vector2 = vector + forward * num2;
					float num3 = 0f - Vector3.Dot(position - vector2, forward);
					float y2 = y - 4f * y / Mathf.Pow(data.JumpInfo[l].x, 2f) * Mathf.Pow(num3 - data.JumpInfo[l].x / 2f, 2f);
					array3[m + 1] = vector2 + new Vector3(0f, y2, 0f);
					Gizmos.DrawLine(array3[m], array3[m + 1]);
					vector = vector2;
				}
				Gizmos.color = color2;
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
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(ball.transform.position.y);
			ball.CallBeginJump(ball.transform.position, ball.transform.position + data.JumpInfo[currentPartIndex].x * ball.transform.forward, ball.transform.forward, data.JumpInfo[currentPartIndex].y, BaseRole.JumpType.Super);
		}
		if (!ball.IfJumping)
		{
			ball.CallBeginJump(ball.transform.position, ball.transform.position + data.JumpInfo[currentPartIndex].x * ball.transform.forward, ball.transform.forward, data.JumpInfo[currentPartIndex].y, BaseRole.JumpType.Super);
		}
		if (ball.IfDropping)
		{
			ball.CallEndDrop(ball.transform.position.y);
			ball.CallBeginJump(ball.transform.position, ball.transform.position + data.JumpInfo[currentPartIndex].x * ball.transform.forward, ball.transform.forward, data.JumpInfo[currentPartIndex].y, BaseRole.JumpType.Super);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_AutoMoveJumpTrigger_DATA rD_AutoMoveJumpTrigger_DATA = JsonUtility.FromJson<RD_AutoMoveJumpTrigger_DATA>(rd_data as string);
		tileObject.SetTransData(rD_AutoMoveJumpTrigger_DATA.tileObject);
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].gameObject.SetActive(rD_AutoMoveJumpTrigger_DATA.colliders[i]);
		}
		currentState = rD_AutoMoveJumpTrigger_DATA.currentState;
		currentPartIndex = rD_AutoMoveJumpTrigger_DATA.currentPartIndex;
		currentPointIndex = rD_AutoMoveJumpTrigger_DATA.currentPointIndex;
		partCount = rD_AutoMoveJumpTrigger_DATA.partCount;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_AutoMoveJumpTrigger_DATA rD_AutoMoveJumpTrigger_DATA = new RD_AutoMoveJumpTrigger_DATA();
		rD_AutoMoveJumpTrigger_DATA.tileObject = tileObject.GetTransData();
		rD_AutoMoveJumpTrigger_DATA.colliders = new bool[colliders.Length];
		for (int i = 0; i < colliders.Length; i++)
		{
			rD_AutoMoveJumpTrigger_DATA.colliders[i] = colliders[i].gameObject.activeSelf;
		}
		rD_AutoMoveJumpTrigger_DATA.currentState = currentState;
		rD_AutoMoveJumpTrigger_DATA.currentPartIndex = currentPartIndex;
		rD_AutoMoveJumpTrigger_DATA.currentPointIndex = currentPointIndex;
		rD_AutoMoveJumpTrigger_DATA.partCount = partCount;
		return JsonUtility.ToJson(rD_AutoMoveJumpTrigger_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_AutoMoveJumpTrigger_DATA rD_AutoMoveJumpTrigger_DATA = Bson.ToObject<RD_AutoMoveJumpTrigger_DATA>(rd_data);
		tileObject.SetTransData(rD_AutoMoveJumpTrigger_DATA.tileObject);
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].gameObject.SetActive(rD_AutoMoveJumpTrigger_DATA.colliders[i]);
		}
		currentState = rD_AutoMoveJumpTrigger_DATA.currentState;
		currentPartIndex = rD_AutoMoveJumpTrigger_DATA.currentPartIndex;
		currentPointIndex = rD_AutoMoveJumpTrigger_DATA.currentPointIndex;
		partCount = rD_AutoMoveJumpTrigger_DATA.partCount;
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_AutoMoveJumpTrigger_DATA rD_AutoMoveJumpTrigger_DATA = new RD_AutoMoveJumpTrigger_DATA();
		rD_AutoMoveJumpTrigger_DATA.tileObject = tileObject.GetTransData();
		rD_AutoMoveJumpTrigger_DATA.colliders = new bool[colliders.Length];
		for (int i = 0; i < colliders.Length; i++)
		{
			rD_AutoMoveJumpTrigger_DATA.colliders[i] = colliders[i].gameObject.activeSelf;
		}
		rD_AutoMoveJumpTrigger_DATA.currentState = currentState;
		rD_AutoMoveJumpTrigger_DATA.currentPartIndex = currentPartIndex;
		rD_AutoMoveJumpTrigger_DATA.currentPointIndex = currentPointIndex;
		rD_AutoMoveJumpTrigger_DATA.partCount = partCount;
		return Bson.ToBson(rD_AutoMoveJumpTrigger_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
