public class BroadCastData
{
	private BroadCastType type = BroadCastType.NONE;

	private int goodId = -1;

	private string info = "";

	public BroadCastType Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public int GoodId
	{
		get
		{
			return goodId;
		}
		set
		{
			goodId = value;
		}
	}

	public string Info
	{
		get
		{
			return info;
		}
		set
		{
			info = value;
		}
	}

	public void Clear()
	{
		type = BroadCastType.NONE;
		info = "";
		goodId = -1;
	}
}
