using Foundation;
using RS2;
using UnityEngine;

public class TempLineAlpha : MonoBehaviour
{
	public Material lineMat;

	private float beginPosZ;

	private float dis;

	private float maxAlpha;

	private bool needChange;

	private void Awake()
	{
		Mod.Event.Subscribe(EventArgs<TempLineAlphaEventArgs>.EventId, HandleEvent);
		SetMaterialAlpha(0f);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<TempLineAlphaEventArgs>.EventId, HandleEvent);
	}

	private void Update()
	{
		if (needChange)
		{
			float percentByPosZ = GetPercentByPosZ(BaseRole.BallPosition.z);
			if (percentByPosZ >= 1f)
			{
				needChange = false;
				SetMaterialAlpha(0f);
			}
			float num = 1f - Mathf.Abs(percentByPosZ - 0.5f) * 2f;
			SetMaterialAlpha(maxAlpha * num);
		}
	}

	private void SetMaterialAlpha(float a)
	{
		a /= 255f;
		Color color = lineMat.GetColor("_TintColor");
		lineMat.SetColor("_TintColor", new Color(color.r, color.g, color.b, a));
	}

	private void HandleEvent(object sender, EventArgs e)
	{
		TempLineAlphaEventArgs tempLineAlphaEventArgs = e as TempLineAlphaEventArgs;
		if (tempLineAlphaEventArgs != null)
		{
			dis = tempLineAlphaEventArgs.dis;
			maxAlpha = tempLineAlphaEventArgs.maxAlpha;
			beginPosZ = BaseRole.BallPosition.z;
			needChange = true;
		}
	}

	public float GetPercentByPosZ(float posz)
	{
		return GetDeltaZ(posz) / dis;
	}

	private float GetDeltaZ(float posZ)
	{
		return posZ - beginPosZ;
	}
}
