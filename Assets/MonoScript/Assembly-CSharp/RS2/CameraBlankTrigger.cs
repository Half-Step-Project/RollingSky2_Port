using System;
using System.IO;
using Foundation;
using UnityEngine;
using UnityExpansion;

namespace RS2
{
	public class CameraBlankTrigger : BaseTriggerBox
	{
		public enum TriggerState
		{
			None,
			TransparentToBlank,
			Blank,
			BlankToTransparent,
			Transparent,
			InActive
		}

		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public bool IfOpenBlank;

			public float BlankSpeed;

			public Vector3 DeltaPos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				IfOpenBlank = bytes.GetBoolean(ref startIndex);
				BlankSpeed = bytes.GetSingle(ref startIndex);
				DeltaPos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(IfOpenBlank.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BlankSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(DeltaPos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TriggerData data;

		private TriggerState currentState;

		private Transform modelPart;

		private Material modelMat;

		private Vector3 localCollidePos;

		private Color defaultColor;

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<GameCameraCutEventArgs>.EventId, OnGameCameraCut);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<GameCameraCutEventArgs>.EventId, OnGameCameraCut);
		}

		public override void SetDefaultValue(object[] objs)
		{
			data.BlankSpeed = 1f;
		}

		public override void Initialize()
		{
			base.Initialize();
			if (modelMat == null)
			{
				modelPart = base.transform.Find("model");
				modelMat = modelPart.GetComponentInChildren<MeshRenderer>().material;
				defaultColor = modelMat.color;
			}
			localCollidePos = Vector3.zero;
			currentState = TriggerState.None;
			ResetModelPart();
		}

		public override void ResetElement()
		{
			base.ResetElement();
			currentState = TriggerState.None;
			localCollidePos = Vector3.zero;
			ResetModelPart();
		}

		public override void UpdateElement()
		{
			if (!data.IfOpenBlank)
			{
				return;
			}
			if (currentState == TriggerState.TransparentToBlank)
			{
				FollowCameraMove();
				float transparentToBlankPercent = GetTransparentToBlankPercent();
				SetAlpha(transparentToBlankPercent);
				if (transparentToBlankPercent >= 1f)
				{
					currentState = TriggerState.Blank;
				}
			}
			else if (currentState == TriggerState.Blank)
			{
				FollowCameraMove();
			}
			else if (currentState == TriggerState.BlankToTransparent)
			{
				FollowCameraMove();
				float blankToTransparentPercent = GetBlankToTransparentPercent();
				SetAlpha(1f - blankToTransparentPercent);
				if (blankToTransparentPercent >= 1f)
				{
					currentState = TriggerState.Transparent;
					ResetModelPart();
				}
			}
			else
			{
				TriggerState currentState2 = currentState;
				int num = 4;
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (data.IfOpenBlank)
			{
				if (currentState == TriggerState.None)
				{
					modelPart.gameObject.SetActive(true);
					currentState = TriggerState.TransparentToBlank;
				}
			}
			else if (currentState == TriggerState.None)
			{
				Mod.Event.Fire(this, Mod.Reference.Acquire<GameCameraCutEventArgs>().Initialize(data.IfOpenBlank, base.transform.position));
				currentState = TriggerState.InActive;
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
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

		private void OnGameCameraCut(object sender, Foundation.EventArgs e)
		{
			if ((currentState == TriggerState.Blank || currentState == TriggerState.TransparentToBlank) && sender as CameraBlankTrigger != this)
			{
				GameCameraCutEventArgs gameCameraCutEventArgs = e as GameCameraCutEventArgs;
				if (gameCameraCutEventArgs != null && !gameCameraCutEventArgs.IfOpenBlank)
				{
					localCollidePos = base.transform.parent.InverseTransformPoint(gameCameraCutEventArgs.CollidePosition);
					currentState = TriggerState.BlankToTransparent;
				}
			}
		}

		private void ResetModelPart()
		{
			modelPart.localPosition = Vector3.zero;
			modelPart.localEulerAngles = Vector3.zero;
			SetAlpha(0f);
			modelPart.gameObject.SetActive(false);
		}

		private void FollowCameraMove()
		{
			Transform camera = CameraController.theCamera.m_Camera;
			modelPart.eulerAngles = camera.eulerAngles;
			modelPart.position = camera.position + data.DeltaPos;
		}

		private float GetTransparentToBlankPercent()
		{
			if (data.BlankSpeed == 0f)
			{
				return 0f;
			}
			return (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.transform.localPosition.z) / data.BlankSpeed;
		}

		private float GetBlankToTransparentPercent()
		{
			if (data.BlankSpeed == 0f)
			{
				return 0f;
			}
			return (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - localCollidePos.z) / data.BlankSpeed;
		}

		private void SetAlpha(float percent)
		{
			percent = Mathf.Clamp(percent, 0f, 1f);
			if ((bool)modelMat)
			{
				Color color = defaultColor;
				color.a = percent * defaultColor.a;
				modelMat.color = color;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_CameraBlankTrigger_DATA rD_CameraBlankTrigger_DATA = JsonUtility.FromJson<RD_CameraBlankTrigger_DATA>(rd_data as string);
			currentState = rD_CameraBlankTrigger_DATA.currentState;
			modelPart.SetTransData(rD_CameraBlankTrigger_DATA.modelPart);
			localCollidePos = rD_CameraBlankTrigger_DATA.localCollidePos.ToVector3();
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_CameraBlankTrigger_DATA
			{
				currentState = currentState,
				modelPart = modelPart.GetTransData(),
				localCollidePos = localCollidePos.ToMyVector3()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_CameraBlankTrigger_DATA rD_CameraBlankTrigger_DATA = Bson.ToObject<RD_CameraBlankTrigger_DATA>(rd_data);
			currentState = rD_CameraBlankTrigger_DATA.currentState;
			modelPart.SetTransData(rD_CameraBlankTrigger_DATA.modelPart);
			localCollidePos = rD_CameraBlankTrigger_DATA.localCollidePos.ToVector3();
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_CameraBlankTrigger_DATA
			{
				currentState = currentState,
				modelPart = modelPart.GetTransData(),
				localCollidePos = localCollidePos.ToMyVector3()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
