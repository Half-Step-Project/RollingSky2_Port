using Foundation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RS2
{
	public class AssetButtonHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		private Goods_goodsTable m_goodData;

		public void SetAssetData(Goods_goodsTable data)
		{
			m_goodData = data;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (m_goodData != null)
			{
				AssetPromptBoxFormData assetPromptBoxFormData = new AssetPromptBoxFormData();
				assetPromptBoxFormData.m_target = GetComponent<RectTransform>();
				assetPromptBoxFormData.m_goodID = m_goodData.Id;
				Mod.UI.OpenUIForm(UIFormId.AssetPromptBoxForm, assetPromptBoxFormData);
			}
		}
	}
}
