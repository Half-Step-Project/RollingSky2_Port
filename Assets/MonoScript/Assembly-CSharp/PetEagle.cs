using System;
using System.Collections.Generic;
using UnityEngine;

public class PetEagle : PetBase, IPetMounts, IPetPathToMove
{
	private Vector3 m_gameInitializationPosition = new Vector3(-20f, 2f, -5f);

	private Vector3 m_targetPosition = new Vector3(-0.52f, 2.15f, 3f);

	private Vector3 m_followSpeed = new Vector3(5f, 2f, 20f);

	private float m_upMountsDuration = 1f;

	private float m_downMountsDuration = 1f;

	private AudioSource m_audioSource;

	private Vector3 m_initialScale;

	private Quaternion m_intialRotation;

	private PathToMoveByPetTrigger.PathToMoveByPetTriggerData m_pathData;

	public float UpMountsDuration
	{
		get
		{
			return m_upMountsDuration;
		}
		set
		{
			m_upMountsDuration = value;
		}
	}

	public float DownMountsDuration
	{
		get
		{
			return m_downMountsDuration;
		}
		set
		{
			m_downMountsDuration = value;
		}
	}

	public PathToMoveByPetTrigger.PathToMoveByPetTriggerData PathtoMoveData
	{
		get
		{
			return m_pathData;
		}
		set
		{
			m_pathData = value;
		}
	}

