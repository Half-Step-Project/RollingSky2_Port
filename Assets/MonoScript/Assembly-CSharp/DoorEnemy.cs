using System;
using System.IO;
using Foundation;
using UnityEngine;

public class DoorEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public Vector3 BeginPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public EnemyData data;

	public GameObject model1;

	public GameObject model2;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.BeginDistance = (float)objs[0];
			data.ResetDistance = (float)objs[1];
			data.BeginPos = base.transform.position;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		if (model1 == null)
		{
			model1 = base.transform.Find("model1").gameObject;
			model1.SetActive(true);
		}
		if (model2 == null)
		{
			model2 = base.transform.Find("model2").gameObject;
			model2.SetActive(false);
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if ((bool)model1)
		{
			model1.SetActive(true);
		}
		if ((bool)model2)
		{
			model2.SetActive(false);
		}
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active && num >= data.ResetDistance)
		{
			OnTriggerStop();
			commonState = CommonState.End;
		}
	}

	public override void OnTriggerPlay()
	{
		PlayParticle();
		if ((bool)model1)
		{
			model1.SetActive(false);
		}
		if ((bool)model2)
		{
			model2.SetActive(true);
		}
	}

	public override void OnTriggerStop()
	{
		StopParticle();
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EnemyData>(info);
	}

	public override string Write()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		Transform transform = base.transform.Find("triggerPoint");
		if ((bool)transform)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
			Gizmos.color = color;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_DoorEnemy_DATA rD_DoorEnemy_DATA = JsonUtility.FromJson<RD_DoorEnemy_DATA>(rd_data as string);
		commonState = rD_DoorEnemy_DATA.commonState;
		model1.SetActive(rD_DoorEnemy_DATA.model1Active);
		model2.SetActive(rD_DoorEnemy_DATA.model2Active);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_DoorEnemy_DATA
		{
			commonState = commonState,
			model1Active = model1.activeSelf,
			model2Active = model2.activeSelf
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_DoorEnemy_DATA rD_DoorEnemy_DATA = Bson.ToObject<RD_DoorEnemy_DATA>(rd_data);
		commonState = rD_DoorEnemy_DATA.commonState;
		model1.SetActive(rD_DoorEnemy_DATA.model1Active);
		model2.SetActive(rD_DoorEnemy_DATA.model2Active);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_DoorEnemy_DATA
		{
			commonState = commonState,
			model1Active = model1.activeSelf,
			model2Active = model2.activeSelf
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
