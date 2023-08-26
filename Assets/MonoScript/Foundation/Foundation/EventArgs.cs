using System.Runtime.CompilerServices;

namespace Foundation
{
	public abstract class EventArgs : IReference
	{
		protected static class IdHelper
		{
			private static int _nextId;

			internal static int NextId
			{
				get
				{
					return _nextId++;
				}
			}
		}

		internal abstract int Id { get; }

		void IReference.OnRecycle()
		{
			OnRecycle();
		}

		protected abstract void OnRecycle();
	}
	public abstract class EventArgs<T> : EventArgs where T : EventArgs
	{
		[CompilerGenerated]
		private static readonly int _003CEventId_003Ek__BackingField = IdHelper.NextId;

		public static int EventId
		{
			[CompilerGenerated]
			get
			{
				return _003CEventId_003Ek__BackingField;
			}
		}

		internal sealed override int Id
		{
			get
			{
				return EventId;
			}
		}
	}
}
