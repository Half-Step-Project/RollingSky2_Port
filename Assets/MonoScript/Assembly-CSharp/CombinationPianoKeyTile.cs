using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

public class CombinationPianoKeyTile : BaseTile, IBrushTrigger
{
	public enum EaseType
	{
		Linear,
		InOutCubic,
		InOutQuintic,
		InQuintic,
		InQuartic,
		InCubic,
		InQuadratic,
		OutQuintic,
		OutQuartic,
		OutCubic,
		OutInCubic,
		BackInCubic,
		BackInQuartic,
		OutBackCubic,
		OutBackQuartic,
		OutElasticSmall,
		OutElasticBig,
		InElasticSmall,
		InElasticBig
	}

	[Serializable]
	public struct Data : IReadWriteBytes
	{
		[Header("轴的移动:")]
		public bool m_moveBackAndForth;

		public Vector3 m_moveBackAndForthFrom;

		public Vector3 m_moveBackAndForthTo;

		public EaseType m_moveBackAndForthEaseType;

		public float m_moveBackAndForthBegin;

		public float m_moveBackAndForthEnd;

		[Header("轴的旋转:")]
		public bool m_rotate;

		public Vector3 m_rotateFrom;

		public Vector3 m_rotateTo;

		public EaseType m_rotateEaseType;

		public float m_rotateBegin;

		public float m_rotateEnd;

		[Header("滑竿的移动:")]
		public bool m_moveLeftAndRight;

		public Vector3 m_moveLeftAndRightFrom;

		public Vector3 m_moveLeftAndRightTo;

		public float m_moveLeftAndRightBegin;

		public float m_moveLeftAndRightEnd;

		public EaseType m_moveLeftAndRightEaseType;

		public bool m_moveLeftAndRightPingPong;

		public float m_moveLeftAndRightCenterProgress;

		public float m_moveLeftAndRightSpeed;

		[Header("白键的旋转:")]
		public bool m_keyRotate;

		public Vector3 m_keyRotateFrom;

		public Vector3 m_keyRotateTo;

		public EaseType m_keyRotateType;

		public float m_keyRotateBegin;

		public float m_keyRotateEnd;

		[Header("黑键的移动:")]
		public bool m_backKeyMove;

		public Vector3 m_backKeyFrom;

		public Vector3 m_backKeyTo;

		public EaseType m_backKeyType;

		public float m_backKeyBegin;

		public float m_backKeyEnd;

		[Header("踩下后，轴的旋转:")]
		public bool m_stepOn;

		public Vector3 m_stepOnFrom;

		public Vector3 m_stepOnTo;

		public EaseType m_stepOnType;

		public float m_stepOnBegin;

		public float m_stepOnEnd;

