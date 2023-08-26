using UnityEngine;

public class OriginRebirthTrigger : BaseTriggerBox
{
	[Label]
	public bool m_isOnTrigger;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((!GameController.Instance || GameController.Instance.M_gameState != GameController.GAMESTATE.OriginRebirthReset) && !m_isOnTrigger)
		{
			if (GameController.Instance != null)
			{
				RecordOriginRebirthManager.RecordOriginRebirthForTrigger(this);
			}
			m_isOnTrigger = true;
		}
	}

	public override void InitElement()
	{
		base.InitElement();
		m_isOnTrigger = false;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		m_isOnTrigger = false;
	}

	public override void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(base.transform.position, Vector3.up);
		Gizmos.DrawCube(base.transform.TransformPoint(0f, 1f, 0f), new Vector3(1f, 2f, 0.5f));
	}
}
