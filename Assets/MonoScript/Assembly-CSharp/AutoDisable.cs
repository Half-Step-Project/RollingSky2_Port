using System.Collections;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
	public float time = 1.5f;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(time);
		base.gameObject.SetActive(false);
	}
}
