using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
	private void OnWillRenderObject()
	{
		base.transform.LookAt(Camera.main.transform.position, Vector3.up);
	}
}
