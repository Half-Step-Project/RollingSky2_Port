namespace Foundation
{
	public delegate void EventHandler<in T>(object sender, T args) where T : EventArgs;
}
