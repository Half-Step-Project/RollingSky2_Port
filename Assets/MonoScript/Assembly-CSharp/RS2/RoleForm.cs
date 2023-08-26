using System;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class RoleForm : UGUIForm
	{
		private Image back;

		private Image upback;

		private Image downback_0;

		private Image downback_1;

		private CanvasGroup ainmaContainer_chilun;

		private GameObject goldBuyBtn;

		private UIButtonBar selectButtonBar;

		private RoleShopViewRoleUIController roleUIController;

		private RoleShopViewPetUIController petUIController;

		private IRoleShopChildViewUIController currentUIController;

		private List<UnityEngine.Object> loadedAsserts = new List<UnityEngine.Object>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			InitUI();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
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
			if (currentUIController != null)
			{
				currentUIController.Hide();
				currentUIController.Reset();
				currentUIController = null;
			}
			RemoveEventListener();
		}

		private void InitUI()
		{
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			back = dictionary["back"].GetComponent<Image>();
			upback = dictionary["upback"].GetComponent<Image>();
			downback_0 = dictionary["downback_0"].GetComponent<Image>();
			downback_1 = dictionary["downback_1"].GetComponent<Image>();
			ainmaContainer_chilun = dictionary["ainmaContainer_chilun"].GetComponent<CanvasGroup>();
		}

		public void ChangeBgByLevelId(int levelId)
		{
		}

		private void ButtonBarSelectHandler(int index)
		{
			if (currentUIController != null)
			{
				currentUIController.Hide();
				currentUIController.Reset();
			}
			if (index == 0)
			{
				currentUIController = roleUIController;
			}
			else
			{
				currentUIController = petUIController;
			}
			if (currentUIController != null)
			{
				currentUIController.Begin();
				currentUIController.Show();
			}
		}

		private void AddEventListener()
		{
			if (goldBuyBtn != null)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(goldBuyBtn);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(GoldBuyBtnHandler));
			}
		}

		private void RemoveEventListener()
		{
			if (goldBuyBtn != null)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(goldBuyBtn);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(GoldBuyBtnHandler));
			}
		}

		private void OnPlayerGoldNumChange(object sender, Foundation.EventArgs e)
		{
			bool flag = e is GameGoodsNumChangeEventArgs;
		}

		private void GoldBuyBtnHandler(GameObject obj)
		{
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
