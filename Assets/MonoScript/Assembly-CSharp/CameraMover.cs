using UnityEngine;

public class CameraMover
{
	public const float DEFAULT_TARGET_SCALER = 0.25f;

	public static float targetScaler = 0.25f;

	public static float smoothScaler = 6.5f;

	public static readonly float smoothMargin = 0.001f;

	private float targetValue = 2.5f;

	private float smoothValue = 2.5f;

	public Transform transform;

	public CameraMover(Transform _transform, float centerX = 2.5f)
	{
		transform = _transform;
	}

	public void Reset()
	{
		targetValue = 2.5f;
		smoothValue = 2.5f;
		ResetInputParam();
	}

	public void UpdateCameraHorizontal()
	{
		BaseRole theBall = BaseRole.theBall;
		Vector3 position2 = theBall.transform.position;
		Vector3 position = transform.position;
		Vector3 worldRight = Railway.theRailway.GetWorldRight();
		float a = theBall.BallMoveOffset();
		float num = Vector3.Dot(position - Railway.theRailway.GetWorldOrigin(), Railway.theRailway.GetWorldRight());
		float num2 = Mathf.Lerp(a, 0f, targetScaler);
		Vector3 vector = (num2 - num) * worldRight;
		if (theBall.CurrentState != BaseRole.BallState.CrashDie)
		{
			num2 = GetElastic(num2);
			position += vector;
		}
		transform.position = position;
	}

	public void UpdateCameraHorizontalAddFollow()
	{
		BaseRole theBall = BaseRole.theBall;
		Vector3 position2 = theBall.transform.position;
		Vector3 position = transform.position;
		Vector3 worldRight = Railway.theRailway.GetWorldRight();
		float a = theBall.BallMoveOffset();
		float num = Vector3.Dot(position - Railway.theRailway.GetWorldOrigin(), Railway.theRailway.GetWorldRight());
		float num2 = Mathf.Lerp(a, 0f, targetScaler);
		Vector3 vector = (num2 - num) * worldRight;
		if (theBall.CurrentState != BaseRole.BallState.CrashDie)
		{
			num2 = GetElastic(num2);
			position += vector;
		}
		if (CameraController.theCamera.GetFollowData != null)
		{
			float y2 = Mathf.Lerp(b: theBall.transform.TransformPoint(CameraController.theCamera.GetFollowData.m_cameraPoint).y, a: transform.position.y, t: Time.deltaTime * CameraController.theCamera.GetFollowData.m_followSpeed);
			transform.position = new Vector3(position.x, y2, position.z);
		}
		else
		{
			transform.position = position;
		}
	}

	private float GetElastic(float newValue)
	{
		if (Mathf.Abs(targetValue - newValue) <= smoothMargin)
		{
			smoothValue = targetValue;
		}
		else
		{
			targetValue = newValue;
			smoothValue = Mathf.Lerp(smoothValue, targetValue, smoothScaler * Time.smoothDeltaTime);
		}
		return smoothValue;
	}

	public void ResetInputParam(float target = 0.25f)
	{
		targetScaler = target;
	}

	public float GetTargetValue()
	{
		return targetValue;
	}

	public void SetTargetValue(float targetVal)
	{
		targetValue = targetVal;
	}

	public float GetSmoothValue()
	{
		return smoothValue;
	}

	public void SetSmoothValue(float smoothVal)
	{
		smoothValue = smoothVal;
	}
}
