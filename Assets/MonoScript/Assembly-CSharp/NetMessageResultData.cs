using System;

[Serializable]
public class NetMessageResultData
{
	public int code = -10000;

	public string data = "";

	public string message = "";

	public override string ToString()
	{
		return "code:" + code + ",message:" + message + ",data:" + data;
	}
}
