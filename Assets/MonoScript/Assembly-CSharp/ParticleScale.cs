using UnityEngine;

public class ParticleScale : MonoBehaviour
{
	private void Awake()
	{
		Vector3 localScale = base.transform.localScale;
		float num = MonoSingleton<GameTools>.Instacne.UIScale();
		base.transform.localScale = new Vector3(localScale.x * num, localScale.y * num, localScale.z * num);
	}
}
