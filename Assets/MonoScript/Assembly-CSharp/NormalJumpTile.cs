using System;
using System.IO;
using Foundation;
using UnityEngine;

public class NormalJumpTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float MoveSpeed;

		public float DefaultEmission;

		public float TargetEmission;

		public float EmissionScaler;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveSpeed = bytes.GetSingle(ref startIndex);
			DefaultEmission = bytes.GetSingle(ref startIndex);
			TargetEmission = bytes.GetSingle(ref startIndex);
			EmissionScaler = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DefaultEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EmissionScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private float collidePos;

	private Transform state1;

	private Transform state2;

	private Material jumpMat;

	public static readonly float jumpHeight = 1.5f;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.MoveSpeed = (float)objs[0];
			data.DefaultEmission = (float)objs[1];
			data.TargetEmission = (float)objs[2];
			data.EmissionScaler = (float)objs[3];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		if (state1 == null || state2 == null)
		{
			state1 = base.transform.Find("model/state1");
			state2 = base.transform.Find("model/state2");
		}
		if ((bool)state1)
		{
			state1.gameObject.SetActive(true);
			jumpMat = state1.GetComponent<MeshRenderer>().material;
			jumpMat.SetFloat("_Emmission", data.DefaultEmission);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(false);
		}
		beginPos = (endPos = StartLocalPos);
		collidePos = 10000f;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		collidePos = 10000f;
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos;
		PlayByPercent(GetPercent(distance));
		ChangeEmissionByPercent(GetEmissionPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(2f, Mathf.Max(0f, distance));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent > 0f && percent <= 1f)
		{
			float num = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num, endPos.z);
		}
		else if (percent > 1f && percent < 2f)
		{
			float num2 = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num2, endPos.z);
		}
		else if (percent <= 0f || percent >= 2f)
		{
			base.transform.localPosition = endPos;
		}
	}

	private float GetEmissionPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(0f, distance * data.EmissionScaler));
	}

	private void ChangeEmissionByPercent(float percent)
	{
		if (percent > 0f && percent < 1f && (bool)jumpMat && data.DefaultEmission >= -1f)
		{
			jumpMat.SetFloat("_Emmission", Mathf.Lerp(data.DefaultEmission, data.TargetEmission, percent));
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			ball.OnTileEnter(this);
			if (ball.IfJumpingDown)
			{
				ball.CallEndJump(base.transform.position.y);
				ball.CallBeginJump(base.transform.position, base.transform.position + 3.8f * ball.transform.forward, ball.transform.forward, jumpHeight, BaseRole.JumpType.Normal);
				collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
				PlaySoundEffect();
			}
			else if (!ball.IfJumping)
			{
				ball.CallBeginJump(base.transform.position, base.transform.position + 3.8f * ball.transform.forward, ball.transform.forward, jumpHeight, BaseRole.JumpType.Normal);
				collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
				PlaySoundEffect();
			}
			else if (ball.IfDropping)
			{
				ball.CallEndDrop(base.transform.position.y);
				ball.CallBeginJump(base.transform.position, base.transform.position + 3.8f * ball.transform.forward, ball.transform.forward, jumpHeight, BaseRole.JumpType.Normal);
				collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
				PlaySoundEffect();
			}
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

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_NormalJumpTile_DATA rD_NormalJumpTile_DATA = JsonUtility.FromJson<RD_NormalJumpTile_DATA>(rd_data as string);
		collidePos = rD_NormalJumpTile_DATA.collidePos;
		base.transform.SetTransData(rD_NormalJumpTile_DATA.trans);
		state1.SetTransData(rD_NormalJumpTile_DATA.state1);
		state2.SetTransData(rD_NormalJumpTile_DATA.state2);
		if (jumpMat != null)
		{
			jumpMat.SetFloat("_Emmission", rD_NormalJumpTile_DATA.m_jumpMat);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_NormalJumpTile_DATA rD_NormalJumpTile_DATA = new RD_NormalJumpTile_DATA();
		rD_NormalJumpTile_DATA.collidePos = collidePos;
		rD_NormalJumpTile_DATA.trans = base.transform.GetTransData();
		rD_NormalJumpTile_DATA.state1 = state1.GetTransData();
		rD_NormalJumpTile_DATA.state2 = state2.GetTransData();
		if (jumpMat != null)
		{
			rD_NormalJumpTile_DATA.m_jumpMat = jumpMat.GetFloat("_Emmission");
		}
		return JsonUtility.ToJson(rD_NormalJumpTile_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_NormalJumpTile_DATA rD_NormalJumpTile_DATA = Bson.ToObject<RD_NormalJumpTile_DATA>(rd_data);
		collidePos = rD_NormalJumpTile_DATA.collidePos;
		base.transform.SetTransData(rD_NormalJumpTile_DATA.trans);
		state1.SetTransData(rD_NormalJumpTile_DATA.state1);
		state2.SetTransData(rD_NormalJumpTile_DATA.state2);
		if (jumpMat != null)
		{
			jumpMat.SetFloat("_Emmission", rD_NormalJumpTile_DATA.m_jumpMat);
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_NormalJumpTile_DATA rD_NormalJumpTile_DATA = new RD_NormalJumpTile_DATA();
		rD_NormalJumpTile_DATA.collidePos = collidePos;
		rD_NormalJumpTile_DATA.trans = base.transform.GetTransData();
		rD_NormalJumpTile_DATA.state1 = state1.GetTransData();
		rD_NormalJumpTile_DATA.state2 = state2.GetTransData();
		if (jumpMat != null)
		{
			rD_NormalJumpTile_DATA.m_jumpMat = jumpMat.GetFloat("_Emmission");
		}
		return Bson.ToBson(rD_NormalJumpTile_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
