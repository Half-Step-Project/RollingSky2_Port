using UnityEngine.Events;

public class NetMessageData : IPoolObj
{
	public UnityAction<NetMessageResultData> succesHandler;

	public UnityAction<string> errorHandler;

	public string content = "";

	public int messageId = -1;

	public string error = "";

	public long responseCode = -1L;

	public void Init()
	{
	}

	public void Release()
	{
		content = "";
		messageId = -1;
		succesHandler = null;
		errorHandler = null;
	}
}
