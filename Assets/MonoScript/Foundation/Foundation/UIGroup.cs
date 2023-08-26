using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Foundation
{
	[ExecuteInEditMode]
	public sealed class UIGroup : MonoBehaviour
	{
		private sealed class UIFormInfo
		{
			[CompilerGenerated]
			private readonly UIForm _003CUIForm_003Ek__BackingField;

			public UIForm UIForm
			{
				[CompilerGenerated]
				get
				{
					return _003CUIForm_003Ek__BackingField;
				}
			}

			public bool Paused { get; set; }

			public bool Covered { get; set; }

			public UIFormInfo(UIForm uiForm)
			{
				if (uiForm == null)
				{
					Log.Error("UIForm is invalid.");
					return;
				}
				_003CUIForm_003Ek__BackingField = uiForm;
				Paused = false;
				Covered = true;
			}
		}

		private readonly LinkedList<UIFormInfo> _uiFormInfos = new LinkedList<UIFormInfo>();

		private int _depth;

		private Camera _cachedUICamera;

		private Canvas _cachedCanvas;

		private CanvasScaler _cachedCanvasScaler;

		private readonly LinkedList<int> _orderOpenFormList = new LinkedList<int>();

		private readonly Queue<UIFormOpenCacheData> _cacheWillOrderOpenFormDatas = new Queue<UIFormOpenCacheData>();

		public const int DepthFactor = 10000;

		public string Name { get; private set; }

		public int Depth
		{
			get
			{
				return _depth;
			}
			set
			{
				if (_depth != value)
				{
					_depth = value;
					_cachedCanvas.overrideSorting = true;
					_cachedCanvas.sortingOrder = 10000 * _depth;
					Refresh();
				}
			}
		}

		public int UIFormCount
		{
			get
			{
				return _uiFormInfos.Count;
			}
		}

		public UIForm CurrentUIForm
		{
			get
			{
				LinkedListNode<UIFormInfo> first = _uiFormInfos.First;
				if (first == null)
				{
					return null;
				}
				return first.Value.UIForm;
			}
		}

		public UIForm[] UIForms
		{
			get
			{
				List<UIForm> list = new List<UIForm>(_uiFormInfos.Count);
				for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					UIFormInfo value = linkedListNode.Value;
					list.Add(value.UIForm);
				}
				return list.ToArray();
			}
		}

		internal Camera UICamera
		{
			get
			{
				return _cachedUICamera;
			}
			set
			{
				if (!(value == null))
				{
					_cachedUICamera = value;
					_cachedCanvas.worldCamera = _cachedUICamera;
				}
			}
		}

		internal void Init(string name, int depth)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.Error("UIGroup name is invalid.");
				return;
			}
			Name = name;
			Depth = depth;
		}

		public bool HasUIForm(int uiFormId)
		{
			for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				UIFormInfo value = linkedListNode.Value;
				if (value.UIForm.Id == uiFormId)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasUIForm(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("UIForm asset name is invalid.");
				return false;
			}
			for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				UIFormInfo value = linkedListNode.Value;
				if (value.UIForm.AssetName == assetName)
				{
					return true;
				}
			}
			return false;
		}

		public UIForm GetUIForm(int uiFormId)
		{
			for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				UIFormInfo value = linkedListNode.Value;
				if (value.UIForm.Id == uiFormId)
				{
					return value.UIForm;
				}
			}
			return null;
		}

		public UIForm GetUIForm(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("UIForm asset name is invalid.");
				return null;
			}
			for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				UIFormInfo value = linkedListNode.Value;
				if (value.UIForm.AssetName == assetName)
				{
					return value.UIForm;
				}
			}
			return null;
		}

		public UIForm[] GetUIForms(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Warning("UIForm asset name is invalid.");
				return null;
			}
			List<UIForm> list = new List<UIForm>();
			for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				UIFormInfo value = linkedListNode.Value;
				if (value.UIForm.AssetName == assetName)
				{
					list.Add(value.UIForm);
				}
			}
			return list.ToArray();
		}

		internal void Tick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("UIMod.UIGroup.Tick");
			for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				UIFormInfo value = linkedListNode.Value;
				if (!value.Paused)
				{
					value.UIForm.OnTick(elapseSeconds, realElapseSeconds);
				}
			}
			Profiler.EndSample();
		}

		internal void AddUIForm(UIForm uiForm, bool hidden)
		{
			UIFormInfo uIFormInfo = new UIFormInfo(uiForm);
			LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First;
			if (linkedListNode != null)
			{
				if (linkedListNode.Value.UIForm.Id < uiForm.Id)
				{
					_uiFormInfos.AddFirst(uIFormInfo);
					return;
				}
				while (linkedListNode != null)
				{
					if (linkedListNode.Value.UIForm.Id > uIFormInfo.UIForm.Id)
					{
						linkedListNode = linkedListNode.Next;
						if (linkedListNode == null)
						{
							_uiFormInfos.AddLast(uIFormInfo);
						}
						continue;
					}
					_uiFormInfos.AddBefore(linkedListNode, uIFormInfo);
					break;
				}
			}
			else
			{
				_uiFormInfos.AddFirst(uIFormInfo);
			}
		}

		internal void RemoveUIForm(UIForm uiForm)
		{
			UIFormInfo uiFormInfo;
			if (!TryGetUIFormInfo(uiForm, out uiFormInfo))
			{
				Log.Warning("Can not find UIFormInfo for id '" + uiForm.Id + "', UIForm asset name is '" + uiForm.AssetName + "'.");
				return;
			}
			if (!uiFormInfo.Covered)
			{
				uiFormInfo.Covered = true;
				uiForm.OnCover();
			}
			if (!uiFormInfo.Paused)
			{
				uiFormInfo.Paused = true;
				uiForm.OnPause();
			}
			_uiFormInfos.Remove(uiFormInfo);
		}

		internal void RefocusUIForm(UIForm uiForm, object userData)
		{
			UIFormInfo uiFormInfo;
			if (!TryGetUIFormInfo(uiForm, out uiFormInfo))
			{
				Log.Warning("Can not find UIFormInfo.");
				return;
			}
			_uiFormInfos.Remove(uiFormInfo);
			_uiFormInfos.AddFirst(uiFormInfo);
		}

		internal void Hidden(UIForm uiForm, bool hidden)
		{
			UIFormInfo uiFormInfo;
			if (!TryGetUIFormInfo(uiForm, out uiFormInfo))
			{
				Log.Warning("Can not find UIFormInfo.");
			}
			else
			{
				uiForm.Active(!hidden);
			}
		}

		internal void Refresh()
		{
			LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First;
			bool flag = false;
			bool flag2 = false;
			int uIFormCount = UIFormCount;
			while (linkedListNode != null)
			{
				UIFormInfo value = linkedListNode.Value;
				value.UIForm.OnDepthChanged(Depth, uIFormCount--);
				if (flag)
				{
					if (!value.Covered)
					{
						value.Covered = true;
						value.UIForm.OnCover();
					}
					if (!value.Paused)
					{
						value.Paused = true;
						value.UIForm.OnPause();
					}
				}
				else
				{
					if (value.Paused)
					{
						value.Paused = false;
						value.UIForm.OnResume();
					}
					if (value.UIForm.PauseCovered)
					{
						flag = true;
					}
					if (flag2)
					{
						if (!value.Covered)
						{
							value.Covered = true;
							value.UIForm.OnCover();
						}
					}
					else
					{
						if (value.Covered)
						{
							value.Covered = false;
							value.UIForm.OnReveal();
						}
						flag2 = true;
					}
				}
				linkedListNode = linkedListNode.Next;
			}
		}

		private bool TryGetUIFormInfo(UIForm uiForm, out UIFormInfo uiFormInfo)
		{
			if (uiForm != null)
			{
				for (LinkedListNode<UIFormInfo> linkedListNode = _uiFormInfos.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					UIFormInfo value = linkedListNode.Value;
					if (value.UIForm == uiForm)
					{
						uiFormInfo = value;
						return true;
					}
				}
			}
			uiFormInfo = null;
			return false;
		}

		internal void AddOrderOpenFormId(int formId)
		{
			_orderOpenFormList.AddFirst(formId);
		}

		private void RemoveOrderOpenFormId(int formId)
		{
			_orderOpenFormList.Remove(formId);
		}

		internal void AddCacheOrderOpenFormData(UIFormOpenCacheData cacheFormData)
		{
			_cacheWillOrderOpenFormDatas.Enqueue(cacheFormData);
		}

		internal void OpenCachedForm()
		{
			while (_cacheWillOrderOpenFormDatas.Count > 0)
			{
				UIFormOpenCacheData uIFormOpenCacheData = _cacheWillOrderOpenFormDatas.Dequeue();
				RemoveOrderOpenFormId(uIFormOpenCacheData.Form.Id);
				uIFormOpenCacheData.Form.OnOpen(uIFormOpenCacheData.UserData);
				UIMod.OpenSuccessEventArgs args = UIMod.OpenSuccessEventArgs.Make(uIFormOpenCacheData.Form, uIFormOpenCacheData.Duration, uIFormOpenCacheData.UserData);
				Mod.Event.Fire(this, args);
			}
		}

		internal bool HadOrderForm(UIForm uiForm)
		{
			LinkedListNode<int> first = _orderOpenFormList.First;
			return first.Value != uiForm.Id;
		}

		private void Awake()
		{
			_cachedCanvas = base.gameObject.GetOrAddComponent<Canvas>();
			_cachedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
			_cachedCanvas.worldCamera = Mod.UI.UICamera;
			_cachedCanvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
			_cachedCanvasScaler = base.gameObject.GetOrAddComponent<CanvasScaler>();
			_cachedCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			_cachedCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			_cachedCanvasScaler.referenceResolution = new Vector2(720f, 1280f);
			_cachedCanvas.overrideSorting = true;
			_cachedCanvas.sortingOrder = 10000 * _depth;
			RectTransform component = GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = Vector2.zero;
		}
	}
}
