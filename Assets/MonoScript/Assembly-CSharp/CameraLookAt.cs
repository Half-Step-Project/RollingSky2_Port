using UnityEngine;

[ExecuteInEditMode]
public class CameraLookAt : MonoBehaviour
{
	public static CameraLookAt Instance;

	public Transform target;

	[Range(0f, 50f)]
	public float m_lookAtSlerpSpeed = 10f;

	private Vector3 lastTargetPos;

	private Vector3 lastCameraPos;

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		if (Instance != null)
		{
			Instance = null;
		}
	}

	private void Update()
	{
		if (!(target == null))
		{
			Quaternion b = Quaternion.LookRotation(target.position - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.smoothDeltaTime * m_lookAtSlerpSpeed);
			lastTargetPos = target.localPosition;
			lastCameraPos = base.transform.localPosition;
		}
	}

	public void ResetLookAtTarget()
	{
		if (!(target == null))
		{
			Quaternion rotation = Quaternion.LookRotation(target.position - base.transform.position);
			base.transform.rotation = rotation;
			lastTargetPos = target.localPosition;
			lastCameraPos = base.transform.localPosition;
		}
	}

	private void OnDrawGizmos()
	{
		if (!(target == null))
		{
			if (!Application.isPlaying)
			{
				Quaternion rotation = Quaternion.LookRotation(target.position - base.transform.position);
				base.transform.rotation = rotation;
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(target.position, Vector3.one / 2f);
			Gizmos.DrawLine(base.transform.position, target.position);
		}
	}
}
