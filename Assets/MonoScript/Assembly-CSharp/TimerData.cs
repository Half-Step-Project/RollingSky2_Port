using System;

public sealed class TimerData : AbsTimerData
{
	private Action action;

	public override Delegate Action
	{
		get
		{
			return action;
		}
		set
		{
			action = value as Action;
		}
	}

	public override void DoAction()
	{
		if (action != null)
		{
			action();
		}
	}
}
public sealed class TimerData<T> : AbsTimerData
{
	private Action<T> action;

	private T args;

	public T Args
	{
		get
		{
			return args;
		}
		set
		{
			args = value;
		}
	}

	public override Delegate Action
	{
		get
		{
			return action;
		}
		set
		{
			action = value as Action<T>;
		}
	}

	public override void DoAction()
	{
		if (action != null)
		{
			action(args);
		}
	}
}
public sealed class TimerData<T, U> : AbsTimerData
{
	private Action<T, U> action;

	private T args1;

	private U args2;

	public U Args2
	{
		get
		{
			return args2;
		}
		set
		{
			args2 = value;
		}
	}

	public T Args1
	{
		get
		{
			return args1;
		}
		set
		{
			args1 = value;
		}
	}

	public override Delegate Action
	{
		get
		{
			return action;
		}
		set
		{
			action = value as Action<T, U>;
		}
	}

	public override void DoAction()
	{
		if (action != null)
		{
			action(args1, args2);
		}
	}
}
