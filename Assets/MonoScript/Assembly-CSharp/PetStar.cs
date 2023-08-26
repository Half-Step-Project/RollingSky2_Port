using System;
using System.Collections.Generic;
using UnityEngine;

public class PetStar : PetBase, IPetPathToMove
{
	private Vector3 m_readyPosition = new Vector3(2.5f, 27f, 29.5f);

	private Vector3 m_readyEulerAngles = new Vector3(0f, 180f, 0f);

	private Vector3 m_targetPosition = new Vector3(-0.6f, 2f, 7f);

	private Vector3 m_followSpeed = new Vector3(5f, 2f, 20f);

	private PathToMoveByPetTrigger.PathToMoveByPetTriggerData m_pathData;

	private TrailRenderer[] m_trailRenderers;

	private ParticleSystem[] m_particleSystems;

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
		m_petAnimator = dictionary["Pet_Star"].GetComponent<Animator>();
		m_particleSystems = dictionary["effect"].GetComponentsInChildren<ParticleSystem>();
		m_trailRenderers = dictionary["wings"].GetComponentsInChildren<TrailRenderer>();
	}

	protected override void OnSwitchPetState(PetState petState)
	{
		m_onUpdate = null;
		AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = null;
		switch (petState)
		{
		case PetState.Ready:
			base.gameObject.transform.position = m_readyPosition;
			base.gameObject.transform.eulerAngles = m_readyEulerAngles;
			m_petAnimator.Play("Ready", 0, 0f);
			PlayParticleSystems(m_particleSystems);
			StopTrailRenderers(m_trailRenderers);
			m_petAnimator.SetBool("ToAdmission", false);
			m_petAnimator.SetBool("ToAdmissionNext", false);
			m_petAnimator.SetBool("ToFollow", false);
			m_petAnimator.SetBool("ToPathToMove", false);
			m_petAnimator.SetBool("ToDeath", false);
			break;
		case PetState.RebirthReady:
		case PetState.OriginRebirthReady:
			base.gameObject.transform.position = m_readyPosition;
			base.gameObject.transform.eulerAngles = m_readyEulerAngles;
			m_petAnimator.Play("Ready", 0, 0f);
			PlayParticleSystems(m_particleSystems);
			StopTrailRenderers(m_trailRenderers);
			m_petAnimator.SetBool("ToAdmission", false);
			m_petAnimator.SetBool("ToAdmissionNext", false);
			m_petAnimator.SetBool("ToFollow", false);
			m_petAnimator.SetBool("ToPathToMove", false);
			m_petAnimator.SetBool("ToDeath", false);
			break;
		case PetState.Admission:
			PlayTrailRenderers(m_trailRenderers);
			AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = delegate(GameObject a, string s)
			{
				switch (s)
				{
				case "Admission":
					m_petAnimator.SetBool("ToAdmission", false);
					m_petAnimator.SetBool("ToAdmissionNext", true);
					StopTrailRenderers(m_trailRenderers);
					base.transform.position = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
					base.transform.parent = BaseRole.theBall.transform;
					break;
				case "FastAdmission":
					m_petAnimator.SetBool("ToAdmissionNext", false);
					SwitchPetState(PetState.Follow);
					break;
				case "ShowTrailRenderer":
					PlayTrailRenderers(m_trailRenderers);
					break;
				}
			};
			m_petAnimator.SetBool("ToAdmission", true);
			break;
		case PetState.Follow:
			m_petAnimator.SetBool("ToFollow", true);
			base.transform.position = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
			base.transform.parent = null;
			m_petAnimator.Play("Follow", 0, 0f);
			m_onUpdate = delegate
			{
				Vector3 vector = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
				base.transform.position = new Vector3(Mathf.Lerp(base.transform.position.x, vector.x, Time.deltaTime * m_followSpeed.x), Mathf.Lerp(base.transform.position.y, vector.y, Time.deltaTime * m_followSpeed.y), Mathf.Lerp(base.transform.position.z, vector.z, Time.deltaTime * m_followSpeed.z));
			};
			break;
		case PetState.PathToMove:
			OnPathToMove();
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
			m_petAnimator.SetBool("ToDeath", true);
			break;
		}
		case PetState.CloseAtHand:
			m_petAnimator.Play("Ready", 0, 0f);
			break;
		case PetState.Pose:
		case PetState.FastAdmission:
		case PetState.UpMounts:
		case PetState.DownMounts:
			break;
		}
	}

	private void PlayParticleSystems(ParticleSystem[] particleSystems)
	{
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].Play();
		}
	}

	private void StopParticleSystems(ParticleSystem[] particleSystems)
	{
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].Stop();
		}
	}

	private void PlayTrailRenderers(TrailRenderer[] trailRenderers)
	{
		for (int i = 0; i < trailRenderers.Length; i++)
		{
			trailRenderers[i].gameObject.SetActive(true);
		}
	}

	private void StopTrailRenderers(TrailRenderer[] trailRenderers)
	{
		for (int i = 0; i < trailRenderers.Length; i++)
		{
			trailRenderers[i].gameObject.SetActive(false);
		}
	}

	public void OnPathToMove()
	{
		base.gameObject.transform.parent = null;
		m_petAnimator.SetBool("ToPathToMove", true);
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
					m_petAnimator.SetBool("ToPathToMove", false);
					if (m_pathData.m_isFinishedStop)
					{
						SwitchPetState(m_pathData.m_finishedRangePetState);
					}
					else
					{
						m_petAnimator.SetBool("ToAdmissionNext", true);
						StopTrailRenderers(m_trailRenderers);
						base.transform.position = BaseRole.theBall.transform.TransformPoint(m_targetPosition);
						base.transform.parent = BaseRole.theBall.transform;
						AnimationEventListen.Get(m_petAnimator.gameObject).m_onStringDelegate = delegate(GameObject a, string s)
						{
							if (s == "FastAdmission")
							{
								m_petAnimator.SetBool("ToAdmissionNext", false);
								SwitchPetState(PetState.Follow);
							}
							else if (s == "ShowTrailRenderer")
							{
								PlayTrailRenderers(m_trailRenderers);
							}
						};
						m_petAnimator.SetBool("ToAdmission", true);
						m_onUpdate = null;
					}
				}
			}
		};
	}
}
