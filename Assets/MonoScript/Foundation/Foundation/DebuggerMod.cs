using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	[AddComponentMenu("Framework/Debugger")]
	[DisallowMultipleComponent]
	public sealed class DebuggerMod : ModBase
	{
		[Serializable]
		private sealed class ConsoleInfo
		{
			[SerializeField]
			private int _maxLine = 300;

			[SerializeField]
			private string _dateTimeFormat = "[HH:mm:ss.fff] ";

			[SerializeField]
			private Color32 _infoColor = Color.white;

			[SerializeField]
			private Color32 _warningColor = Color.yellow;

			[SerializeField]
			private Color32 _errorColor = Color.red;

			[SerializeField]
			private Color32 _exceptionColor = new Color(0.7f, 0.2f, 0.2f);

			public int MaxLine
			{
				get
				{
					return _maxLine;
				}
			}

			public string DateTimeFormat
			{
				get
				{
					return _dateTimeFormat;
				}
			}

			public Color32 InfoColor
			{
				get
				{
					return _infoColor;
				}
			}

			public Color32 WarningColor
			{
				get
				{
					return _warningColor;
				}
			}

			public Color32 ErrorColor
			{
				get
				{
					return _errorColor;
				}
			}

			public Color32 ExceptionColor
			{
				get
				{
					return _exceptionColor;
				}
			}
		}

		private sealed class DebuggerWindowGroup : IDebuggerWindowGroup, IDebuggerWindow
		{
			private readonly List<IDebuggerWindow> _windows = new List<IDebuggerWindow>();

			private readonly List<string> _windowNames = new List<string>();

			public int WindowCount
			{
				get
				{
					return _windows.Count;
				}
			}

			public int SelectedIndex { get; set; }

			public IDebuggerWindow SelectedWindow
			{
				get
				{
					if (SelectedIndex < _windows.Count)
					{
						return _windows[SelectedIndex];
					}
					return null;
				}
			}

			public string[] WindowNames
			{
				get
				{
					return _windowNames.ToArray();
				}
			}

			public void OnInit(params object[] args)
			{
			}

			public void OnExit()
			{
				for (int i = 0; i < _windows.Count; i++)
				{
					_windows[i].OnExit();
				}
				_windows.Clear();
				_windowNames.Clear();
			}

			public void OnEnter()
			{
				SelectedWindow.OnEnter();
			}

			public void OnLeave()
			{
				SelectedWindow.OnLeave();
			}

			public void OnTick(float elapseSeconds, float realElapseSeconds)
			{
				Profiler.BeginSample("DebuggerMod.DebuggerWindowGroup.OnTick");
				SelectedWindow.OnTick(elapseSeconds, realElapseSeconds);
				Profiler.EndSample();
			}

			public void OnDraw()
			{
			}

			public IDebuggerWindow GetWindow(string path)
			{
				if (string.IsNullOrEmpty(path))
				{
					return null;
				}
				int num = path.IndexOf('/');
				if (num < 0 || num >= path.Length - 1)
				{
					return InternalGetWindow(path);
				}
				string name = path.Substring(0, num);
				string path2 = path.Substring(num + 1);
				DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetWindow(name);
				if (debuggerWindowGroup == null)
				{
					return null;
				}
				return debuggerWindowGroup.GetWindow(path2);
			}

			public bool SelectWindow(string path)
			{
				if (string.IsNullOrEmpty(path))
				{
					return false;
				}
				int num = path.IndexOf('/');
				if (num < 0 || num >= path.Length - 1)
				{
					return InternalSelectWindow(path);
				}
				string name = path.Substring(0, num);
				string path2 = path.Substring(num + 1);
				DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetWindow(name);
				if (debuggerWindowGroup == null || !InternalSelectWindow(name))
				{
					return false;
				}
				return debuggerWindowGroup.SelectWindow(path2);
			}

			public void RegisterWindow(string path, IDebuggerWindow window)
			{
				if (string.IsNullOrEmpty(path))
				{
					return;
				}
				int num = path.IndexOf('/');
				if (num < 0 || num >= path.Length - 1)
				{
					if (InternalGetWindow(path) == null)
					{
						_windows.Add(window);
						_windowNames.Add(path);
					}
					return;
				}
				string text = path.Substring(0, num);
				string path2 = path.Substring(num + 1);
				DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetWindow(text);
				if (debuggerWindowGroup == null)
				{
					if (InternalGetWindow(text) != null)
					{
						Log.Warning("Debugger window has been registered, can not create debugger window group.");
						return;
					}
					debuggerWindowGroup = new DebuggerWindowGroup();
					_windows.Add(debuggerWindowGroup);
					_windowNames.Add(text);
				}
				debuggerWindowGroup.RegisterWindow(path2, window);
			}

			private IDebuggerWindow InternalGetWindow(string name)
			{
				for (int i = 0; i < _windowNames.Count; i++)
				{
					if (_windowNames[i] == name)
					{
						return _windows[i];
					}
				}
				return null;
			}

			private bool InternalSelectWindow(string name)
			{
				for (int i = 0; i < _windowNames.Count; i++)
				{
					if (!(_windowNames[i] != name))
					{
						SelectedIndex = i;
						return true;
					}
				}
				return false;
			}
		}

		[SerializeField]
		private DebuggerActiveWindowType _activeWindowType;

		[SerializeField]
		private ConsoleInfo _consoleInfo = new ConsoleInfo();

		public static readonly Rect DefaultIconRect = new Rect(10f, 10f, 60f, 60f);

		public static readonly Rect DefaultWindowRect = new Rect(10f, 10f, 640f, 480f);

		private static readonly string[] _fpsText = new string[100]
		{
			"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
			"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
			"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
			"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
			"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
			"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
			"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
			"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
			"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
			"90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
		};

		private readonly FpsCounter _fpsCounter = new FpsCounter(0.5f);

		private readonly ConsoleWindow _consoleWindow = new ConsoleWindow();

		private readonly SystemInformationWindow _systemInfoWindow = new SystemInformationWindow();

		private readonly EnvironmentInformationWindow _envInfoWindow = new EnvironmentInformationWindow();

		private readonly ScreenInformationWindow _screenInfoWindow = new ScreenInformationWindow();

		private readonly GraphicsInformationWindow _graphicsInfoWindow = new GraphicsInformationWindow();

		private readonly InputSummaryInformationWindow _inputSummaryInfoWindow = new InputSummaryInformationWindow();

		private readonly InputTouchInformationWindow _inputTouchInfoWindow = new InputTouchInformationWindow();

		private readonly InputAccelerationInformationWindow _inputAccInfoWindow = new InputAccelerationInformationWindow();

		private readonly InputGyroscopeInformationWindow _inputGyroscopeInfoWindow = new InputGyroscopeInformationWindow();

		private readonly InputCompassInformationWindow _inputCompassInfoWindow = new InputCompassInformationWindow();

		private readonly PathInformationWindow _pathInfoWindow = new PathInformationWindow();

		private readonly SceneInformationWindow _sceneInfoWindow = new SceneInformationWindow();

		private readonly TimeInformationWindow _timeInfoWindow = new TimeInformationWindow();

		private readonly QualityInformationWindow _qualityInfoWindow = new QualityInformationWindow();

		private readonly ProfilerInformationWindow _profilerInfoWindow = new ProfilerInformationWindow();

		private readonly RuntimeMemoryInformationWindow<UnityEngine.Object> _runtimeMemAllInfoWindow = new RuntimeMemoryInformationWindow<UnityEngine.Object>();

		private readonly RuntimeMemoryInformationWindow<Texture> _runtimeMemTextureInfoWindow = new RuntimeMemoryInformationWindow<Texture>();

		private readonly RuntimeMemoryInformationWindow<Mesh> _runtimeMemMeshInfWindow = new RuntimeMemoryInformationWindow<Mesh>();

		private readonly RuntimeMemoryInformationWindow<Material> _runtimeMemMaterialInfoWindow = new RuntimeMemoryInformationWindow<Material>();

		private readonly RuntimeMemoryInformationWindow<AnimationClip> _runtimeMemAnimationClipInfoWindow = new RuntimeMemoryInformationWindow<AnimationClip>();

		private readonly RuntimeMemoryInformationWindow<AudioClip> _runtimeMemAudioClipInfoWindow = new RuntimeMemoryInformationWindow<AudioClip>();

		private readonly RuntimeMemoryInformationWindow<Font> _runtimeMemFontInfoWindow = new RuntimeMemoryInformationWindow<Font>();

		private readonly RuntimeMemoryInformationWindow<GameObject> _runtimeMemGameObjectInfoWindow = new RuntimeMemoryInformationWindow<GameObject>();

		private readonly RuntimeMemoryInformationWindow<Component> _runtimeMemComponentInfoWindow = new RuntimeMemoryInformationWindow<Component>();

		private readonly ObjectPoolInformationWindow _objectPoolInfoWindow = new ObjectPoolInformationWindow();

		private readonly SettingsWindow _settingsWindow = new SettingsWindow();

		private readonly OperationsWindow _operationsWindow = new OperationsWindow();

		private readonly DebuggerWindowGroup _windowRoot = new DebuggerWindowGroup();

		private readonly Rect _dragRect = new Rect(0f, 0f, float.MaxValue, 25f);

		private bool _activeWindow;

		private bool _showWindow;

		private Rect _iconRect = DefaultIconRect;

		private Rect _windowRect = DefaultWindowRect;

		private bool _lastEnableInput;

		public Rect IconRect
		{
			get
			{
				return _iconRect;
			}
			set
			{
				_iconRect = value;
			}
		}

		public Rect WindowRect
		{
			get
			{
				return _windowRect;
			}
			set
			{
				_windowRect = value;
			}
		}

		public bool ActiveWindow
		{
			get
			{
				return _activeWindow;
			}
			set
			{
				_activeWindow = value;
			}
		}

		public IDebuggerWindowGroup WindowRoot
		{
			get
			{
				return _windowRoot;
			}
		}

		public void RegisterWindow(string path, IDebuggerWindow window, params object[] args)
		{
			if (string.IsNullOrEmpty(path))
			{
				Log.Warning("Path is invalid.");
				return;
			}
			if (window == null)
			{
				Log.Error("Debugger window is invalid.");
				return;
			}
			_windowRoot.RegisterWindow(path, window);
			window.OnInit(args);
		}

		public IDebuggerWindow GetWindow(string path)
		{
			return _windowRoot.GetWindow(path);
		}

		public bool SelectWindow(string path)
		{
			return _windowRoot.SelectWindow(path);
		}

		protected override void Awake()
		{
			Mod.Debugger = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			RegisterWindow("Console", _consoleWindow);
			RegisterWindow("Information/System", _systemInfoWindow);
			RegisterWindow("Information/Environment", _envInfoWindow);
			RegisterWindow("Information/Screen", _screenInfoWindow);
			RegisterWindow("Information/Graphics", _graphicsInfoWindow);
			RegisterWindow("Information/Input/Summary", _inputSummaryInfoWindow);
			RegisterWindow("Information/Input/Touch", _inputTouchInfoWindow);
			RegisterWindow("Information/Input/Acceleration", _inputAccInfoWindow);
			RegisterWindow("Information/Input/Gyroscope", _inputGyroscopeInfoWindow);
			RegisterWindow("Information/Input/Compass", _inputCompassInfoWindow);
			RegisterWindow("Information/Other/Scene", _sceneInfoWindow);
			RegisterWindow("Information/Other/Path", _pathInfoWindow);
			RegisterWindow("Information/Other/Time", _timeInfoWindow);
			RegisterWindow("Information/Other/Quality", _qualityInfoWindow);
			RegisterWindow("Profiler/Summary", _profilerInfoWindow);
			RegisterWindow("Profiler/Memory/All", _runtimeMemAllInfoWindow);
			RegisterWindow("Profiler/Memory/Texture", _runtimeMemTextureInfoWindow);
			RegisterWindow("Profiler/Memory/Mesh", _runtimeMemMeshInfWindow);
			RegisterWindow("Profiler/Memory/Material", _runtimeMemMaterialInfoWindow);
			RegisterWindow("Profiler/Memory/AnimationClip", _runtimeMemAnimationClipInfoWindow);
			RegisterWindow("Profiler/Memory/AudioClip", _runtimeMemAudioClipInfoWindow);
			RegisterWindow("Profiler/Memory/Font", _runtimeMemFontInfoWindow);
			RegisterWindow("Profiler/Memory/GameObject", _runtimeMemGameObjectInfoWindow);
			RegisterWindow("Profiler/Memory/Component", _runtimeMemComponentInfoWindow);
			RegisterWindow("Profiler/Object Pool", _objectPoolInfoWindow);
			RegisterWindow("Other/Settings", _settingsWindow);
			RegisterWindow("Other/Operations", _operationsWindow);
			_consoleWindow.MaxLine = _consoleInfo.MaxLine;
			_consoleWindow.DateTimeFormat = _consoleInfo.DateTimeFormat;
			_consoleWindow.InfoColor = _consoleInfo.InfoColor;
			_consoleWindow.WarningColor = _consoleInfo.WarningColor;
			_consoleWindow.ErrorColor = _consoleInfo.ErrorColor;
			_consoleWindow.ExceptionColor = _consoleInfo.ExceptionColor;
			if (_activeWindowType == DebuggerActiveWindowType.Auto)
			{
				_activeWindow = Debug.isDebugBuild;
			}
			else
			{
				_activeWindow = _activeWindowType == DebuggerActiveWindowType.Open;
			}
			base.enabled = _activeWindow;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("DebuggerMod.OnTick");
			_fpsCounter.Tick(elapseSeconds, realElapseSeconds);
			if (_activeWindow && _showWindow)
			{
				_windowRoot.OnTick(elapseSeconds, realElapseSeconds);
			}
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			_activeWindow = false;
			_windowRoot.OnExit();
		}

		private void OnValidate()
		{
			if (_activeWindowType == DebuggerActiveWindowType.Auto)
			{
				_activeWindow = Debug.isDebugBuild;
			}
			else
			{
				_activeWindow = _activeWindowType == DebuggerActiveWindowType.Open;
			}
			base.enabled = _activeWindow;
		}

		private void DrawWindow(int windowId)
		{
			GUI.DragWindow(_dragRect);
			DrawDebuggerWindowGroup(_windowRoot);
		}

		private void DrawDebuggerWindowGroup(IDebuggerWindowGroup windowGroup)
		{
			if (windowGroup == null)
			{
				return;
			}
			List<string> list = new List<string>();
			string[] windowNames = windowGroup.WindowNames;
			for (int i = 0; i < windowNames.Length; i++)
			{
				list.Add("<b>" + windowNames[i] + "</b>");
			}
			if (windowGroup == _windowRoot)
			{
				list.Add("<b>Close</b>");
			}
			int num = GUILayout.Toolbar(windowGroup.SelectedIndex, list.ToArray(), GUILayout.Height(30f), GUILayout.MaxWidth(Screen.width));
			if (num >= windowGroup.WindowCount)
			{
				_showWindow = false;
				UIInputModule.UIInput.EnableInput = _lastEnableInput;
				return;
			}
			if (windowGroup.SelectedIndex != num)
			{
				windowGroup.SelectedWindow.OnLeave();
				windowGroup.SelectedIndex = num;
				windowGroup.SelectedWindow.OnEnter();
			}
			IDebuggerWindowGroup windowGroup2;
			if ((windowGroup2 = windowGroup.SelectedWindow as IDebuggerWindowGroup) != null)
			{
				DrawDebuggerWindowGroup(windowGroup2);
			}
			IDebuggerWindow selectedWindow = windowGroup.SelectedWindow;
			if (selectedWindow != null)
			{
				selectedWindow.OnDraw();
			}
		}

		private void DrawIconWindow(int windowId)
		{
			GUI.DragWindow(_dragRect);
			GUILayout.Space(5f);
			_consoleWindow.RefreshCount();
			Color32 color = ((_consoleWindow.ExceptionCount > 0) ? _consoleWindow.GetLogStringColor(LogType.Exception) : ((_consoleWindow.ErrorCount > 0) ? _consoleWindow.GetLogStringColor(LogType.Error) : ((_consoleWindow.WarningCount <= 0) ? _consoleWindow.GetLogStringColor(LogType.Log) : _consoleWindow.GetLogStringColor(LogType.Warning))));
			Color contentColor = GUI.contentColor;
			GUI.contentColor = color;
			string text = _fpsText[Mathf.RoundToInt(_fpsCounter.CurrentFps) % _fpsText.Length];
			bool flag = GUILayout.Button(text, GUILayout.Width(100f), GUILayout.Height(40f));
			GUI.contentColor = contentColor;
			if (flag)
			{
				_showWindow = true;
				_lastEnableInput = UIInputModule.UIInput.EnableInput;
				UIInputModule.UIInput.EnableInput = false;
			}
		}

		private void OnGUI()
		{
			if (_activeWindow && !(UIInputModule.UIInput == null))
			{
				if (_showWindow)
				{
					_windowRect = GUILayout.Window(0, _windowRect, DrawWindow, "<b>DEBUGGER</b>");
				}
				else
				{
					_iconRect = GUILayout.Window(0, _iconRect, DrawIconWindow, "<b>DEBUGGER</b>");
				}
			}
		}
	}
}
