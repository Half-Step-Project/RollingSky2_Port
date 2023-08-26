using UnityEngine;

public interface IRebirth
{
	bool IsRecordRebirth();

	object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale);

	void ResetBySavePointData(object obj);

	void StartRunningForRebirthData(object obj);
}
