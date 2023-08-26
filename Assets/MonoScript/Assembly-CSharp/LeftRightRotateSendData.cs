using System;

[Serializable]
public struct LeftRightRotateSendData
{
	public LeftRightState m_state;

	public int m_groupID;

	public static LeftRightRotateSendData DefaultValue
	{
		get
		{
			LeftRightRotateSendData result = default(LeftRightRotateSendData);
			result.m_state = LeftRightState.Left;
			result.m_groupID = 1;
			return result;
		}
	}
}
