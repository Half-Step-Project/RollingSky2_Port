using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class RankForm : UGUIForm
	{
		private Image back;

		private Image upback;

		private Image downback_0;

		private Image downback_1;

		private GameObject debugBtn;

		private Text debugTxt;

		private CanvasGroup ainmaContainer_chilun;

		private List<Object> loadedAsserts = new List<Object>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			InitUI();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			SetDebugInfo();
			AddEventListener();
			ainmaContainer_chilun.alpha = 0f;
			ainmaContainer_chilun.DOFade(1f, 1.5f).OnComplete(delegate
			{
				ainmaContainer_chilun.alpha = 1f;
			});
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
		}

		private void InitUI()
		{
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			back = dictionary["back"].GetComponent<Image>();
			upback = dictionary["upback"].GetComponent<Image>();
			downback_0 = dictionary["downback_0"].GetComponent<Image>();
			downback_1 = dictionary["downback_1"].GetComponent<Image>();
			ainmaContainer_chilun = dictionary["ainmaContainer_chilun"].GetComponent<CanvasGroup>();
			debugBtn = dictionary["debugBtn"];
			debugTxt = dictionary["debugTxt"].GetComponent<Text>();
			debugBtn.gameObject.SetActive(false);
		}

		public void AddEventListener()
		{
			EventTriggerListener.Get(debugBtn).onClick = DebugHandler;
		}

		private void DebugHandler(GameObject go)
		{
			GameController.IfNotDeath = !GameController.IfNotDeath;
			SetDebugInfo();
		}

		public void ChangeBgByLevelId(int levelId)
		{
		}

		private void SetDebugInfo()
		{
			if (GameController.IfNotDeath)
			{
				debugTxt.text = "无敌:YES";
			}
			else
			{
				debugTxt.text = "无敌:NO";
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}
	}
}
