using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class FollowRotatePlatform : BaseGroup, IBrushTrigger
	{
		[Serializable]
		public struct GroupData : IReadWriteBytes
		{
			public float DeltaX;

			public Vector3 BeginEular;

			public Vector3 EndEular;

			public float BeginDistance;

			public float EndFollowDistance;

			public float EndRotateDistance;

			[HideInInspector]
			public Vector3 BeginPos;

			[HideInInspector]
			public Vector3 EndFollowPos;

			[HideInInspector]
			public Vector3 EndRotatePos;

			[HideInInspector]
			public Bounds CollideBounds;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				DeltaX = bytes.GetSingle(ref startIndex);
				BeginEular = bytes.GetVector3(ref startIndex);
				EndEular = bytes.GetVector3(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
				EndFollowDistance = bytes.GetSingle(ref startIndex);
				EndRotateDistance = bytes.GetSingle(ref startIndex);
				BeginPos = bytes.GetVector3(ref startIndex);
				EndFollowPos = bytes.GetVector3(ref startIndex);
				EndRotatePos = bytes.GetVector3(ref startIndex);
				Vector3 vector = bytes.GetVector3(ref startIndex);
				Vector3 vector2 = bytes.GetVector3(ref startIndex);
				CollideBounds = new Bounds(vector, vector2);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(DeltaX.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginEular.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndEular.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndFollowDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndRotateDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndFollowPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndRotatePos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(CollideBounds.center.GetBytes(), ref offset);
					memoryStream.WriteByteArray(CollideBounds.size.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public GroupData data;

		private Vector3 gridLocalPos;

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
			base.transform.SetPositionX(base.transform.position.x + data.DeltaX);
			gridLocalPos = base.groupTransform.InverseTransformPoint(base.transform.position);
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
		}

		public override void UpdateElement()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - gridLocalPos.z;
			if (commonState == CommonState.None)
			{
				if (num >= data.BeginDistance)
				{
					commonState = CommonState.Active;
				}
			}
			else
			{
				if (commonState == CommonState.End || commonState != CommonState.Active)
				{
					return;
				}
				if (num >= data.EndRotateDistance && num >= data.EndFollowDistance)
				{
					commonState = CommonState.End;
					return;
				}
				if (num <= data.EndRotateDistance)
				{
					UnityExtension.SetPositionZ(z: Railway.theRailway.transform.position.z, transform: base.transform);
				}
				float t = (num - data.BeginDistance) / (data.EndRotateDistance - data.BeginDistance);
				base.transform.eulerAngles = Vector3.Lerp(data.BeginEular, data.EndEular, t);
			}
		}

		public void TriggerEnter(BaseRole ball, Collider collider)
		{
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<GroupData>(info);
			Transform transform = base.transform.Find("affectArea");
			BoxCollider componentInChildren = transform.GetComponentInChildren<BoxCollider>(true);
			componentInChildren.center = data.CollideBounds.center - transform.transform.position;
			componentInChildren.size = data.CollideBounds.size;
			base.transform.Find("triggerBegin").position = data.BeginPos;
			base.transform.Find("triggerEndRotate").position = data.EndRotatePos;
			base.transform.Find("triggerEndFollow").position = data.EndFollowPos;
		}

		public override string Write()
		{
			Transform transform = base.transform.Find("triggerBegin");
			data.BeginPos = transform.position;
			data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform2 = base.transform.Find("triggerEndRotate");
			data.EndRotatePos = transform2.position;
			data.EndRotateDistance = base.transform.parent.InverseTransformPoint(transform2.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform3 = base.transform.Find("triggerEndFollow");
			data.EndFollowPos = transform3.position;
			data.EndFollowDistance = base.transform.parent.InverseTransformPoint(transform3.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			BoxCollider componentInChildren = base.transform.Find("affectArea").GetComponentInChildren<BoxCollider>(true);
			data.CollideBounds = componentInChildren.bounds;
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<GroupData>(bytes);
			Transform transform = base.transform.Find("affectArea");
			BoxCollider componentInChildren = transform.GetComponentInChildren<BoxCollider>(true);
			componentInChildren.center = data.CollideBounds.center - transform.transform.position;
			componentInChildren.size = data.CollideBounds.size;
			base.transform.Find("triggerBegin").position = data.BeginPos;
			base.transform.Find("triggerEndRotate").position = data.EndRotatePos;
			base.transform.Find("triggerEndFollow").position = data.EndFollowPos;
		}

		public override byte[] WriteBytes()
		{
			Transform transform = base.transform.Find("triggerBegin");
			data.BeginPos = transform.position;
			data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform2 = base.transform.Find("triggerEndRotate");
			data.EndRotatePos = transform2.position;
			data.EndRotateDistance = base.transform.parent.InverseTransformPoint(transform2.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			Transform transform3 = base.transform.Find("triggerEndFollow");
			data.EndFollowPos = transform3.position;
			data.EndFollowDistance = base.transform.parent.InverseTransformPoint(transform3.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			BoxCollider componentInChildren = base.transform.Find("affectArea").GetComponentInChildren<BoxCollider>(true);
			data.CollideBounds = componentInChildren.bounds;
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override Bounds GetGroupBounds(byte[] byteData)
		{
			return StructTranslatorUtility.ToStructure<GroupData>(byteData).CollideBounds;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_FollowRotatePlatform_DATA rD_FollowRotatePlatform_DATA = Bson.ToObject<RD_FollowRotatePlatform_DATA>(rd_data);
			base.transform.SetTransData(rD_FollowRotatePlatform_DATA.transData);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_FollowRotatePlatform_DATA
			{
				transData = base.transform.GetTransData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Transform transform = base.transform.Find("triggerBegin");
			if ((bool)transform)
			{
				Color color = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = color;
			}
			Transform transform2 = base.transform.Find("triggerEndRotate");
			if ((bool)transform2)
			{
				Color color2 = Gizmos.color;
				Gizmos.color = Color.yellow;
				Gizmos.DrawCube(transform2.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = color2;
			}
			Transform transform3 = base.transform.Find("triggerEndFollow");
			if ((bool)transform3)
			{
				Color color3 = Gizmos.color;
				Gizmos.color = Color.red;
				Gizmos.DrawCube(transform3.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = color3;
			}
		}
	}
}
