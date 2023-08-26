using UnityEngine;

public class WhiteScreenTrigger : BaseTriggerBox
{
	private GameObject _FlowerCamera;

	private int finishIndex;

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

	public override void TriggerEnter(BaseRole ball)
	{
		_FlowerCamera = Object.Instantiate(ResourcesManager.Load<GameObject>("Prefab/Effect/FlowerCamera"));
		_FlowerCamera.transform.parent = Camera.main.transform;
		_FlowerCamera.transform.localPosition = Vector3.zero;
		_FlowerCamera.transform.localEulerAngles = Vector3.zero;
	}

	private void finishHandler()
	{
		finishIndex++;
	}

	private void clear()
	{
	}

	public override void ResetElement()
	{
		base.ResetElement();
		clear();
	}
}
