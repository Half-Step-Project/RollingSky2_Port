using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public sealed class UpdatingForm : MonoBehaviour
	{
		private static GameObject s_UpdatingForm;

		[SerializeField]
		private Text m_MessageText;

		private object m_UserData;

		public static UpdatingForm Current { get; private set; }

		public static bool OpenUpdating(object userData)
		{
			if (Current != null)
			{
				Log.Warning("The updating has been opened.");
				return false;
			}
			if (s_UpdatingForm == null)
			{
				GameObject gameObject = Resources.Load<GameObject>("Builtin/UI/UpdatingForm");
				if (gameObject == null)
				{
					Log.Error("The updating form asset is invalid.");
					return false;
				}
				s_UpdatingForm = Object.Instantiate(gameObject);
				Transform obj = s_UpdatingForm.transform;
				obj.position = Vector3.zero;
				obj.localScale = Vector3.one;
				obj.rotation = Quaternion.identity;
			}
			Current = s_UpdatingForm.GetComponent<UpdatingForm>();
			return Current.Open(userData);
		}

		public static void Destory()
		{
			if (Current != null)
			{
				Current.Close();
			}
			if (s_UpdatingForm != null)
			{
				Object.Destroy(s_UpdatingForm);
				s_UpdatingForm = null;
			}
		}

		public void SetText(string message)
		{
			RefreshText(message);
		}

		private bool Open(object userData)
		{
			m_UserData = userData;
			RefreshText(string.Empty);
			s_UpdatingForm.SetActive(true);
			return true;
		}

		private void Close()
		{
			m_MessageText.text = null;
			m_UserData = null;
			s_UpdatingForm.SetActive(false);
			Current = null;
		}

		private void RefreshText(string messageText)
		{
			if (string.IsNullOrEmpty(messageText))
			{
				messageText = Mod.Localization.Get("CheckVersion.Tips");
			}
			m_MessageText.text = messageText;
		}
	}
}
