using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class BlackSandTrigger : BaseTriggerBox
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
				anim = base.transform.Find("model/BlackSand_Dead_2").GetComponent<Animation>();
				anim.gameObject.SetActive(false);
			}
			Mod.Event.Subscribe(EventArgs<DropDieEventArgs>.EventId, OnTreeKillDropDie);
		}

		public override void ResetElement()
		{
			if ((bool)anim)
			{
				anim.gameObject.SetActive(true);
				anim.Play();
				anim["XiaoWangZi_BlackSand_Dead"].normalizedTime = 0f;
				anim.Sample();
				anim.Stop();
				anim.gameObject.SetActive(false);
				base.transform.position = StartPos;
			}
			Mod.Event.Unsubscribe(EventArgs<DropDieEventArgs>.EventId, OnTreeKillDropDie);
		}

		private void OnTreeKillDropDie(object sender, Foundation.EventArgs e)
		{
			DropDieEventArgs dropDieEventArgs = e as DropDieEventArgs;
			if (dropDieEventArgs != null)
			{
				Vector3 diePosition = dropDieEventArgs.DiePosition;
				if ((bool)anim && base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z <= data.ValidDistance)
				{
					base.transform.position = diePosition;
					anim.gameObject.SetActive(true);
					anim["XiaoWangZi_BlackSand_Dead"].normalizedTime = 0f;
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

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Color color = Gizmos.color;
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(base.transform.position, 0.5f);
			Gizmos.color = color;
		}
	}
}
