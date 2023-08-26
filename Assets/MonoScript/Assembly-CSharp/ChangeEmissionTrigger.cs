using System;
using System.IO;
using Foundation;
using UnityEngine;

public class ChangeEmissionTrigger : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float LerpTime;

		public Vector3 BeginPos;

		public float DefaultEmission;

		public float TargetEmission;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			LerpTime = bytes.GetSingle(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
			DefaultEmission = bytes.GetSingle(ref startIndex);
			TargetEmission = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(LerpTime.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DefaultEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetEmission.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public EnemyData data;

	private Material modelMat;

	private float currentPlayTime;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.BeginDistance = (float)objs[0];
			data.ResetDistance = (float)objs[1];
			data.LerpTime = (float)objs[2];
			data.DefaultEmission = (float)objs[3];
			data.TargetEmission = (float)objs[4];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		if (modelMat == null)
		{
			modelMat = base.transform.Find("model").GetComponent<MeshRenderer>().material;
		}
		modelMat.SetFloat("_Emmission", data.DefaultEmission);
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if ((bool)modelMat)
		{
			modelMat.SetFloat("_Emmission", data.DefaultEmission);
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
				currentPlayTime = 0f;
			}
		}
		else
		{
			if (commonState != CommonState.Active)
			{
				return;
			}
			if (num >= data.ResetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
				currentPlayTime = 0f;
				return;
			}
			currentPlayTime += Time.deltaTime;
			if (data.LerpTime > 0f)
			{
				SetColor(currentPlayTime / data.LerpTime, data.DefaultEmission, data.TargetEmission, modelMat);
			}
			else
			{
				SetColor(1f, data.DefaultEmission, data.TargetEmission, modelMat);
			}
		}
	}

	public override void OnTriggerPlay()
	{
		PlayParticle();
	}

	public override void OnTriggerStop()
	{
		StopParticle();
	}

	private void SetColor(float percent, float beginCol, float targetCol, Material mat)
	{
		if ((bool)mat)
		{
			mat.SetFloat("_Emmission", Mathf.Lerp(beginCol, targetCol, percent));
		}
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
}
