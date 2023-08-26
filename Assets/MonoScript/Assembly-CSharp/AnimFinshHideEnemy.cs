using System.Collections.Generic;
using UnityEngine;

public class AnimFinshHideEnemy : AnimEnemy
{
	public List<GameObject> HideList = new List<GameObject>();

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
		for (int i = 0; i < HideList.Count; i++)
		{
			HideList[i].SetActive(true);
		}
	}

	public override void OnTriggerStop()
	{
		base.OnTriggerStop();
		for (int i = 0; i < HideList.Count; i++)
		{
			HideList[i].SetActive(false);
		}
	}
}
