using System.Collections;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UnityEngine;
using UnityEngine.Networking;

public sealed class NetWorkManager : MonoSingleton<NetWorkManager>
{
	private const string UMRequestErrorEventID = "RequesetError";

	private const string GAME_ID = "1018";

	private Dictionary<string, string> m_UMErrorInfo = new Dictionary<string, string>();

	private bool m_IsSynNetwork;

	public bool IsSynNetwork
	{
		get
		{
			return m_IsSynNetwork;
		}
		set
		{
			m_IsSynNetwork = value;
		}
	}

	public string URL
	{
		get
		{
			if (false || Director.Ins.ConnectedTestServer)
			{
				return "http://49.51.231.176:9000/rollingSky2/operation";
			}
			return "https://gdcasapi.cmcm.com/rollingSky2/operation";
		}
	}

	public void Send(NetMessageData messageData)
	{
		if (m_IsSynNetwork)
		{
			StartCoroutine(_Send(messageData));
		}
	}

	private IEnumerator _Send(NetMessageData messageData)
	{
		using (UnityWebRequest webRequest = new UnityWebRequest(URL))
		{
			webRequest.disposeDownloadHandlerOnDispose = true;
			webRequest.disposeUploadHandlerOnDispose = true;
			webRequest.timeout = 20;
			webRequest.method = "POST";
			webRequest.SetRequestHeader("gameId", "1018");
			webRequest.SetRequestHeader("userId", GetUserId());
			webRequest.SetRequestHeader("version", GetAppVer());
			webRequest.SetRequestHeader("protocolVer", messageData.messageId.ToString());
			webRequest.SetRequestHeader("platform", GetDeviceSystemHeadInfo());
			DownloadHandler downHandler = new DownloadHandlerBuffer();
			UploadHandler uploadHandler2 = (webRequest.uploadHandler = CreateUploaderHandler(messageData.content));
			webRequest.downloadHandler = downHandler;
			yield return webRequest.SendWebRequest();
			if (webRequest.isDone && webRequest.error == null)
			{
				if (messageData.succesHandler != null)
				{
					NetMessageResultData arg = JsonUtility.FromJson<NetMessageResultData>(Encoding.UTF8.GetString(downHandler.data));
					messageData.succesHandler(arg);
				}
			}
			else if (messageData.errorHandler != null)
			{
				string text = string.Format("responseCode:{0},error:{1}", webRequest.responseCode, webRequest.error);
				Log.Info(string.Format("NetWork:{0},error:{1}", messageData.messageId, text));
				messageData.error = webRequest.error;
				messageData.responseCode = webRequest.responseCode;
				messageData.errorHandler(text);
				m_UMErrorInfo.Clear();
				m_UMErrorInfo.Add("url", webRequest.url);
				m_UMErrorInfo.Add("messageId", messageData.messageId.ToString());
				m_UMErrorInfo.Add("responseCode", messageData.responseCode.ToString());
				m_UMErrorInfo.Add("errorInfo", webRequest.error);
				m_UMErrorInfo.Add("internetReachability", Application.internetReachability.ToString());
			}
			ObjectPool<NetMessageData>.Instance.Release(messageData);
		}
	}

	private UploadHandler CreateUploaderHandler(string content)
	{
		if (content == null || content.Length == 0)
		{
			return null;
		}
		return new UploadHandlerRaw(Encoding.UTF8.GetBytes(content))
		{
			contentType = "application/json; charset=utf-8"
		};
	}

	public string GetUserId()
	{
		string text = "";
		Log.Info(string.Format("Current Device User ID : {0}", text));
		return text;
	}

	public string GetAppVer()
	{
		return Application.version;
	}

	public string GetDeviceSystemHeadInfo()
	{
		return "";
	}
}