		public void ReadBytes(byte[] bytes, ref int startIndex)
		{
			m_moveBackAndForth = bytes.GetBoolean(ref startIndex);
			m_moveBackAndForthFrom = bytes.GetVector3(ref startIndex);
			m_moveBackAndForthTo = bytes.GetVector3(ref startIndex);
			m_moveBackAndForthEaseType = (EaseType)bytes.GetInt32(ref startIndex);
			m_moveBackAndForthBegin = bytes.GetSingle(ref startIndex);
			m_moveBackAndForthEnd = bytes.GetSingle(ref startIndex);
			m_rotate = bytes.GetBoolean(ref startIndex);
			m_rotateFrom = bytes.GetVector3(ref startIndex);
			m_rotateTo = bytes.GetVector3(ref startIndex);
			m_rotateEaseType = (EaseType)bytes.GetInt32(ref startIndex);
			m_rotateBegin = bytes.GetSingle(ref startIndex);
			m_rotateEnd = bytes.GetSingle(ref startIndex);
			m_moveLeftAndRight = bytes.GetBoolean(ref startIndex);
			m_moveLeftAndRightFrom = bytes.GetVector3(ref startIndex);
			m_moveLeftAndRightTo = bytes.GetVector3(ref startIndex);
			m_moveLeftAndRightBegin = bytes.GetSingle(ref startIndex);
			m_moveLeftAndRightEnd = bytes.GetSingle(ref startIndex);
			m_moveLeftAndRightEaseType = (EaseType)bytes.GetInt32(ref startIndex);
			m_moveLeftAndRightPingPong = bytes.GetBoolean(ref startIndex);
			m_moveLeftAndRightCenterProgress = bytes.GetSingle(ref startIndex);
			m_moveLeftAndRightSpeed = bytes.GetSingle(ref startIndex);
			m_keyRotate = bytes.GetBoolean(ref startIndex);
			m_keyRotateFrom = bytes.GetVector3(ref startIndex);
			m_keyRotateTo = bytes.GetVector3(ref startIndex);
			m_keyRotateType = (EaseType)bytes.GetInt32(ref startIndex);
			m_keyRotateBegin = bytes.GetSingle(ref startIndex);
			m_keyRotateEnd = bytes.GetSingle(ref startIndex);
			m_backKeyMove = bytes.GetBoolean(ref startIndex);
			m_backKeyFrom = bytes.GetVector3(ref startIndex);
			m_backKeyTo = bytes.GetVector3(ref startIndex);
			m_backKeyType = (EaseType)bytes.GetInt32(ref startIndex);
			m_backKeyBegin = bytes.GetSingle(ref startIndex);
			m_backKeyEnd = bytes.GetSingle(ref startIndex);
			m_stepOn = bytes.GetBoolean(ref startIndex);
			m_stepOnFrom = bytes.GetVector3(ref startIndex);
			m_stepOnTo = bytes.GetVector3(ref startIndex);
			m_stepOnType = (EaseType)bytes.GetInt32(ref startIndex);
			m_stepOnBegin = bytes.GetSingle(ref startIndex);
			m_stepOnEnd = bytes.GetSingle(ref startIndex);
		}

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			ReadBytes(bytes, ref startIndex);
		}

		public byte[] WriteBytes()
		{
			int offset = 0;
			return WriteBytes(ref offset);
		}

		public byte[] WriteBytes(ref int offset)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteByteArray(m_moveBackAndForth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveBackAndForthFrom.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveBackAndForthTo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_moveBackAndForthEaseType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveBackAndForthBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveBackAndForthEnd.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_rotate.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_rotateFrom.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_rotateTo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_rotateEaseType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_rotateBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_rotateEnd.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightFrom.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightTo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightEnd.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_moveLeftAndRightEaseType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightPingPong.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightCenterProgress.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_moveLeftAndRightSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_keyRotate.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_keyRotateFrom.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_keyRotateTo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_keyRotateType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_keyRotateBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_keyRotateEnd.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_backKeyMove.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_backKeyFrom.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_backKeyTo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_backKeyType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_backKeyBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_backKeyEnd.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_stepOn.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_stepOnFrom.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_stepOnTo.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_stepOnType).GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_stepOnBegin.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_stepOnEnd.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	protected GameObject m_axisObject;

	protected GameObject m_slipperyObject;

	protected GameObject m_writeKeyObject;

	protected GameObject m_blackKeyObject;

	protected BoxCollider m_writeKeyCollider;

	protected BoxCollider m_blackKeyCollider;

	public Data m_data;

	public float m_distance;

	private float m_p;

	private float m_clampDistance;

	private float m_movement;

	private float m_onceDistance;

	private float m_clampDistanceRemainder;

	private float m_positiveAndNegative;

	private int m_clampDistanceInteger;

	private bool m_moveBackAndForthFinished;

	private bool m_rotateFinshed;

	private bool m_moveLeftAndRightFinished;

	private bool m_keyRotateFinished;

	private bool m_backKeyMoveFinished;

	private bool m_stepOnFinished;

	private bool m_moveBackAndForthStart;

	private bool m_rotateStart;

	private bool m_moveLeftAndRightStart;

	private bool m_keyRotateStart;

	private bool m_backKeyMoveStart;

	private Vector3 m_vector3_01 = Vector3.zero;

	protected Vector3 m_axisObjectPosition;

	protected Vector3 m_axisObjectRotate;

	protected Vector3 m_slipperyObjectPosition;

	protected Vector3 m_slipperyObjectRotate;

	protected Vector3 m_blackKeyObjectPosition;

	protected Vector3 m_blackKeyObjectRotate;

	protected Vector3 m_writeKeyObjectPosition;

	protected Vector3 m_writeKeyObjectRotate;

	public override float TileWidth
	{
		get
		{
			return 1.5f + BaseTile.RecycleHeightTolerance;
		}
	}

