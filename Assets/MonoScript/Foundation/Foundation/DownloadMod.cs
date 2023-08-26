using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Foundation
{
	[AddComponentMenu("Framework/Download")]
	[DisallowMultipleComponent]
	public sealed class DownloadMod : ModBase
	{
		private sealed class DownloadTask : Task
		{
			[CompilerGenerated]
			private readonly string _003CPath_003Ek__BackingField;

			[CompilerGenerated]
			private readonly string _003CUri_003Ek__BackingField;

			[CompilerGenerated]
			private readonly float _003CTimeout_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CUserData_003Ek__BackingField;

			public string Path
			{
				[CompilerGenerated]
				get
				{
					return _003CPath_003Ek__BackingField;
				}
			}

			public string Uri
			{
				[CompilerGenerated]
				get
				{
					return _003CUri_003Ek__BackingField;
				}
			}

			public float Timeout
			{
				[CompilerGenerated]
				get
				{
					return _003CTimeout_003Ek__BackingField;
				}
			}

			public object UserData
			{
				[CompilerGenerated]
				get
				{
					return _003CUserData_003Ek__BackingField;
				}
			}

			public DownloadTask(string path, string uri, float timeout, object userData)
			{
				_003CPath_003Ek__BackingField = path;
				_003CUri_003Ek__BackingField = uri;
				_003CTimeout_003Ek__BackingField = timeout;
				_003CUserData_003Ek__BackingField = userData;
			}
		}

		private sealed class DownloadAgent : ITaskAgent<DownloadTask>
		{
			private UnityWebRequest _webRequest;

			private float _waitTime;

			private int _downloadedSize;

			public DownloadTask Task { get; private set; }

			void ITaskAgent<DownloadTask>.Init()
			{
			}

			void ITaskAgent<DownloadTask>.Boot(DownloadTask downloadTask)
			{
				Task = downloadTask;
				_waitTime = 0f;
				_downloadedSize = 0;
				Mod.Download.OnDownloadStart(this);
				string directoryName = Path.GetDirectoryName(Task.Path);
				if (directoryName == null)
				{
					Task.Status = TaskStatus.Error;
					Mod.Download.OnDownloadFailure(this, "Create directory failed.");
				}
				else
				{
					Directory.CreateDirectory(directoryName);
					DoDownload();
				}
			}

			void ITaskAgent<DownloadTask>.Tick(float elapseSeconds, float realElapseSeconds)
			{
				Profiler.BeginSample("DownloadMod.TaskAgent.Tick");
				if (Task.Status == TaskStatus.Doing)
				{
					_waitTime += realElapseSeconds;
					if (_waitTime >= Task.Timeout)
					{
						Task.Status = TaskStatus.Error;
						Mod.Download.OnDownloadFailure(this, "Timeout");
					}
					else
					{
						WebRequestTick();
						Profiler.EndSample();
					}
				}
			}

			void ITaskAgent<DownloadTask>.Recycle()
			{
				Task = null;
				if (_webRequest != null)
				{
					_webRequest.Abort();
					_webRequest.Dispose();
					_webRequest = null;
				}
			}

			void ITaskAgent<DownloadTask>.Exit()
			{
			}

			private void DoDownload()
			{
				_webRequest = UnityWebRequest.Get(Task.Uri);
				_webRequest.downloadHandler = new DownloadHandlerFile(Task.Path)
				{
					removeFileOnAbort = true
				};
				_webRequest.SendWebRequest();
			}

			private void DoDownload(int fromPos)
			{
				Dictionary<string, string> formFields = new Dictionary<string, string>(1) { 
				{
					"Range",
					"bytes=" + fromPos + "-"
				} };
				_webRequest = UnityWebRequest.Post(Task.Uri, formFields);
				_webRequest.downloadHandler = new DownloadHandlerFile(Task.Path)
				{
					removeFileOnAbort = true
				};
				_webRequest.SendWebRequest();
			}

			private void DoDownload(int fromPos, int toPos)
			{
				Dictionary<string, string> formFields = new Dictionary<string, string>(1) { 
				{
					"Range",
					"bytes=" + fromPos + "-" + toPos
				} };
				_webRequest = UnityWebRequest.Post(Task.Uri, formFields);
				_webRequest.downloadHandler = new DownloadHandlerFile(Task.Path)
				{
					removeFileOnAbort = true
				};
				_webRequest.SendWebRequest();
			}

			private void WebRequestTick()
			{
				if (_webRequest == null)
				{
					return;
				}
				if (_webRequest.isNetworkError)
				{
					Task.Status = TaskStatus.Error;
					Mod.Download.OnDownloadFailure(this, _webRequest.error);
					return;
				}
				if (_webRequest.isHttpError)
				{
					Task.Status = TaskStatus.Error;
					Mod.Download.OnDownloadFailure(this, string.Format("http error: {0}", _webRequest.responseCode));
					return;
				}
				if (!_webRequest.downloadHandler.isDone)
				{
					_waitTime = 0f;
					int num = (int)_webRequest.downloadedBytes;
					if (_downloadedSize < num)
					{
						_downloadedSize = num;
						Mod.Download.OnDownloadUpdate(this, num, _webRequest.downloadProgress);
					}
					return;
				}
				int num2 = (int)_webRequest.downloadedBytes;
				if (num2 <= 0)
				{
					string message = "When the " + Task.Uri + " download has been completed, the data is empty or the length is invalid.";
					Task.Status = TaskStatus.Error;
					Mod.Download.OnDownloadFailure(this, message);
				}
				else
				{
					Task.Status = TaskStatus.Done;
					Mod.Download.OnDownloadSuccess(this, num2);
				}
			}
		}

		public sealed class FailureEventArgs : EventArgs<FailureEventArgs>
		{
			public int TaskId { get; private set; }

			public string Path { get; private set; }

			public string Uri { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			public static FailureEventArgs Make(int taskId, string path, string uri, string message, object userData)
			{
				FailureEventArgs failureEventArgs = Mod.Reference.Acquire<FailureEventArgs>();
				failureEventArgs.TaskId = taskId;
				failureEventArgs.Path = path;
				failureEventArgs.Uri = uri;
				failureEventArgs.Message = message;
				failureEventArgs.UserData = userData;
				return failureEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		public sealed class StartEventArgs : EventArgs<StartEventArgs>
		{
			public int TaskId { get; private set; }

			public string Path { get; private set; }

			public string Uri { get; private set; }

			public object UserData { get; private set; }

			public static StartEventArgs Make(int taskId, string path, string uri, object userData)
			{
				StartEventArgs startEventArgs = Mod.Reference.Acquire<StartEventArgs>();
				startEventArgs.TaskId = taskId;
				startEventArgs.Path = path;
				startEventArgs.Uri = uri;
				startEventArgs.UserData = userData;
				return startEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		public sealed class SuccessEventArgs : EventArgs<SuccessEventArgs>
		{
			public int TaskId { get; private set; }

			public string Path { get; private set; }

			public string Uri { get; private set; }

			public int Length { get; private set; }

			public object UserData { get; private set; }

			public static SuccessEventArgs Make(int taskId, string path, string uri, int length, object userData)
			{
				SuccessEventArgs successEventArgs = Mod.Reference.Acquire<SuccessEventArgs>();
				successEventArgs.TaskId = taskId;
				successEventArgs.Path = path;
				successEventArgs.Uri = uri;
				successEventArgs.Length = length;
				successEventArgs.UserData = userData;
				return successEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		public sealed class UpdateEventArgs : EventArgs<UpdateEventArgs>
		{
			public int TaskId { get; private set; }

			public string Path { get; private set; }

			public string Uri { get; private set; }

			public int Length { get; private set; }

			public float Progress { get; private set; }

			public object UserData { get; private set; }

			public static UpdateEventArgs Make(int taskId, string path, string uri, int length, float progress, object userData)
			{
				UpdateEventArgs updateEventArgs = Mod.Reference.Acquire<UpdateEventArgs>();
				updateEventArgs.TaskId = taskId;
				updateEventArgs.Path = path;
				updateEventArgs.Uri = uri;
				updateEventArgs.Length = length;
				updateEventArgs.Progress = progress;
				updateEventArgs.UserData = userData;
				return updateEventArgs;
			}

			protected override void OnRecycle()
			{
				UserData = null;
			}
		}

		[SerializeField]
		[Range(5f, 120f)]
		private float _timeout = 30f;

		[SerializeField]
		[Range(1f, 128f)]
		private int _agentCount = 8;

		private readonly TaskPool<DownloadTask> _taskPool = new TaskPool<DownloadTask>();

		public float Timeout
		{
			get
			{
				return _timeout;
			}
		}

		public int TotalAgentCount
		{
			get
			{
				return _taskPool.TotalAgentCount;
			}
		}

		public int FreeAgentCount
		{
			get
			{
				return _taskPool.FreeAgentCount;
			}
		}

		public int WorkingAgentCount
		{
			get
			{
				return _taskPool.WorkingAgentCount;
			}
		}

		public int WaitingTaskCount
		{
			get
			{
				return _taskPool.WaitingTaskCount;
			}
		}

		public int AddTask(string path, string uri, object userData = null)
		{
			if (string.IsNullOrEmpty(path))
			{
				Log.Warning("path is invalid.");
				return -1;
			}
			if (string.IsNullOrEmpty(uri))
			{
				Log.Warning("uri is invalid.");
				return -1;
			}
			if (_taskPool.TotalAgentCount <= 0)
			{
				Log.Warning("Must add task agent first.");
				return -1;
			}
			DownloadTask downloadTask = new DownloadTask(path, uri, _timeout, userData);
			if (!_taskPool.AddTask(downloadTask))
			{
				return -1;
			}
			return downloadTask.Id;
		}

		public bool RemoveTask(int taskId)
		{
			return _taskPool.RemoveTask(taskId);
		}

		public void RemoveAllTasks()
		{
			_taskPool.RemoveTasks();
		}

		private void OnDownloadStart(DownloadAgent taskAgent)
		{
			DownloadTask task = taskAgent.Task;
			StartEventArgs args = StartEventArgs.Make(task.Id, task.Path, task.Uri, task.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnDownloadUpdate(DownloadAgent taskAgent, int size, float progress)
		{
			DownloadTask task = taskAgent.Task;
			UpdateEventArgs args = UpdateEventArgs.Make(task.Id, task.Path, task.Uri, size, progress, task.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnDownloadSuccess(DownloadAgent taskAgent, int size)
		{
			DownloadTask task = taskAgent.Task;
			SuccessEventArgs args = SuccessEventArgs.Make(task.Id, task.Path, task.Uri, size, task.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnDownloadFailure(DownloadAgent taskAgent, string message)
		{
			DownloadTask task = taskAgent.Task;
			FailureEventArgs args = FailureEventArgs.Make(task.Id, task.Path, task.Uri, message, task.UserData);
			Mod.Event.Fire(this, args);
		}

		protected override void Awake()
		{
			Mod.Download = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			for (int i = 0; i < _agentCount; i++)
			{
				_taskPool.AddAgent(new DownloadAgent());
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("DownloadMod.OnTick");
			_taskPool.Tick(elapseSeconds, realElapseSeconds);
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			base.OnExit();
			_taskPool.Destroy();
		}
	}
}
