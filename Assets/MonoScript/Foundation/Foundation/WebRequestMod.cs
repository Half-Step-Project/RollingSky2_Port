using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Foundation
{
	[AddComponentMenu("Framework/WebRequest")]
	[DisallowMultipleComponent]
	public sealed class WebRequestMod : ModBase
	{
		public sealed class FailureEventArgs : EventArgs<FailureEventArgs>
		{
			public int TaskId { get; private set; }

			public string Uri { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			public static FailureEventArgs Make(int taskId, string uri, string message, object userData)
			{
				FailureEventArgs failureEventArgs = Mod.Reference.Acquire<FailureEventArgs>();
				failureEventArgs.TaskId = taskId;
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

			public string Uri { get; private set; }

			public object UserData { get; private set; }

			public static StartEventArgs Make(int taskId, string uri, object userData)
			{
				StartEventArgs startEventArgs = Mod.Reference.Acquire<StartEventArgs>();
				startEventArgs.TaskId = taskId;
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

			public string Uri { get; private set; }

			public object UserData { get; private set; }

			public byte[] Bytes { get; private set; }

			public string Text { get; private set; }

			public static SuccessEventArgs Make(int taskId, string uri, byte[] bytes, string text, object userData)
			{
				SuccessEventArgs successEventArgs = Mod.Reference.Acquire<SuccessEventArgs>();
				successEventArgs.TaskId = taskId;
				successEventArgs.Uri = uri;
				successEventArgs.Bytes = bytes;
				successEventArgs.UserData = userData;
				successEventArgs.Text = text;
				return successEventArgs;
			}

			protected override void OnRecycle()
			{
				Bytes = null;
				Text = null;
				UserData = null;
			}
		}

		private sealed class WebRequestTask : Task
		{
			[CompilerGenerated]
			private readonly string _003CUri_003Ek__BackingField;

			[CompilerGenerated]
			private readonly bool _003CIsBytes_003Ek__BackingField;

			[CompilerGenerated]
			private readonly float _003CTimeout_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CPostData_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CUserData_003Ek__BackingField;

			public string Uri
			{
				[CompilerGenerated]
				get
				{
					return _003CUri_003Ek__BackingField;
				}
			}

			public bool IsBytes
			{
				[CompilerGenerated]
				get
				{
					return _003CIsBytes_003Ek__BackingField;
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

			public object PostData
			{
				[CompilerGenerated]
				get
				{
					return _003CPostData_003Ek__BackingField;
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

			public WebRequestTask(string uri, object postData, bool isBytes, float timeout, object userData)
			{
				_003CUri_003Ek__BackingField = uri;
				_003CIsBytes_003Ek__BackingField = isBytes;
				_003CPostData_003Ek__BackingField = postData;
				_003CTimeout_003Ek__BackingField = timeout;
				_003CUserData_003Ek__BackingField = userData;
			}
		}

		private sealed class WebRequestAgent : ITaskAgent<WebRequestTask>
		{
			private UnityWebRequest _webRequest;

			private float _waitTime;

			public WebRequestTask Task { get; private set; }

			void ITaskAgent<WebRequestTask>.Init()
			{
			}

			void ITaskAgent<WebRequestTask>.Boot(WebRequestTask task)
			{
				Task = task;
				_waitTime = 0f;
				Mod.WebRequest.OnWebRequestStart(this);
				Request();
			}

			void ITaskAgent<WebRequestTask>.Tick(float elapseSeconds, float realElapseSeconds)
			{
				Profiler.BeginSample("WebRequestMod.WebRequestAgent.Tick");
				if (Task.Status == TaskStatus.Doing)
				{
					_waitTime += realElapseSeconds;
					if (_waitTime >= Task.Timeout)
					{
						Task.Status = TaskStatus.Error;
						Mod.WebRequest.OnWebRequestFailure(this, "Timeout");
					}
					else
					{
						WebRequestTick();
						Profiler.EndSample();
					}
				}
			}

			void ITaskAgent<WebRequestTask>.Recycle()
			{
				Task = null;
				if (_webRequest != null)
				{
					_webRequest.Abort();
					_webRequest.Dispose();
					_webRequest = null;
				}
			}

			void ITaskAgent<WebRequestTask>.Exit()
			{
			}

			private void Request()
			{
				object postData = Task.PostData;
				if (postData != null)
				{
					byte[] array;
					if ((array = postData as byte[]) == null)
					{
						string text;
						if ((text = postData as string) == null)
						{
							WWWForm wWWForm;
							if ((wWWForm = postData as WWWForm) != null)
							{
								WWWForm wwwForm = wWWForm;
								DoRequest(wwwForm);
							}
							else
							{
								Log.Warning("The post data is invalid.");
							}
						}
						else
						{
							string postStr = text;
							DoRequest(postStr);
						}
					}
					else
					{
						byte[] postData2 = array;
						DoRequest(postData2);
					}
				}
				else
				{
					DoRequest();
				}
			}

			private void DoRequest()
			{
				_webRequest = UnityWebRequest.Get(Task.Uri);
				_webRequest.timeout = (int)Task.Timeout;
				_webRequest.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
				_webRequest.SetRequestHeader("Pragma", "no-cache");
				_webRequest.downloadHandler = new DownloadHandlerBuffer();
				_webRequest.SendWebRequest();
			}

			private void DoRequest(byte[] postData)
			{
				_webRequest = UnityWebRequest.Post(Task.Uri, postData.GetString());
				_webRequest.timeout = (int)Task.Timeout;
				_webRequest.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
				_webRequest.SetRequestHeader("Pragma", "no-cache");
				_webRequest.downloadHandler = new DownloadHandlerBuffer();
				_webRequest.SendWebRequest();
			}

			private void DoRequest(string postStr)
			{
				_webRequest = UnityWebRequest.Post(Task.Uri, postStr);
				_webRequest.timeout = (int)Task.Timeout;
				_webRequest.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
				_webRequest.SetRequestHeader("Pragma", "no-cache");
				_webRequest.downloadHandler = new DownloadHandlerBuffer();
				_webRequest.SendWebRequest();
			}

			private void DoRequest(WWWForm wwwForm)
			{
				_webRequest = UnityWebRequest.Post(Task.Uri, wwwForm);
				_webRequest.timeout = (int)Task.Timeout;
				_webRequest.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
				_webRequest.SetRequestHeader("Pragma", "no-cache");
				_webRequest.downloadHandler = new DownloadHandlerBuffer();
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
					Mod.WebRequest.OnWebRequestFailure(this, _webRequest.error);
				}
				else if (_webRequest.isHttpError)
				{
					Task.Status = TaskStatus.Error;
					Mod.WebRequest.OnWebRequestFailure(this, string.Format("Web request http error: {0}", _webRequest.responseCode));
				}
				else if (_webRequest.isDone)
				{
					Task.Status = TaskStatus.Done;
					byte[] bytes = null;
					string text = null;
					if (Task.IsBytes)
					{
						bytes = _webRequest.downloadHandler.data;
					}
					else
					{
						text = _webRequest.downloadHandler.text;
					}
					Mod.WebRequest.OnWebRequestSuccess(this, bytes, text);
				}
			}
		}

		[SerializeField]
		[Range(0f, 120f)]
		private float _timeout = 30f;

		[SerializeField]
		private int _agentCount = 4;

		private readonly TaskPool<WebRequestTask> _taskPool = new TaskPool<WebRequestTask>();

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

		public int AddTask(string uri, object postData, bool isBytes, object userData = null)
		{
			return AddTask(uri, postData, isBytes, _timeout, userData);
		}

		public int AddTask(string uri, object postData, bool isBytes, float timeout, object userData = null)
		{
			if (string.IsNullOrEmpty(uri))
			{
				Log.Warning("Web request uri is invalid.");
				return -1;
			}
			if (_taskPool.TotalAgentCount <= 0)
			{
				Log.Warning("Must add task agent first.");
				return -1;
			}
			WebRequestTask webRequestTask = new WebRequestTask(uri, postData, isBytes, timeout, userData);
			if (!_taskPool.AddTask(webRequestTask))
			{
				return -1;
			}
			return webRequestTask.Id;
		}

		public bool RemoveTask(int taskId)
		{
			return _taskPool.RemoveTask(taskId);
		}

		public void RemoveAllTasks()
		{
			_taskPool.RemoveTasks();
		}

		private void OnWebRequestStart(WebRequestAgent taskAgent)
		{
			WebRequestTask task = taskAgent.Task;
			StartEventArgs args = StartEventArgs.Make(task.Id, task.Uri, task.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnWebRequestSuccess(WebRequestAgent taskAgent, byte[] bytes, string text)
		{
			WebRequestTask task = taskAgent.Task;
			SuccessEventArgs args = SuccessEventArgs.Make(task.Id, task.Uri, bytes, text, task.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnWebRequestFailure(WebRequestAgent taskAgent, string message)
		{
			WebRequestTask task = taskAgent.Task;
			FailureEventArgs args = FailureEventArgs.Make(task.Id, task.Uri, message, task.UserData);
			Mod.Event.Fire(this, args);
		}

		protected override void Awake()
		{
			Mod.WebRequest = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			for (int i = 0; i < _agentCount; i++)
			{
				_taskPool.AddAgent(new WebRequestAgent());
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("WebRequestMod.OnTick");
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
