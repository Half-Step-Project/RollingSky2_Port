using UnityEngine;

public static class ElementRebirthUtil
{
	public static RD_ElementTransform_DATA GetTransData(this Transform trans)
	{
		RD_ElementTransform_DATA rD_ElementTransform_DATA = new RD_ElementTransform_DATA();
		if (trans != null)
		{
			rD_ElementTransform_DATA.ifActive = trans.gameObject.activeSelf;
			rD_ElementTransform_DATA.position = trans.position;
			rD_ElementTransform_DATA.quaternion = trans.rotation;
			rD_ElementTransform_DATA.localScale = trans.localScale;
		}
		return rD_ElementTransform_DATA;
	}

	public static void SetTransData(this Transform trans, RD_ElementTransform_DATA transData)
	{
		if (trans != null && transData != null)
		{
			trans.gameObject.SetActive(transData.ifActive);
			trans.position = transData.position;
			trans.rotation = transData.quaternion;
			trans.localScale = transData.localScale;
		}
	}

	public static RD_ElementTransform_DATA[] GetTransData(this Transform[] transforms)
	{
		RD_ElementTransform_DATA[] array = new RD_ElementTransform_DATA[transforms.Length];
		if (transforms != null)
		{
			for (int i = 0; i < transforms.Length; i++)
			{
				array[i] = transforms[i].GetTransData();
			}
		}
		return array;
	}

	public static void SetTransData(this Transform[] transforms, RD_ElementTransform_DATA[] datas)
	{
		if (transforms != null && datas != null)
		{
			for (int i = 0; i < transforms.Length; i++)
			{
				transforms[i].SetTransData(datas[i]);
			}
		}
	}

