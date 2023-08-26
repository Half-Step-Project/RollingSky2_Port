using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class MagicCubeTile3x3 : BaseTile
	{
		public enum ClockDir
		{
			eClockWise,
			eAntiClockWise
		}

		[Serializable]
		public struct TileData : IReadWriteBytes
		{
			public float RotateAngle;

			public float RotateSpeed;

			public ClockDir RotateDir;

			public float BeginDistance;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				RotateAngle = bytes.GetSingle(ref startIndex);
				RotateSpeed = bytes.GetSingle(ref startIndex);
				RotateDir = (ClockDir)bytes.GetInt32(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(RotateAngle.GetBytes(), ref offset);
					memoryStream.WriteByteArray(RotateSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(((int)RotateDir).GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TileData data;

		public override float TileWidth
		{
			get
			{
				return 1.61f;
			}
		}

		public override float TileMagin
		{
			get
			{
				return 1.5f;
			}
		}

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
			commonState = CommonState.None;
			base.transform.localEulerAngles = StartLocalEuler;
		}

		public override void UpdateElement()
		{
			float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance;
			if (commonState == CommonState.None)
			{
				return;
			}
			if (commonState == CommonState.Active)
			{
				float percent = GetPercent(distance);
				PlayByPercent(percent);
				if (percent >= 1f)
				{
					commonState = CommonState.End;
				}
			}
			else
			{
				CommonState commonState2 = commonState;
				int num = 5;
			}
		}

		public override float GetPercent(float distance)
		{
			return Mathf.Min(1f, Mathf.Max(-1f, distance * data.RotateSpeed));
		}

		public override void PlayByPercent(float percent)
		{
			if (!(percent < 0f))
			{
				int num = ((data.RotateDir != 0) ? 1 : (-1));
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				localEulerAngles.z = (float)num * percent * data.RotateAngle;
				base.transform.localEulerAngles = localEulerAngles;
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			base.TriggerEnter(ball);
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TileData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}

		public void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<MagicBoxCallEventArgs>.EventId, OnTriggerCall);
		}

		public void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<MagicBoxCallEventArgs>.EventId, OnTriggerCall);
		}

		private void OnTriggerCall(object sender, Foundation.EventArgs e)
		{
			MagicBoxCallEventArgs magicBoxCallEventArgs = e as MagicBoxCallEventArgs;
			if (magicBoxCallEventArgs == null)
			{
				return;
			}
			MagicBoxData magicBoxInfo = magicBoxCallEventArgs.MagicBoxInfo;
			if (commonState == CommonState.None)
			{
				int num = point.m_y - magicBoxInfo.TriggerPoint.m_y;
				if (magicBoxInfo.GridTrans == base.groupTransform && num >= 0 && (float)num <= magicBoxInfo.ValidDistance)
				{
					commonState = CommonState.Active;
					data.RotateAngle = magicBoxInfo.RotateAngle;
					data.RotateSpeed = magicBoxInfo.RotateSpeed;
				}
			}
		}
	}
}
