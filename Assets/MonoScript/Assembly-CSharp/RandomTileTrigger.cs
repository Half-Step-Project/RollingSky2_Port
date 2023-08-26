using System;
using System.IO;
using Foundation;
using UnityEngine;

public class RandomTileTrigger : BaseTriggerBox, IRandomTrigger
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public bool IfOrder;

		public int[] OrderList;

		public Bounds CollideBounds;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfOrder = bytes.GetBoolean(ref startIndex);
			int @int = bytes.GetInt32(ref startIndex);
			OrderList = new int[@int];
			for (int i = 0; i < @int; i++)
			{
				OrderList[i] = bytes.GetInt32(ref startIndex);
			}
			Vector3 vector = bytes.GetVector3(ref startIndex);
			Vector3 vector2 = bytes.GetVector3(ref startIndex);
			CollideBounds = new Bounds(vector, vector2);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(IfOrder.GetBytes(), ref offset);
				memoryStream.WriteByteArray(OrderList.Length.GetBytes(), ref offset);
				for (int i = 0; i < OrderList.Length; i++)
				{
					memoryStream.WriteByteArray(OrderList[i].GetBytes(), ref offset);
				}
				memoryStream.WriteByteArray(CollideBounds.center.GetBytes(), ref offset);
				memoryStream.WriteByteArray(CollideBounds.size.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	public int SelectIndex { get; private set; }

	public bool IfOrder
	{
		get
		{
			return data.IfOrder;
		}
	}

	public int[] OrderArray
	{
		get
		{
			return data.OrderList;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		RandomAnimController.Instance.ReqisterTrigger(m_uuId, this);
		SelectIndex = RandomAnimController.Instance.GetSelectIndexByTriggerId(m_uuId);
		base.transform.Find("affectArea").GetComponentInChildren<BoxCollider>(true);
	}

	public override void ReadBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
			Transform transform = base.transform.Find("affectArea");
			BoxCollider componentInChildren = transform.GetComponentInChildren<BoxCollider>(true);
			componentInChildren.center = data.CollideBounds.center - transform.transform.position;
			componentInChildren.size = data.CollideBounds.size;
		}
	}

	public override byte[] WriteBytes()
	{
		BoxCollider componentInChildren = base.transform.Find("affectArea").GetComponentInChildren<BoxCollider>(true);
		data.CollideBounds = componentInChildren.bounds;
		return StructTranslatorUtility.ToByteArray(data);
	}
}
