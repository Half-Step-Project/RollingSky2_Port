using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public sealed class LoadingForm : UGUIForm
	{
		private string[] tipArray;

		public Text m_PrecentText;

		private SwitchSceneProcedure m_SwitchSceneProcedure;

		public Text tipTxt;

		private List<Object> loadedAsserts = new List<Object>();

		public float Progress { get; private set; }

		public void SetProgress(float progress)
		{
			Progress = progress;
			if (m_PrecentText != null)
			{
				m_PrecentText.text = string.Format("{0}%", ((int)(progress * 100f)).ToString());
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			InitUI();
		}

		protected override void OnOpen(object userData)
		{
			MenuForm.isLoading = true;
			base.OnOpen(userData);
			tipArray = new string[4]
			{
				Mod.Localization.GetInfoById(1002),
				Mod.Localization.GetInfoById(1003),
				Mod.Localization.GetInfoById(1004),
				Mod.Localization.GetInfoById(1005)
			};
			m_SwitchSceneProcedure = (SwitchSceneProcedure)userData;
			if (m_SwitchSceneProcedure == null)
			{
				Log.Warning("SwitchSceneProcedure is invalid when open LoadingForm.");
			}
			else
			{
				tipTxt.text = tipArray[Random.Range(0, tipArray.Length)].Replace("\\n", "\n");
			}
		}

		protected override void OnClose(object userData)
		{
			m_SwitchSceneProcedure = null;
			base.OnClose(userData);
		}

		private void InitUI()
		{
		}

		protected override void OnUnload()
		{
			MenuForm.isLoading = false;
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				if (Mod.Resource != null)
				{
					Mod.Resource.UnloadAsset(loadedAsserts[i]);
				}
			}
			loadedAsserts.Clear();
		}
	}
}
