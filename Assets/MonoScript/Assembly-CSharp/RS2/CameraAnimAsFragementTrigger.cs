using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;
using User.TileMap;

namespace RS2
{
	public class CameraAnimAsFragementTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public string m_triggerPath;

			public float m_beginDistance;

			public float m_resetDistance;

			public OperatorType m_operatorType;

			public int m_needFragmentCount;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_triggerPath = bytes.GetStringWithSize(ref startIndex);
				m_beginDistance = bytes.GetSingle(ref startIndex);
				m_resetDistance = bytes.GetSingle(ref startIndex);
				m_operatorType = (OperatorType)bytes.GetInt32(ref startIndex);
				m_needFragmentCount = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_triggerPath.GetBytesWithSize(), ref offset);
					memoryStream.WriteByteArray(m_beginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_resetDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(((int)m_operatorType).GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_needFragmentCount.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string BASE_PATH = "Assets/_RS2Art/Res/Brush/Related/Animations/";

		[HideInInspector]
		public AnimationClip m_Animation;

		public TriggerData m_data;

		private int m_currentFragment;

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
			m_currentFragment = 0;
			if (m_Animation == null)
			{
				string path = string.Format("{0}{1}.anim", BASE_PATH, m_data.m_triggerPath);
				m_Animation = MapController.Instance.GetRelatedAnimationClipByPath(path);
			}
		}

		public override void UpdateElement()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (commonState == CommonState.None)
			{
				if (num >= m_data.m_beginDistance)
				{
					OnTriggerPlay();
					commonState = CommonState.Active;
				}
			}
			else if (commonState == CommonState.Active && num >= m_data.m_resetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if (m_Animation != null)
			{
				CameraController.theCamera.TriggerRemoveAnimClip(m_Animation, m_Animation.name);
			}
			commonState = CommonState.None;
			StopListeningForCollectionEvents();
		}

		public override void OnTriggerPlay()
		{
			base.OnTriggerPlay();
			Mod.Event.Subscribe(EventArgs<GainedDropEventArgs>.EventId, OnCollectEventCall);
		}

		public override void OnTriggerStop()
		{
			base.OnTriggerStop();
			StopListeningForCollectionEvents();
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState != CommonState.Active)
			{
				return;
			}
			bool flag = false;
			switch (m_data.m_operatorType)
			{
			case OperatorType.GreaterThanOrEqual:
				flag = m_currentFragment >= m_data.m_needFragmentCount;
				break;
			case OperatorType.LessThanOrEqual:
				flag = m_currentFragment <= m_data.m_needFragmentCount;
				break;
			case OperatorType.GreaterThan:
				flag = m_currentFragment > m_data.m_needFragmentCount;
				break;
			case OperatorType.Equal:
				flag = m_currentFragment == m_data.m_needFragmentCount;
				break;
			case OperatorType.LessThan:
				flag = m_currentFragment < m_data.m_needFragmentCount;
				break;
			}
			if (flag)
			{
				if (m_Animation != null)
				{
					CameraController.theCamera.TriggerPlayAnimClip(m_Animation, m_Animation.name);
				}
				else
				{
					Debug.LogError("wyj=CameraAnimTrigger has no anim!!!==" + base.name);
				}
			}
			StopListeningForCollectionEvents();
		}

		private void OnCollectEventCall(object sender, Foundation.EventArgs e)
		{
			GainedDropEventArgs gainedDropEventArgs = e as GainedDropEventArgs;
			if (gainedDropEventArgs != null && gainedDropEventArgs.m_dropData.m_type == DropType.TRIGGERFRAGMENT)
			{
				DealCollectEventCallbacks();
			}
		}

		private void DealCollectEventCallbacks()
		{
			m_currentFragment++;
		}

		private void StopListeningForCollectionEvents()
		{
			Mod.Event.Unsubscribe(EventArgs<GainedDropEventArgs>.EventId, OnCollectEventCall);
			m_currentFragment = 0;
			commonState = CommonState.End;
		}

		public override string Write()
		{
			return JsonUtility.ToJson(m_data);
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<TriggerData>(info);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (TriggerData)objs[0];
		}

		public override void AppendRelatedInfo(ref List<RelatedAssetData> relatedAssetList)
		{
			RelatedAssetData item = new RelatedAssetData(string.Format("{0}{1}.anim", BASE_PATH, m_data.m_triggerPath));
			relatedAssetList.Add(item);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Vector3 position = base.gameObject.transform.position;
			Vector3 from = base.gameObject.transform.TransformPoint(new Vector3(0f, 0f, m_data.m_beginDistance));
			Gizmos.color = Color.red;
			Gizmos.DrawLine(from, position);
			Gizmos.color = Color.white;
		}
	}
}
