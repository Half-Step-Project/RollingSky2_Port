using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CameraTransferTrigger : BaseTriggerBox
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

		private List<Material> m_modelMats = new List<Material>();

		private List<Color> m_defaultColors = new List<Color>();

		private Vector3 localCollidePos;

		private Color defaultColor;

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
			if (modelPart == null)
			{
				modelPart = base.transform.Find("model");
				Renderer[] componentsInChildren = modelPart.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
					{
						if (componentsInChildren[i].materials[j] != null)
						{
							m_modelMats.Add(componentsInChildren[i].materials[j]);
							m_defaultColors.Add(componentsInChildren[i].materials[j].color);
						}
					}
				}
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
			for (int i = 0; i < m_modelMats.Count; i++)
			{
				Color color = m_defaultColors[i];
				color.a = percent * m_defaultColors[i].a;
				m_modelMats[i].color = color;
			}
		}
	}
}
