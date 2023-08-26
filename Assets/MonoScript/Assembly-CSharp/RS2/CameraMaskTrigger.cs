using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CameraMaskTrigger : BaseTriggerBox, IRebirth
	{
		[Serializable]
		public struct ElementData : IReadWriteBytes
		{
			public float m_begin;

			public float m_end;

			public float m_alphaDistance;

			public string m_alphaFiledName;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_begin = bytes.GetSingle(ref startIndex);
				m_end = bytes.GetSingle(ref startIndex);
				m_alphaDistance = bytes.GetSingle(ref startIndex);
				m_alphaFiledName = bytes.GetStringWithSize(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_begin.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_end.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_alphaDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_alphaFiledName.GetBytesWithSize(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public struct RebirthData
		{
			public float m_distance;
		}

		public ElementData m_data;

		public float m_distance;

		[HideInInspector]
		public GameObject m_background;

		[HideInInspector]
		public Transform m_backgroundTransform;

		[HideInInspector]
		public Renderer m_backgroundRenderer;

		[HideInInspector]
		public Camera m_mainCamera;

		[HideInInspector]
		public float m_backgroundTransfromPointZ = 0.5f;

		private bool m_isTrigger;

		private bool m_isFinished;

		private float m_toShowProgress;

		private float m_toHideProgress;

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			if (m_background == null)
			{
				m_background = dictionary["background"];
			}
			if (m_backgroundRenderer == null && m_background != null)
			{
				m_backgroundRenderer = m_background.GetComponent<Renderer>();
			}
			if (m_backgroundTransform == null && m_background != null)
			{
				m_backgroundTransform = m_background.transform.parent;
			}
			if (m_mainCamera == null)
			{
				m_mainCamera = Camera.main;
			}
			BackgroundToCamera();
			SetRendererFloat(0f);
			m_isTrigger = false;
			m_isFinished = false;
		}

		public override void UpdateElement()
		{
			base.UpdateElement();
			m_distance = BaseRole.theBall.transform.position.z - base.transform.position.z;
			UpdateDistance();
		}

		private void UpdateDistance()
		{
			if (m_distance >= m_data.m_begin && m_distance <= m_data.m_end)
			{
				if (!m_isTrigger)
				{
					BackgroundToCamera();
					m_isTrigger = true;
				}
				m_toShowProgress = (m_distance - m_data.m_begin) / m_data.m_alphaDistance;
				m_toHideProgress = (m_data.m_end - m_distance) / m_data.m_alphaDistance;
				if (m_toShowProgress >= 0f && m_toShowProgress <= 1f)
				{
					SetRendererFloat(m_toShowProgress);
				}
				if (m_toHideProgress >= 0f && m_toHideProgress <= 1f)
				{
					SetRendererFloat(m_toHideProgress);
				}
				if (m_toShowProgress >= 1f && m_toHideProgress >= 1f)
				{
					SetRendererFloat(1f);
				}
			}
			else if (m_distance > m_data.m_end && !m_isFinished)
			{
				BackgroundRefresh();
				m_isFinished = true;
			}
		}

		private void SetRendererFloat(float value)
		{
			if (m_backgroundRenderer != null)
			{
				MaterialTool.SetMaterialFloat(m_backgroundRenderer, m_data.m_alphaFiledName, value);
			}
		}

		private void BackgroundToCamera()
		{
			if (m_background != null)
			{
				Vector3 position = m_mainCamera.transform.TransformPoint(new Vector3(0f, 0f, m_backgroundTransfromPointZ));
				Vector3 vector = m_mainCamera.WorldToViewportPoint(position);
				Vector3 position2 = m_mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, vector.z));
				Vector3 position3 = m_mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, vector.z));
				Vector3 vector2 = m_mainCamera.transform.InverseTransformPoint(position3);
				Vector3 vector3 = m_mainCamera.transform.InverseTransformPoint(position2);
				float x = vector2.x - vector3.x;
				float y = vector2.y - vector3.y;
				m_background.transform.parent = m_mainCamera.transform;
				m_background.transform.position = position;
				m_background.transform.localRotation = Quaternion.identity;
				m_background.transform.localScale = new Vector3(x, y, 1f);
			}
		}

		private void BackgroundRefresh()
		{
			if (m_background != null)
			{
				if (m_backgroundTransform != null)
				{
					m_background.transform.parent = m_backgroundTransform;
					m_background.transform.localPosition = Vector3.zero;
					m_background.transform.localRotation = Quaternion.identity;
					m_background.transform.localScale = Vector3.one;
				}
				SetRendererFloat(0f);
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			m_isTrigger = false;
			m_isFinished = false;
			BackgroundRefresh();
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (ElementData)objs[0];
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<ElementData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(m_data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<ElementData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		public bool IsRecordRebirth()
		{
			return true;
		}

		public object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			return 0;
		}

		public void ResetBySavePointData(object obj)
		{
			UpdateElement();
		}

		public void StartRunningForRebirthData(object obj)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_distance = Bson.ToObject<RebirthData>(rd_data).m_distance;
			UpdateDistance();
		}

		public override byte[] RebirthWriteByteData()
		{
			RebirthData rebirthData = default(RebirthData);
			rebirthData.m_distance = m_distance;
			return Bson.ToBson(rebirthData);
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_distance = JsonUtility.FromJson<RebirthData>((string)rd_data).m_distance;
			UpdateDistance();
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			RebirthData rebirthData = default(RebirthData);
			rebirthData.m_distance = m_distance;
			return JsonUtility.ToJson(rebirthData);
		}
	}
}
