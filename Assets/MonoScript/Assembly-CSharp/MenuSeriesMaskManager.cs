using UnityEngine;

public class MenuSeriesMaskManager : MonoBehaviour
{
	private void Start()
	{
		base.transform.Find("Series_Effect/Distortion_smoke").gameObject.SetActive(false);
	}

	private void Update()
	{
	}
}
