using System;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Core")]
	public sealed class CoreMod : ModBase
	{
		[SerializeField]
		private bool _editorMode = true;

		[SerializeField]
		private SystemLanguage _editorLanguage;

		[SerializeField]
		[Range(30f, 120f)]
		private int _frameRate = 60;

		[Range(0f, 8f)]
		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private bool _runInBackground = true;

		[SerializeField]
		private bool _neverSleep;

		[SerializeField]
		private bool _enableLog = true;

		[SerializeField]
		private LogLevel _logLevel;

		private float _speedBeforePause = 1f;

		public bool EditorMode
		{
			get
			{
				if (Application.isEditor)
				{
					if (Application.isPlaying)
					{
						return _editorMode;
					}
					return true;
				}
				return false;
			}
		}

		public SystemLanguage EditorLanguage
		{
			get
			{
				if (!Application.isEditor)
				{
					Log.Error("EditorLanguage is only use in editor.");
				}
				return _editorLanguage;
			}
		}

		public int FrameRate
		{
			get
			{
				return _frameRate;
			}
			set
			{
				Application.targetFrameRate = (_frameRate = value);
			}
		}

		public float Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				Time.timeScale = (_speed = ((value > 0f) ? value : 0f));
			}
		}

		public bool IsPaused
		{
			get
			{
				return _speed <= 0f;
			}
		}

		public bool IsNormalGameSpeed
		{
			get
			{
				return Math.Abs(_speed - 1f) <= float.Epsilon;
			}
		}

		public bool RunInBackground
		{
			get
			{
				return _runInBackground;
			}
			set
			{
				Application.runInBackground = (_runInBackground = value);
			}
		}

		public bool NeverSleep
		{
			get
			{
				return _neverSleep;
			}
			set
			{
				_neverSleep = value;
				Screen.sleepTimeout = (_neverSleep ? (-1) : (-2));
			}
		}

		public bool EnableLog
		{
			get
			{
				return _enableLog;
			}
			set
			{
				Log.Enabled = (_enableLog = value);
			}
		}

		public LogLevel LogLevel
		{
			get
			{
				return _logLevel;
			}
			set
			{
				Log.Level = (_logLevel = value);
			}
		}

		public void Pause()
		{
			if (!IsPaused)
			{
				_speedBeforePause = Speed;
				Speed = 0f;
			}
		}

		public void Resume()
		{
			if (IsPaused)
			{
				Speed = _speedBeforePause;
			}
		}

		public void ResetToNormalSpeed()
		{
			if (!IsNormalGameSpeed)
			{
				Speed = 1f;
			}
		}

		protected override void Awake()
		{
			Mod.Core = this;
			ConvertUtility.ScreenDpi = Screen.dpi;
			Json.Handler = UnityJsonHandler.Default;
			Zip.Handler = IcsZipHandler.Default;
			if (Application.isEditor && _editorLanguage == SystemLanguage.Unknown)
			{
				_editorLanguage = Application.systemLanguage;
			}
			if (!Debug.isDebugBuild && _logLevel < LogLevel.Warning)
			{
				_logLevel = LogLevel.Warning;
			}
			Application.targetFrameRate = _frameRate;
			Time.timeScale = _speed;
			Application.runInBackground = _runInBackground;
			Screen.sleepTimeout = (_neverSleep ? (-1) : (-2));
			Log.Enabled = _enableLog;
			Log.Level = _logLevel;
			Input.multiTouchEnabled = false;
			Application.lowMemory += OnLowMemory;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		private void Update()
		{
			Mod.Tick(Time.deltaTime, Time.unscaledDeltaTime);
		}

		private void OnLowMemory()
		{
			Log.Warning("Low memory have been reported ...");
		}
	}
}
