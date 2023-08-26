using UnityEngine;

public class ParticleScaleGroup : MonoBehaviour
{
	private void Awake()
	{
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			Vector3 localScale = child.localScale;
			float num = MonoSingleton<GameTools>.Instacne.UIScale();
			child.localScale = new Vector3(localScale.x * num, localScale.y * num, localScale.z * num);
		}
	}
}
