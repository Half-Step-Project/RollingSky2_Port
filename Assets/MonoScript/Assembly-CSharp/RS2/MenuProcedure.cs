using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class MenuProcedure : BaseProcedure
	{
		private const int InvalidSceneId = -1;

		private int _nextSceneId = -1;

		internal void WillToScene(int sceneTableId)
		{
			if (sceneTableId == 1)
			{
				Log.Error("The scene id is invalid.");
			}
			else
			{
				_nextSceneId = sceneTableId;
			}
		}

		protected override void OnEnter(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnEnter(procedureOwner);
			Camera uICamera = Mod.UI.UICamera;
			if (uICamera != null)
			{
				uICamera.useOcclusionCulling = false;
				uICamera.allowHDR = false;
				uICamera.allowMSAA = false;
			}
			uICamera = Mod.Scene.MainCamera;
			if (uICamera != null)
			{
				uICamera.useOcclusionCulling = false;
				uICamera.allowHDR = false;
				uICamera.allowMSAA = true;
			}
			PlayerDataModule.Instance.PluginAdController.Init();
			_nextSceneId = -1;
			MonoSingleton<GameTools>.Instacne.RequreNativeAd();
			ProgressGameActivity();
			Mod.ObjectPool.Unload();
			Mod.Resource.UnloadUnusedAssets();
			Mod.Event.FireNow(this, Mod.Reference.Acquire<EnterMenuProcedureEventArgs>().Initialize());
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			Mod.UI.CloseUIForm(UIFormId.MenuViewForm);
			Mod.UI.CloseLoadedUIForms();
			_nextSceneId = -1;
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
			ProcessInputKey();
			if (_nextSceneId != -1)
			{
				procedureOwner.SetData("NextSceneId", (VarInt)_nextSceneId);
				ChangeState<SwitchSceneProcedure>(procedureOwner);
			}
		}

		private void ProgressGameActivity()
		{
			if (MonoSingleton<GameTools>.Instacne.CanAutoShowGiftForm())
			{
				PlayerDataModule.Instance.PlayerGiftPackageData.GetNextGiftId();
				PlayerDataModule.Instance.PlayerGiftPackageData.IsHadRecommand = true;
				PlayerDataModule.Instance.PlayerGiftPackageData.TodayHadRecommand = true;
			}
			Dictionary<int, IActiveTimeController>.Enumerator enumerator = PlayerDataModule.Instance.PlayerActiveTimeDic.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.IsEnable(DateTime.Now))
				{
					enumerator.Current.Value.Reward();
					enumerator.Current.Value.ResetTime();
				}
			}
		}

		private void ProcessInputKey()
		{
		}
	}
}
