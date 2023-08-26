using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class TreeKillTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float ValidDistance;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ValidDistance = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return ValidDistance.GetBytes();
			}
		}

		public Animation anim;

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
			if (anim == null)
			{
				anim = GetComponentInChildren<Animation>();
				anim.gameObject.SetActive(false);
			}
			Mod.Event.Subscribe(EventArgs<GameFailEventArgs>.EventId, OnTreeKillDropDie);
		}

		public override void ResetElement()
		{
			if ((bool)anim)
			{
				anim.gameObject.SetActive(true);
				anim.Play();
				anim["anim01"].normalizedTime = 0f;
				anim.Sample();
				anim.Stop();
				anim.gameObject.SetActive(false);
				base.transform.position = StartPos;
			}
			Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnTreeKillDropDie);
		}

		private void OnTreeKillDropDie(object sender, Foundation.EventArgs e)
		{
			GameFailEventArgs gameFailEventArgs = e as GameFailEventArgs;
			if (gameFailEventArgs == null || gameFailEventArgs.FailType != GameFailEventArgs.GameFailType.Special)
			{
				return;
			}
			Vector3 diePosition = gameFailEventArgs.DiePosition;
			if ((bool)anim)
			{
				float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
				if (num > 0f && num <= data.ValidDistance)
				{
					base.transform.position = diePosition;
					anim.gameObject.SetActive(true);
					anim["anim01"].normalizedTime = 0f;
					anim.Play();
					BaseRole.theBall.SetSpecialDieFlag(true, true);
				}
			}
		}

		private void OnDestroy()
		{
			Mod.Event.Unsubscribe(EventArgs<GameFailEventArgs>.EventId, OnTreeKillDropDie);
		}

		public override void Read(string info)
		{
			if (!string.IsNullOrEmpty(info))
			{
				data = JsonUtility.FromJson<TriggerData>(info);
			}
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override void SetDefaultValue(object[] objs)
		{
			data.ValidDistance = (float)objs[0];
		}
	}
}
