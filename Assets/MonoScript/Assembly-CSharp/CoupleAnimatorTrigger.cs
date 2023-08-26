using System;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class CoupleAnimatorTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public CoupleThiefAnim animType;

		public CoupleAnimReceiverType receiver;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			animType = (CoupleThiefAnim)bytes.GetInt32(ref startIndex);
			receiver = (CoupleAnimReceiverType)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(((int)animType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)receiver).GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

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
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
	}

	public override void TriggerEnter(BaseRole ball)
	{
	}

	public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
	{
		if (commonState == CommonState.None)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<CoupleThiefAnimEventArgs>().Initialize((int)data.animType, data.receiver));
			commonState = CommonState.Active;
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		if (Grid.m_isShowTriggerBox)
		{
			BoxCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoxCollider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 center = componentsInChildren[i].center;
				Vector3 size = componentsInChildren[i].size;
				Vector3[] array = new Vector3[8]
				{
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f))
				};
				Gizmos.color = Color.red;
				Gizmos.DrawLine(array[0], array[1]);
				Gizmos.DrawLine(array[1], array[2]);
				Gizmos.DrawLine(array[2], array[3]);
				Gizmos.DrawLine(array[3], array[0]);
				Gizmos.DrawLine(array[4], array[5]);
				Gizmos.DrawLine(array[5], array[6]);
				Gizmos.DrawLine(array[6], array[7]);
				Gizmos.DrawLine(array[7], array[4]);
				Gizmos.DrawLine(array[0], array[4]);
				Gizmos.DrawLine(array[1], array[5]);
				Gizmos.DrawLine(array[2], array[6]);
				Gizmos.DrawLine(array[3], array[7]);
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawLine(componentsInChildren[i].bounds.center, base.gameObject.transform.position);
				Gizmos.color = new Color(1f, 0f, 0f, 1f);
				Gizmos.DrawSphere(base.gameObject.transform.position, 0.5f);
			}
		}
	}
}
