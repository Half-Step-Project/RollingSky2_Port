namespace Foundation
{
	public delegate void FsmEventHandler<TOwner>(IFsm<TOwner> fsm, object sender, object userData) where TOwner : class;
}
