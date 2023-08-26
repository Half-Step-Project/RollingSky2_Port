using System;
using System.IO;
using Foundation;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

namespace RS2
{
	public class PathToMoveByRoleAsFragementTrigger : BaseTriggerBox, IPathRebirth
	{
		[Serializable]
		public struct PathToMoveByRoleAsFragementTriggerData : IReadWriteBytes
		{
			public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData m_pathData;

			public float m_beginDistance;

			public float m_resetDistance;

			[Header("GreaterThanOrEqual(大于等于) LessThanOrEqual(小于等于) GreaterThan(大于) Equal（等于） LessThan（小于）")]
			public OperatorType m_operatorType;

			public int m_needFragmentCount;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_beginDistance = bytes.GetSingle(ref startIndex);
				m_resetDistance = bytes.GetSingle(ref startIndex);
				m_operatorType = (OperatorType)bytes.GetInt32(ref startIndex);
				m_needFragmentCount = bytes.GetInt32(ref startIndex);
				m_pathData = default(PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData);
				byte[] array = new byte[bytes.Length - startIndex];
				Array.Copy(bytes, startIndex, array, 0, array.Length);
				m_pathData.ReadBytes(array);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_beginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_resetDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(((int)m_operatorType).GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_needFragmentCount.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_pathData.WriteBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public PathToMoveByRoleAsFragementTriggerData m_data;

		private int m_currentFragment;

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
			commonState = CommonState.None;
			m_currentFragment = 0;
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
			if (commonState == CommonState.Active)
			{
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
					PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData pathData = GetPathData();
					ball.CallChangeToPathToMove(pathData, m_uuId);
				}
				StopListeningForCollectionEvents();
			}
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
			if (m_data.m_pathData.m_smooth <= 0)
			{
				Debug.LogError("PathToMoveByRoleAsFragementTrigger.m_pathData.smooth <=0");
			}
			Grid componentInParent = base.transform.GetComponentInParent<Grid>();
			if (componentInParent != null)
			{
				Vector3[] array = new Vector3[m_data.m_pathData.m_positions.Length];
				for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
				{
					Vector3 position = base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
					array[i] = componentInParent.transform.InverseTransformPoint(position);
				}
				Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_pathData.m_smooth);
				if (pathByPositions.Length < 500)
				{
					m_data.m_pathData.m_bezierPositions = pathByPositions;
				}
			}
			return JsonUtility.ToJson(m_data);
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<PathToMoveByRoleAsFragementTriggerData>(info);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<PathToMoveByRoleAsFragementTriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (PathToMoveByRoleAsFragementTriggerData)objs[0];
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			ThreeBezier.DrawGizmos(base.gameObject, m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
			Vector3 position = base.gameObject.transform.position;
			Vector3 from = base.gameObject.transform.TransformPoint(new Vector3(0f, 0f, m_data.m_beginDistance));
			Gizmos.color = Color.red;
			Gizmos.DrawLine(from, position);
			Gizmos.color = Color.white;
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_PathToMoveByRoleAsFragementTrigger_DATA rD_PathToMoveByRoleAsFragementTrigger_DATA = JsonUtility.FromJson<RD_PathToMoveByRoleAsFragementTrigger_DATA>(rd_data as string);
			m_currentFragment = rD_PathToMoveByRoleAsFragementTrigger_DATA.m_currentFragment;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_PathToMoveByRoleAsFragementTrigger_DATA
			{
				m_currentFragment = m_currentFragment
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_PathToMoveByRoleAsFragementTrigger_DATA rD_PathToMoveByRoleAsFragementTrigger_DATA = Bson.ToObject<RD_PathToMoveByRoleAsFragementTrigger_DATA>(rd_data);
			m_currentFragment = rD_PathToMoveByRoleAsFragementTrigger_DATA.m_currentFragment;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_PathToMoveByRoleAsFragementTrigger_DATA
			{
				m_currentFragment = m_currentFragment
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}

		public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData GetPathData()
		{
			PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData result = m_data.m_pathData.Copy();
			if (m_data.m_pathData.m_bezierPositions == null || m_data.m_pathData.m_bezierPositions.Length == 0)
			{
				for (int i = 0; i < result.m_positions.Length; i++)
				{
					result.m_positions[i] = base.transform.TransformPoint(result.m_positions[i]);
				}
			}
			return result;
		}
	}
}
