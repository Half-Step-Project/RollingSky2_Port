using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class AssetPromptBoxUINode : MonoBehaviour
	{
		public Image m_icon;

		public Text m_name;

		public Text m_message;

		private Goods_goodsTable m_assetData;

		private List<object> m_loadedAsserts = new List<object>();

		private bool m_isInit;

		public Goods_goodsTable CurrentAssetData
		{
			get
			{
				return m_assetData;
			}
		}

		public void OnInit()
		{
		}

		public void OnOpen(Goods_goodsTable goodData)
		{
			m_isInit = true;
			m_assetData = goodData;
			m_name.text = Mod.Localization.GetInfoById(m_assetData.Name);
			m_message.text = Mod.Localization.GetInfoById(m_assetData.Desc);
			string _iconName = m_assetData.IconId.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_iconName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!m_isInit)
				{
					OnRelease();
				}
				else if (m_icon != null && asset != null)
				{
					m_icon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					m_loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _iconName, assetName, errorMessage));
			}));
		}

		public void OnClose()
		{
			OnRelease();
		}

		private void OnRelease()
		{
			m_isInit = false;
			for (int i = 0; i < m_loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
			}
			m_loadedAsserts.Clear();
		}
	}
}
