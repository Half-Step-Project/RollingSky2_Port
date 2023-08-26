using System;
using System.Collections.Generic;

[Serializable]
public struct GroupsSendData
{
	public List<int> groupIDs;

	public static GroupsSendData DefaultValue
	{
		get
		{
			GroupsSendData result = default(GroupsSendData);
			result.groupIDs = new List<int> { 1 };
			return result;
		}
	}
}
