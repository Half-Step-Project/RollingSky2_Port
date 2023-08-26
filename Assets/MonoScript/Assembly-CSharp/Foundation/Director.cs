using Foundation.Bugly;
using UnityEngine;

namespace Foundation
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Director")]
	public sealed class Director : MonoBehaviour
	{
		[SerializeField]
		private bool _developMode;

		[SerializeField]
		private bool _requestUpdateResources = true;

		[SerializeField]
		private bool _releaseRemoteHost;

		[SerializeField]
		private bool _didNotConsumePower;

		[SerializeField]
		private bool _connectedGameServer = true;

		[SerializeField]
		private bool _connectedTestServer;

		private static Director _director;

		public static bool EnableRiskPerception { get; private set; }

		public static Director Ins
		{
			get
			{
				if (_director == null)
				{
					_director = Object.FindObjectOfType<Director>();
				}
				return _director;
			}
		}

		public bool RequestUpdateResources
		{
			get
			{
				return _requestUpdateResources;
			}
			set
			{
				_requestUpdateResources = value;
			}
		}

		public bool ReleaseRemoteHost
		{
			get
			{
				return _releaseRemoteHost;
			}
			set
			{
				_releaseRemoteHost = value;
			}
		}

		public bool DidNotConsumePower
		{
			get
			{
				return _didNotConsumePower;
			}
			set
			{
				_didNotConsumePower = value;
			}
		}

		public bool ConnectedGameServer
		{
			get
			{
				return _connectedGameServer;
			}
			set
			{
				_connectedGameServer = value;
			}
		}

		public bool DevelopMode
		{
			get
			{
				return _developMode;
			}
			set
			{
				_developMode = value;
			}
		}

		public bool ConnectedTestServer
		{
			get
			{
				return _connectedTestServer;
			}
			set
			{
				_connectedTestServer = value;
			}
		}

		private void Start()
		{
			Mod.InitEx += BuglyAgent.InitSDK;
			Mod.Init();
		}

		private void OnDestroy()
		{
			_director = null;
		}
	}
}