	public override float TileHeight
	{
		get
		{
			return base.TileHeight;
		}
	}

	public override float RealPosY
	{
		get
		{
			if (m_writeKeyObject != null && m_writeKeyCollider != null)
			{
				return m_writeKeyCollider.transform.position.y + m_writeKeyCollider.center.y + m_writeKeyCollider.size.y / 2f;
			}
			return base.transform.position.y;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	protected float SwitchEaseType(EaseType easeType, float t, float from, float to, float d)
	{
		float num = to - from;
		switch (easeType)
		{
		case EaseType.Linear:
			num = Linear(t, from, num, d);
			break;
		case EaseType.InOutCubic:
			num = InOutCubic(t, from, num, d);
			break;
		case EaseType.InOutQuintic:
			num = InOutQuintic(t, from, num, d);
			break;
		case EaseType.InQuintic:
			num = InQuintic(t, from, num, d);
			break;
		case EaseType.InQuartic:
			num = InQuartic(t, from, num, d);
			break;
		case EaseType.InCubic:
			num = InCubic(t, from, num, d);
			break;
		case EaseType.InQuadratic:
			num = InQuadratic(t, from, num, d);
			break;
		case EaseType.OutQuintic:
			num = OutQuintic(t, from, num, d);
			break;
		case EaseType.OutQuartic:
			num = OutQuartic(t, from, num, d);
			break;
		case EaseType.OutCubic:
			num = OutCubic(t, from, num, d);
			break;
		case EaseType.OutInCubic:
			num = OutInCubic(t, from, num, d);
			break;
		case EaseType.BackInCubic:
			num = BackInCubic(t, from, num, d);
			break;
		case EaseType.BackInQuartic:
			num = BackInQuartic(t, from, num, d);
			break;
		case EaseType.OutBackCubic:
			num = OutBackCubic(t, from, num, d);
			break;
		case EaseType.OutBackQuartic:
			num = OutBackQuartic(t, from, num, d);
			break;
		case EaseType.OutElasticSmall:
			num = OutElasticSmall(t, from, num, d);
			break;
		case EaseType.OutElasticBig:
			num = OutElasticBig(t, from, num, d);
			break;
		case EaseType.InElasticSmall:
			num = InElasticSmall(t, from, num, d);
			break;
		case EaseType.InElasticBig:
			num = InElasticBig(t, from, num, d);
			break;
		}
		return num;
	}

	private float Linear(float time, float begion, float change, float duration)
	{
		time /= duration;
		return begion + change * time;
	}

	private float InOutCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (-2f * num2 + 3f * num);
	}

	private float InOutQuintic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (6f * num2 * num + -15f * num * num + 10f * num2);
	}

