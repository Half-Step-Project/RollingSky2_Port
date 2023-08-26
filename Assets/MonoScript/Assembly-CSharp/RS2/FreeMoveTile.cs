using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class FreeMoveTile : BaseTile
	{
		[Serializable]
		public struct TileData : IReadWriteBytes
		{
			public Vector3 BeginPos;

			public Vector3 BeginEularAngle;

			public Vector3 EndPos;

			public Vector3 EndEularAngle;

			public float BeginDistance;

			public float SpeedScaler;

			public float MoveScaler;

			public bool IfTrigger;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginPos = bytes.GetVector3(ref startIndex);
				BeginEularAngle = bytes.GetVector3(ref startIndex);
				EndPos = bytes.GetVector3(ref startIndex);
				EndEularAngle = bytes.GetVector3(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
				SpeedScaler = bytes.GetSingle(ref startIndex);
				MoveScaler = bytes.GetSingle(ref startIndex);
				IfTrigger = bytes.GetBoolean(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginEularAngle.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndEularAngle.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
					memoryStream.WriteByteArray(MoveScaler.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfTrigger.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string AnimName1 = "anim01";

		public static readonly string AnimName2 = "anim02";

		public TileData data;

		private Transform beginPoint;

		private Transform endPoint;

		private Animation anim;

		private bool ifPlayAnim;

		protected Transform effectChild;

		protected ParticleSystem[] particles;

		private float distance;

		private RD_FreeMoveTile_DATA cacheData;

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			data.BeginDistance = (float)objs[0];
			data.SpeedScaler = (float)objs[1];
			data.MoveScaler = (float)objs[2];
		}

		public override void Initialize()
		{
			base.Initialize();
			if (beginPoint == null)
			{
				beginPoint = base.transform.Find("path/beginPoint");
			}
			if (endPoint == null)
			{
				endPoint = base.transform.Find("path/endPoint");
			}
			beginPoint.position = data.BeginPos;
			beginPoint.eulerAngles = data.BeginEularAngle;
			endPoint.position = data.EndPos;
			endPoint.eulerAngles = data.EndEularAngle;
			base.transform.position = beginPoint.position;
			commonState = CommonState.None;
			anim = GetComponentInChildren<Animation>();
			if ((bool)anim && data.SpeedScaler > 0f)
			{
				float speedScaler = data.SpeedScaler;
				if ((bool)anim.GetClip(AnimName1))
				{
					anim[AnimName1].speed = speedScaler;
				}
				if ((bool)anim.GetClip(AnimName2))
				{
					anim[AnimName2].speed = speedScaler;
				}
			}
			if (effectChild == null)
			{
				effectChild = base.transform.Find("effect");
				if (effectChild == null)
				{
					effectChild = base.transform.Find("model/effect");
				}
				if ((bool)effectChild)
				{
					particles = effectChild.GetComponentsInChildren<ParticleSystem>();
					PlayParticle(particles, false);
				}
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			PlayAnim(anim, false);
			PlayParticle(particles, false);
			ifPlayAnim = false;
		}

		public override void UpdateElement()
		{
			distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(data.EndPos).z + data.BeginDistance;
			SwitchStateByDistance(distance);
		}

		private void SwitchStateByDistance(float distance)
		{
			if (commonState == CommonState.None)
			{
				if (data.IfTrigger)
				{
					return;
				}
				PlayByPercent(GetPercent(distance));
				if (GetPercent(distance) > 0f && !ifPlayAnim)
				{
					PlayAnim(anim, AnimName1, true);
					if (!data.IfTrigger)
					{
						PlayParticle(particles, true);
					}
					ifPlayAnim = true;
				}
			}
			else if (commonState == CommonState.Active)
			{
				float percent = GetPercent(distance);
				PlayByPercent(percent);
				if (percent > 0f && !ifPlayAnim)
				{
					PlayAnim(anim, AnimName1, true);
					if (!data.IfTrigger)
					{
						PlayParticle(particles, true);
					}
					ifPlayAnim = true;
				}
				if (percent >= 1f)
				{
					commonState = CommonState.InActive;
				}
			}
			else
			{
				CommonState commonState2 = commonState;
				int num = 5;
			}
		}

		protected override void OnCollideBall(BaseRole ball)
		{
			base.OnCollideBall(ball);
			if (commonState == CommonState.InActive)
			{
				PlayAnim(anim, AnimName2, true);
				commonState = CommonState.None;
			}
		}

		public override float GetPercent(float distance)
		{
			return Mathf.Min(1f, Mathf.Max(0f, distance * data.MoveScaler));
		}

		public override void PlayByPercent(float percent)
		{
			base.transform.position = Vector3.Lerp(data.BeginPos, data.EndPos, percent);
			base.transform.eulerAngles = Vector3.Lerp(data.BeginEularAngle, data.EndEularAngle, percent);
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TileData>(info);
			if (beginPoint == null)
			{
				beginPoint = base.transform.Find("path/beginPoint");
			}
			if (endPoint == null)
			{
				endPoint = base.transform.Find("path/endPoint");
			}
			beginPoint.position = data.BeginPos;
			beginPoint.eulerAngles = data.BeginEularAngle;
			endPoint.position = data.EndPos;
			endPoint.eulerAngles = data.EndEularAngle;
		}

		public override string Write()
		{
			if (beginPoint == null)
			{
				beginPoint = base.transform.Find("path/beginPoint");
			}
			if (endPoint == null)
			{
				endPoint = base.transform.Find("path/endPoint");
			}
			data.BeginPos = beginPoint.position;
			data.BeginEularAngle = beginPoint.eulerAngles;
			data.EndPos = endPoint.position;
			data.EndEularAngle = endPoint.eulerAngles;
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TileData>(bytes);
			if (beginPoint == null)
			{
				beginPoint = base.transform.Find("path/beginPoint");
			}
			if (endPoint == null)
			{
				endPoint = base.transform.Find("path/endPoint");
			}
			beginPoint.position = data.BeginPos;
			beginPoint.eulerAngles = data.BeginEularAngle;
			endPoint.position = data.EndPos;
			endPoint.eulerAngles = data.EndEularAngle;
		}

		public override byte[] WriteBytes()
		{
			if (beginPoint == null)
			{
				beginPoint = base.transform.Find("path/beginPoint");
			}
			if (endPoint == null)
			{
				endPoint = base.transform.Find("path/endPoint");
			}
			data.BeginPos = beginPoint.position;
			data.BeginEularAngle = beginPoint.eulerAngles;
			data.EndPos = endPoint.position;
			data.EndEularAngle = endPoint.eulerAngles;
			return StructTranslatorUtility.ToByteArray(data);
		}

		public void OnDrawGizmos()
		{
			if (beginPoint == null)
			{
				beginPoint = base.transform.Find("path/beginPoint");
			}
			if (endPoint == null)
			{
				endPoint = base.transform.Find("path/endPoint");
			}
			Color color = Gizmos.color;
			Gizmos.color = Color.green;
			Gizmos.matrix = Matrix4x4.TRS(beginPoint.position, beginPoint.rotation, new Vector3(1f, 0.2f, 1f));
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = Matrix4x4.identity;
			Gizmos.matrix = Matrix4x4.TRS(endPoint.position, endPoint.rotation, new Vector3(1f, 0.2f, 1f));
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = Matrix4x4.identity;
			Gizmos.color = color;
		}

		public void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<TileFreeMoveCallEventArgs>.EventId, OnTileFreeMoveCall);
		}

		public void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<TileFreeMoveCallEventArgs>.EventId, OnTileFreeMoveCall);
		}

		private void OnTileFreeMoveCall(object sender, Foundation.EventArgs e)
		{
			TileFreeMoveCallEventArgs tileFreeMoveCallEventArgs = e as TileFreeMoveCallEventArgs;
			if (tileFreeMoveCallEventArgs == null || !data.IfTrigger || commonState != 0)
			{
				return;
			}
			FreeMoveData tileMoveData = tileFreeMoveCallEventArgs.TileMoveData;
			if (tileMoveData.IfTriggerControl)
			{
				data.BeginDistance = tileMoveData.BeginDistance;
				data.SpeedScaler = tileMoveData.SpeedScaler;
				data.MoveScaler = tileMoveData.MoveScaler;
			}
			float num = tileMoveData.GridTrans.InverseTransformPoint(base.transform.position).z - tileMoveData.GridTrans.InverseTransformPoint(tileMoveData.TriggerPos).z;
			if (num > 0f && num <= tileMoveData.ValidDistance)
			{
				commonState = CommonState.Active;
				if (data.IfTrigger)
				{
					PlayParticle(particles, true);
				}
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_FreeMoveTile_DATA rD_FreeMoveTile_DATA = JsonUtility.FromJson<RD_FreeMoveTile_DATA>(rd_data as string);
			ifPlayAnim = rD_FreeMoveTile_DATA.ifPlayAnim;
			commonState = rD_FreeMoveTile_DATA.commonState;
			distance = rD_FreeMoveTile_DATA.distance;
			SwitchStateByDistance(distance);
			anim.SetAnimData(rD_FreeMoveTile_DATA.anim, ProcessState.Pause);
			particles.SetParticlesData(rD_FreeMoveTile_DATA.particles, ProcessState.Pause);
			cacheData = rD_FreeMoveTile_DATA;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_FreeMoveTile_DATA
			{
				ifPlayAnim = ifPlayAnim,
				commonState = commonState,
				distance = distance,
				anim = anim.GetAnimData(),
				particles = particles.GetParticlesData()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			if (cacheData.ifPlayAnim)
			{
				anim.SetAnimData(cacheData.anim, ProcessState.UnPause);
			}
			particles.SetParticlesData(cacheData.particles, ProcessState.UnPause);
			cacheData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_FreeMoveTile_DATA rD_FreeMoveTile_DATA = Bson.ToObject<RD_FreeMoveTile_DATA>(rd_data);
			ifPlayAnim = rD_FreeMoveTile_DATA.ifPlayAnim;
			commonState = rD_FreeMoveTile_DATA.commonState;
			distance = rD_FreeMoveTile_DATA.distance;
			SwitchStateByDistance(distance);
			anim.SetAnimData(rD_FreeMoveTile_DATA.anim, ProcessState.Pause);
			particles.SetParticlesData(rD_FreeMoveTile_DATA.particles, ProcessState.Pause);
			cacheData = rD_FreeMoveTile_DATA;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_FreeMoveTile_DATA
			{
				ifPlayAnim = ifPlayAnim,
				commonState = commonState,
				distance = distance,
				anim = anim.GetAnimData(),
				particles = particles.GetParticlesData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			if (cacheData.ifPlayAnim)
			{
				anim.SetAnimData(cacheData.anim, ProcessState.UnPause);
			}
			particles.SetParticlesData(cacheData.particles, ProcessState.UnPause);
			cacheData = null;
		}
	}
}
