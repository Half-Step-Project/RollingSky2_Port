using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private const float shakeTimeMax = 1f;

	private const float CameraShakesMax = 7f;

	private const float CameraShakeForceMax = 0.5f;

	public const float EnemyLaserShakeForce = 0.8f;

	public const float BallDeathShakeForce = 0.5f;

	public const float EnemyCrusherShakeForce = 0.5f;

	public const float EnemyPounderShakeForce = 0.5f;

	private static bool isShaking;

	private static float shakeAmmount;

	private static float shakePercent;

	private static float shakePercentStarting;

	private static float shakeTimer;

	private static float shakeTimerStarting;

	private static Transform mainCamShaker;

	private GameController gameController;

	private void Start()
	{
		mainCamShaker = base.transform;
		gameController = GameController.Instance;
	}

	private void Update()
	{
		if (gameController.M_gameState != GameController.GAMESTATE.Pause)
		{
			if (isShaking)
			{
				CalculateShakeAmmount();
			}
			if (isShaking && mainCamShaker != null)
			{
				mainCamShaker.localPosition = new Vector3(mainCamShaker.localPosition.x, mainCamShaker.localPosition.y + shakeAmmount, mainCamShaker.localPosition.z);
			}
		}
	}

	public static void ShakeCamera(float shakeForcePercent, bool posteffect_shake = false)
	{
		if (shakeForcePercent > shakePercent)
		{
			isShaking = true;
			shakePercent = shakeForcePercent;
			shakePercentStarting = shakePercent;
			shakeTimer = 1f * shakePercent;
			shakeTimerStarting = shakeTimer;
		}
	}

	public static void ResetShakeCamra()
	{
		shakeTimer = 0f;
		isShaking = false;
	}

	private static void CalculateShakeAmmount()
	{
		shakeTimer -= Time.smoothDeltaTime;
		if (shakeTimer < 0f)
		{
			shakeTimer = 0f;
			isShaking = false;
		}
		shakePercent = shakeTimer / shakeTimerStarting;
		float num = FloatAnim.Wave11(shakePercent * 7f);
		shakeAmmount = 0.5f * num * shakePercentStarting;
	}
}
