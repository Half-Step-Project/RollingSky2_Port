using UnityEngine;

public class LightRotationTrigger : BaseTriggerBox
{
	public Vector3 endRotation = Vector3.zero;

	public float time = 2f;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
	}

	public override void ResetElement()
	{
		base.ResetElement();
	}

	public override void TriggerEnter(BaseRole ball)
	{
	}
}