	public override void OnGameInitialization()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		m_petAnimator = dictionary["Pet_Eagle"].GetComponent<Animator>();
		m_audioSource = dictionary["Pet_Eagle"].GetComponent<AudioSource>();
		m_petAnimator.SetBool("FlyToTurn", false);
		m_petAnimator.SetBool("GlideToFly", false);
		m_initialScale = base.transform.localScale;
		m_intialRotation = base.transform.rotation;
	}

	protected override void OnSwitchPetState(PetState petState)
	{
		m_onUpdate = null;
		AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = null;
		switch (petState)
		{
		case PetState.Ready:
			m_petAnimator.SetBool("FlyToTurn", false);
			m_petAnimator.SetBool("GlideToFly", false);
			m_petAnimator.SetBool("FlyToSmallFly", false);
			m_petAnimator.SetBool("SmallFlyToFly", false);
			m_petAnimator.SetBool("AdmissionToFly", false);
			m_petAnimator.SetBool("ToDeath", false);
			base.transform.position = BaseRole.theBall.transform.TransformPoint(m_gameInitializationPosition);
			base.transform.parent = BaseRole.theBall.transform;
			break;
		case PetState.RebirthReady:
		case PetState.OriginRebirthReady:
			m_petAnimator.SetBool("FlyToTurn", false);
			m_petAnimator.SetBool("GlideToFly", false);
			m_petAnimator.SetBool("FlyToSmallFly", false);
			m_petAnimator.SetBool("SmallFlyToFly", false);
			m_petAnimator.SetBool("AdmissionToFly", false);
			m_petAnimator.SetBool("ToDeath", false);
			base.transform.position = BaseRole.theBall.transform.TransformPoint(m_gameInitializationPosition);
			base.transform.parent = BaseRole.theBall.transform;
			break;
		case PetState.Admission:
			base.transform.position = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
			AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = delegate
			{
				SwitchPetState(PetState.Follow);
			};
			m_petAnimator.Play("Pet_Eagle01_StartDive", 0, 0f);
			PlayAudio();
			break;
		case PetState.FastAdmission:
			base.transform.position = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
			AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = delegate
			{
				SwitchPetState(PetState.Follow);
			};
			m_petAnimator.Play("Pet_Eagle01_StartDive_Resurrection", 0, 0f);
			PlayAudio();
			break;
		case PetState.Follow:
			base.transform.parent = null;
			base.transform.rotation = m_intialRotation;
			AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = null;
			if (!m_petAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pet_Eagle01_LoopFly"))
			{
				m_petAnimator.SetBool("FlyToTurn", false);
				m_petAnimator.SetBool("GlideToFly", true);
				m_petAnimator.SetBool("FlyToSmallFly", false);
				m_petAnimator.SetBool("SmallFlyToFly", true);
				m_petAnimator.SetBool("ToDeath", false);
				m_petAnimator.SetBool("AdmissionToFly", true);
			}
			m_onUpdate = delegate
			{
				Vector3 vector = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
				base.transform.position = new Vector3(Mathf.Lerp(base.transform.position.x, vector.x, Time.deltaTime * m_followSpeed.x), Mathf.Lerp(base.transform.position.y, vector.y, Time.deltaTime * m_followSpeed.y), Mathf.Lerp(base.transform.position.z, vector.z, Time.deltaTime * m_followSpeed.z));
				float num = (0f - (vector.x - base.transform.position.x)) * 30f;
				if (Mathf.Abs(num) > 15f)
				{
					if (!m_petAnimator.GetCurrentAnimatorStateInfo(0).IsName("SmallFly"))
					{
						m_petAnimator.SetBool("SmallFlyToFly", false);
						m_petAnimator.SetBool("FlyToSmallFly", true);
					}
				}
				else if (!m_petAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pet_Eagle01_LoopFly"))
				{
					m_petAnimator.SetBool("FlyToSmallFly", false);
					m_petAnimator.SetBool("SmallFlyToFly", true);
				}
				num = Mathf.Clamp(num, -60f, 60f);
				base.transform.rotation = Quaternion.Euler(0f, 0f, num);
			};
			break;
		case PetState.UpMounts:
			OnUpMounts();
			break;
		case PetState.DownMounts:
			OnDownMounts();
			break;
		case PetState.Death:
		{
			Vector3 _currentPostion = base.transform.position;
			float _currentDistance = UnityEngine.Random.Range(4, 7);
			m_onUpdate = delegate
			{
				Vector3 b = new Vector3(BaseRole.theBall.transform.position.x, _currentPostion.y, BaseRole.theBall.BodyPart.RoleHead.position.z + _currentDistance);
				base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * 2f);
			};
			if (!m_petAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pet_Eagle01_LoopDeadFly"))
			{
				m_petAnimator.SetBool("ToDeath", true);
			}
			break;
		}
		case PetState.PathToMove:
			OnPathToMove();
			break;
		case PetState.Pose:
		case PetState.CloseAtHand:
			break;
		}
	}

	private void PlayAudio()
	{
		if ((bool)m_audioSource && GameController.Instance.GetPlayerDataModule.IsSoundPlayOn())
		{
			m_audioSource.Play();
		}
	}

	public void OnUpMounts()
	{
		m_petAnimator.SetBool("FlyToTurn", false);
		m_petAnimator.SetBool("GlideToFly", false);
		m_petAnimator.SetBool("FlyToSmallFly", false);
		m_petAnimator.SetBool("SmallFlyToFly", false);
		m_petAnimator.SetBool("AdmissionToFly", false);
		m_petAnimator.SetBool("ToDeath", false);
		Vector3 _to = new Vector3(m_initialScale.x * 3f, m_initialScale.y * 3f, m_initialScale.z * 3f);
		Quaternion _lastRotation = base.transform.rotation;
		Vector3 _lastPosition = base.transform.position;
		float _positionCurrentTime = 0f;
		float _positionCurrentProgress = 0f;
		bool _isOnTargetPosition = false;
		m_onUpdate = delegate
		{
			_positionCurrentTime += Time.deltaTime;
			if (_positionCurrentTime >= 0f && _positionCurrentTime < m_upMountsDuration)
			{
				m_petAnimator.SetBool("FlyToTurn", true);
				_positionCurrentProgress = _positionCurrentTime / m_upMountsDuration;
				base.transform.parent = BaseRole.theBall.BodyPart.RoleLeftFoot.transform;
				base.transform.localScale = Vector3.Lerp(m_initialScale, _to, _positionCurrentProgress);
				base.transform.rotation = Quaternion.Lerp(_lastRotation, BaseRole.theBall.ballModelObj.transform.rotation, _positionCurrentProgress);
				base.transform.position = Vector3.Lerp(_lastPosition, BaseRole.theBall.BodyPart.RoleLeftFoot.transform.position + new Vector3(0f, -0.2f, 0f), _positionCurrentProgress);
			}
			if (!_isOnTargetPosition && _positionCurrentTime > m_upMountsDuration)
			{
				m_petAnimator.SetBool("AdmissionToFly", true);
				base.transform.position = BaseRole.theBall.BodyPart.RoleLeftFoot.transform.position + new Vector3(0f, -0.2f, 0f);
				base.transform.localScale = _to;
				base.transform.rotation = BaseRole.theBall.ballModelObj.transform.rotation;
				base.transform.parent = BaseRole.theBall.BodyPart.RoleLeftFoot.transform;
				_isOnTargetPosition = true;
			}
		};
	}

	public void OnDownMounts()
	{
		float m_scaleTime = 1f;
		float _scaleCurrentTime = 0f;
		float _scaleCurrentProgress = 0f;
		bool _isScaleOver = false;
		Vector3 _scaleFrome = base.transform.localScale;
		m_petAnimator.Play("Pet_Eagle01_LoopFly", 0, 0f);
		m_onUpdate = delegate
		{
			_scaleCurrentTime += Time.deltaTime;
			if (_scaleCurrentTime >= 0f && _scaleCurrentTime <= m_scaleTime)
			{
				_scaleCurrentProgress = _scaleCurrentTime / m_scaleTime;
				base.transform.localScale = Vector3.Lerp(_scaleFrome, m_initialScale, _scaleCurrentProgress);
			}
			if (!_isScaleOver && _scaleCurrentTime > m_scaleTime)
			{
				m_petAnimator.SetBool("FlyToTurn", false);
				m_petAnimator.SetBool("GlideToFly", true);
				base.transform.parent = null;
				base.transform.localScale = m_initialScale;
				SwitchPetState(PetState.Follow);
				_isScaleOver = true;
			}
		};
	}

	public void OnPathToMove()
	{
		base.gameObject.transform.parent = null;
		m_petAnimator.SetBool("FlyToTurn", true);
		Vector3[] _path = null;
		if (m_pathData.m_bezierPositions == null || m_pathData.m_bezierPositions.Length == 0)
		{
			Vector3[] array = new Vector3[m_pathData.m_positions.Length + 1];
			array[0] = base.transform.position;
			for (int i = 0; i < m_pathData.m_positions.Length; i++)
			{
				array[i + 1] = m_pathData.m_positions[i];
			}
			_path = Bezier.GetPathByPositions(array, 10);
		}
		else
		{
			Vector3[] pathByPositions = Bezier.GetPathByPositions(new Vector3[2]
			{
				base.transform.position,
				m_pathData.m_bezierPositions[0]
			}, 4);
			Vector3[] array2 = new Vector3[pathByPositions.Length + m_pathData.m_bezierPositions.Length - 1];
			Array.Copy(pathByPositions, array2, pathByPositions.Length - 1);
			Array.Copy(m_pathData.m_bezierPositions, 0, array2, pathByPositions.Length - 1, m_pathData.m_bezierPositions.Length);
			_path = array2;
		}
		Vector3 _one = _path[0];
		int _nextIndex = 1;
		Vector3 _two = _path[_nextIndex];
		float _startTime = Time.time;
		float time = m_pathData.m_time;
		float _averageTime = time / (float)_path.Length;
		float _currentProportion = 0f;
		m_onUpdate = delegate
		{
			_currentProportion = (Time.time - _startTime) / _averageTime;
			if (m_pathData.m_isLookAtNextPoint)
			{
				Quaternion b = Quaternion.LookRotation(_two - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * m_pathData.m_lookAtSpeed);
			}
			base.transform.position = Vector3.Lerp(_one, _two, _currentProportion);
			if (_currentProportion >= 0.95f)
			{
				if (_nextIndex + 1 < _path.Length)
				{
					_startTime = Time.time;
					_one = _path[_nextIndex];
					_nextIndex++;
					_two = _path[_nextIndex];
				}
				else
				{
					base.transform.rotation = m_intialRotation;
					if (m_pathData.m_isFinishedStop)
					{
						SwitchPetState(m_pathData.m_finishedRangePetState);
					}
					else
					{
						m_petAnimator.Play("Pet_Eagle01_StartDive_Resurrection", 0, 0f);
						SwitchPetState(PetState.Follow);
					}
				}
			}
		};
	}
}
