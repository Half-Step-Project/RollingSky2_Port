using UnityEngine;

namespace Foundation
{
	public abstract class ScrollableDebuggerWindowBase : IDebuggerWindow
	{
		private const float TitleWidth = 240f;

		private Vector2 _scrollPosition = Vector2.zero;

		public virtual void OnInit(params object[] args)
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnLeave()
		{
		}

		public virtual void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		public void OnDraw()
		{
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
			OnDrawScrollableWindow();
			GUILayout.EndScrollView();
		}

		protected abstract void OnDrawScrollableWindow();

		protected void DrawItem(string title, string content)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(title, GUILayout.Width(240f));
			GUILayout.Label(content);
			GUILayout.EndHorizontal();
		}
	}
}
