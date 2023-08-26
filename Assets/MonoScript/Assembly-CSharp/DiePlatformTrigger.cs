using UnityEngine;

public class DiePlatformTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
		{
			BaseRole.theBall.BeginDropDie();
		}
	}
}
