using System;
using UnityEngine;

namespace RisingWin.Library
{
	public class CloudBase : MonoBehaviour
	{
		public bool initizlize;

		public bool isOnline;

		public virtual void Init()
		{
			initizlize = true;
		}

		public virtual uint GetServerRealTime()
		{
			return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}
	}
}
