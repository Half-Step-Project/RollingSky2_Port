using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	[Serializable]
	internal sealed class ConsoleWindow : IDebuggerWindow
	{
		private sealed class LogNode
		{
			[CompilerGenerated]
			private readonly DateTime _003CLogTime_003Ek__BackingField = DateTime.Now;

			[CompilerGenerated]
			private readonly LogType _003CLogType_003Ek__BackingField;

			[CompilerGenerated]
			private readonly string _003CLogMessage_003Ek__BackingField;

			[CompilerGenerated]
			private readonly string _003CStackTrack_003Ek__BackingField;

			public DateTime LogTime
			{
				[CompilerGenerated]
				get
				{
					return _003CLogTime_003Ek__BackingField;
				}
			}

			public LogType LogType
			{
				[CompilerGenerated]
				get
				{
					return _003CLogType_003Ek__BackingField;
				}
			}

			public string LogMessage
			{
				[CompilerGenerated]
				get
				{
					return _003CLogMessage_003Ek__BackingField;
				}
			}

			public string StackTrack
			{
				[CompilerGenerated]
				get
				{
					return _003CStackTrack_003Ek__BackingField;
				}
			}

			public LogNode(LogType logType, string logMessage, string stackTrack)
			{
				_003CLogType_003Ek__BackingField = logType;
				_003CLogMessage_003Ek__BackingField = logMessage;
				_003CStackTrack_003Ek__BackingField = stackTrack;
			}
		}

		private readonly LinkedList<LogNode> _logs = new LinkedList<LogNode>();

		private LinkedListNode<LogNode> _selectedNode;

		private Vector2 _logScrollPosition = Vector2.zero;

		private Vector2 _stackScrollPosition = Vector2.zero;

		private bool _errorFilter;

		private bool _lastLockScroll;

		private bool _lastInfoFilter;

		private bool _lastWarningFilter;

		private bool _lastErrorFilter;

		private bool _lastExceptionFilter;

		public int WarningCount { get; private set; }

		public int InfoCount { get; private set; }

		public bool LockScroll { get; set; }

		public bool InfoFilter { get; private set; }

		public int ErrorCount { get; private set; }

		public int ExceptionCount { get; private set; }

		public bool WarningFilter { get; private set; }

		public bool ExceptionFilter { get; private set; }

		public int MaxLine { get; set; }

		public string DateTimeFormat { get; set; }

		public Color32 InfoColor { get; set; }

		public Color32 WarningColor { get; set; }

		public Color32 ErrorColor { get; set; }

		public Color32 ExceptionColor { get; set; }

		public void OnInit(params object[] args)
		{
			Application.logMessageReceived += OnLogMessageReceived;
			LockScroll = (_lastLockScroll = Mod.Setting.GetBool("Debugger.Console.LockScroll"));
			InfoFilter = (_lastInfoFilter = Mod.Setting.GetBool("Debugger.Console.InfoFilter", true));
			WarningFilter = (_lastWarningFilter = Mod.Setting.GetBool("Debugger.Console.WarningFilter", true));
			_errorFilter = (_lastErrorFilter = Mod.Setting.GetBool("Debugger.Console.ErrorFilter", true));
			ExceptionFilter = (_lastExceptionFilter = Mod.Setting.GetBool("Debugger.Console.ExceptionFilter", true));
		}

		public void OnExit()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			Clear();
		}

		public void OnEnter()
		{
		}

		public void OnLeave()
		{
		}

		public void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("DebuggerMod.DebuggerWindowGroup.ConsoleWindow.OnTick");
			if (_lastLockScroll != LockScroll)
			{
				_lastLockScroll = LockScroll;
				Mod.Setting.SetBool("Debugger.Console.LockScroll", LockScroll);
			}
			if (_lastInfoFilter != InfoFilter)
			{
				_lastInfoFilter = InfoFilter;
				Mod.Setting.SetBool("Debugger.Console.InfoFilter", InfoFilter);
			}
			if (_lastWarningFilter != WarningFilter)
			{
				_lastWarningFilter = WarningFilter;
				Mod.Setting.SetBool("Debugger.Console.WarningFilter", WarningFilter);
			}
			if (_lastErrorFilter != _errorFilter)
			{
				_lastErrorFilter = _errorFilter;
				Mod.Setting.SetBool("Debugger.Console.ErrorFilter", _errorFilter);
			}
			if (_lastExceptionFilter != ExceptionFilter)
			{
				_lastExceptionFilter = ExceptionFilter;
				Mod.Setting.SetBool("Debugger.Console.ExceptionFilter", ExceptionFilter);
			}
			Profiler.EndSample();
		}

		public void OnDraw()
		{
			RefreshCount();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear All", GUILayout.Width(100f)))
			{
				Clear();
			}
			LockScroll = GUILayout.Toggle(LockScroll, "Lock Scroll", GUILayout.Width(90f));
			GUILayout.FlexibleSpace();
			InfoFilter = GUILayout.Toggle(InfoFilter, "Info (" + InfoCount + ")", GUILayout.Width(90f));
			WarningFilter = GUILayout.Toggle(WarningFilter, "Warning (" + WarningCount + ")", GUILayout.Width(100f));
			_errorFilter = GUILayout.Toggle(_errorFilter, "Error (" + ErrorCount + ")", GUILayout.Width(80f));
			ExceptionFilter = GUILayout.Toggle(ExceptionFilter, "Exception (" + ExceptionCount + ")", GUILayout.Width(120f));
			GUILayout.EndHorizontal();
			GUILayout.BeginVertical("box");
			if (LockScroll)
			{
				_logScrollPosition.y = float.MaxValue;
			}
			_logScrollPosition = GUILayout.BeginScrollView(_logScrollPosition);
			bool flag = false;
			for (LinkedListNode<LogNode> linkedListNode = _logs.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				switch (linkedListNode.Value.LogType)
				{
				case LogType.Log:
					if (!InfoFilter)
					{
						continue;
					}
					break;
				case LogType.Warning:
					if (!WarningFilter)
					{
						continue;
					}
					break;
				case LogType.Error:
					if (!_errorFilter)
					{
						continue;
					}
					break;
				case LogType.Exception:
					if (!ExceptionFilter)
					{
						continue;
					}
					break;
				}
				if (GUILayout.Toggle(_selectedNode == linkedListNode, GetLogString(linkedListNode.Value)))
				{
					flag = true;
					if (_selectedNode != linkedListNode)
					{
						_selectedNode = linkedListNode;
						_stackScrollPosition = Vector2.zero;
					}
				}
			}
			if (!flag)
			{
				_selectedNode = null;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			_stackScrollPosition = GUILayout.BeginScrollView(_stackScrollPosition, GUILayout.Height(100f));
			if (_selectedNode != null)
			{
				GUILayout.BeginHorizontal();
				Color32 logStringColor = GetLogStringColor(_selectedNode.Value.LogType);
				GUILayout.Label(string.Format("<color=#{0:x2}{1:x2}{2:x2}{3:x2}><b>{4}</b></color>", logStringColor.r, logStringColor.g, logStringColor.b, logStringColor.a, _selectedNode.Value.LogMessage));
				if (GUILayout.Button("COPY", GUILayout.Width(60f), GUILayout.Height(30f)))
				{
					TextEditor textEditor = new TextEditor
					{
						text = _selectedNode.Value.LogMessage + "\n\n" + _selectedNode.Value.StackTrack
					};
					textEditor.OnFocus();
					textEditor.Copy();
				}
				GUILayout.EndHorizontal();
				GUILayout.Label(_selectedNode.Value.StackTrack);
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}

		public void RefreshCount()
		{
			InfoCount = 0;
			WarningCount = 0;
			ErrorCount = 0;
			ExceptionCount = 0;
			for (LinkedListNode<LogNode> linkedListNode = _logs.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				switch (linkedListNode.Value.LogType)
				{
				case LogType.Log:
					InfoCount++;
					break;
				case LogType.Warning:
					WarningCount++;
					break;
				case LogType.Error:
					ErrorCount++;
					break;
				case LogType.Exception:
					ExceptionCount++;
					break;
				}
			}
		}

		internal Color32 GetLogStringColor(LogType logType)
		{
			Color32 result = Color.white;
			switch (logType)
			{
			case LogType.Log:
				result = InfoColor;
				break;
			case LogType.Warning:
				result = WarningColor;
				break;
			case LogType.Error:
				result = ErrorColor;
				break;
			case LogType.Exception:
				result = ExceptionColor;
				break;
			}
			return result;
		}

		private void Clear()
		{
			_logs.Clear();
		}

		private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
		{
			if (logType == LogType.Assert)
			{
				logType = LogType.Error;
			}
			_logs.AddLast(new LogNode(logType, logMessage, stackTrace));
			while (_logs.Count > MaxLine)
			{
				_logs.RemoveFirst();
			}
		}

		private string GetLogString(LogNode logNode)
		{
			Color32 logStringColor = GetLogStringColor(logNode.LogType);
			return string.Format("<color=#{0:x2}{1:x2}{2:x2}{3:x2}>{4}{5}</color>", logStringColor.r, logStringColor.g, logStringColor.b, logStringColor.a, logNode.LogTime.ToString(DateTimeFormat), logNode.LogMessage);
		}
	}
}
