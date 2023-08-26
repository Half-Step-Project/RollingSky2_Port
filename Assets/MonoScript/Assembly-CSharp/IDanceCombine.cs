using UnityEngine;

public interface IDanceCombine
{
	Transform DancerTrans { get; }

	void OnCombineDancer(Transform parent);

	void OnSeperateDancer(Transform parent);

	void OnDanceTogether(int danceType);

	int GetDancingState();
}