	public static RD_ElementAnim_DATA GetAnimData(this Animation anim, string animName = "")
	{
		RD_ElementAnim_DATA rD_ElementAnim_DATA = new RD_ElementAnim_DATA();
		if ((bool)anim)
		{
			if (string.IsNullOrEmpty(animName))
			{
				foreach (AnimationState item in anim)
				{
					if (anim.IsPlaying(item.name))
					{
						animName = item.name;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(animName))
			{
				animName = anim.clip.name;
			}
			if (!string.IsNullOrEmpty(animName))
			{
				rD_ElementAnim_DATA.animaName = animName;
				rD_ElementAnim_DATA.animTime = anim[rD_ElementAnim_DATA.animaName].time;
				rD_ElementAnim_DATA.ifPlaying = anim.isPlaying;
				return rD_ElementAnim_DATA;
			}
		}
		return null;
	}

	public static void SetAnimData(this Animation anim, RD_ElementAnim_DATA animData, ProcessState state)
	{
		if (!(anim != null) || animData == null || string.IsNullOrEmpty(animData.animaName))
		{
			return;
		}
		anim[animData.animaName].time = animData.animTime;
		if (animData.ifPlaying)
		{
			if ((state & ProcessState.Play) != 0)
			{
				anim.Play(animData.animaName);
			}
			if ((state & ProcessState.Pause) != 0)
			{
				anim.Play(animData.animaName);
				anim.Sample();
				anim.Stop();
			}
			if ((state & ProcessState.UnPause) != 0)
			{
				anim.Play(animData.animaName);
			}
			if ((state & ProcessState.Stop) != 0)
			{
				anim.Stop();
			}
		}
		else
		{
			anim.Stop();
		}
	}

	public static RD_ElementAnimator_DATA GetAnimData(this Animator animator, int layer = 0)
	{
		RD_ElementAnimator_DATA rD_ElementAnimator_DATA = new RD_ElementAnimator_DATA();
		if ((bool)animator)
		{
			rD_ElementAnimator_DATA.ifPlaying = animator.isActiveAndEnabled;
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			rD_ElementAnimator_DATA.speed = currentAnimatorStateInfo.speed;
			rD_ElementAnimator_DATA.normalizedTime = currentAnimatorStateInfo.normalizedTime;
			rD_ElementAnimator_DATA.animHash = currentAnimatorStateInfo.shortNameHash;
		}
		return rD_ElementAnimator_DATA;
	}

	public static void SetAnimData(this Animator animator, RD_ElementAnimator_DATA animatorData, ProcessState state, int layer = 0)
	{
		if ((bool)animator && animatorData.ifPlaying)
		{
			switch (state)
			{
			case ProcessState.Play:
				animator.Play(animatorData.animHash, 0, animatorData.normalizedTime);
				animator.speed = animatorData.speed;
				break;
			case ProcessState.UnPause:
				animator.speed = animatorData.speed;
				break;
			case ProcessState.Pause:
				animator.Play(animatorData.animHash, 0, animatorData.normalizedTime);
				animator.speed = 0f;
				break;
			case ProcessState.Stop:
				animator.enabled = false;
				break;
			}
		}
	}

	public static RD_ElementParticle_DATA GetParticleData(this ParticleSystem particle, int index = 0)
	{
		RD_ElementParticle_DATA rD_ElementParticle_DATA = new RD_ElementParticle_DATA();
		if (particle != null)
		{
			rD_ElementParticle_DATA.particleEnabled = particle.gameObject.activeSelf;
			rD_ElementParticle_DATA.isPlaying = particle.isPlaying;
			rD_ElementParticle_DATA.isEmitting = particle.isEmitting;
			rD_ElementParticle_DATA.particleTime = particle.time;
			rD_ElementParticle_DATA.particleIndex = index;
		}
		return rD_ElementParticle_DATA;
	}

	public static void SetParticleData(this ParticleSystem particle, RD_ElementParticle_DATA particleData, ProcessState state)
	{
		if (!(particle != null) || particleData == null)
		{
			return;
		}
		particle.gameObject.SetActive(particleData.particleEnabled);
		if (particleData.particleEnabled && particleData.isPlaying && particleData.isEmitting)
		{
			particle.Simulate(particleData.particleTime);
			if ((state & ProcessState.Play) != 0)
			{
				particle.Play();
			}
			if ((state & ProcessState.Pause) != 0)
			{
				particle.Pause();
			}
			if ((state & ProcessState.UnPause) != 0)
			{
				particle.Play();
			}
			if ((state & ProcessState.Stop) != 0)
			{
				particle.Stop();
			}
		}
		else
		{
			particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	public static RD_ElementParticle_DATA[] GetParticlesData(this ParticleSystem[] particles)
	{
		RD_ElementParticle_DATA[] array = null;
		if (particles == null)
		{
			array = new RD_ElementParticle_DATA[0];
		}
		else
		{
			array = new RD_ElementParticle_DATA[particles.Length];
			for (int i = 0; i < particles.Length; i++)
			{
				RD_ElementParticle_DATA rD_ElementParticle_DATA = new RD_ElementParticle_DATA();
				rD_ElementParticle_DATA.particleIndex = i;
				rD_ElementParticle_DATA.isPlaying = particles[i].isPlaying;
				rD_ElementParticle_DATA.isEmitting = particles[i].isEmitting;
				rD_ElementParticle_DATA.particleEnabled = particles[i].gameObject.activeSelf;
				rD_ElementParticle_DATA.particleTime = particles[i].time;
				array[i] = rD_ElementParticle_DATA;
			}
		}
		return array;
	}

	public static RD_ElementParticle_DATA[] GetParticlesData(this Transform effectRoot)
	{
		RD_ElementParticle_DATA[] array = null;
		if ((bool)effectRoot)
		{
			int childCount = effectRoot.childCount;
			if (childCount == 0)
			{
				array = new RD_ElementParticle_DATA[0];
			}
			else
			{
				array = new RD_ElementParticle_DATA[childCount];
				for (int i = 0; i < childCount; i++)
				{
					ParticleSystem component = effectRoot.GetChild(i).GetComponent<ParticleSystem>();
					if (!(component == null))
					{
						array[i] = component.GetParticleData(i);
					}
				}
			}
		}
		return array;
	}

	public static RD_ElementAnim_DATA[] GetAnimationData(this Transform effectRoot)
	{
		RD_ElementAnim_DATA[] array = null;
		if ((bool)effectRoot)
		{
			int childCount = effectRoot.childCount;
			if (childCount == 0)
			{
				array = new RD_ElementAnim_DATA[0];
			}
			else
			{
				array = new RD_ElementAnim_DATA[childCount];
				for (int i = 0; i < childCount; i++)
				{
					Animation componentInChildren = effectRoot.GetChild(i).GetComponentInChildren<Animation>();
					if ((bool)componentInChildren)
					{
						array[i] = componentInChildren.GetAnimData();
					}
					else
					{
						array[i] = new RD_ElementAnim_DATA();
					}
				}
			}
		}
		return array;
	}

	public static void SetParticlesData(this ParticleSystem[] particles, RD_ElementParticle_DATA[] particlesData, ProcessState state)
	{
		if (particles == null || particlesData == null)
		{
			return;
		}
		for (int i = 0; i < particles.Length; i++)
		{
			if (particles[i] != null)
			{
				particles[i].SetParticleData(particlesData[i], state);
			}
		}
	}

	public static RD_ElementTrail_DATA GetTrailData(this TrailRenderer trail)
	{
		RD_ElementTrail_DATA rD_ElementTrail_DATA = new RD_ElementTrail_DATA();
		if (rD_ElementTrail_DATA != null)
		{
			rD_ElementTrail_DATA.trailEnabled = trail.gameObject.activeSelf;
			rD_ElementTrail_DATA.trailTime = trail.time;
		}
		return rD_ElementTrail_DATA;
	}

	public static void SetTrailData(this TrailRenderer trail, RD_ElementTrail_DATA trailData, ProcessState state)
	{
		if (!(trail != null) || trailData == null)
		{
			return;
		}
		trail.gameObject.SetActive(trailData.trailEnabled);
		if (trailData.trailEnabled)
		{
			trail.time = trailData.trailTime;
			ProcessState processState = state & ProcessState.Play;
			ProcessState processState2 = state & ProcessState.Pause;
			ProcessState processState3 = state & ProcessState.UnPause;
			if ((state & ProcessState.Stop) != 0)
			{
				trail.Clear();
			}
		}
		else
		{
			trail.Clear();
		}
	}

	public static RD_Audio_DATA GetAudioData(this AudioSource audioSource)
	{
		RD_Audio_DATA rD_Audio_DATA = new RD_Audio_DATA();
		if (audioSource != null)
		{
			rD_Audio_DATA.m_enable = audioSource.enabled;
			rD_Audio_DATA.m_isPlaying = audioSource.isPlaying;
			rD_Audio_DATA.m_time = audioSource.time;
			rD_Audio_DATA.m_volume = audioSource.volume;
		}
		return rD_Audio_DATA;
	}

	public static void SetAudioData(this AudioSource audioSource, RD_Audio_DATA audioData, ProcessState state)
	{
		if (!(audioSource != null) || audioData == null)
		{
			return;
		}
		audioSource.enabled = audioData.m_enable;
		if (audioData.m_enable)
		{
			audioSource.time = audioData.m_time;
			audioSource.volume = audioData.m_volume;
			if ((state & ProcessState.Play) != 0)
			{
				audioSource.Play();
			}
			if ((state & ProcessState.Pause) != 0)
			{
				audioSource.Pause();
			}
			if ((state & ProcessState.UnPause) != 0)
			{
				audioSource.UnPause();
			}
			if ((state & ProcessState.Stop) != 0)
			{
				audioSource.Stop();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	public static RD_BezierMove_Data GetBezierData(this BezierMover bezierMover)
	{
		RD_BezierMove_Data rD_BezierMove_Data = new RD_BezierMove_Data();
		if (bezierMover != null)
		{
			rD_BezierMove_Data.currentIndex = bezierMover.GetIndex();
			rD_BezierMove_Data.cachedDistance = bezierMover.GetDistance();
		}
		return rD_BezierMove_Data;
	}

	public static void SetBezierData(this BezierMover bezier, RD_BezierMove_Data bezierData)
	{
		if (bezier != null && bezierData != null)
		{
			bezier.SetIndex(bezierData.currentIndex);
			bezier.SetDistance(bezierData.cachedDistance);
		}
	}

	public static RD_RailwayMover_DATA GetRailwayMoverData(this RailwayMover railwayMover)
	{
		RD_RailwayMover_DATA rD_RailwayMover_DATA = new RD_RailwayMover_DATA();
		if (railwayMover != null)
		{
			rD_RailwayMover_DATA.CurrentDir = railwayMover.CurrentDir;
			rD_RailwayMover_DATA.currentMoveInfo = railwayMover.currentMoveInfo;
		}
		return rD_RailwayMover_DATA;
	}

	public static void SetRailwayMoverData(this RailwayMover railwayMover, RD_RailwayMover_DATA railwayMoverData)
	{
		if (railwayMover != null && railwayMoverData != null)
		{
			MoveData currentMoveInfo = railwayMoverData.currentMoveInfo;
			currentMoveInfo.InitializeData();
			railwayMover.ResetMoveData(currentMoveInfo, railwayMoverData.CurrentDir);
		}
	}

	public static RD_CameraMover_DATA GetCameraMoverData(this CameraMover cameraMover)
	{
		RD_CameraMover_DATA rD_CameraMover_DATA = new RD_CameraMover_DATA();
		if (cameraMover != null)
		{
			rD_CameraMover_DATA.targetScaler = CameraMover.targetScaler;
			rD_CameraMover_DATA.smoothScaler = CameraMover.smoothScaler;
			rD_CameraMover_DATA.targetValue = cameraMover.GetTargetValue();
			rD_CameraMover_DATA.smoothValue = cameraMover.GetSmoothValue();
			rD_CameraMover_DATA.transData = cameraMover.transform.GetTransData();
		}
		return rD_CameraMover_DATA;
	}

	public static void SetCameraMoverData(this CameraMover cameraMover, RD_CameraMover_DATA cameraMoverData)
	{
		if (cameraMover != null && cameraMoverData != null)
		{
			CameraMover.targetScaler = cameraMoverData.targetScaler;
			CameraMover.smoothScaler = cameraMoverData.smoothScaler;
			cameraMover.SetTargetValue(cameraMoverData.targetValue);
			cameraMover.SetSmoothValue(cameraMoverData.smoothValue);
			cameraMover.transform.SetTransData(cameraMoverData.transData);
		}
	}

	public static RD_JumpInfo_Data GetJumpInfo(this JumpUtil jumpUtil)
	{
		RD_JumpInfo_Data rD_JumpInfo_Data = new RD_JumpInfo_Data();
		rD_JumpInfo_Data.IfJumping = jumpUtil != null;
		if (rD_JumpInfo_Data.IfJumping)
		{
			rD_JumpInfo_Data.BeginPos = jumpUtil.BeginPos;
			rD_JumpInfo_Data.EndPos = jumpUtil.EndPos;
			rD_JumpInfo_Data.JumpNormal = jumpUtil.JumpNormal;
			rD_JumpInfo_Data.MaxHeight = jumpUtil.MaxHeight;
			rD_JumpInfo_Data.JumpBeginY = jumpUtil.JumpBeginY;
		}
		return rD_JumpInfo_Data;
	}

	public static void SetJumpInfo(ref JumpUtil jumpUtil, RD_JumpInfo_Data jumpInfo)
	{
		if (jumpInfo.IfJumping)
		{
			jumpUtil = new JumpUtil(jumpInfo.BeginPos, jumpInfo.EndPos, jumpInfo.JumpNormal, jumpInfo.MaxHeight, jumpInfo.JumpBeginY);
		}
		else
		{
			jumpUtil = null;
		}
	}
}
