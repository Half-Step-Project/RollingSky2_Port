using System;

namespace My.Core
{
	public abstract class Singleton<T> where T : class, new()
	{
		private static T instance;

		private static object lockHelper = new object();

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					lock (lockHelper)
					{
						if (instance == null)
						{
							instance = new T();
						}
					}
				}
				return instance;
			}
		}

		protected Singleton()
		{
			if (instance != null)
			{
				throw new InvalidOperationException("Can't create singleton instance more than once.");
			}
		}
	}
}
