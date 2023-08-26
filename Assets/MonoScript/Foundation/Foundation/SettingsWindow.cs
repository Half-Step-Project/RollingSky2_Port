using System;
using UnityEngine;

namespace Foundation
{
	internal sealed class SettingsWindow : ScrollableDebuggerWindowBase
	{
		private float _lastIconX;

		private float _lastIconY;

		private float _lastWindowX;

		private float _lastWindowY;

		private float _lastWindowWidth;

		private float _lastWindowHeight;

		public override void OnInit(params object[] args)
		{
			_lastIconX = Mod.Setting.GetFloat("Debugger.Icon.X", DebuggerMod.DefaultIconRect.x);
			_lastIconY = Mod.Setting.GetFloat("Debugger.Icon.Y", DebuggerMod.DefaultIconRect.y);
			_lastWindowX = Mod.Setting.GetFloat("Debugger.Window.X", DebuggerMod.DefaultWindowRect.x);
			_lastWindowY = Mod.Setting.GetFloat("Debugger.Window.Y", DebuggerMod.DefaultWindowRect.y);
			_lastWindowWidth = Mod.Setting.GetFloat("Debugger.Window.Width", DebuggerMod.DefaultWindowRect.width);
			_lastWindowHeight = Mod.Setting.GetFloat("Debugger.Window.Height", DebuggerMod.DefaultWindowRect.height);
			Mod.Debugger.IconRect = new Rect(_lastIconX, _lastIconY, DebuggerMod.DefaultIconRect.width, DebuggerMod.DefaultIconRect.height);
			Mod.Debugger.WindowRect = new Rect(_lastWindowX, _lastWindowY, _lastWindowWidth, _lastWindowHeight);
		}

		public override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("DebuggerMod.DebuggerWindowGroup.SettingsWindow.OnTick");
			if (Math.Abs(_lastIconX - Mod.Debugger.IconRect.x) >= float.Epsilon)
			{
				_lastIconX = Mod.Debugger.IconRect.x;
				Mod.Setting.SetFloat("Debugger.Icon.X", Mod.Debugger.IconRect.x);
			}
			if (Math.Abs(_lastIconY - Mod.Debugger.IconRect.y) >= float.Epsilon)
			{
				_lastIconY = Mod.Debugger.IconRect.y;
				Mod.Setting.SetFloat("Debugger.Icon.Y", Mod.Debugger.IconRect.y);
			}
			if (Math.Abs(_lastWindowX - Mod.Debugger.WindowRect.x) >= float.Epsilon)
			{
				_lastWindowX = Mod.Debugger.WindowRect.x;
				Mod.Setting.SetFloat("Debugger.Window.X", Mod.Debugger.WindowRect.x);
			}
			if (Math.Abs(_lastWindowY - Mod.Debugger.WindowRect.y) >= float.Epsilon)
			{
				_lastWindowY = Mod.Debugger.WindowRect.y;
				Mod.Setting.SetFloat("Debugger.Window.Y", Mod.Debugger.WindowRect.y);
			}
			if (Math.Abs(_lastWindowWidth - Mod.Debugger.WindowRect.width) >= float.Epsilon)
			{
				_lastWindowWidth = Mod.Debugger.WindowRect.width;
				Mod.Setting.SetFloat("Debugger.Window.Width", Mod.Debugger.WindowRect.width);
			}
			if (Math.Abs(_lastWindowHeight - Mod.Debugger.WindowRect.height) >= float.Epsilon)
			{
				_lastWindowHeight = Mod.Debugger.WindowRect.height;
				Mod.Setting.SetFloat("Debugger.Window.Height", Mod.Debugger.WindowRect.height);
			}
			Profiler.EndSample();
		}

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Window Settings</b>");
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Position:", GUILayout.Width(60f));
			GUILayout.Label("Drag window caption to move position.");
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			float num = Mod.Debugger.WindowRect.width;
			GUILayout.Label("Width:", GUILayout.Width(60f));
			if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
			{
				num -= 1f;
			}
			num = GUILayout.HorizontalSlider(num, 100f, (float)Screen.width - 20f);
			if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
			{
				num += 1f;
			}
			num = Mathf.Clamp(num, 100f, (float)Screen.width - 20f);
			if (Math.Abs(num - Mod.Debugger.WindowRect.width) >= float.Epsilon)
			{
				Rect windowRect = Mod.Debugger.WindowRect;
				windowRect.width = num;
				Mod.Debugger.WindowRect = windowRect;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			float num2 = Mod.Debugger.WindowRect.height;
			GUILayout.Label("Height:", GUILayout.Width(60f));
			if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
			{
				num2 -= 1f;
			}
			num2 = GUILayout.HorizontalSlider(num2, 100f, (float)Screen.height - 20f);
			if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
			{
				num2 += 1f;
			}
			num2 = Mathf.Clamp(num2, 100f, (float)Screen.height - 20f);
			if (Math.Abs(num2 - Mod.Debugger.WindowRect.height) >= float.Epsilon)
			{
				Rect windowRect2 = Mod.Debugger.WindowRect;
				windowRect2.height = num2;
				Mod.Debugger.WindowRect = windowRect2;
			}
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Reset Window Settings", GUILayout.Height(30f)))
			{
				Mod.Debugger.IconRect = DebuggerMod.DefaultIconRect;
				Mod.Debugger.WindowRect = DebuggerMod.DefaultWindowRect;
			}
			GUILayout.EndVertical();
		}
	}
}
