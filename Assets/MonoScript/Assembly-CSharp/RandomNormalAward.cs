using System;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;

public class RandomNormalAward : BaseEnemy, IAwardComplete, IAward
{
	[Serializable]
	public struct RandomItem
	{
		public Vector3 ItemPos;

		public bool IfShow;

		public string ItemAnim;

		public float BeginDistance;
	}

	[Serializable]
	public struct AwardData : IReadWriteBytes
	{
		public int RandomTotal;

		public int TriggerUuid;

		public RandomItem[] RandomItems;

		public float RotateSpeed;

		[Label]
		public int sortID;

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
				randomItem.ItemPos = bytes.GetVector3(ref startIndex);
				randomItem.IfShow = bytes.GetBoolean(ref startIndex);
				randomItem.ItemAnim = bytes.GetStringWithSize(ref startIndex);
				randomItem.BeginDistance = bytes.GetSingle(ref startIndex);
				array[i] = randomItem;
			}
			RandomItems = array;
			RotateSpeed = bytes.GetSingle(ref startIndex);
			sortID = bytes.GetInt32(ref startIndex);
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
					memoryStream.WriteByteArray(randomItem.ItemPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(randomItem.IfShow.GetBytes(), ref offset);
					memoryStream.WriteByteArray(randomItem.ItemAnim.GetBytesWithSize(), ref offset);
					memoryStream.WriteByteArray(randomItem.BeginDistance.GetBytes(), ref offset);
				}
				memoryStream.WriteByteArray(RotateSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(sortID.GetBytes(), ref offset);
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

	public AwardData data;

	public int SelectIndex = -1;

	public int DebugIndex = -1;

	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	protected Transform modelPart;

	private Animation anim;

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
			base.transform.position = StartPos + data.RandomItems[SelectIndex].ItemPos;
			commonState = CommonState.None;
			anim = GetComponentInChildren<Animation>(true);
		}
		else
		{
			Log.Error("Can not find trigger", base.gameObject);
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType dropType = GetDropType();
		dataModule.IsShowForAwardByDropType(dropType, data.sortID);
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
		StopParticle();
	}

	public override void UpdateElement()
	{
		if (SelectIndex < 0)
		{
			return;
		}
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
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
		else if (commonState == CommonState.Active)
		{
			base.transform.Rotate(Vector3.up * data.RotateSpeed);
		}
	}

	public override void OnDrawGizmos()
	{
		if (DebugIndex == -1)
		{
			for (int i = 0; i < data.RandomItems.Length; i++)
			{
				RandomItem randomItem = data.RandomItems[i];
				Vector3 vector = base.transform.TransformPoint(randomItem.ItemPos);
				Color color = DebugColors[i];
				if (!randomItem.IfShow)
				{
					color = Color.black;
				}
				Gizmos.color = color;
				Gizmos.DrawCube(vector, Vector3.one);
				Vector3 vector2 = vector;
				vector2.z += data.RandomItems[i].BeginDistance;
				Gizmos.DrawCube(vector2, new Vector3(0.8f, 0.1f, 0.1f));
				Gizmos.DrawLine(vector, vector2);
			}
		}
		else
		{
			if (DebugIndex >= data.RandomItems.Length || DebugIndex >= DebugColors.Length)
			{
				return;
			}
			RandomItem randomItem2 = data.RandomItems[DebugIndex];
			Vector3 position = base.transform.position;
			Vector3 vector3 = base.transform.TransformPoint(randomItem2.ItemPos);
			Color color2 = DebugColors[DebugIndex];
			string itemAnim = randomItem2.ItemAnim;
			if (!randomItem2.IfShow)
			{
				color2 = Color.black;
			}
			Gizmos.color = color2;
			Gizmos.DrawCube(vector3, Vector3.one);
			Vector3 vector4 = vector3;
			vector4.z += data.RandomItems[DebugIndex].BeginDistance;
			Gizmos.DrawCube(vector4, new Vector3(0.8f, 0.1f, 0.1f));
			Gizmos.DrawLine(vector3, vector4);
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
			data = StructTranslatorUtility.ToStructure<AwardData>(bytes);
		}
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		OnCollideBall(ball);
	}

	protected virtual void OnCollideBall(BaseRole ball)
	{
	}

	public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
	{
		OnCollideCouple(couple, collider);
	}

	protected virtual void OnCollideCouple(BaseCouple couple, Collider collider)
	{
	}

	public int GetAwardSortID()
	{
		return data.sortID;
	}

	public void SetAwardSortID(int id)
	{
		data.sortID = id;
	}

	public virtual DropType GetDropType()
	{
		return DropType.NONE;
	}

	public bool IsHaveFragment()
	{
		return false;
	}

	public int GetHaveFragmentCount()
	{
		return 0;
	}
}
