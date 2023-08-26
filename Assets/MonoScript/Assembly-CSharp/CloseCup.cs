using UnityEngine;

public class CloseCup : MonoBehaviour
{
	private void Update()
	{
		if (base.gameObject.activeSelf && Input.anyKeyDown)
		{
			base.gameObject.SetActive(false);
		}
	}
}
