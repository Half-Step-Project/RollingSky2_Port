using UnityEngine;

namespace Foundation
{
	public sealed class UIForm : MonoBehaviour
	{
		private bool _active;

		public int DepthInGroup { get; private set; }

		public int Id { get; private set; }

		public string AssetName { get; private set; }

		public bool PauseCovered { get; private set; }

		public UIGroup Group { get; private set; }

		public UIFormLogic Logic { get; private set; }

		internal void OnInit(int id, string assetName, UIGroup group, bool pauseCovered, bool isNew, bool hidden, object userData)
		{
			Id = id;
			AssetName = assetName;
			if (isNew)
			{
				Group = group;
			}
			else if (Group != group)
			{
				Log.Error("UIGroup is inconsistent for non-new-instance UI form.");
				return;
			}
			DepthInGroup = 0;
			PauseCovered = pauseCovered;
			Active(!hidden);
			if (isNew)
			{
				Logic = GetComponent<UIFormLogic>();
				if (Logic == null)
				{
					Log.Error("Can not get UIForm logic.");
				}
				else
				{
					Logic.OnInit(userData);
				}
			}
		}

		internal void OnRecycle()
		{
			Id = -1;
			DepthInGroup = 0;
			PauseCovered = true;
		}

		internal void OnOpen(object userData)
		{
			Logic.OnOpen(userData);
			Active(_active);
		}

		internal void OnClose(object userData)
		{
			Logic.OnClose(userData);
			Active(false);
		}

		internal void OnPause()
		{
			Logic.OnPause();
			Active(false);
		}

		internal void OnResume()
		{
			Logic.OnResume();
			Active(true);
		}

		internal void OnCover()
		{
			Logic.OnCover();
		}

		internal void OnReveal()
		{
			Logic.OnReveal();
		}

		internal void OnRefocus(object userData)
		{
			Logic.OnRefocus(userData);
		}

		internal void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("UIMod.UIForm.OnTick");
			Logic.OnTick(elapseSeconds, realElapseSeconds);
			Profiler.EndSample();
		}

		internal void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
		{
			DepthInGroup = depthInUIGroup;
			Logic.OnDepthChanged(uiGroupDepth, depthInUIGroup);
		}

		internal void OnUnload()
		{
			if (Logic != null)
			{
				Logic.OnUnload();
			}
		}

		internal void Active(bool active)
		{
			if (_active != active)
			{
				_active = active;
				base.gameObject.SetActive(_active);
			}
		}
	}
}
