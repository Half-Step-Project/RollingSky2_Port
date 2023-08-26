using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class UpdateProcedure : BaseProcedure
	{
		private const int RequestVersionRetryCount = 5;

		private const float RequestVersionTimeout = 2f;

		private bool _resourceInitComplete;

		private int _needUpdateNum;

		private int _currentUpdateNum;

		private BuildInfo _buildInfo;

		private BuiltinDialogParams _buildDialogParams;

		private int _retryCount;

		protected override void OnEnter(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnEnter(procedureOwner);
			Mod.Event.Subscribe(EventArgs<WebRequestMod.SuccessEventArgs>.EventId, OnWebRequestSuccess);
			Mod.Event.Subscribe(EventArgs<WebRequestMod.FailureEventArgs>.EventId, OnWebRequestFailure);
			Mod.Event.Subscribe(EventArgs<VersionListUpdateSuccessEventArgs>.EventId, OnVersionListUpdateSuccess);
			Mod.Event.Subscribe(EventArgs<VersionListUpdateFailureEventArgs>.EventId, OnVersionListUpdateFailure);
			Mod.Event.Subscribe(EventArgs<BundleCheckCompleteEventArgs>.EventId, OnResourceCheckComplete);
			Mod.Event.Subscribe(EventArgs<BundleUpdateStartEventArgs>.EventId, OnResourceUpdateStart);
			Mod.Event.Subscribe(EventArgs<BundleUpdateFailureEventArgs>.EventId, OnResourceUpdateFailure);
			Mod.Event.Subscribe(EventArgs<BundleUpdateChangedEventArgs>.EventId, OnResourceUpdateChanged);
			Mod.Event.Subscribe(EventArgs<BundleUpdateSuccessEventArgs>.EventId, OnResourceUpdateSuccess);
			Mod.Event.Subscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnResourceUpdateAllComplete);
			if (Application.internetReachability != 0 && Director.Ins.RequestUpdateResources)
			{
				RequestVersion();
				_buildDialogParams.ShowType = BuiltinDialogShowType.Info;
				_buildDialogParams.InfoMessage = Mod.Localization.Get("UpdateProcedure.RequestVersionInfo");
				BuiltinDialogForm.OpenDialog(_buildDialogParams);
			}
			else
			{
				InitResources(false);
			}
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			Mod.Event.Unsubscribe(EventArgs<WebRequestMod.SuccessEventArgs>.EventId, OnWebRequestSuccess);
			Mod.Event.Unsubscribe(EventArgs<WebRequestMod.FailureEventArgs>.EventId, OnWebRequestFailure);
			Mod.Event.Unsubscribe(EventArgs<VersionListUpdateSuccessEventArgs>.EventId, OnVersionListUpdateSuccess);
			Mod.Event.Unsubscribe(EventArgs<VersionListUpdateFailureEventArgs>.EventId, OnVersionListUpdateFailure);
			Mod.Event.Unsubscribe(EventArgs<BundleCheckCompleteEventArgs>.EventId, OnResourceCheckComplete);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateStartEventArgs>.EventId, OnResourceUpdateStart);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateFailureEventArgs>.EventId, OnResourceUpdateFailure);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateChangedEventArgs>.EventId, OnResourceUpdateChanged);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateSuccessEventArgs>.EventId, OnResourceUpdateSuccess);
			Mod.Event.Unsubscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnResourceUpdateAllComplete);
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
			if (_resourceInitComplete)
			{
				ChangeState<PreloadProcedure>(procedureOwner);
			}
		}

		private void RequestVersion()
		{
			string applicableVersion = Mod.Resource.ApplicableVersion;
			string uri = Application.platform.ToString();
			string uri2 = Mod.Localization.Language.ToString();
			string uri3 = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString();
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("GameVersion", WebUtility.EscapeString(applicableVersion));
			wWWForm.AddField("Platform", WebUtility.EscapeString(uri));
			wWWForm.AddField("Language", WebUtility.EscapeString(uri2));
			wWWForm.AddField("UseWifi", WebUtility.EscapeString(uri3));
			Mod.WebRequest.AddTask(Mod.Resource.BuildInfoUri, wWWForm, false, 2f, this);
		}

		private void OnWebRequestSuccess(object sender, EventArgs e)
		{
			WebRequestMod.SuccessEventArgs successEventArgs = (WebRequestMod.SuccessEventArgs)e;
			if (successEventArgs.UserData != this)
			{
				return;
			}
			try
			{
				_buildInfo = BuildInfo.FromJson(successEventArgs.Text);
				if (_buildInfo.ApplicableGameVersion != Mod.Resource.ApplicableVersion)
				{
					Log.Warning("The resource update bundle is not match game version.");
					InitResources(false);
				}
				else if (_buildInfo.ForceUpdateGame)
				{
					_buildDialogParams.ShowType = BuiltinDialogShowType.Alert;
					_buildDialogParams.AlertMessage = Mod.Localization.Get("ForceUpdate.Message");
					_buildDialogParams.ConfirmText = Mod.Localization.Get("ForceUpdate.UpdateButton");
					_buildDialogParams.CancelText = Mod.Localization.Get("ForceUpdate.QuitButton");
					_buildDialogParams.OnClickCancel = ShutdownModInternal;
					_buildDialogParams.OnClickConfirm = ShutdownModInternal;
					BuiltinDialogForm.OpenDialog(_buildDialogParams);
				}
				else
				{
					InitResources(true);
				}
			}
			catch
			{
				Log.Warning("Parse BuildInfo failure.");
				InitResources(false);
			}
		}

		private void ShutdownModInternal(object userData)
		{
			BuiltinDialogForm.Destory();
			Mod.Shutdown();
		}

		private void OnWebRequestFailure(object sender, EventArgs e)
		{
			WebRequestMod.FailureEventArgs failureEventArgs = (WebRequestMod.FailureEventArgs)e;
			if (failureEventArgs.UserData == this)
			{
				if (_retryCount < 5)
				{
					_retryCount++;
					Log.Info(string.Format("Check version web request failure, message: {0} retrycount: {1}", failureEventArgs.Message, _retryCount));
					RequestVersion();
				}
				else
				{
					Log.Warning(string.Format("Check version web request failure, message: {0}", failureEventArgs.Message));
					InitResources(false);
				}
			}
		}

		private void InitResources(bool updateable)
		{
			if (!updateable)
			{
				CheckResources();
				return;
			}
			if (Mod.Resource.VersionListCheck(_buildInfo.InternalResourceVersion, _buildInfo.CurrentVersion.VersionListCrc))
			{
				Mod.Resource.VersionListUpdate(_buildInfo.CurrentVersion.VersionListCrc);
				_buildDialogParams.ShowType = BuiltinDialogShowType.Info;
				_buildDialogParams.InfoMessage = Mod.Localization.Get("UpdateProcedure.GetResourceVersionFile");
				BuiltinDialogForm.OpenDialog(_buildDialogParams);
			}
			else
			{
				CheckResources();
			}
			_buildInfo = null;
		}

		private void CheckResources()
		{
			Mod.Resource.BundleCheck();
			_buildDialogParams.ShowType = BuiltinDialogShowType.Info;
			_buildDialogParams.InfoMessage = Mod.Localization.Get("UpdateProcedure.CheckResource");
			BuiltinDialogForm.OpenDialog(_buildDialogParams);
		}

		private void OnVersionListUpdateSuccess(object sender, EventArgs e)
		{
			CheckResources();
		}

		private void OnVersionListUpdateFailure(object sender, EventArgs e)
		{
			CheckResources();
		}

		private void OnResourceCheckComplete(object sender, EventArgs e)
		{
			BundleCheckCompleteEventArgs bundleCheckCompleteEventArgs = (BundleCheckCompleteEventArgs)e;
			_needUpdateNum = bundleCheckCompleteEventArgs.UpdateCount;
			_currentUpdateNum = 0;
			if (bundleCheckCompleteEventArgs.UpdateCount <= 0)
			{
				_resourceInitComplete = true;
			}
			else
			{
				Mod.Resource.UpdateBundles();
			}
		}

		private void OnResourceUpdateStart(object sender, EventArgs e)
		{
		}

		private void OnResourceUpdateFailure(object sender, EventArgs e)
		{
		}

		private void OnResourceUpdateAllComplete(object sender, EventArgs e)
		{
			_resourceInitComplete = true;
		}

		private void OnResourceUpdateSuccess(object sender, EventArgs e)
		{
			_currentUpdateNum++;
			string progreeMessage = Mod.Localization.Get("UpdateProcedure.UpdateProgress", _currentUpdateNum, _needUpdateNum);
			_buildDialogParams.ShowType = BuiltinDialogShowType.ProgreeBar;
			_buildDialogParams.ProgreeMessage = progreeMessage;
			float progress = (float)_currentUpdateNum * 1f / (float)_needUpdateNum;
			_buildDialogParams.Progress = progress;
			BuiltinDialogForm.OpenDialog(_buildDialogParams);
		}

		private void OnResourceUpdateChanged(object sender, EventArgs e)
		{
			BundleUpdateChangedEventArgs bundleUpdateChangedEventArg = (BundleUpdateChangedEventArgs)e;
		}
	}
}
