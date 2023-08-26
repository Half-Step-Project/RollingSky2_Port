using System;
using UnityEngine;

[Serializable]
public class MoveData
{
	public float Vbegin;

	public float Vtarget;

	public Vector3 Pbegin;

	public Vector3 Ptarget;

	public Vector3 ForwardDir;

	private float distance;

	private float accelerateTime;

	private float VbeginSqr;

	private float VtargetSqr;

	public float Acceleration { get; private set; }

	public MoveData()
	{
	}

	public MoveData(float vBegin, float vTarget, Vector3 pBegin, Vector3 pTarget, Vector3 forDir)
	{
		Vbegin = vBegin;
		Vtarget = vTarget;
		Pbegin = pBegin;
		Ptarget = pTarget;
		ForwardDir = forDir;
		InitializeData();
	}

	public void InitializeData()
	{
		VbeginSqr = Vbegin * Vbegin;
		VtargetSqr = Vtarget * Vtarget;
		distance = Vector3.Dot(Ptarget - Pbegin, ForwardDir);
		if (distance == 0f)
		{
			Acceleration = 0f;
			accelerateTime = 0f;
		}
		else
		{
			Acceleration = (VtargetSqr - VbeginSqr) * 0.5f / distance;
			accelerateTime = 2f * distance / (Vbegin + Vtarget);
		}
	}

	public void ReverseDir()
	{
		ForwardDir = -1f * ForwardDir;
	}

	public bool GetNextPos(Vector3 currentPos, float deltaTime, ref Vector3 nextPos)
	{
		float num = Vector3.Dot(currentPos - Pbegin, ForwardDir);
		if (num < distance && Acceleration != 0f)
		{
			float num2 = (Mathf.Pow(2f * Acceleration * num + VbeginSqr, 0.5f) - Vbegin) / Acceleration + deltaTime;
			if (num2 >= accelerateTime)
			{
				nextPos = Ptarget + ForwardDir * (num2 - accelerateTime);
				return false;
			}
			nextPos = Pbegin + ForwardDir * (Vbegin * num2 + 0.5f * Acceleration * num2 * num2);
			return true;
		}
		nextPos = currentPos + ForwardDir * Vtarget * deltaTime;
		return false;
	}
}
