using System.Collections;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public static class UIExtension
	{
		public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
		{
			float time = 0f;
			float originalAlpha = canvasGroup.alpha;
			while (time < duration)
			{
				time += Time.deltaTime;
				canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
				yield return new WaitForEndOfFrame();
			}
			canvasGroup.alpha = alpha;
		}

		public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
		{
			float time = 0f;
			float originalValue = slider.value;
			while (time < duration)
			{
				time += Time.deltaTime;
				slider.value = Mathf.Lerp(originalValue, value, time / duration);
				yield return new WaitForEndOfFrame();
			}
			slider.value = value;
		}

		public static UGUIForm GetUIForm(this UIMod ui, UIFormId uiFormId, string uiGroupName = null)
		{
			string uIFormAssetName = GetUIFormAssetName(uiFormId);
			UIForm uIForm;
			if (string.IsNullOrEmpty(uiGroupName))
			{
				uIForm = ui.GetUIForm(uIFormAssetName);
				if (uIForm == null)
				{
					return null;
				}
				return (UGUIForm)uIForm.Logic;
			}
			UIGroup uIGroup = ui.GetUIGroup(uiGroupName);
			if (uIGroup == null)
			{
				return null;
			}
			uIForm = uIGroup.GetUIForm(uIFormAssetName);
			if (uIForm == null)
			{
				return null;
			}
			return (UGUIForm)uIForm.Logic;
		}

		public static bool HasUIForm(this UIMod ui, UIFormId uiFormId, string uiGroupName = null)
		{
			return ui.GetUIForm(uiFormId, uiGroupName) != null;
		}

		public static int OpenUIForm(this UIMod ui, UIFormId uiFormId, object userData = null, bool disableInputWhenLoading = false)
		{
			if (disableInputWhenLoading)
			{
				MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
			}
			return ui.OpenUIForm(uiFormId, false, userData);
		}

		public static int OpenUIForm(this UIMod ui, UIFormId uiFormId, bool hidden, object userData = null)
		{
			string text = null;
			UIForms_uiformTable uIForms_uiformTable = null;
			IDataTable<UIForms_uiformTable> dataTable = Mod.DataTable.Get<UIForms_uiformTable>();
			if (dataTable == null)
			{
				text = uiFormId.ToString();
			}
			else
			{
				uIForms_uiformTable = dataTable.Get((int)uiFormId);
				text = ((uIForms_uiformTable == null) ? uiFormId.ToString() : AssetUtility.GetUIFormAsset(uIForms_uiformTable.AssetName));
			}
			if (uIForms_uiformTable == null)
			{
				return -1;
			}
			UIForm uIForm = ui.GetUIForm(text);
			if (uIForms_uiformTable.AllowMultiInstance == 0 && (uIForm != null || ui.IsLoadingUIForm(text)))
			{
				if (uIForm != null)
				{
					return uIForm.Id;
				}
				return -1;
			}
			return ui.OpenUIForm(text, uIForms_uiformTable.UiGroupName, uIForms_uiformTable.PauseCoveredUIForm != 0, hidden, userData);
		}

		public static void CloseUIForm(this UIMod ui, UIFormId uiFormId, string uiGroupName = null)
		{
			UGUIForm uiForm = ui.GetUIForm(uiFormId, uiGroupName);
			if (!uiForm)
			{
				return;
			}
			if ((bool)uiForm.closeAnim)
			{
				uiForm.closeAnim.SetTrigger("close");
				MonoSingleton<GameTools>.Instacne.DisableInputForAWhile();
				TimerHeap.AddTimer((uint)(uiForm.closeTime * 1000f), 0u, delegate
				{
					MonoSingleton<GameTools>.Instacne.EnableInput();
					ui.CloseUIForm(uiForm.UIForm);
				});
			}
			else
			{
				ui.CloseUIForm(uiForm.UIForm);
			}
		}

		public static List<UIFormId> GetAllLoadUIFormIds(this UIMod ui)
		{
			List<UIFormId> list = new List<UIFormId>();
			UIForms_uiformTable[] records = Mod.DataTable.Get<UIForms_uiformTable>().Records;
			UIForm[] loadedUIForms = ui.LoadedUIForms;
			for (int i = 0; i < records.Length; i++)
			{
				string uIFormAsset = AssetUtility.GetUIFormAsset(records[i].AssetName);
				for (int j = 0; j < loadedUIForms.Length; j++)
				{
					if (loadedUIForms[j].AssetName.Equals(uIFormAsset))
					{
						list.Add((UIFormId)records[i].Id);
					}
				}
			}
			return list;
		}

		public static bool UIFormIsOpen(this UIMod ui, UIFormId uiformId)
		{
			string uIFormAssetName = GetUIFormAssetName(uiformId);
			UIForm[] loadedUIForms = ui.LoadedUIForms;
			for (int i = 0; i < loadedUIForms.Length; i++)
			{
				if (loadedUIForms[i].AssetName == uIFormAssetName)
				{
					return true;
				}
			}
			return false;
		}

		public static UIFormId GetUIFormId(this UIMod ui, UIForm uiForm)
		{
			UIForms_uiformTable[] records = Mod.DataTable.Get<UIForms_uiformTable>().Records;
			string assetName = uiForm.AssetName;
			for (int i = 0; i < records.Length; i++)
			{
				if (AssetUtility.GetUIFormAsset(records[i].AssetName) == assetName)
				{
					return (UIFormId)records[i].Id;
				}
			}
			return UIFormId.Undefined;
		}

		public static string GetUIFormAssetName(UIFormId uiFormId)
		{
			IDataTable<UIForms_uiformTable> dataTable = Mod.DataTable.Get<UIForms_uiformTable>();
			if (dataTable == null)
			{
				return uiFormId.ToString();
			}
			UIForms_uiformTable uIForms_uiformTable = dataTable.Get((int)uiFormId);
			if (uIForms_uiformTable == null)
			{
				return uiFormId.ToString();
			}
			return AssetUtility.GetUIFormAsset(uIForms_uiformTable.AssetName);
		}
	}
}
