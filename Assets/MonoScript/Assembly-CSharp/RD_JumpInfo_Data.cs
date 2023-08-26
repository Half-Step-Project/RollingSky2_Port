using System;
using UnityEngine;

[Serializable]
public sealed class RD_JumpInfo_Data
{
	public bool IfJumping;

	public Vector3 BeginPos;

	public Vector3 EndPos;

	public Vector3 JumpNormal;

	public float MaxHeight;

	public float JumpBeginY;
}
