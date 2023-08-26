using System;

namespace Foundation
{
	[Flags]
	public enum EventMode
	{
		Default = 0,
		AllowNoHandler = 1,
		AllowMultiHandler = 2,
		AllowDuplicateHandler = 4
	}
}
