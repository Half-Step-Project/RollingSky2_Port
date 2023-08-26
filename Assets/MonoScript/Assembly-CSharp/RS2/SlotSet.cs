using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class SlotSet : MonoBehaviour
	{
		private readonly Dictionary<int, Slot> _Slots = new Dictionary<int, Slot>();

		private readonly Dictionary<int, object> _CacheAssets = new Dictionary<int, object>();

		[SerializeField]
		private Slot[] m_Slots;

		[SerializeField]
		private Animator m_SlotsAnimator;

		public void Initialize()
		{
			if (_Slots.Count != 0)
			{
				_Slots.Clear();
			}
			for (int i = 0; i < m_Slots.Length; i++)
			{
				_Slots.Add(i, m_Slots[i]);
			}
			foreach (PlayerLocalInstrumentData allInstrumentData in PlayerDataModule.Instance.GetAllInstrumentDataList())
			{
				if ((int)allInstrumentData.SlotIndex >= 1 && !allInstrumentData.IsBaton())
				{
					AddMusicalInstrument(allInstrumentData);
				}
			}
		}

		public void AddMusicalInstrument(PlayerLocalInstrumentData data)
		{
			int slotID = (int)data.SlotIndex - 1;
			if (slotID >= 7)
			{
				return;
			}
			Slot slot;
			if (!_Slots.TryGetValue(slotID, out slot))
			{
				Log.Error(string.Format("Has no slot at id '{0}'.", slotID));
				return;
			}
			if (!slot.IsEmpty)
			{
				Log.Error("This slot already had a Instrument, remove first.");
				return;
			}
			string instrumentAsset = AssetUtility.GetInstrumentAsset(data.AssetName());
			Mod.Resource.LoadAsset(instrumentAsset, new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				_CacheAssets.Add(slotID, asset);
				MusicalInstrument component = Object.Instantiate(asset as GameObject).GetComponent<MusicalInstrument>();
				component.Initialize(data);
				slot.AddMusicalInstrument(component);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message '{2}'.", data.AssetName(), assetName, errorMessage));
			}));
		}

		public void RemoveMusicalInstrument(int slotID)
		{
			Slot value;
			if (_Slots.TryGetValue(slotID, out value))
			{
				value.RemoveMusicalInstrument();
				Mod.Resource.UnloadAsset(_CacheAssets[slotID]);
			}
			else
			{
				Log.Error(string.Format("Has no slot at id '{0}'.", slotID));
			}
		}

		public Slot GetSlot(int slotID)
		{
			return _Slots[slotID];
		}

		public void PlayAnim(string trigger)
		{
			m_SlotsAnimator.SetTrigger(trigger);
			Log.Info(string.Format("{0}----->{1}", base.name, trigger.ToString()));
		}

		public void ShowInstrumentLevel(bool fadeOut)
		{
			float instrumentLevelShowTime = GameCommon.instrumentLevelShowTime;
			foreach (KeyValuePair<int, Slot> slot in _Slots)
			{
				if (!slot.Value.IsEmpty)
				{
					if (fadeOut)
					{
						slot.Value.MI.LevelTextFadeOut(instrumentLevelShowTime);
					}
					else
					{
						slot.Value.MI.ShowLevelText();
					}
				}
			}
		}

		private void OnDestroy()
		{
			Release();
		}

		public void Release()
		{
			foreach (KeyValuePair<int, Slot> slot in _Slots)
			{
				if (!slot.Value.IsEmpty)
				{
					slot.Value.RemoveMusicalInstrument();
					slot.Value.Release();
				}
			}
			_Slots.Clear();
			foreach (KeyValuePair<int, object> cacheAsset in _CacheAssets)
			{
				Mod.Resource.UnloadAsset(cacheAsset.Value);
			}
			_CacheAssets.Clear();
		}
	}
}
