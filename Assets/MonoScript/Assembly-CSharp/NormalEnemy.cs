using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class NormalEnemy : BaseEnemy
{
	public override void Initialize()
	{
		base.Initialize();
		PlayParticle();
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Grid.m_isShowDraw && m_id == 227)
		{
			BoxCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoxCollider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 center = componentsInChildren[i].center;
				Vector3 size = componentsInChildren[i].size;
				Vector3[] array = new Vector3[8]
				{
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f))
				};
				Gizmos.color = new Color(1f, 0f, 0f, 1f);
				Gizmos.DrawLine(array[0], array[1]);
				Gizmos.DrawLine(array[1], array[2]);
				Gizmos.DrawLine(array[2], array[3]);
				Gizmos.DrawLine(array[3], array[0]);
				Gizmos.DrawLine(array[4], array[5]);
				Gizmos.DrawLine(array[5], array[6]);
				Gizmos.DrawLine(array[6], array[7]);
				Gizmos.DrawLine(array[7], array[4]);
				Gizmos.DrawLine(array[0], array[4]);
				Gizmos.DrawLine(array[1], array[5]);
				Gizmos.DrawLine(array[2], array[6]);
				Gizmos.DrawLine(array[3], array[7]);
				Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
				Gizmos.DrawLine(componentsInChildren[i].bounds.center, base.gameObject.transform.position);
				Gizmos.color = new Color(1f, 0f, 0f, 1f);
				Gizmos.DrawSphere(base.gameObject.transform.position, 0.5f);
			}
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		TriggerEnter(BaseRole.theBall);
	}
}
