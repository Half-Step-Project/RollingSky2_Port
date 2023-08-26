using UnityEngine;

public class PerformanceHelper : MonoBehaviour
{
	private void Start()
	{
		DeviceQuality qualityLevel = DeviceManager.Instance.QualityLevel;
		bool flag = DeviceManager.Instance.IsLowEndQualityLevel((int)qualityLevel);
		base.gameObject.SetActive(!flag);
		Object.Destroy(this);
	}
}