	private float InQuintic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (num2 * num);
	}

	private float InQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		return begion + change * (num * num);
	}

	private float InCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time * time;
		return begion + change * num;
	}

	private float InQuadratic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		return begion + change * num;
	}

	private float OutQuintic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (num2 * num + -5f * num * num + 10f * num2 + -10f * num + 5f * time);
	}

	private float OutQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (-1f * num * num + 4f * num2 + -6f * num + 4f * time);
	}

	private float OutCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (num2 + -3f * num + 3f * time);
	}

	private float OutInCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (4f * num2 + -6f * num + 3f * time);
	}

	private float OutInQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (6f * num2 + -9f * num + 4f * time);
	}

	private float BackInCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (4f * num2 + -3f * num);
	}

	private float BackInQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (2f * num * num + 2f * num2 + -3f * num);
	}

	private float OutBackCubic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (4f * num2 + -9f * num + 6f * time);
	}

	private float OutBackQuartic(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (-2f * num * num + 10f * num2 + -15f * num + 8f * time);
	}

	private float OutElasticSmall(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (33f * num2 * num + -106f * num * num + 126f * num2 + -67f * num + 15f * time);
	}

	private float OutElasticBig(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (56f * num2 * num + -175f * num * num + 200f * num2 + -100f * num + 20f * time);
	}

	private float InElasticSmall(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (33f * num2 * num + -59f * num * num + 32f * num2 + -5f * num);
	}

	private float InElasticBig(float time, float begion, float change, float duration)
	{
		float num = (time /= duration) * time;
		float num2 = num * time;
		return begion + change * (56f * num2 * num + -105f * num * num + 60f * num2 + -10f * num);
	}

	public override void Initialize()
	{
		FindTileChindren();
		RecordingObjectsData();
		ResetFinished();
		UpdateElement();
		base.Initialize();
	}

	public override void UpdateElement()
	{
		if (Application.isPlaying)
		{
			m_distance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		}
		if (m_data.m_moveBackAndForth && m_axisObject != null)
		{
			if (m_distance < m_data.m_moveBackAndForthBegin && !m_moveBackAndForthStart)
			{
				m_axisObject.transform.localPosition = m_data.m_moveBackAndForthFrom;
				m_moveBackAndForthStart = true;
			}
			else if (m_distance >= m_data.m_moveBackAndForthBegin && m_distance <= m_data.m_moveBackAndForthEnd)
			{
				m_p = (m_data.m_moveBackAndForthBegin - m_distance) / (m_data.m_moveBackAndForthBegin - m_data.m_moveBackAndForthEnd);
				m_vector3_01.x = SwitchEaseType(m_data.m_moveBackAndForthEaseType, m_p, m_data.m_moveBackAndForthFrom.x, m_data.m_moveBackAndForthTo.x, 1f);
				m_vector3_01.y = SwitchEaseType(m_data.m_moveBackAndForthEaseType, m_p, m_data.m_moveBackAndForthFrom.y, m_data.m_moveBackAndForthTo.y, 1f);
				m_vector3_01.z = SwitchEaseType(m_data.m_moveBackAndForthEaseType, m_p, m_data.m_moveBackAndForthFrom.z, m_data.m_moveBackAndForthTo.z, 1f);
				m_axisObject.transform.localPosition = m_vector3_01;
			}
			if (m_distance >= m_data.m_moveBackAndForthEnd && !m_moveBackAndForthFinished)
			{
				m_axisObject.transform.localPosition = m_data.m_moveBackAndForthTo;
				m_moveBackAndForthFinished = true;
			}
		}
		if (m_data.m_rotate && m_axisObject != null)
		{
			if (m_distance < m_data.m_rotateBegin && !m_rotateStart)
			{
				m_axisObject.transform.localEulerAngles = m_data.m_rotateFrom;
				m_rotateStart = true;
			}
			else if (m_distance >= m_data.m_rotateBegin && m_distance <= m_data.m_rotateEnd)
			{
				m_p = (m_data.m_rotateBegin - m_distance) / (m_data.m_rotateBegin - m_data.m_rotateEnd);
				m_vector3_01.x = SwitchEaseType(m_data.m_rotateEaseType, m_p, m_data.m_rotateFrom.x, m_data.m_rotateTo.x, 1f);
				m_vector3_01.y = SwitchEaseType(m_data.m_rotateEaseType, m_p, m_data.m_rotateFrom.y, m_data.m_rotateTo.y, 1f);
				m_vector3_01.z = SwitchEaseType(m_data.m_rotateEaseType, m_p, m_data.m_rotateFrom.z, m_data.m_rotateTo.z, 1f);
				m_axisObject.transform.localEulerAngles = m_vector3_01;
			}
			if (m_distance >= m_data.m_rotateEnd && !m_rotateFinshed)
			{
				m_axisObject.transform.localEulerAngles = m_data.m_rotateTo;
				m_rotateFinshed = true;
			}
		}
		if (m_data.m_moveLeftAndRight && m_slipperyObject != null)
		{
			if (m_distance < m_data.m_moveLeftAndRightBegin && !m_moveLeftAndRightStart)
			{
				m_slipperyObject.transform.localPosition = m_data.m_moveLeftAndRightFrom;
				m_moveLeftAndRightStart = true;
			}
			else if (m_distance >= m_data.m_moveLeftAndRightBegin && m_distance <= m_data.m_moveLeftAndRightEnd)
			{
				if (!m_data.m_moveLeftAndRightPingPong)
				{
					m_p = (m_data.m_moveLeftAndRightBegin - m_distance) / (m_data.m_moveLeftAndRightBegin - m_data.m_moveLeftAndRightEnd);
					m_vector3_01.x = SwitchEaseType(m_data.m_moveLeftAndRightEaseType, m_p, m_data.m_moveLeftAndRightFrom.x, m_data.m_moveLeftAndRightTo.x, 1f);
					m_vector3_01.y = SwitchEaseType(m_data.m_moveLeftAndRightEaseType, m_p, m_data.m_moveLeftAndRightFrom.y, m_data.m_moveLeftAndRightTo.y, 1f);
					m_vector3_01.z = SwitchEaseType(m_data.m_moveLeftAndRightEaseType, m_p, m_data.m_moveLeftAndRightFrom.z, m_data.m_moveLeftAndRightTo.z, 1f);
					m_slipperyObject.transform.localPosition = m_vector3_01;
				}
				else
				{
					m_clampDistance = m_distance - m_data.m_moveLeftAndRightBegin;
					m_movement = m_clampDistance * m_data.m_moveLeftAndRightSpeed;
					m_p = (Mathf.Sin(m_movement / (float)Math.PI + Mathf.Asin(1f * m_data.m_moveLeftAndRightCenterProgress - 0.5f) * (float)Math.PI) + 1f) * 0.5f;
					m_slipperyObject.transform.localPosition = Vector3.Lerp(m_data.m_moveLeftAndRightFrom, m_data.m_moveLeftAndRightTo, m_p);
				}
			}
			if (m_distance >= m_data.m_moveLeftAndRightEnd && !m_data.m_moveLeftAndRightPingPong && !m_moveLeftAndRightFinished)
			{
				m_slipperyObject.transform.localPosition = m_data.m_moveLeftAndRightTo;
				m_moveLeftAndRightFinished = true;
			}
		}
		if (m_data.m_keyRotate && m_writeKeyObject != null)
		{
			if (m_distance < m_data.m_keyRotateBegin && !m_keyRotateStart)
			{
				m_writeKeyObject.transform.localEulerAngles = m_data.m_keyRotateFrom;
				m_keyRotateStart = true;
			}
			else if (m_distance >= m_data.m_keyRotateBegin && m_distance <= m_data.m_keyRotateEnd)
			{
				m_p = (m_data.m_keyRotateBegin - m_distance) / (m_data.m_keyRotateBegin - m_data.m_keyRotateEnd);
				m_vector3_01.x = SwitchEaseType(m_data.m_keyRotateType, m_p, m_data.m_keyRotateFrom.x, m_data.m_keyRotateTo.x, 1f);
				m_vector3_01.y = SwitchEaseType(m_data.m_keyRotateType, m_p, m_data.m_keyRotateFrom.y, m_data.m_keyRotateTo.y, 1f);
				m_vector3_01.z = SwitchEaseType(m_data.m_keyRotateType, m_p, m_data.m_keyRotateFrom.z, m_data.m_keyRotateTo.z, 1f);
				m_writeKeyObject.transform.localEulerAngles = m_vector3_01;
			}
			if (m_distance >= m_data.m_keyRotateEnd && !m_keyRotateFinished)
			{
				m_writeKeyObject.transform.localEulerAngles = m_data.m_keyRotateTo;
				m_keyRotateFinished = true;
			}
		}
		if (m_data.m_backKeyMove && m_blackKeyObject != null)
		{
			if (m_distance < m_data.m_backKeyBegin && !m_backKeyMoveStart)
			{
				m_blackKeyObject.transform.localPosition = m_data.m_backKeyFrom;
				m_backKeyMoveStart = true;
			}
			else if (m_distance >= m_data.m_backKeyBegin && m_distance <= m_data.m_backKeyEnd)
			{
				m_p = (m_data.m_backKeyBegin - m_distance) / (m_data.m_backKeyBegin - m_data.m_backKeyEnd);
				m_vector3_01.x = SwitchEaseType(m_data.m_backKeyType, m_p, m_data.m_backKeyFrom.x, m_data.m_backKeyTo.x, 1f);
				m_vector3_01.y = SwitchEaseType(m_data.m_backKeyType, m_p, m_data.m_backKeyFrom.y, m_data.m_backKeyTo.y, 1f);
				m_vector3_01.z = SwitchEaseType(m_data.m_backKeyType, m_p, m_data.m_backKeyFrom.z, m_data.m_backKeyTo.z, 1f);
				m_blackKeyObject.transform.localPosition = m_vector3_01;
			}
			if (m_distance >= m_data.m_backKeyEnd && !m_backKeyMoveFinished)
			{
				m_blackKeyObject.transform.localPosition = m_data.m_backKeyTo;
				m_backKeyMoveFinished = true;
			}
		}
		if (m_data.m_stepOn && m_axisObject != null)
		{
			if (m_distance >= m_data.m_stepOnBegin && m_distance <= m_data.m_stepOnEnd)
			{
				m_onceDistance = (m_data.m_stepOnEnd - m_data.m_stepOnBegin) / 2f;
				m_clampDistance = m_data.m_stepOnBegin - m_distance;
				m_clampDistanceRemainder = m_clampDistance % m_onceDistance;
				m_clampDistanceInteger = Mathf.FloorToInt(m_clampDistance / m_onceDistance);
				m_positiveAndNegative = m_clampDistanceRemainder / m_onceDistance;
				m_p = ((m_clampDistanceInteger % 2 == 0) ? (1f + m_positiveAndNegative) : (0f - m_positiveAndNegative));
				m_vector3_01.x = SwitchEaseType(m_data.m_stepOnType, m_p, m_data.m_stepOnFrom.x, m_data.m_stepOnTo.x, 1f);
				m_vector3_01.y = SwitchEaseType(m_data.m_stepOnType, m_p, m_data.m_stepOnFrom.y, m_data.m_stepOnTo.y, 1f);
				m_vector3_01.z = SwitchEaseType(m_data.m_stepOnType, m_p, m_data.m_stepOnFrom.z, m_data.m_stepOnTo.z, 1f);
				m_axisObject.transform.localEulerAngles = m_vector3_01;
			}
			if (m_distance >= m_data.m_stepOnEnd && !m_stepOnFinished)
			{
				m_axisObject.transform.localEulerAngles = m_data.m_stepOnFrom;
				m_stepOnFinished = true;
			}
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<Data>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (Data)objs[0];
	}

	public override void ResetElement()
	{
		m_axisObject.transform.rotation = Quaternion.identity;
		ResetFinished();
		RefreshObjectsData();
		base.ResetElement();
	}

	private void FindTileChindren()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		dictionary.TryGetValue("Waltz_Tile_Piano_Zhou", out m_axisObject);
		dictionary.TryGetValue("Waltz_Tile_Piano01_Gan", out m_slipperyObject);
		dictionary.TryGetValue("Waltz_Tile_Piano01_White", out m_writeKeyObject);
		dictionary.TryGetValue("Waltz_Tile_Piano01_Black", out m_blackKeyObject);
		m_writeKeyCollider = ((m_writeKeyObject == null) ? null : m_writeKeyObject.GetComponent<BoxCollider>());
		m_blackKeyCollider = ((m_blackKeyObject == null) ? null : m_blackKeyObject.GetComponent<BoxCollider>());
	}

	protected void ResetFinished()
	{
		m_moveBackAndForthFinished = false;
		m_rotateFinshed = false;
		m_moveLeftAndRightFinished = false;
		m_keyRotateFinished = false;
		m_backKeyMoveFinished = false;
		m_stepOnFinished = false;
		m_moveBackAndForthStart = false;
		m_rotateStart = false;
		m_moveLeftAndRightStart = false;
		m_keyRotateStart = false;
		m_backKeyMoveStart = false;
	}

	public virtual void TriggerEnter(BaseRole ball, Collider collider)
	{
		if (m_writeKeyCollider != null && collider == m_writeKeyCollider)
		{
			TriggerEnter(ball);
		}
		else if (m_blackKeyCollider != null && collider == m_blackKeyCollider)
		{
			CrashBall(ball);
		}
	}

	protected void CrashBall(BaseRole ball)
	{
		if ((bool)ball && !GameController.IfNotDeath)
		{
			ball.CrashBall();
		}
	}

	protected virtual void RecordingObjectsData()
	{
		if (m_axisObject != null)
		{
			m_axisObjectPosition = m_axisObject.transform.localPosition;
			m_axisObjectRotate = m_axisObject.transform.localEulerAngles;
		}
		if (m_slipperyObject != null)
		{
			m_slipperyObjectPosition = m_slipperyObject.transform.localPosition;
			m_slipperyObjectRotate = m_slipperyObject.transform.localEulerAngles;
		}
		if (m_blackKeyObject != null)
		{
			m_blackKeyObjectPosition = m_blackKeyObject.transform.localPosition;
			m_blackKeyObjectRotate = m_blackKeyObject.transform.localEulerAngles;
		}
		if (m_writeKeyObject != null)
		{
			m_writeKeyObjectPosition = m_writeKeyObject.transform.localPosition;
			m_writeKeyObjectRotate = m_writeKeyObject.transform.localEulerAngles;
		}
	}

	protected virtual void RefreshObjectsData()
	{
		if (m_axisObject != null)
		{
			m_axisObject.transform.localPosition = m_axisObjectPosition;
			m_axisObject.transform.localEulerAngles = m_axisObjectRotate;
		}
		if (m_slipperyObject != null)
		{
			m_slipperyObject.transform.localPosition = m_slipperyObjectPosition;
			m_slipperyObject.transform.localEulerAngles = m_slipperyObjectRotate;
		}
		if (m_blackKeyObject != null)
		{
			m_blackKeyObject.transform.localPosition = m_blackKeyObjectPosition;
			m_blackKeyObject.transform.localEulerAngles = m_blackKeyObjectRotate;
		}
		if (m_writeKeyObject != null)
		{
			m_writeKeyObject.transform.localPosition = m_writeKeyObjectPosition;
			m_writeKeyObject.transform.localEulerAngles = m_writeKeyObjectRotate;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_CombinationPianoKeyTile_DATA rD_CombinationPianoKeyTile_DATA = JsonUtility.FromJson<RD_CombinationPianoKeyTile_DATA>(rd_data as string);
		if (m_axisObject != null)
		{
			m_axisObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_axisObject);
		}
		if (m_slipperyObject != null)
		{
			m_slipperyObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_slipperyObject);
		}
		if (m_writeKeyObject != null)
		{
			m_writeKeyObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_writeKeyObject);
		}
		if (m_blackKeyObject != null)
		{
			m_blackKeyObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_blackKeyObject);
		}
		commonState = rD_CombinationPianoKeyTile_DATA.commonState;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_CombinationPianoKeyTile_DATA rD_CombinationPianoKeyTile_DATA = new RD_CombinationPianoKeyTile_DATA();
		if (m_axisObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_axisObject = m_axisObject.transform.GetTransData();
		}
		if (m_slipperyObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_slipperyObject = m_slipperyObject.transform.GetTransData();
		}
		if (m_writeKeyObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_writeKeyObject = m_writeKeyObject.transform.GetTransData();
		}
		if (m_blackKeyObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_blackKeyObject = m_blackKeyObject.transform.GetTransData();
		}
		rD_CombinationPianoKeyTile_DATA.commonState = commonState;
		return JsonUtility.ToJson(rD_CombinationPianoKeyTile_DATA);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_CombinationPianoKeyTile_DATA rD_CombinationPianoKeyTile_DATA = Bson.ToObject<RD_CombinationPianoKeyTile_DATA>(rd_data);
		if (m_axisObject != null)
		{
			m_axisObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_axisObject);
		}
		if (m_slipperyObject != null)
		{
			m_slipperyObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_slipperyObject);
		}
		if (m_writeKeyObject != null)
		{
			m_writeKeyObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_writeKeyObject);
		}
		if (m_blackKeyObject != null)
		{
			m_blackKeyObject.transform.SetTransData(rD_CombinationPianoKeyTile_DATA.m_blackKeyObject);
		}
		commonState = rD_CombinationPianoKeyTile_DATA.commonState;
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_CombinationPianoKeyTile_DATA rD_CombinationPianoKeyTile_DATA = new RD_CombinationPianoKeyTile_DATA();
		if (m_axisObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_axisObject = m_axisObject.transform.GetTransData();
		}
		if (m_slipperyObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_slipperyObject = m_slipperyObject.transform.GetTransData();
		}
		if (m_writeKeyObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_writeKeyObject = m_writeKeyObject.transform.GetTransData();
		}
		if (m_blackKeyObject != null)
		{
			rD_CombinationPianoKeyTile_DATA.m_blackKeyObject = m_blackKeyObject.transform.GetTransData();
		}
		rD_CombinationPianoKeyTile_DATA.commonState = commonState;
		return Bson.ToBson(rD_CombinationPianoKeyTile_DATA);
	}
}
