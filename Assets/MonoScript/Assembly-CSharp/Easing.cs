using UnityEngine;

public class Easing
{
	public static Vector2 EasingVector2(float t, Vector2 from, Vector2 to, float d, EaseType easeType = EaseType.Linear)
	{
		float x = EasingFloat(t, from.x, to.x, d, easeType);
		float y = EasingFloat(t, from.y, to.y, d, easeType);
		return new Vector2(x, y);
	}

	public static Color EasingColor(float t, Color from, Color to, float d, EaseType easeType = EaseType.Linear)
	{
		float t2 = EasingFloat(t, 0f, 1f, d, easeType);
		return Color.Lerp(from, to, t2);
	}

	public static Vector3 EasingVector3(float t, Vector3 from, Vector3 to, float d, EaseType easeType = EaseType.Linear)
	{
		float t2 = EasingFloat(t, 0f, 1f, d, easeType);
		return Vector3.Lerp(from, to, t2);
	}

	public static Quaternion EasingQuaternion(float t, Vector3 from, Vector3 to, float d, EaseType easeType = EaseType.Linear)
	{
		return EasingQuaternion(t, Quaternion.Euler(from), Quaternion.Euler(to), d, easeType);
	}

	public static Quaternion EasingQuaternion(float t, Quaternion from, Quaternion to, float d, EaseType easeType = EaseType.Linear)
	{
		float t2 = EasingFloat(t, 0f, 1f, d, easeType);
		return Quaternion.Lerp(from, to, t2);
	}

	public static float EasingFloat(float t, float from, float to, float d, EaseType easeType = EaseType.Linear)
	{
		float num = to - from;
		switch (easeType)
		{
		case EaseType.Linear:
			num = Linear(t, from, num, d);
			break;
		case EaseType.InOutCubic:
			num = InOutCubic(t, from, num, d);
			break;
		case EaseType.InOutQuintic:
			num = InOutQuintic(t, from, num, d);
			break;
		case EaseType.InQuintic:
			num = InQuintic(t, from, num, d);
			break;
		case EaseType.InQuartic:
			num = InQuartic(t, from, num, d);
			break;
		case EaseType.InCubic:
			num = InCubic(t, from, num, d);
			break;
		case EaseType.InQuadratic:
			num = InQuadratic(t, from, num, d);
			break;
		case EaseType.OutQuintic:
			num = OutQuintic(t, from, num, d);
			break;
		case EaseType.OutQuartic:
			num = OutQuartic(t, from, num, d);
			break;
		case EaseType.OutCubic:
			num = OutCubic(t, from, num, d);
			break;
		case EaseType.OutInCubic:
			num = OutInCubic(t, from, num, d);
			break;
		case EaseType.BackInCubic:
			num = BackInCubic(t, from, num, d);
			break;
		case EaseType.BackInQuartic:
			num = BackInQuartic(t, from, num, d);
			break;
		case EaseType.OutBackCubic:
			num = OutBackCubic(t, from, num, d);
			break;
		case EaseType.OutBackQuartic:
			num = OutBackQuartic(t, from, num, d);
			break;
		case EaseType.OutElasticSmall:
			num = OutElasticSmall(t, from, num, d);
			break;
		case EaseType.OutElasticBig:
			num = OutElasticBig(t, from, num, d);
			break;
		case EaseType.InElasticSmall:
			num = InElasticSmall(t, from, num, d);
			break;
		case EaseType.InElasticBig:
			num = InElasticBig(t, from, num, d);
			break;
		}
		return num;
	}

	private static float Linear(float time, float begion, float change, float duration)
	{
		time /= duration;
		return begion + change * time;
	}

	private static float InOutCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (-2f * num2 + 3f * num);
	}

	private static float InOutQuintic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (6f * num2 * num + -15f * num * num + 10f * num2);
	}

	private static float InQuintic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (num2 * num);
	}

	private static float InQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		return begion + change * (num * num);
	}

	private static float InCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time * time;
		return begion + change * num;
	}

	private static float InQuadratic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		return begion + change * num;
	}

	private static float OutQuintic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (num2 * num + -5f * num * num + 10f * num2 + -10f * num + 5f * time);
	}

	private static float OutQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (-1f * num * num + 4f * num2 + -6f * num + 4f * time);
	}

	private static float OutCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (num2 + -3f * num + 3f * time);
	}

	private static float OutInCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (4f * num2 + -6f * num + 3f * time);
	}

	private static float OutInQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (6f * num2 + -9f * num + 4f * time);
	}

	private static float BackInCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (4f * num2 + -3f * num);
	}

	private static float BackInQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (2f * num * num + 2f * num2 + -3f * num);
	}

	private static float OutBackCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (4f * num2 + -9f * num + 6f * time);
	}

	private static float OutBackQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (-2f * num * num + 10f * num2 + -15f * num + 8f * time);
	}

	private static float OutElasticSmall(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (33f * num2 * num + -106f * num * num + 126f * num2 + -67f * num + 15f * time);
	}

	private static float OutElasticBig(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (56f * num2 * num + -175f * num * num + 200f * num2 + -100f * num + 20f * time);
	}

	private static float InElasticSmall(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (33f * num2 * num + -59f * num * num + 32f * num2 + -5f * num);
	}

	private static float InElasticBig(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (56f * num2 * num + -105f * num * num + 60f * num2 + -10f * num);
	}
}
