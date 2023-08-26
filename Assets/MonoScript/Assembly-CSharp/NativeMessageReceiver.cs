using System;
using System.Collections;
using Foundation;
using LitJson;
using UnityEngine;

public sealed class NativeMessageReceiver : MonoBehaviour
{
	public void SendFeedbackMessage(string json)
	{
		Debug.LogError("flx----->" + json);
		if (!json.Equals(string.Empty))
		{
			WWWForm wWWForm = new WWWForm();
			try
			{
				JsonData jsonData = JsonMapper.ToObject(json);
				wWWForm.AddField("app_id", 49);
				wWWForm.AddField("chanel", "0");
				wWWForm.AddField("type", "feedback");
				wWWForm.AddField("model", jsonData["model"].ToString());
				wWWForm.AddField("sysversion", jsonData["sysversion"].ToString());
				wWWForm.AddField("uuid", jsonData["uuid"].ToString());
				wWWForm.AddField("version", jsonData["version"].ToString());
				wWWForm.AddField("contact", jsonData["contact"].ToString());
				wWWForm.AddField("syslang", Mod.Localization.Language.ToString());
				wWWForm.AddField("content", jsonData["feedback_msg"].ToString());
			}
			catch (Exception)
			{
			}
			StartCoroutine(SendPost("https://tuc.ksmobile.net/report", wWWForm));
		}
	}

	private IEnumerator SendPost(string _url, WWWForm _wForm)
	{
		WWW postData = new WWW(_url, _wForm);
		yield return postData;
		string error = postData.error;
	}
}
