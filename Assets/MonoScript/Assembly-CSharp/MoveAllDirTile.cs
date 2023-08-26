using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MoveAllDirTile : BaseTile, IRebirth
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public TileDirection MoveDirection;

		public float MoveDistance;

		public float BeginDistance;

		public float SpeedScaler;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDirection = (TileDirection)bytes.GetInt32(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(((int)MoveDirection).GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const float Half_Pi = (float)Math.PI / 2f;

	public TileData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private Animation anim;

	private float halfColliderWidth;

	private float halfColliderHeight;

	private float playPercent;

	private RD_MoveAllDirTile_DATA m_rebirthData;

	private bool m_isRebirth;

	public override float TileWidth
	{
		get
		{
			return halfColliderWidth + BaseTile.RecycleWidthTolerance;
		}
	}

	public override float TileHeight
	{
		get
		{
			return halfColliderHeight + BaseTile.RecycleHeightTolerance;
		}
	}

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
		beginPos = StartLocalPos;
		data.MoveDistance = Mathf.Abs(data.MoveDistance);
		if (data.MoveDirection == TileDirection.Down)
		{
			endPos = beginPos + new Vector3(0f, 0f - data.MoveDistance, 0f);
		}
		else if (data.MoveDirection == TileDirection.Up)
		{
			endPos = beginPos + new Vector3(0f, data.MoveDistance, 0f);
		}
		else if (data.MoveDirection == TileDirection.Left)
		{
			endPos = beginPos + new Vector3(0f - data.MoveDistance, 0f, 0f);
		}
		else if (data.MoveDirection == TileDirection.Right)
		{
			endPos = beginPos + new Vector3(data.MoveDistance, 0f, 0f);
		}
		else if (data.MoveDirection == TileDirection.Forward)
		{
			endPos = beginPos + new Vector3(0f, 0f, data.MoveDistance);
		}
		else if (data.MoveDirection == TileDirection.Backward)
		{
			endPos = beginPos + new Vector3(0f, 0f, 0f - data.MoveDistance);
		}
		data.SpeedScaler = Mathf.Abs(data.SpeedScaler);
		anim = GetComponentInChildren<Animation>();
		playPercent = 0f;
		m_isRebirth = false;
	}

	public override void LateInitialize()
	{
		base.LateInitialize();
		BoxCollider componentInChildren = GetComponentInChildren<BoxCollider>();
		if ((bool)componentInChildren)
		{
			halfColliderWidth = componentInChildren.size.x * 0.5f;
			halfColliderHeight = componentInChildren.size.z * 0.5f;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		playPercent = 0f;
		PlayAnim(anim, false);
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance;
		playPercent = GetPercent(distance);
		PlayByPercent(playPercent);
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(-1f, distance * data.SpeedScaler));
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
		if (!m_isRebirth)
		{
			PlayAnim(anim, true);
		}
	}

	public override void PlayByPercent(float percent)
	{
		if (percent < 0f)
		{
			percent = (1f + percent) * ((float)Math.PI / 2f);
			base.transform.localPosition = Vector3.Lerp(beginPos, endPos, 1f - Mathf.Sin(percent + (float)Math.PI / 2f));
		}
		else
		{
			base.transform.localPosition = endPos;
		}
	}

	public override void SetBakeState()
	{
		Initialize();
		if (data.MoveDirection == TileDirection.Up)
		{
			base.transform.localPosition = beginPos + new Vector3(0f, data.MoveDistance, 0f);
		}
	}

	public override void SetBaseState()
	{
		if (data.MoveDirection == TileDirection.Up)
		{
			Initialize();
			base.transform.localPosition = beginPos + new Vector3(0f, 0f - data.MoveDistance, 0f);
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

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_isRebirth = true;
		m_rebirthData = JsonUtility.FromJson<RD_MoveAllDirTile_DATA>(rd_data as string);
		anim.SetAnimData(m_rebirthData.animData, ProcessState.Pause);
		PlayByPercent(m_rebirthData.playPercent);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_MoveAllDirTile_DATA
		{
			animData = anim.GetAnimData(),
			playPercent = playPercent
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		m_isRebirth = false;
		anim.SetAnimData(m_rebirthData.animData, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_isRebirth = true;
		m_rebirthData = Bson.ToObject<RD_MoveAllDirTile_DATA>(rd_data);
		anim.SetAnimData(m_rebirthData.animData, ProcessState.Pause);
		PlayByPercent(m_rebirthData.playPercent);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_MoveAllDirTile_DATA
		{
			animData = anim.GetAnimData(),
			playPercent = playPercent
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		m_isRebirth = false;
		anim.SetAnimData(m_rebirthData.animData, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
