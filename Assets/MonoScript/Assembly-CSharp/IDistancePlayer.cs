public interface IDistancePlayer
{
	float GetPercent(float distance);

	void PlayByPercent(float percent);

	void DebugPlay(float distance);
}
