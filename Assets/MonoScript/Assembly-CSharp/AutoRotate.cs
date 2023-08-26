using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public float RotateSpeed = 1f;

	public Vector3 RotateAxis = Vector3.up;

	private void Start()
	{
		Debug.LogError("警告：禁止在正式资源中使用AutoRotate。");
		base.enabled = false;
	}

	private void Update()
	{
		base.transform.Rotate(RotateAxis * RotateSpeed);
	}
}
