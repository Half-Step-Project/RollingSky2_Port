using System;

public sealed class KeyedPriorityQueueHeadChangedEventArgs<T> : EventArgs where T : class
{
	private T oldFirstElement;

	private T newFirstElement;

	public T OldFirstElement
	{
		get
		{
			return oldFirstElement;
		}
	}

	public T NewFirstElement
	{
		get
		{
			return newFirstElement;
		}
	}

	public KeyedPriorityQueueHeadChangedEventArgs(T oldFirstElement, T newFirstElement)
	{
		this.oldFirstElement = oldFirstElement;
		this.newFirstElement = newFirstElement;
	}
}
