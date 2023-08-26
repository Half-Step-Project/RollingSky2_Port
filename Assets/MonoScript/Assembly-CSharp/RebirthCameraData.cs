using UnityEngine;

public class RebirthCameraData
{
	public Vector3 CameraLocalPos;

	public Vector3 CameraLocalAngle;

	public Vector3 RealCameraLocalPos;

	public Vector3 CameraTargetLocalPos;

	public FollowData m_followData;

	public CameraController.CameraState m_cameraState;

	public string m_animationName = string.Empty;

	public float m_animationTime;

	public RebirthCurvedBendData m_curvedBendData;

	public float m_farClipPlane = 100f;

	public float m_fiedOfView = 85f;

	public bool m_isPlayParticle;

	public float m_lookAtSlerpSpeed = 10f;

	public int m_playingParticleIndex = -1;
}
