using System;

public interface IActiveTimeController
{
	void Init(DateTime initTime);

	bool IsEnable(DateTime nowTime);

	string GetTimeKey();

	void ResetTime();

	void Reward();
}
