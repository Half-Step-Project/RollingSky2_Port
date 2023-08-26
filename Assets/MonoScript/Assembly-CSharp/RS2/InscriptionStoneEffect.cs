using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class InscriptionStoneEffect : BaseEnemy
	{
		public enum EmissionState
		{
			None,
			Active,
			End
		}

		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public float BeginAnimDistance;

			public float BeginEmissionDistance;

			public float ResetDistance;

			public float BaseBallSpeed;

			public Color SourceColor;

			public float SourceEmission;

			public Color TargetColor;

			public float TargetEmission;

			public float EmissionDistance;

			public bool IfAutoPlay;

			public bool IfLoop;

			public float AudioPlayTime;

			public Vector3 BeginAnimPos;

			public Vector3 BeginEmissionPos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginAnimDistance = bytes.GetSingle(ref startIndex);
				BeginEmissionDistance = bytes.GetSingle(ref startIndex);
				ResetDistance = bytes.GetSingle(ref startIndex);
				BaseBallSpeed = bytes.GetSingle(ref startIndex);
				SourceColor = bytes.GetColor(ref startIndex);
				SourceEmission = bytes.GetSingle(ref startIndex);
				TargetColor = bytes.GetColor(ref startIndex);
				TargetEmission = bytes.GetSingle(ref startIndex);
				EmissionDistance = bytes.GetSingle(ref startIndex);
				IfAutoPlay = bytes.GetBoolean(ref startIndex);
				IfLoop = bytes.GetBoolean(ref startIndex);
				AudioPlayTime = bytes.GetSingle(ref startIndex);
				BeginAnimPos = bytes.GetVector3(ref startIndex);
				BeginEmissionPos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginAnimDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginEmissionDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(SourceColor.GetBytes(), ref offset);
					memoryStream.WriteByteArray(SourceEmission.GetBytes(), ref offset);
					memoryStream.WriteByteArray(TargetColor.GetBytes(), ref offset);
					memoryStream.WriteByteArray(TargetEmission.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EmissionDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfAutoPlay.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
					memoryStream.WriteByteArray(AudioPlayTime.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginAnimPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginEmissionPos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string EmissionColorName = "_EmissionColor";

		public static readonly string EmissionValName = "_Emmission";

		public static readonly string TriggerAnimPoint = "triggerAnimPoint";

		public static readonly string TriggerEmissionPoint = "triggerEmissionPoint";

		public static readonly string MaterialPart = "mats";

		[Range(0f, 1f)]
		public float DebugPercent;

		public EnemyData data;

		private Animation anim;

		private GameElementSoundPlayer soundEventPlayer;

		private Material[] modelMat;

		private ParticleSystem[] additionParticles;

		private EmissionState currentEmissionState;

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
			currentEmissionState = EmissionState.None;
			anim = GetComponentInChildren<Animation>();
			if ((bool)anim)
			{
				if (audioSource != null)
				{
					soundEventPlayer = anim.gameObject.GetComponent<GameElementSoundPlayer>();
					if (soundEventPlayer == null)
					{
						soundEventPlayer = anim.gameObject.AddComponent<GameElementSoundPlayer>();
						soundEventPlayer.gameElement = this;
						soundEventPlayer.RegistAudioEvent(anim.GetClip("anim01"), data.AudioPlayTime);
					}
				}
				if (data.BaseBallSpeed > 0f)
				{
					float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
					anim["anim01"].speed = speed;
					if (data.IfAutoPlay)
					{
						PlayAnim(anim, true);
					}
				}
				additionParticles = anim.transform.GetComponentsInChildren<ParticleSystem>();
				PlayParticle(additionParticles, false);
			}
			MeshRenderer[] componentsInChildren = base.transform.Find(MaterialPart).GetComponentsInChildren<MeshRenderer>();
			modelMat = new Material[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				modelMat[i] = componentsInChildren[i].sharedMaterial;
			}
			SetEmissionParam(0f);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			OnTriggerStop();
			currentEmissionState = EmissionState.None;
		}

		public override void UpdateElement()
		{
			float num = 0f;
			num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (commonState == CommonState.None)
			{
				if (num >= data.BeginAnimDistance)
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
			if (currentEmissionState == EmissionState.None)
			{
				if (num >= data.BeginEmissionDistance)
				{
					currentEmissionState = EmissionState.Active;
				}
			}
			else if (currentEmissionState == EmissionState.Active)
			{
				float num2 = (num - data.BeginEmissionDistance) / data.EmissionDistance;
				SetEmissionParam(num2);
				if (num2 >= 1f)
				{
					currentEmissionState = EmissionState.End;
				}
			}
			else
			{
				EmissionState currentEmissionState2 = currentEmissionState;
				int num3 = 2;
			}
		}

		public override void OnTriggerPlay()
		{
			if ((bool)anim)
			{
				if (data.IfLoop)
				{
					anim.wrapMode = WrapMode.Loop;
				}
				else
				{
					anim.wrapMode = WrapMode.ClampForever;
				}
				PlayAnim(anim, true);
			}
			PlayParticle(additionParticles, true);
			PlayParticle();
		}

		public override void OnTriggerStop()
		{
			PlayAnim(anim, false);
			PlayParticle(additionParticles, false);
			StopParticle();
			SetEmissionParam(0f);
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
			base.transform.Find(TriggerAnimPoint).position = data.BeginAnimPos;
			base.transform.Find(TriggerEmissionPoint).position = data.BeginEmissionPos;
		}

		public override string Write()
		{
			Transform transform = base.transform.Find(TriggerAnimPoint);
			data.BeginAnimPos = transform.position;
			data.BeginAnimDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform2 = base.transform.Find(TriggerEmissionPoint);
			data.BeginEmissionPos = transform2.position;
			data.BeginEmissionDistance = base.transform.parent.InverseTransformPoint(transform2.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
			base.transform.Find(TriggerAnimPoint).position = data.BeginAnimPos;
			base.transform.Find(TriggerEmissionPoint).position = data.BeginEmissionPos;
		}

		public override byte[] WriteBytes()
		{
			Transform transform = base.transform.Find(TriggerAnimPoint);
			data.BeginAnimPos = transform.position;
			data.BeginAnimDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform2 = base.transform.Find(TriggerEmissionPoint);
			data.BeginEmissionPos = transform2.position;
			data.BeginEmissionDistance = base.transform.parent.InverseTransformPoint(transform2.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			return StructTranslatorUtility.ToByteArray(data);
		}

		private void SetEmissionParam(float percent)
		{
			Color value = Color.Lerp(data.SourceColor, data.TargetColor, percent);
			float value2 = Mathf.Lerp(data.SourceEmission, data.TargetEmission, percent);
			if (modelMat != null)
			{
				int num = modelMat.Length;
				for (int i = 0; i < num; i++)
				{
					Material obj = modelMat[i];
					obj.SetColor(EmissionColorName, value);
					obj.SetFloat(EmissionValName, value2);
				}
			}
		}
	}
}
