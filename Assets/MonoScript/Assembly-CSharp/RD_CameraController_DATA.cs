using System;

[Serializable]
internal class RD_CameraController_DATA
{
	public CameraController.CameraState m_cameraState;

	public bool ifPlayParticles;

	public int currentParticleIndex;

	public bool ifFollow;

	public FollowData m_followData;

	public bool m_isPlayingCurvedBend;

	public CurvedBendData m_curvedBendFrom;

	public CurvedBendData m_curvedBendTo;

	public float m_curvedBendDuration;

	public float m_currentCurvedBendTime;

	public float m_curvedBendProgress;

	public RD_ElementTransform_DATA mainCamTransData;

	public float farClipPlane;

	public float fieldOfView;

	public RD_ElementTransform_DATA targetTransData;

	public float m_lookAtSlerpSpeed;

	public RD_CameraMover_DATA cameraMover;

	public RD_ElementAnim_DATA cameraAnimation;

	public RD_ElementParticle_DATA[] cameraParticlesData;

	public RD_ElementParticle_DATA[] effectRootData;

	public RD_ElementAnim_DATA[] effectAnimations;

	public CurvedBendData currentCurveBendData;
}
