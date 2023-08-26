using UnityEngine;

namespace Foundation
{
	public abstract class UIFormLogic : MonoBehaviour
	{
		public UIForm UIForm
		{
			get
			{
				return GetComponent<UIForm>();
			}
		}

		public string Name
		{
			get
			{
				return base.gameObject.name;
			}
			set
			{
				base.gameObject.name = value;
			}
		}

		public bool IsAvailable
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		public Transform CachedTransform { get; private set; }

		protected internal virtual void OnInit(object userData)
		{
			if (CachedTransform == null)
			{
				CachedTransform = base.transform;
			}
		}

		protected internal virtual void OnOpen(object userData)
		{
		}

		protected internal virtual void OnClose(object userData)
		{
		}

		protected internal virtual void OnPause()
		{
		}

		protected internal virtual void OnResume()
		{
		}

		protected internal virtual void OnCover()
		{
		}

		protected internal virtual void OnReveal()
		{
		}

		protected internal virtual void OnRefocus(object userData)
		{
		}

		protected internal virtual void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		protected internal virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
		{
		}

		protected internal virtual void OnUnload()
		{
		}
	}
}
