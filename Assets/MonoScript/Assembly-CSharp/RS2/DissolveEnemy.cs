using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class DissolveEnemy : BaseEnemy
	{
		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public float BeginDistance;

			public float BeginPercent;

			public float TargetPercent;

			public float DissolveScaler;

			public Vector3 BeginPos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginDistance = bytes.GetSingle(ref startIndex);
				BeginPercent = bytes.GetSingle(ref startIndex);
				TargetPercent = bytes.GetSingle(ref startIndex);
				DissolveScaler = bytes.GetSingle(ref startIndex);
				BeginPos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginPercent.GetBytes(), ref offset);
					memoryStream.WriteByteArray(TargetPercent.GetBytes(), ref offset);
					memoryStream.WriteByteArray(DissolveScaler.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string DissolutionStrengthVal = "_Tex_strength";

		public static readonly string TriggerPointName = "triggerPoint";

		public EnemyData data;

		private Material modelMat;

		private Transform triggerPoint;

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
				data.BeginPercent = (float)objs[1];
				data.TargetPercent = (float)objs[2];
				data.DissolveScaler = (float)objs[3];
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			commonState = CommonState.None;
			if (modelMat == null)
			{
				modelMat = base.transform.Find("model").GetComponentInChildren<MeshRenderer>().material;
			}
			PlayByPercent(0f);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			PlayByPercent(0f);
			commonState = CommonState.None;
		}

		public override void UpdateElement()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z - data.BeginDistance;
			if (commonState == CommonState.None)
			{
				if (num >= 0f)
				{
					commonState = CommonState.Active;
				}
			}
			else if (commonState == CommonState.Active)
			{
				float percent = GetPercent(num);
				PlayByPercent(percent);
				if (percent >= 1f)
				{
					commonState = CommonState.InActive;
				}
			}
			else
			{
				CommonState commonState2 = commonState;
				int num2 = 4;
			}
		}

		public override float GetPercent(float distance)
		{
			if (data.DissolveScaler == 0f)
			{
				return 0f;
			}
			return distance / data.DissolveScaler;
		}

		public override void PlayByPercent(float percent)
		{
			float t = Mathf.Clamp(percent, 0f, 1f);
			if ((bool)modelMat)
			{
				modelMat.SetFloat(DissolutionStrengthVal, Mathf.Lerp(data.BeginPercent, data.TargetPercent, t));
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
			if (triggerPoint == null)
			{
				triggerPoint = base.transform.Find(TriggerPointName);
			}
			if (triggerPoint != null)
			{
				triggerPoint.position = data.BeginPos;
			}
		}

		public override string Write()
		{
			if (triggerPoint == null)
			{
				triggerPoint = base.transform.Find(TriggerPointName);
			}
			if (triggerPoint != null)
			{
				data.BeginPos = triggerPoint.position;
				data.BeginDistance = base.transform.parent.InverseTransformPoint(data.BeginPos).z - base.transform.localPosition.z;
			}
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
			if (triggerPoint == null)
			{
				triggerPoint = base.transform.Find(TriggerPointName);
			}
			if (triggerPoint != null)
			{
				triggerPoint.position = data.BeginPos;
			}
		}

		public override byte[] WriteBytes()
		{
			if (triggerPoint == null)
			{
				triggerPoint = base.transform.Find(TriggerPointName);
			}
			if (triggerPoint != null)
			{
				data.BeginPos = triggerPoint.position;
				data.BeginDistance = base.transform.parent.InverseTransformPoint(data.BeginPos).z - base.transform.localPosition.z;
			}
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (triggerPoint == null)
			{
				triggerPoint = base.transform.Find(TriggerPointName);
				if (triggerPoint == null)
				{
					return;
				}
			}
			Color color = Gizmos.color;
			Gizmos.color = Color.green;
			Gizmos.DrawCube(triggerPoint.position, new Vector3(1f, 0.1f, 0.1f));
			Gizmos.color = color;
		}
	}
}
