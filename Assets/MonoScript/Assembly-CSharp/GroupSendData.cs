using System;

[Serializable]
public struct GroupSendData
{
	public int groupID;

	public static GroupSendData DefaultValue
	{
		get
		{
			GroupSendData result = default(GroupSendData);
			result.groupID = 1;
			return result;
		}
	}
}
