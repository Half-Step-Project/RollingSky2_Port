using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class SequenceLoginAwardItem : MonoBehaviour
	{
		private enum AwardState
		{
			NONE = -1,
			CAN_GET,
			HAD_GET,
			CANNOT_GET_TIMEOUT,
			CANNOT_GET_TIMENOTO
		}

		public Image m_awardIcon;

		public GameObject m_hadGetFlag;

		public GameObject selectGo;

		public Text m_descTxt;

		public Text m_awardTxt;

		private AssetLoadCallbacks m_assetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		private AwardState m_State = AwardState.NONE;

		private int m_id = -1;

		private Dictionary<int, int> m_goodsDic;

		public void Init()
		{
			m_awardIcon.gameObject.SetActive(false);
			m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_awardIcon != null)
				{
					m_awardIcon.gameObject.SetActive(true);
					m_awardIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				}
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
		}

		public void SetData(int id)
		{
			m_id = id;
			SequenceLoginAward_table sequenceLoginAward_table = Mod.DataTable.Get<SequenceLoginAward_table>()[id];
			if (sequenceLoginAward_table != null)
			{
				m_goodsDic = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(sequenceLoginAward_table.GoodTeamIds);
				int num = 0;
				Dictionary<int, int>.Enumerator enumerator = m_goodsDic.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int key = enumerator.Current.Key;
					num = enumerator.Current.Value;
				}
				string assetName = MonoSingleton<GameTools>.Instacne.GoodsTeamIconId(sequenceLoginAward_table.GoodTeamIds).ToString();
				Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
				m_awardTxt.text = string.Format("X{0}", num);
				m_descTxt.text = string.Format(Mod.Localization.GetInfoById(158), id);
				RefreshState();
			}
		}

		private void RefreshState()
		{
			int num = m_id - 1;
			int num2 = PlayerDataModule.Instance.SequenceLoginData.NowIndex();
			int awardRecord = PlayerDataModule.Instance.SequenceLoginData.GetAwardRecord(num);
			if (num == num2)
			{
				if (awardRecord == 0)
				{
					m_State = AwardState.CAN_GET;
				}
				else
				{
					m_State = AwardState.HAD_GET;
				}
			}
			else if (num < num2)
			{
				if (awardRecord == 0)
				{
					m_State = AwardState.CANNOT_GET_TIMEOUT;
				}
				else
				{
					m_State = AwardState.HAD_GET;
				}
			}
			else
			{
				m_State = AwardState.CANNOT_GET_TIMENOTO;
			}
			ControlItemByState();
		}

		private void ControlItemByState()
		{
			switch (m_State)
			{
			case AwardState.CAN_GET:
				m_hadGetFlag.SetActive(false);
				break;
			case AwardState.HAD_GET:
				m_hadGetFlag.SetActive(true);
				break;
			case AwardState.CANNOT_GET_TIMEOUT:
				m_hadGetFlag.SetActive(false);
				break;
			case AwardState.CANNOT_GET_TIMENOTO:
				m_hadGetFlag.SetActive(false);
				break;
			}
		}

		public void Clear()
		{
		}

		public void Release()
		{
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}

		public void GetGoods(int factor = 1)
		{
			if (m_State != 0)
			{
				return;
			}
			if (m_goodsDic != null && m_goodsDic.Count > 0)
			{
				Dictionary<int, int>.Enumerator enumerator = m_goodsDic.GetEnumerator();
				while (enumerator.MoveNext())
				{
					PlayerDataModule.Instance.ChangePlayerGoodsNum(enumerator.Current.Key, enumerator.Current.Value * factor, AssertChangeType.ACTIVITY);
				}
				SequenceLoginAward_table sequenceLoginAward_table = Mod.DataTable.Get<SequenceLoginAward_table>()[m_id];
				if (sequenceLoginAward_table != null)
				{
					BroadResult(sequenceLoginAward_table.GoodTeamIds, factor);
				}
			}
			m_State = AwardState.HAD_GET;
			PlayerDataModule.Instance.SequenceLoginData.SetAwardRecord(m_id - 1);
			ControlItemByState();
			Mod.Event.Fire(this, Mod.Reference.Acquire<GetLoginAwardEvent>().Initialize(m_id - 1));
		}

		public bool CanGet()
		{
			return m_State == AwardState.CAN_GET;
		}

		private void BroadResult(int goodsTeamId, int goodsTeamNum)
		{
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsTeamId = goodsTeamId;
			getGoodsData.GoodsTeamNum = goodsTeamNum;
			getGoodsData.GoodsTeam = true;
			Singleton<UIPopupManager>.Instance.PopupUI(UIFormId.GetGoodsForm, getGoodsData, UIPopupManager.PriorityType.Priority_50);
		}

		public void ShowSelectGo(bool isShow)
		{
			selectGo.SetActive(isShow && m_State != AwardState.HAD_GET);
		}
	}
}
