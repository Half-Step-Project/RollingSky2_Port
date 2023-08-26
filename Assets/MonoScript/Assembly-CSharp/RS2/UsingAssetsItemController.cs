using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class UsingAssetsItemController : MonoBehaviour
	{
		[SerializeField]
		private Image assetIcon;

		[SerializeField]
		private Image assetBack;

		[SerializeField]
		private Image assetSelect;

		[SerializeField]
		private Text assetNum;

		[SerializeField]
		private ParticleSystem m_particleSystem;

		private bool isUsingAssert;

		public bool m_isCanClick = true;

		private int m_Id;

		private UsingAssetItemData assetData;

		private AssetLoadCallbacks assetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		public PlayerDataModule GetPlayerDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
			}
		}

		public int Id
		{
			get
			{
				return m_Id;
			}
		}

		public void Init()
		{
			assetIcon.gameObject.SetActive(false);
			assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!(assetIcon == null))
				{
					assetIcon.gameObject.SetActive(true);
					assetIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
		}

		public void SetData(UsingAssetItemData data)
		{
			assetData = data;
			if (assetData != null)
			{
				m_Id = assetData.GoodsId;
				string assetName = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(assetData.GoodsId).ToString();
				Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), assetLoadCallBack);
				assetNum.text = string.Concat(Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(4));
				m_particleSystem.Stop(true);
			}
			if (assetData != null)
			{
				int goodsId = assetData.GoodsId;
				if (goodsId == 4)
				{
					ShowButton(IsCanShowShield());
				}
			}
			AddEventHandler();
		}

		private void ShowButton(bool enable)
		{
			assetIcon.gameObject.SetActive(enable);
			MaskableGraphic[] componentsInChildren = base.gameObject.GetComponentsInChildren<MaskableGraphic>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enable;
			}
		}

		private void AddEventHandler()
		{
			if (m_isCanClick)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(base.gameObject);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(ClickHandler));
			}
			Mod.Event.Subscribe(EventArgs<PropsAddEventArgs>.EventId, UsingAssetHandler);
		}

		private void RemoveEventHandler()
		{
			if (m_isCanClick)
			{
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(base.gameObject);
				eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(ClickHandler));
			}
			Mod.Event.Unsubscribe(EventArgs<PropsAddEventArgs>.EventId, UsingAssetHandler);
		}

		private void UsingAssetHandler(object sender, Foundation.EventArgs e)
		{
			if ((e as PropsAddEventArgs).m_propsName == PropsName.SHIELD && IsCanShowShield())
			{
				m_particleSystem.Play(true);
			}
		}

		private void ClickHandler(GameObject go)
		{
			UsingAsset();
		}

		public void UsingAsset()
		{
			if (assetData != null)
			{
				int goodsId = assetData.GoodsId;
				if (goodsId == 4)
				{
					UsingShield();
				}
			}
		}

		public void Reset()
		{
			assetData = null;
			isUsingAssert = false;
			m_Id = 0;
			RemoveEventHandler();
		}

		public void Release()
		{
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}

		private void UsingShield()
		{
			if (IsCanShowShield())
			{
				m_particleSystem.Play(true);
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.SHIELD));
			}
		}

		private bool IsCanShowShield()
		{
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			if (GetPlayerDataModule.BufferIsEnable(GameCommon.START_FREE_SHIELD))
			{
				return dataModule.CurrentOriginRebirth == null;
			}
			return false;
		}
	}
}
