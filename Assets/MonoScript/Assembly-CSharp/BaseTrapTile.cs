using Foundation;
using UnityEngine;

public class BaseTrapTile : BaseTile
{
	public TrapTileData data;

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
			return false;
		}
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			data = JsonUtility.FromJson<TrapTileData>(info);
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TrapTileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
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
}
