using System;
using UnityEngine;

namespace RisingWin.Library
{
	public class DirectionFastForwardProcessor
	{
		private enum AxisState
		{
			NONE = 0,
			CLICK = 1,
			FAST_FORWARD = 2
		}

		private const float BOUNCE_THRESHOLD = 0.4f;

		private const float SLOWEST_INTERVAL = 0.2f;

		private const float FASTEST_INTERVAL = 0.05f;

		private AxisState State;

		private float Value;

		private float IntervalTime;

		private float IntervalSetting;

		public void OnProcess(float pDirectionValue, float pPreviousDirectionValue, float pDeltaTime, Action<float> pOnChangedCallback)
		{
			switch (State)
			{
			case AxisState.NONE:
				Value = 0f;
				IntervalTime = 0f;
				IntervalSetting = 0.2f;
				if (Mathf.Abs(pDirectionValue) >= 0.4f)
				{
					Value = pDirectionValue;
					State = AxisState.CLICK;
				}
				break;
			case AxisState.CLICK:
				pOnChangedCallback(Value);
				State = AxisState.FAST_FORWARD;
				break;
			case AxisState.FAST_FORWARD:
				if (Mathf.Sign(pDirectionValue) != Mathf.Sign(pPreviousDirectionValue))
				{
					State = AxisState.NONE;
				}
				else if (Mathf.Abs(pDirectionValue) >= 0.4f)
				{
					IntervalTime += pDeltaTime;
					if (IntervalTime >= IntervalSetting)
					{
						IntervalTime = 0f;
						IntervalSetting -= 0.01f;
						if (IntervalSetting < 0.05f)
						{
							IntervalSetting = 0.05f;
						}
						pOnChangedCallback(pDirectionValue);
					}
				}
				else
				{
					State = AxisState.NONE;
				}
				break;
			}
		}
	}
}
