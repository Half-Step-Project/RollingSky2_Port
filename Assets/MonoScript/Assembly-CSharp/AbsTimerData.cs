using System;

public abstract class AbsTimerData
{
	public uint Id { get; set; }

	public uint Interval { get; set; }

	public uint NextTick { get; set; }

	public abstract Delegate Action { get; set; }

	public abstract void DoAction();
}
