using UnityEngine;

public sealed class JumpUtil
{
	public enum JumpDirection
	{
		forward,
		backward
	}

	private float beginPosZ;

	private float endPosZ;

	private Vector3 beginPos;

	private Vector3 endPos;

	private Vector3 jumpForward;

	private float maxHeight;

	private float jumpBeginY;

	private float jumpDistance;

	public Vector3 BeginPos
	{
		get
		{
			return beginPos;
		}
	}

	public Vector3 EndPos
	{
		get
		{
			return endPos;
		}
	}

	public Vector3 JumpNormal
	{
		get
		{
			return jumpForward;
		}
	}

	public float MaxHeight
	{
		get
		{
			return maxHeight;
		}
	}

	public float JumpBeginY
	{
		get
		{
			return jumpBeginY;
		}
	}

	public JumpDirection JumpDir { get; private set; }

	public JumpUtil()
	{
	}

	public JumpUtil(float beginPosZ, float endPosZ, float maxHeight, float jumpBeginY)
	{
		SetJumpInfo(beginPosZ, endPosZ, maxHeight, jumpBeginY);
	}

	public JumpUtil(Vector3 beginPos, Vector3 endPos, Vector3 jumpNormal, float maxHeight, float jumpBeginY)
	{
		SetJumpInfo(beginPos, endPos, jumpNormal, maxHeight, jumpBeginY);
	}

	public void SetJumpInfo(float beginPosZ, float endPosZ, float maxHeight, float jumpBeginY)
	{
		this.beginPosZ = beginPosZ;
		this.endPosZ = endPosZ;
		this.maxHeight = maxHeight;
		this.jumpBeginY = jumpBeginY;
		jumpDistance = beginPosZ - endPosZ;
		JumpDir = ((!(jumpDistance > 0f)) ? JumpDirection.backward : JumpDirection.forward);
	}

	public void SetJumpInfo(Vector3 beginPos, Vector3 endPos, Vector3 jumpForward, float maxHeight, float jumpBeginY)
	{
		this.beginPos = beginPos;
		this.endPos = endPos;
		this.jumpForward = jumpForward;
		this.maxHeight = maxHeight;
		this.jumpBeginY = jumpBeginY;
		jumpDistance = Vector3.Dot(endPos - beginPos, jumpForward);
		JumpDir = ((!(jumpDistance > 0f)) ? JumpDirection.backward : JumpDirection.forward);
	}

	public float GetHeightByPosZ(float posZ, ref float percent)
	{
		percent = Mathf.Max(0f, GetPercentByPosZ(posZ));
		return GetHeightByPercent(percent);
	}

	public float GetHeightByPosZ(Vector3 pos, ref float percent)
	{
		percent = Mathf.Max(0f, GetPercentByPosZ(pos));
		return GetHeightByPercent(percent);
	}

	public float GetHeightByPercent(float percent)
	{
		return jumpBeginY + GetDeltaHeightByPercent(percent);
	}

	private float GetDeltaHeightByPercent(float percent)
	{
		float num = 0f;
		num = percent * jumpDistance;
		return maxHeight - 4f * maxHeight / Mathf.Pow(jumpDistance, 2f) * Mathf.Pow(num - jumpDistance / 2f, 2f);
	}

	public float GetPercentByPosZ(float posZ)
	{
		return GetDeltaZ(posZ) / jumpDistance;
	}

	public float GetPercentByPosZ(Vector3 pos)
	{
		return GetDeltaZ(pos) / jumpDistance;
	}

	private float GetDeltaZ(float posZ)
	{
		return posZ - beginPosZ;
	}

	private float GetDeltaZ(Vector3 pos)
	{
		return Vector3.Dot(pos - beginPos, jumpForward);
	}
}
