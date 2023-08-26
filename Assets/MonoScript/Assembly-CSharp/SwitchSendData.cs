using System;

[Serializable]
public struct SwitchSendData
{
	public SwitchState m_switchState;

	public int m_onGroupID;

	public static SwitchSendData DefaultValue
	{
		get
		{
			SwitchSendData result = default(SwitchSendData);
			result.m_switchState = SwitchState.On;
			result.m_onGroupID = 1;
			return result;
		}
	}
}
