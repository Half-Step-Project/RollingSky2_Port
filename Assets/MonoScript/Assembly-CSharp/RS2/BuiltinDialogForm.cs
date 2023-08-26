using System.Collections;
using System.Text;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public sealed class BuiltinDialogForm : MonoBehaviour
	{
		private static GameObject s_DialogForm;

		[SerializeField]
		private Text m_InfoTxt;

		[SerializeField]
		private Text m_AlertText;

		[SerializeField]
		private Text m_ProgressInfoText;

		[SerializeField]
		private Text m_ConfirmText;

		[SerializeField]
		private Text m_CancelText;

		[SerializeField]
		private GameObject m_ConfirmBtn;

		[SerializeField]
		private GameObject m_CancelBtn;

		[SerializeField]
		private GameObject m_ProgressBar;

		[SerializeField]
		private GameObject m_SubAlertUI;

		[SerializeField]
		private GameObject m_InfoUI;

		[SerializeField]
		private Image m_ProgressBarFore;

		[SerializeField]
		private float m_ConfirmBtnPosX;

		[SerializeField]
		private float m_ProgressLength;

		[SerializeField]
		private Text m_HeadText;

		[SerializeField]
		private Text m_VersionText;

		private Action<object> m_OnClickConfirm;

		private Action<object> m_OnClickCancel;

		private BuiltinDialogParams m_dialogParams;

		public static BuiltinDialogForm Current { get; private set; }

		public static bool OpenDialog(BuiltinDialogParams dialogParams)
		{
			if (s_DialogForm == null)
			{
				GameObject gameObject = Resources.Load<GameObject>("Builtin/UI/BuiltinDialogForm");
				if (gameObject == null)
				{
					Log.Error("The BuiltinDialogForm asset load failed.");
					return false;
				}
				s_DialogForm = Object.Instantiate(gameObject);
				Transform obj = s_DialogForm.transform;
				obj.position = Vector3.zero;
				obj.localScale = Vector3.one;
				obj.rotation = Quaternion.identity;
				s_DialogForm.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
				s_DialogForm.SetActive(true);
			}
			if (Current == null)
			{
				Current = s_DialogForm.GetComponent<BuiltinDialogForm>();
			}
			return Current.Open(dialogParams);
		}

		public static void Destory()
		{
			if (Current != null)
			{
				Current.Close();
				Current = null;
			}
			if (s_DialogForm != null)
			{
				Object.Destroy(s_DialogForm);
				s_DialogForm = null;
			}
		}

		public static IEnumerator RealDestroy()
		{
			Current.GetComponent<CanvasGroup>().DOFade(0f, 1f);
			yield return new WaitForSeconds(1f);
			Destory();
		}

		public void OnConfirmButtonClick()
		{
			if (m_OnClickConfirm != null)
			{
				m_OnClickConfirm(m_dialogParams.UserData);
			}
		}

		public void OnCancelButtonClick()
		{
			if (m_OnClickCancel != null)
			{
				m_OnClickCancel(m_dialogParams.UserData);
			}
		}

		private bool Open(BuiltinDialogParams userData)
		{
			m_dialogParams = userData;
			switch (m_dialogParams.ShowType)
			{
			case BuiltinDialogShowType.Info:
				ShowInfo();
				break;
			case BuiltinDialogShowType.ProgreeBar:
				ShowProgressInfo();
				break;
			case BuiltinDialogShowType.Alert:
				ShowAlertInfo();
				break;
			}
			if (m_dialogParams.PauseGame)
			{
				Mod.Core.Pause();
			}
			return true;
		}

		private void ShowInfo()
		{
			if (m_InfoUI != null)
			{
				m_InfoUI.SetActive(true);
			}
			if (m_SubAlertUI != null)
			{
				m_SubAlertUI.SetActive(false);
			}
			if (m_ProgressBar != null)
			{
				m_ProgressBar.SetActive(false);
			}
			if (m_InfoTxt != null)
			{
				m_InfoTxt.text = m_dialogParams.InfoMessage;
			}
			if (m_HeadText != null)
			{
				m_HeadText.text = Mod.Localization.Get("PreloadProcedure.AlertHead");
			}
			if (m_VersionText != null)
			{
				m_VersionText.text = string.Format("{0}.{1}", Mod.Resource.ApplicableVersion, Mod.Resource.InternalVersion);
			}
		}

		private void ShowAlertInfo()
		{
			m_SubAlertUI.SetActive(true);
			m_ProgressBar.SetActive(false);
			m_InfoUI.SetActive(false);
			m_AlertText.text = m_dialogParams.AlertMessage;
			RefreshConfirmText(m_dialogParams.ConfirmText);
			m_OnClickConfirm = m_dialogParams.OnClickConfirm;
			RefreshCancelText(m_dialogParams.CancelText);
			m_OnClickCancel = m_dialogParams.OnClickCancel;
			m_ConfirmBtn.SetActive(m_OnClickConfirm != null);
			if (m_OnClickCancel == null)
			{
				m_CancelBtn.SetActive(false);
				m_ConfirmBtn.transform.localPosition = new Vector3(0f, m_ConfirmBtn.transform.localPosition.y, 0f);
			}
			else
			{
				m_CancelBtn.SetActive(true);
				m_ConfirmBtn.transform.localPosition = new Vector3(m_ConfirmBtnPosX, m_ConfirmBtn.transform.localPosition.y, 0f);
			}
		}

		private void ShowProgressInfo()
		{
			m_SubAlertUI.SetActive(false);
			m_ProgressBar.SetActive(true);
			m_InfoUI.SetActive(false);
			m_ProgressInfoText.text = m_dialogParams.ProgreeMessage;
			m_ProgressBarFore.rectTransform.sizeDelta = new Vector2(m_ProgressLength * m_dialogParams.Progress, m_ProgressBarFore.rectTransform.sizeDelta.y);
		}

		private void CloseProgressInfo()
		{
			m_ProgressBar.SetActive(false);
			m_ProgressInfoText.text = string.Empty;
		}

		private void CloseInfo()
		{
			m_InfoUI.SetActive(false);
			m_InfoTxt.text = string.Empty;
		}

		private void CloseAlertInfo()
		{
			m_SubAlertUI.SetActive(false);
			m_AlertText.text = string.Empty;
			m_ConfirmText.text = string.Empty;
			m_OnClickConfirm = null;
			m_CancelText.text = string.Empty;
			m_OnClickCancel = null;
			m_ConfirmBtn.SetActive(false);
			m_CancelBtn.SetActive(false);
			m_ConfirmBtn.transform.localPosition = new Vector3(m_ConfirmBtnPosX, m_ConfirmBtn.transform.localPosition.y, 0f);
		}

		public void Close()
		{
			if (m_dialogParams.PauseGame)
			{
				Mod.Core.Resume();
			}
			CloseProgressInfo();
			CloseAlertInfo();
			CloseInfo();
			s_DialogForm.SetActive(false);
		}

		private void RefreshConfirmText(string confirmText)
		{
			if (string.IsNullOrEmpty(confirmText))
			{
				confirmText = Mod.Localization.Get("BuiltinDialog.ConfirmButton");
			}
			m_ConfirmText.text = confirmText;
		}

		private void RefreshCancelText(string cancelText)
		{
			if (string.IsNullOrEmpty(cancelText))
			{
				cancelText = Mod.Localization.Get("BuiltinDialog.CancelButton");
			}
			m_CancelText.text = cancelText;
		}

		public void SetMessage(string message)
		{
			if (m_AlertText != null)
			{
				m_AlertText.text = message;
			}
		}

		public void ShowUpdateUI()
		{
			if (m_ConfirmBtn != null)
			{
				m_ConfirmBtn.SetActive(false);
			}
			if (m_CancelBtn != null)
			{
				m_CancelBtn.SetActive(false);
			}
			if (m_ProgressBar != null)
			{
				m_ProgressBar.SetActive(true);
			}
		}

		public void ShowProgressContent(string content)
		{
			m_InfoTxt.gameObject.SetActive(true);
			m_InfoTxt.text = content;
		}

		public void AnimationKeyFrame(int index)
		{
			string text = m_InfoTxt.text;
			int num = text.IndexOf('.');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(text))
			{
				stringBuilder.Append(text);
				for (int i = 0; i <= index; i++)
				{
					stringBuilder.Append(".");
				}
				m_InfoTxt.text = stringBuilder.ToString();
			}
		}
	}
}
