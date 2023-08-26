using System;
using UnityEngine;

[Serializable]
public class RolePathNode
{
	public Vector3 Position;

	public Quaternion PartRotation;

	public RolePathNode()
	{
	}

	public RolePathNode(Transform trans)
	{
		Position = trans.position;
	}
}
