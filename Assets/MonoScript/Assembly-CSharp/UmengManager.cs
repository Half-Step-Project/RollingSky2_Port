using UnityEngine;

public class UmengManager : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
	}
}
