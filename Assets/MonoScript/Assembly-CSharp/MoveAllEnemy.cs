using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MoveAllEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float MoveDistance;

		public float BeginDistance;

		public float SpeedScaler;

		public EnemyDirection MoveDirection;

		public float SoundDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDistance = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
			MoveDirection = (EnemyDirection)bytes.GetInt32(ref startIndex);
			SoundDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)MoveDirection).GetBytes(), ref offset);
				memoryStream.WriteByteArray(SoundDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const float Half_Pi = (float)Math.PI / 2f;

	public EnemyData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private bool ifSoundPlayed;

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
		if (data.MoveDirection == EnemyDirection.Down)
		{
			endPos = beginPos + new Vector3(0f, 0f - data.MoveDistance, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Up)
		{
			endPos = beginPos + new Vector3(0f, data.MoveDistance, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Left)
		{
			endPos = beginPos + new Vector3(0f - data.MoveDistance, 0f, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Right)
		{
			endPos = beginPos + new Vector3(data.MoveDistance, 0f, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Forward)
		{
			endPos = beginPos + new Vector3(0f, 0f, data.MoveDistance);
		}
		else if (data.MoveDirection == EnemyDirection.Backward)
		{
			endPos = beginPos + new Vector3(0f, 0f, 0f - data.MoveDistance);
		}
		data.SpeedScaler = Mathf.Abs(data.SpeedScaler);
		ifSoundPlayed = false;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		ifSoundPlayed = false;
	}

	public override void UpdateElement()
	{
		float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance;
		if (audioSource != null && GameController.Instance.GetPlayerDataModule.IsSoundPlayOn() && !ifSoundPlayed && num >= data.SoundDistance)
		{
			audioSource.Play();
			ifSoundPlayed = true;
		}
		PlayByPercent(GetPercent(num));
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(-1f, distance * data.SpeedScaler));
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
		if (data.MoveDirection == EnemyDirection.Up)
		{
			PlayByPercent(1f);
		}
	}

	public override void SetBaseState()
	{
		if (data.MoveDirection == EnemyDirection.Up)
		{
			PlayByPercent(0f);
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EnemyData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_MoveAllEnemy_DATA rD_MoveAllEnemy_DATA = JsonUtility.FromJson<RD_MoveAllEnemy_DATA>(rd_data as string);
		base.transform.SetTransData(rD_MoveAllEnemy_DATA.transData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_MoveAllEnemy_DATA
		{
			transData = base.transform.GetTransData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_MoveAllEnemy_DATA rD_MoveAllEnemy_DATA = Bson.ToObject<RD_MoveAllEnemy_DATA>(rd_data);
		base.transform.SetTransData(rD_MoveAllEnemy_DATA.transData);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_MoveAllEnemy_DATA
		{
			transData = base.transform.GetTransData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
