using System;
using UnityEngine;

namespace DG.DemiLib
{
	[Serializable]
	public struct Range
	{
		public float min;

		public float max;

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float RandomWithin()
		{
			float num = max - min;
			return min + UnityEngine.Random.Range(0f, num);
		}
	}
}
