using Foundation;
using UnityEngine;

namespace RS2
{
	public class UsingAssertForm : UGUIForm
	{
		public UIPersonalAssetsList m_personalAssetsList;

		public InsideGameDataModule GetInsideGameDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnInit();
				if (DeviceManager.Instance.IsNeedSpecialAdapte())
				{
					MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(m_personalAssetsList.transform as RectTransform);
				}
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventHandler();
			if (m_personalAssetsList != null)
			{
				bool flag = false;
				if (GetInsideGameDataModule.CurrentOriginRebirth == null && !flag)
				{
					m_personalAssetsList.gameObject.SetActive(true);
					m_personalAssetsList.OnOpen(UIPersonalAssetsList.ParentType.UsingAssert);
					m_personalAssetsList.IsCanClickButtons = true;
				}
				else
				{
					m_personalAssetsList.gameObject.SetActive(false);
				}
			}
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnUpdate();
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventHandler();
			if (m_personalAssetsList != null)
			{
				m_personalAssetsList.OnClose();
			}
		}

		private void AddEventHandler()
		{
			Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnStartGame);
		}

		private void RemoveEventHandler()
		{
			Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnStartGame);
		}

		private void RecordBtnClickHandler(GameObject obj)
		{
		}

		protected override void OnUnload()
		{
			base.OnUnload();
		}

		private void OnStartGame(object sender, EventArgs e)
		{
			GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
			if (gameStartEventArgs != null && gameStartEventArgs.StartType == GameStartEventArgs.GameStartType.Normal && (bool)m_personalAssetsList && (bool)m_personalAssetsList.m_bufferContainer && (bool)m_personalAssetsList.m_bufferContainer.m_bufferButton)
			{
				m_personalAssetsList.IsCanClickButtons = false;
			}
		}
	}
}
