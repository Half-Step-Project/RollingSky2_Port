using System;
using System.IO;
using Foundation;
using UnityEngine;

public class RandomAnimEnemy : BaseEnemy
{
	[Serializable]
	public struct RandomItem
	{
		[Range(-2.5f, 2.5f)]
		public float ItemDeltaPosX;

		public bool IfShow;

		public string ItemAnim;

		public float BeginDistance;
	}

	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public int RandomTotal;

		public int TriggerUuid;

		public RandomItem[] RandomItems;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			RandomTotal = bytes.GetInt32(ref startIndex);
			TriggerUuid = bytes.GetInt32(ref startIndex);
			int @int = bytes.GetInt32(ref startIndex);
			RandomItem[] array = new RandomItem[@int];
			for (int i = 0; i < @int; i++)
			{
				RandomItem randomItem = default(RandomItem);
				randomItem.ItemDeltaPosX = bytes.GetSingle(ref startIndex);
				randomItem.IfShow = bytes.GetBoolean(ref startIndex);
				randomItem.ItemAnim = bytes.GetStringWithSize(ref startIndex);
				randomItem.BeginDistance = bytes.GetSingle(ref startIndex);
				array[i] = randomItem;
			}
			RandomItems = array;
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(RandomTotal.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TriggerUuid.GetBytes(), ref offset);
				memoryStream.WriteByteArray(RandomItems.Length.GetBytes(), ref offset);
				for (int i = 0; i < RandomItems.Length; i++)
				{
					RandomItem randomItem = RandomItems[i];
					memoryStream.WriteByteArray(randomItem.ItemDeltaPosX.GetBytes(), ref offset);
					memoryStream.WriteByteArray(randomItem.IfShow.GetBytes(), ref offset);
					memoryStream.WriteByteArray(randomItem.ItemAnim.GetBytesWithSize(), ref offset);
					memoryStream.WriteByteArray(randomItem.BeginDistance.GetBytes(), ref offset);
				}
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly string NodeModelStr = "model";

	public static readonly Color[] DebugColors = new Color[12]
	{
		Color.red,
		Color.green,
		Color.blue,
		Color.grey,
		Color.yellow,
		Color.cyan,
		Color.red,
		Color.green,
		Color.blue,
		Color.grey,
		Color.yellow,
		Color.cyan
	};

	public EnemyData data;

	public int SelectIndex = -1;

	public int DebugIndex = -1;

	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	private Animation anim;

	private Transform modelPart;

	public override void Initialize()
	{
		base.Initialize();
		modelPart = base.transform.Find(NodeModelStr);
		RandomAnimTrigger element = null;
		MapController.Instance.TryGetElementByUUID<RandomAnimTrigger>(data.TriggerUuid, out element);
		if (element != null)
		{
			SelectIndex = element.SelectIndex;
			if (modelPart != null)
			{
				modelPart.gameObject.SetActive(data.RandomItems[SelectIndex].IfShow);
			}
			modelPart.transform.localPosition = new Vector3(data.RandomItems[SelectIndex].ItemDeltaPosX, 0f, 0f);
			commonState = CommonState.None;
			anim = GetComponentInChildren<Animation>(true);
		}
		else
		{
			Log.Error("Can not find trigger", base.gameObject);
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
	}

	public override void UpdateElement()
	{
		if (SelectIndex < 0)
		{
			return;
		}
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState != 0)
		{
			return;
		}
		RandomItem randomItem = data.RandomItems[SelectIndex];
		if (num >= randomItem.BeginDistance)
		{
			if (anim != null && !string.IsNullOrEmpty(randomItem.ItemAnim))
			{
				anim[randomItem.ItemAnim].normalizedTime = 0f;
				anim.Play(randomItem.ItemAnim);
				Log.Info("play anim " + randomItem.ItemAnim);
			}
			commonState = CommonState.Active;
		}
	}

	public override void OnDrawGizmos()
	{
		if (DebugIndex == -1)
		{
			for (int i = 0; i < data.RandomItems.Length; i++)
			{
				RandomItem randomItem = data.RandomItems[i];
				Vector3 position = base.transform.position;
				position.x += randomItem.ItemDeltaPosX;
				Color color = DebugColors[i];
				if (!randomItem.IfShow)
				{
					color = Color.black;
				}
				Gizmos.color = color;
				Gizmos.DrawCube(position, Vector3.one);
				Vector3 vector = position;
				vector.z += data.RandomItems[i].BeginDistance;
				Gizmos.DrawCube(vector, new Vector3(0.8f, 0.1f, 0.1f));
				Gizmos.DrawLine(position, vector);
			}
		}
		else
		{
			if (DebugIndex >= data.RandomItems.Length || DebugIndex >= DebugColors.Length)
			{
				return;
			}
			RandomItem randomItem2 = data.RandomItems[DebugIndex];
			Vector3 position2 = base.transform.position;
			position2.x += randomItem2.ItemDeltaPosX;
			Color color2 = DebugColors[DebugIndex];
			string itemAnim = randomItem2.ItemAnim;
			if (!randomItem2.IfShow)
			{
				color2 = Color.black;
			}
			Gizmos.color = color2;
			Gizmos.DrawCube(position2, Vector3.one);
			Vector3 vector2 = position2;
			vector2.z += data.RandomItems[DebugIndex].BeginDistance;
			Gizmos.DrawCube(vector2, new Vector3(0.8f, 0.1f, 0.1f));
			Gizmos.DrawLine(position2, vector2);
			if (debugPercent != DebugPercent)
			{
				if (anim == null)
				{
					anim = GetComponentInChildren<Animation>(true);
				}
				if (anim != null && !string.IsNullOrEmpty(itemAnim))
				{
					anim.Play(itemAnim);
					anim[itemAnim].normalizedTime = DebugPercent;
					anim.Sample();
					anim.Stop();
				}
				debugPercent = DebugPercent;
			}
		}
	}

	public override void ReadBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
		}
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}
}
