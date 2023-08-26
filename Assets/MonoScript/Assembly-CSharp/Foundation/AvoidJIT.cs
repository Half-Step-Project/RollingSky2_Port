using System.Collections.Generic;

namespace Foundation
{
	internal static class AvoidJIT
	{
		private static void NeverCalledMethod()
		{
			new Dictionary<int, EventHandler<EventArgs>>();
			new Dictionary<int, EventHandler<Packet>>();
			new Dictionary<int, FsmEventHandler<ProcedureMod>>();
		}
	}
}
