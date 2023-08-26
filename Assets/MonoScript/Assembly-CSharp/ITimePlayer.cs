public interface ITimePlayer
{
	float GetPercent(float time);

	void PlayByPercent(float percent);

	void DebugPlay(float time);
}
