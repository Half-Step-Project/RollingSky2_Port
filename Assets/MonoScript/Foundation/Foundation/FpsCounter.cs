namespace Foundation
{
	internal sealed class FpsCounter
	{
		private float _tickInterval;

		private int _frames;

		private float _accumulator;

		private float _timeLeft;

		public float CurrentFps { get; private set; }

		public float UpdateInterval
		{
			get
			{
				return _tickInterval;
			}
			set
			{
				if (value <= 0f)
				{
					Log.Error("Update interval is invalid.");
					return;
				}
				_tickInterval = value;
				Reset();
			}
		}

		public FpsCounter(float updateInterval)
		{
			if (updateInterval <= 0f)
			{
				Log.Error("Update interval is invalid.");
				return;
			}
			_tickInterval = updateInterval;
			Reset();
		}

		public void Tick(float elapseSeconds, float realElapseSeconds)
		{
			_frames++;
			_accumulator += realElapseSeconds;
			_timeLeft -= realElapseSeconds;
			if (_timeLeft <= 0f)
			{
				CurrentFps = ((_accumulator > 0f) ? ((float)_frames / _accumulator) : 0f);
				_frames = 0;
				_accumulator = 0f;
				_timeLeft += _tickInterval;
			}
		}

		private void Reset()
		{
			CurrentFps = 0f;
			_frames = 0;
			_accumulator = 0f;
			_timeLeft = 0f;
		}
	}
}
