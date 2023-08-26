using Foundation;
using UnityEngine;

namespace RS2
{
	public class LevelProcedure : BaseProcedure
	{
		public bool IsChangeToSwitchScene { get; set; }

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
			if (IsChangeToSwitchScene)
			{
				procedureOwner.SetData("NextSceneId", (VarInt)1);
				ChangeState<SwitchSceneProcedure>(procedureOwner);
				IsChangeToSwitchScene = false;
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
			IsChangeToSwitchScene = false;
			GameController.Instance.Initialize();
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).AddPlayerEnterLevelTotalNum();
			Mod.ObjectPool.Unload();
			Mod.Resource.UnloadUnusedAssets();
			Mod.Event.FireNow(this, Mod.Reference.Acquire<EnterLevelProcedureEventArgs>().Initialize());
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);
			HomeForm.QUIT_LEVEL_COUNT++;
		}
	}
}
