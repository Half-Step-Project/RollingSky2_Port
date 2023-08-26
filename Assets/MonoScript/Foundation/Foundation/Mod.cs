using UnityEngine;
using UnityEngine.SceneManagement;

namespace Foundation
{
	public static class Mod
	{
		private enum RunningStatus
		{
			IsRebooting = -1,
			IsRunning = 0,
			IsExiting = 1
		}

		public const int ModLevelId = 0;

		public const string ModLevelName = "Launch";

		private static RunningStatus _runningStatus;

		public static CoreMod Core { get; internal set; }

		public static ReferenceMod Reference { get; internal set; }

		public static SettingMod Setting { get; internal set; }

		public static EventMod Event { get; internal set; }

		public static NetworkMod Network { get; internal set; }

		public static DownloadMod Download { get; internal set; }

		public static WebRequestMod WebRequest { get; internal set; }

		public static ObjectPoolMod ObjectPool { get; internal set; }

		public static ResourceMod Resource { get; internal set; }

		public static ConfigMod Config { get; internal set; }

		public static DataTableMod DataTable { get; internal set; }

		public static LocalizationMod Localization { get; internal set; }

		public static FsmMod Fsm { get; internal set; }

		public static EntityMod Entity { get; internal set; }

		public static SceneMod Scene { get; internal set; }

		public static UIMod UI { get; internal set; }

		public static DebuggerMod Debugger { get; internal set; }

		public static SoundMod Sound { get; internal set; }

		public static ProcedureMod Procedure { get; internal set; }

		public static event Action InitEx;

		public static void Init()
		{
			if (Reference != null)
			{
				Reference.OnInit();
			}
			if (Setting != null)
			{
				Setting.OnInit();
			}
			if (Event != null)
			{
				Event.OnInit();
			}
			if (Network != null)
			{
				Network.OnInit();
			}
			if (Download != null)
			{
				Download.OnInit();
			}
			if (WebRequest != null)
			{
				WebRequest.OnInit();
			}
			if (ObjectPool != null)
			{
				ObjectPool.OnInit();
			}
			if (Resource != null)
			{
				Resource.OnInit();
			}
			if (Config != null)
			{
				Config.OnInit();
			}
			if (DataTable != null)
			{
				DataTable.OnInit();
			}
			if (Localization != null)
			{
				Localization.OnInit();
			}
			if (Fsm != null)
			{
				Fsm.OnInit();
			}
			if (Entity != null)
			{
				Entity.OnInit();
			}
			if (Scene != null)
			{
				Scene.OnInit();
			}
			if (UI != null)
			{
				UI.OnInit();
			}
			if (Sound != null)
			{
				Sound.OnInit();
			}
			if (Procedure != null)
			{
				Procedure.OnInit();
			}
			if (Debugger != null)
			{
				Debugger.OnInit();
			}
			if (Core != null)
			{
				Core.OnInit();
			}
			Action initEx = Mod.InitEx;
			if (initEx != null)
			{
				initEx();
			}
		}

		internal static void Tick(float elapseSeconds, float realElapseSeconds)
		{
			if (Event != null)
			{
				Event.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Network != null)
			{
				Network.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Download != null)
			{
				Download.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (WebRequest != null)
			{
				WebRequest.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Resource != null)
			{
				Resource.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Config != null)
			{
				Config.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (DataTable != null)
			{
				DataTable.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Localization != null)
			{
				Localization.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Fsm != null)
			{
				Fsm.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Entity != null)
			{
				Entity.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Scene != null)
			{
				Scene.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (UI != null)
			{
				UI.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Sound != null)
			{
				Sound.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (ObjectPool != null)
			{
				ObjectPool.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Setting != null)
			{
				Setting.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Reference != null)
			{
				Reference.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Debugger != null)
			{
				Debugger.OnTick(elapseSeconds, realElapseSeconds);
			}
			if (Procedure != null)
			{
				Procedure.OnTick(elapseSeconds, realElapseSeconds);
			}
		}

		public static void Exit()
		{
			if (Procedure != null)
			{
				Procedure.OnExit();
				Procedure = null;
			}
			if (Debugger != null)
			{
				Debugger.OnExit();
				Debugger = null;
			}
			if (Network != null)
			{
				Network.OnExit();
				Network = null;
			}
			if (Download != null)
			{
				Download.OnExit();
				Download = null;
			}
			if (WebRequest != null)
			{
				WebRequest.OnExit();
				WebRequest = null;
			}
			if (UI != null)
			{
				UI.OnExit();
				UI = null;
			}
			if (Entity != null)
			{
				Entity.OnExit();
				Entity = null;
			}
			if (Scene != null)
			{
				Scene.OnExit();
				Scene = null;
			}
			if (Sound != null)
			{
				Sound.OnExit();
				Sound = null;
			}
			if (Fsm != null)
			{
				Fsm.OnExit();
				Fsm = null;
			}
			if (Localization != null)
			{
				Localization.OnExit();
				Localization = null;
			}
			if (DataTable != null)
			{
				DataTable.OnExit();
				DataTable = null;
			}
			if (Config != null)
			{
				Config.OnExit();
				Config = null;
			}
			if (Resource != null)
			{
				Resource.OnExit();
				Resource = null;
			}
			if (Event != null)
			{
				Event.OnExit();
				Event = null;
			}
			if (ObjectPool != null)
			{
				ObjectPool.OnExit();
				ObjectPool = null;
			}
			if (Setting != null)
			{
				Setting.OnExit();
				Setting = null;
			}
			if (Reference != null)
			{
				Reference.OnExit();
				Reference = null;
			}
			if (Core != null)
			{
				Core.OnExit();
				Core = null;
			}
		}

		public static void Reboot()
		{
			if (_runningStatus == RunningStatus.IsRunning)
			{
				_runningStatus = RunningStatus.IsRebooting;
				SceneManager.LoadScene(0);
			}
		}

		public static void Shutdown()
		{
			if (_runningStatus == RunningStatus.IsRunning)
			{
				_runningStatus = RunningStatus.IsExiting;
				Application.Quit();
			}
		}
	}
}
