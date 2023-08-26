using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class GameSettingForm : UGUIForm
	{
		private Dictionary<string, SettingFuncItemController> funcItemControllerDic;

		public GameObject funcItemContainer;

		public GameObject closeBtn;

		public GameObject m_RS1_btn;

		public GameObject m_Car_btn;

		public float m_cellWith = 145f;

		private int currentPage;

		private List<object> loadedAsserts = new List<object>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Init();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			ShowFirstPage();
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
		}

		public void OnCloseBtn()
		{
			Mod.UI.CloseUIForm(UIFormId.GameSettingForm);
		}

		private void Init()
		{
			SetOtherGameState();
			funcItemControllerDic = new Dictionary<string, SettingFuncItemController>();
			if (funcItemContainer != null)
			{
				int childCount = funcItemContainer.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					GameObject gameObject = funcItemContainer.transform.GetChild(i).gameObject;
					SettingFuncItemController settingFuncItemController = gameObject.AddComponent<SettingFuncItemController>();
					settingFuncItemController.SettingForm = this;
					funcItemControllerDic.Add(gameObject.name, settingFuncItemController);
				}
			}
		}

		public void ShowFirstPage()
		{
			currentPage = 0;
			closeBtn.SetActive(false);
			List<SettingFuncType> list = new List<SettingFuncType>
			{
				SettingFuncType.FeedBack,
				SettingFuncType.QQ,
				SettingFuncType.Facebook,
				SettingFuncType.Quality,
				SettingFuncType.Music,
				SettingFuncType.Sound,
				SettingFuncType.TermsOfService,
				SettingFuncType.PrivocyPolicy,
				SettingFuncType.AdChoices
			};
			if (CMPGDPRUtils.Instance.checkIsGDPREnforcedCountry())
			{
				list.Add(SettingFuncType.DeleteMyData);
				list.Add(SettingFuncType.NotCollectMyData);
			}
			RectTransform rectTransform = funcItemContainer.transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.sizeDelta = new Vector2(0f, 145 * list.Count + 72);
			}
			Dictionary<string, SettingFuncItemController>.Enumerator enumerator = funcItemControllerDic.GetEnumerator();
			float num = 0f;
			while (enumerator.MoveNext())
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (enumerator.Current.Key.ToString().Equals(list[i].ToString().ToLower()))
					{
						num = (0f - m_cellWith) * (float)i - 72f;
						enumerator.Current.Value.SetFuncType(list[i]);
						enumerator.Current.Value.transform.localPosition = new Vector3(enumerator.Current.Value.transform.localPosition.x, num, 0f);
						enumerator.Current.Value.SetShowAvaiable(true);
						break;
					}
					enumerator.Current.Value.SetShowAvaiable(false);
				}
			}
		}

		public void ShowSecondPage()
		{
			currentPage = 1;
			closeBtn.SetActive(true);
			List<SettingFuncType> list = new List<SettingFuncType>
			{
				SettingFuncType.TermsOfService,
				SettingFuncType.PrivocyPolicy,
				SettingFuncType.AdChoices,
				SettingFuncType.Music,
				SettingFuncType.Sound
			};
			if (CMPGDPRUtils.Instance.checkIsGDPREnforcedCountry())
			{
				list.Add(SettingFuncType.DataManageMent);
			}
			float num = 0f;
			Dictionary<string, SettingFuncItemController>.Enumerator enumerator = funcItemControllerDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (enumerator.Current.Key.ToString().Equals(list[i].ToString().ToLower()))
					{
						num = 27 - 145 * i;
						enumerator.Current.Value.SetFuncType(list[i]);
						enumerator.Current.Value.transform.localPosition = new Vector3(enumerator.Current.Value.transform.localPosition.x, num, 0f);
						enumerator.Current.Value.SetShowAvaiable(true);
						break;
					}
					enumerator.Current.Value.SetShowAvaiable(false);
				}
			}
		}

		public void ShowThirdPage()
		{
			currentPage = 2;
			closeBtn.SetActive(true);
			List<SettingFuncType> list = new List<SettingFuncType>
			{
				SettingFuncType.DeleteMyData,
				SettingFuncType.NotCollectMyData
			};
			float num = 0f;
			Dictionary<string, SettingFuncItemController>.Enumerator enumerator = funcItemControllerDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (enumerator.Current.Key.ToString().Equals(list[i].ToString().ToLower()))
					{
						num = 27 - 145 * i;
						enumerator.Current.Value.SetFuncType(list[i]);
						enumerator.Current.Value.transform.localPosition = new Vector3(enumerator.Current.Value.transform.localPosition.x, num, 0f);
						enumerator.Current.Value.SetShowAvaiable(true);
						break;
					}
					enumerator.Current.Value.SetShowAvaiable(false);
				}
			}
		}

		private void CloseBtnClickHandler(GameObject go)
		{
			if (currentPage == 2)
			{
				ShowSecondPage();
			}
			else if (currentPage == 1)
			{
				ShowFirstPage();
			}
			else if (currentPage == 0)
			{
				Mod.UI.CloseUIForm(UIFormId.GameSettingForm);
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

		public void OnClickOther1()
		{
			Application.OpenURL("https://itunes.apple.com/app/id1036661603");
		}

		public void OnClickOther2()
		{
			Application.OpenURL("https://itunes.apple.com/app/id1436464683");
		}

		private void SetOtherGameState()
		{
			if (m_RS1_btn != null)
			{
				m_RS1_btn.SetActive(false);
			}
			if (m_Car_btn != null)
			{
				m_Car_btn.SetActive(false);
			}
		}
	}
}
