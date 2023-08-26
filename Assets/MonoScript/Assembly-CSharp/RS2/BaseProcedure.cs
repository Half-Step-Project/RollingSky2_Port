using Foundation;

namespace RS2
{
	public abstract class BaseProcedure : Procedure
	{
		protected const string NextSceneId = "NextSceneId";

		protected const int MenuSceneId = 1;

		protected const int FirstLevelId = 10000;

		protected override void OnInit(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnInit(procedureOwner);
		}

		protected override void OnEnter(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnEnter(procedureOwner);
		}

		protected override void OnTick(IFsm<ProcedureMod> procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(procedureOwner, elapseSeconds, realElapseSeconds);
		}

		protected override void OnLeave(IFsm<ProcedureMod> procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnDestroy(IFsm<ProcedureMod> procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}
	}
}
