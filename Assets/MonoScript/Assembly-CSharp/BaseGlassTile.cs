using System;
using Foundation;
using UnityEngine;

public class BaseGlassTile : BaseTile
{
	public enum GlassState
	{
		Wait,
		Active,
		InActive
	}

	public GlassTileData data;

	public GlassState currentState;

	protected Transform[] borderParts;

	public virtual bool IfBlockLeft
	{
		get
		{
			return data.LeftBlock;
		}
	}

	public virtual bool IfBlockRight
	{
		get
		{
			return data.RightBlock;
		}
	}

	public virtual bool IfBlockUp
	{
		get
		{
			return data.UpBlock;
		}
	}

	public virtual bool IfBlockDown
	{
		get
		{
			return data.DownBlock;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			data = JsonUtility.FromJson<GlassTileData>(info);
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<GlassTileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		data.BeginDistance = (float)objs[0];
		data.EndDistance = (float)objs[1];
		data.Acceleration = (float)objs[2];
		data.SpeedScaler = (float)objs[3];
		data.LeftBlock = (bool)objs[4];
		data.RightBlock = (bool)objs[5];
		data.UpBlock = (bool)objs[6];
		data.DownBlock = (bool)objs[7];
	}

	public override void Initialize()
	{
		base.Initialize();
		borderParts = new Transform[4];
		bool[] array = new bool[4] { data.LeftBlock, data.RightBlock, data.UpBlock, data.DownBlock };
		for (int i = 0; i < 4; i++)
		{
			borderParts[i] = base.transform.Find("model/border" + (i + 1));
			borderParts[i].gameObject.SetActive(array[i]);
		}
		currentState = GlassState.Wait;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentState = GlassState.Wait;
	}

	public override void UpdateElement()
	{
		float num = 0f;
		if (currentState == GlassState.Wait)
		{
			return;
		}
		if (currentState == GlassState.Active)
		{
			num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			PlayByPercent(GetPercent(num));
			if (num >= data.EndDistance)
			{
				currentState = GlassState.InActive;
			}
		}
		else
		{
			GlassState currentState2 = currentState;
			int num2 = 2;
		}
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Max(0f, distance * data.SpeedScaler);
	}

	public override void PlayByPercent(float percent)
	{
		if (percent > 0f)
		{
			Vector3 startLocalPos = StartLocalPos;
			startLocalPos.y -= 0.5f * data.Acceleration * percent * percent;
			base.transform.localPosition = startLocalPos;
		}
	}

	protected virtual void OnDrawGizmos()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.green;
		if (IfBlockLeft)
		{
			Gizmos.DrawCube(base.transform.position + new Vector3(-0.4f, 0f, 0f), new Vector3(0.1f, 0.1f, 1f));
		}
		if (IfBlockRight)
		{
			Gizmos.DrawCube(base.transform.position - new Vector3(-0.4f, 0f, 0f), new Vector3(0.1f, 0.1f, 1f));
		}
		if (IfBlockUp)
		{
			Gizmos.DrawCube(base.transform.position + new Vector3(0f, 0f, 0.4f), new Vector3(1f, 0.1f, 0.1f));
		}
		if (IfBlockDown)
		{
			Gizmos.DrawCube(base.transform.position - new Vector3(0f, 0f, 0.4f), new Vector3(1f, 0.1f, 0.1f));
		}
		Gizmos.color = color;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_BaseGlassTile_DATA rD_BaseGlassTile_DATA = JsonUtility.FromJson<RD_BaseGlassTile_DATA>(rd_data as string);
		currentState = rD_BaseGlassTile_DATA.currentState;
		borderParts.SetTransData(rD_BaseGlassTile_DATA.borderParts);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_BaseGlassTile_DATA
		{
			currentState = currentState,
			borderParts = borderParts.GetTransData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_BaseGlassTile_DATA rD_BaseGlassTile_DATA = Bson.ToObject<RD_BaseGlassTile_DATA>(rd_data);
		currentState = rD_BaseGlassTile_DATA.currentState;
		borderParts.SetTransData(rD_BaseGlassTile_DATA.borderParts);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_BaseGlassTile_DATA
		{
			currentState = currentState,
			borderParts = borderParts.GetTransData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
