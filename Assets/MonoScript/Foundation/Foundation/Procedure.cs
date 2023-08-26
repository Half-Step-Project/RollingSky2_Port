namespace Foundation
{
	public abstract class Procedure : FsmState<ProcedureMod>
	{
		protected internal override void OnInit(IFsm<ProcedureMod> fsm)
		{
			base.OnInit(fsm);
		}

		protected internal override void OnEnter(IFsm<ProcedureMod> fsm)
		{
			base.OnEnter(fsm);
		}

		protected internal override void OnLeave(IFsm<ProcedureMod> fsm, bool isShutdown)
		{
			base.OnLeave(fsm, isShutdown);
		}

		protected internal override void OnDestroy(IFsm<ProcedureMod> fsm)
		{
			base.OnDestroy(fsm);
		}
	}
}
