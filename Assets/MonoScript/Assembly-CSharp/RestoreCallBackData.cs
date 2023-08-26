using System;
using System.Collections.Generic;

[Serializable]
public class RestoreCallBackData
{
	public string status;

	public List<RestoreResultData> result;

	public string message;

	public string userId;

	public string isSandBox;
}
