using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class AnimFreeCollideTile : BaseTile
	{
		[Serializable]
		public struct TileData : IReadWriteBytes
		{
			public float TileWidth;

			public float TileHeight;

			public bool IfTrigger;

			public float BeginDistance;

			public float ResetDistance;

			public float AnimSpeed;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				TileWidth = bytes.GetSingle(ref startIndex);
				TileHeight = bytes.GetSingle(ref startIndex);
				IfTrigger = bytes.GetBoolean(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
				ResetDistance = bytes.GetSingle(ref startIndex);
				AnimSpeed = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(TileWidth.GetBytes(), ref offset);
					memoryStream.WriteByteArray(TileHeight.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfTrigger.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(AnimSpeed.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		private Animation anim;

		public TileData data;

		private bool ifActive;

		public override float TileWidth
		{
			get
			{
				return data.TileWidth * 0.5f + BaseTile.RecycleWidthTolerance;
			}
		}

		public override float TileHeight
		{
			get
			{
				return data.TileHeight * 0.5f + BaseTile.RecycleHeightTolerance;
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
			if (anim == null)
			{
				anim = GetComponentInChildren<Animation>();
			}
			if ((bool)anim)
			{
				anim["anim01"].speed = data.AnimSpeed;
			}
			ifActive = false;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if ((bool)anim)
			{
				anim.Play();
				anim["anim01"].normalizedTime = 0f;
				anim.Sample();
				anim.Stop();
			}
			ifActive = false;
		}

		public override void UpdateElement()
		{
			float num = 0f;
			num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (data.IfTrigger && !ifActive)
			{
				return;
			}
			if (commonState == CommonState.None)
			{
				if (num >= data.BeginDistance)
				{
					OnTriggerPlay();
					commonState = CommonState.Active;
				}
			}
			else if (commonState == CommonState.Active)
			{
				if (num >= data.ResetDistance)
				{
					ResetElement();
					commonState = CommonState.End;
				}
			}
			else if (num < data.BeginDistance)
			{
				commonState = CommonState.None;
			}
		}

		public override void OnTriggerPlay()
		{
			if ((bool)anim)
			{
				anim["anim01"].normalizedTime = 0f;
				anim.Play();
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			if (objs != null)
			{
				data.TileWidth = (float)objs[0];
				data.TileHeight = (float)objs[1];
				data.AnimSpeed = (float)objs[2];
			}
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
			Mod.Event.Subscribe(EventArgs<AnimFreeCollideCallEventArgs>.EventId, OnTriggerCall);
		}

		public void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<AnimFreeCollideCallEventArgs>.EventId, OnTriggerCall);
		}

		private void OnTriggerCall(object sender, Foundation.EventArgs e)
		{
			AnimFreeCollideCallEventArgs animFreeCollideCallEventArgs = e as AnimFreeCollideCallEventArgs;
			if (animFreeCollideCallEventArgs != null && data.IfTrigger && commonState == CommonState.None && !ifActive)
			{
				FreeCollideData freeCollideInfo = animFreeCollideCallEventArgs.FreeCollideInfo;
				float num = freeCollideInfo.GridTrans.InverseTransformPoint(base.transform.position).z - freeCollideInfo.GridTrans.InverseTransformPoint(freeCollideInfo.TriggerPos).z;
				if (num > 0f && num <= freeCollideInfo.ValidDistance)
				{
					ifActive = true;
					Log.Info("Actived by Trigger");
				}
			}
		}
	}
}
