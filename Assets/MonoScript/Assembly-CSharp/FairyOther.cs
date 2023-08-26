using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;

public class FairyOther : BaseFairy
{
	protected class SkillData
	{
		public FairySkillsName mSkillName;

		public string mAnim_active;

		public string mAnim;

		public Transform mMuzzle;

		public ParticleSystem mEffect;

		public TrailRenderer[] mTrails;
	}

	private readonly string ANIMSHOW = "Show";

	private readonly string[] ANIMRUNS = new string[3] { "Run01", "Run02", "Run03" };

	private readonly string ANIMSUCCESS = "Success";

	private readonly string ANIMFAILURE = "Failure";

	private readonly string ANIMJUMP = "Jump";

	public Animator mAnimator;

	public Camera mCamera3D;

	private Vector3 mTargetPosition = new Vector3(-0.8f, 0.7f, -0.37f);

	private Vector3 mFollowSpeed = new Vector3(6f, 6f, 20f);

	private float mRotationZSpeed = 50f;

	private float mRotationYSpeed = 30f;

	private GameObject mSkillParentObj;

	private Animator mSkillAnimator;

	private float mDurationMax = 8f;

	private float mDurationSkillNormal = 0.5f;

	private float mDurationSkillMoveToIcon = 1f;

	private float mDurationSkillMoveToRole = 0.5f;

	private float mDurationCloseEffectToIconOpen = 0.5f;

	private float mDurationCloseEffectToRoleOpen = 0.25f;

	private float mDurationForSkillEffectToIcon = 0.35f;

	private float mDurationForSkillEffectToRole = 0.2f;

	private Ease mEase = Ease.Linear;

	private AnimationEventListen mAnimEventListen;

	private bool mActive;

	private FairySkillsName[] mSkillsName;

	private bool mIsSkillPlaying;

	private float mRangeRunTime = 5f;

	private float mCurrentRunTime;

	private bool mIsWaitPlayStarRunAnim;

	private Dictionary<int, SkillData> mSkillDatas = new Dictionary<int, SkillData>();

	protected override void OnInst()
	{
		mIsSkillPlaying = false;
		Transform transform = base.gameObject.transform.Find("model");
		if (transform != null)
		{
			mAnimator = transform.gameObject.GetComponent<Animator>();
		}
		Transform transform2 = base.gameObject.transform.Find("Skill");
		if (transform2 != null)
		{
			mSkillParentObj = transform2.gameObject;
		}
		Transform transform3 = base.gameObject.transform.Find("Skill/3D/model");
		if (transform != null)
		{
			mSkillAnimator = transform3.gameObject.GetComponent<Animator>();
			mAnimEventListen = transform3.gameObject.GetComponent<AnimationEventListen>();
			AnimationEventListen animationEventListen = mAnimEventListen;
			animationEventListen.m_onStringDelegate = (AnimationEventListen.StringDelegate)Delegate.Combine(animationEventListen.m_onStringDelegate, new AnimationEventListen.StringDelegate(AnimEventListen));
		}
		Transform transform4 = base.gameObject.transform.Find("Skill/3D/Main Camera");
		if (transform4 != null)
		{
			mCamera3D = transform4.gameObject.GetComponent<Camera>();
		}
		LoadSkillData();
	}

	protected override void OnSwitchState()
	{
	}

	protected override void OnUpdateState()
	{
	}

	public override void PlaySkill(bool active, params FairySkillsName[] skillsName)
	{
		mActive = active;
		mSkillsName = skillsName;
		mIsSkillPlaying = true;
		foreach (KeyValuePair<int, SkillData> mSkillData in mSkillDatas)
		{
			mSkillData.Value.mEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			for (int i = 0; i < mSkillData.Value.mTrails.Length; i++)
			{
				mSkillData.Value.mTrails[i].Clear();
				mSkillData.Value.mTrails[i].enabled = false;
			}
			mSkillData.Value.mEffect.gameObject.SetActive(false);
		}
		StopAllCoroutines();
		StartCoroutine(DelayRoleRebrith(0.1f));
		StartCoroutine(WaitSkillFinished(mDurationMax));
	}

	public override bool IsSkillPlaying()
	{
		return mIsSkillPlaying;
	}

	public override void DestroyLocal()
	{
		AnimationEventListen animationEventListen = mAnimEventListen;
		animationEventListen.m_onStringDelegate = (AnimationEventListen.StringDelegate)Delegate.Remove(animationEventListen.m_onStringDelegate, new AnimationEventListen.StringDelegate(AnimEventListen));
		base.DestroyLocal();
	}

	private void LoadSkillData()
	{
		mSkillDatas = new Dictionary<int, SkillData>
		{
			{
				1,
				new SkillData
				{
					mAnim = "buyoutrebirth",
					mAnim_active = "buyoutrebirth_active",
					mSkillName = FairySkillsName.BUYOUTREBIRTH,
					mEffect = base.gameObject.transform.Find("Skill/3D/Point_buyout").GetComponent<ParticleSystem>(),
					mMuzzle = base.gameObject.transform.Find("Skill/3D/model/Bip001/Baton/mofabang/Fire"),
					mTrails = base.gameObject.transform.Find("Skill/3D/Point_buyout").GetComponentsInChildren<TrailRenderer>()
				}
			},
			{
				4,
				new SkillData
				{
					mAnim = "pathguide",
					mAnim_active = "pathguide_active",
					mSkillName = FairySkillsName.PATHGUIDE,
					mEffect = base.gameObject.transform.Find("Skill/3D/Point_pathguide").GetComponent<ParticleSystem>(),
					mMuzzle = base.gameObject.transform.Find("Skill/3D/model/Bip001/Baton/mofabang/Fire"),
					mTrails = base.gameObject.transform.Find("Skill/3D/Point_pathguide").GetComponentsInChildren<TrailRenderer>()
				}
			},
			{
				2,
				new SkillData
				{
					mAnim = "shield",
					mAnim_active = "shield_active",
					mSkillName = FairySkillsName.SHIELD,
					mEffect = base.gameObject.transform.Find("Skill/3D/Point_shield").GetComponent<ParticleSystem>(),
					mMuzzle = base.gameObject.transform.Find("Skill/3D/model/Bip001/Baton/mofabang/Fire"),
					mTrails = base.gameObject.transform.Find("Skill/3D/Point_shield").GetComponentsInChildren<TrailRenderer>()
				}
			},
			{
				3,
				new SkillData
				{
					mAnim = "rebirth",
					mAnim_active = "rebirth_active",
					mSkillName = FairySkillsName.REBIRTH,
					mEffect = base.gameObject.transform.Find("Skill/3D/Point_rebirth").GetComponent<ParticleSystem>(),
					mMuzzle = base.gameObject.transform.Find("Skill/3D/model/Bip001/Baton/mofabang/Fire"),
					mTrails = base.gameObject.transform.Find("Skill/3D/Point_rebirth").GetComponentsInChildren<TrailRenderer>()
				}
			}
		};
	}

	private void OnFireEvent()
	{
		LevelStartForm levelStartForm = Mod.UI.GetUIForm(UIFormId.LevelStartForm) as LevelStartForm;
		Dictionary<int, GuidLineRoot> dictionary = new Dictionary<int, GuidLineRoot>();
		float time = mDurationSkillNormal;
		float time2 = mDurationSkillNormal;
		if (levelStartForm != null)
		{
			GuidLineRoot[] componentsInChildren = levelStartForm.gameObject.GetComponentsInChildren<GuidLineRoot>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				dictionary[(int)componentsInChildren[i].type] = componentsInChildren[i];
			}
		}
		for (int j = 0; j < mSkillsName.Length; j++)
		{
			FairySkillsName fairySkillsName = mSkillsName[j];
			Vector3 position = base.transform.position;
			Vector3 position2 = base.transform.position;
			SkillData skillData = mSkillDatas[(int)fairySkillsName];
			position = skillData.mMuzzle.transform.position;
			skillData.mEffect.transform.position = position;
			if (levelStartForm != null)
			{
				Vector3 position3 = Vector3.zero;
				switch (fairySkillsName)
				{
				case FairySkillsName.SHIELD:
					position3 = dictionary[2].gameObject.transform.position;
					break;
				case FairySkillsName.REBIRTH:
					if (dictionary.ContainsKey(3))
					{
						position3 = dictionary[3].gameObject.transform.position;
					}
					break;
				case FairySkillsName.PATHGUIDE:
					position3 = dictionary[1].gameObject.transform.position;
					break;
				}
				Vector3 vector = Mod.UI.UICamera.WorldToScreenPoint(position3);
				float z = mCamera3D.WorldToScreenPoint(position).z;
				position2 = mCamera3D.ScreenToWorldPoint(new Vector3(vector.x, vector.y, z));
				skillData.mEffect.transform.DOMove(position2, mDurationForSkillEffectToIcon).SetEase(mEase);
				for (int k = 0; k < skillData.mTrails.Length; k++)
				{
					skillData.mTrails[k].enabled = true;
					skillData.mTrails[k].Clear();
				}
				time = mDurationSkillMoveToIcon;
				time2 = mDurationCloseEffectToIconOpen;
			}
			else if (BaseRole.theBall != null)
			{
				Vector3 vector2 = Camera.main.WorldToScreenPoint(BaseRole.theBall.BodyPart.RoleHead.transform.position);
				float z2 = mCamera3D.WorldToScreenPoint(position).z;
				position2 = mCamera3D.ScreenToWorldPoint(new Vector3(vector2.x, vector2.y, z2));
				skillData.mEffect.transform.DOMove(position2, mDurationForSkillEffectToRole).SetEase(mEase);
				for (int l = 0; l < skillData.mTrails.Length; l++)
				{
					skillData.mTrails[l].enabled = true;
					skillData.mTrails[l].Clear();
				}
				time = mDurationSkillMoveToRole;
				time2 = mDurationCloseEffectToRoleOpen;
			}
		}
		StopAllCoroutines();
		StartCoroutine(WaitCloseEffect(time2));
		StartCoroutine(WaitSkillFinished(time));
		Mod.Event.FireNow(this, Mod.Reference.Acquire<SelectSkillFireEventArgs>().Initialize(mActive, mSkillsName));
	}

	private IEnumerator DelayRoleRebrith(float delaytime)
	{
		yield return new WaitForSeconds(delaytime);
		AnimEventListen(base.gameObject, "fire");
	}

	private IEnumerator WaitSkillFinished(float time)
	{
		yield return new WaitForSeconds(time);
		OnFinshed();
	}

	private IEnumerator WaitCloseEffect(float time)
	{
		yield return new WaitForSeconds(time);
		OnCloseEffect();
		Mod.Event.FireNow(this, Mod.Reference.Acquire<SelectSkillLightupEventArgs>().Initialize(mActive, mSkillsName));
	}

	private void OnCloseEffect()
	{
		for (int i = 0; i < mSkillsName.Length; i++)
		{
			FairySkillsName key = mSkillsName[i];
			mSkillDatas[(int)key].mEffect.gameObject.SetActive(false);
		}
	}

	private void OnFinshed()
	{
		if ((bool)mSkillAnimator)
		{
			mSkillAnimator.StopPlayback();
		}
		if ((bool)mSkillParentObj)
		{
			mSkillParentObj.SetActive(false);
		}
		mIsSkillPlaying = false;
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameStartButtonActiveEventArgs>().Initialize(true));
		Mod.Event.FireNow(this, Mod.Reference.Acquire<SelectSkillFinishedEventArgs>().Initialize(mActive, mSkillsName));
	}

	private IEnumerator WaitStartRunAnim(float time)
	{
		yield return new WaitForSeconds(time);
		OnStartRunAnim();
		mIsWaitPlayStarRunAnim = false;
	}

	private void OnStartRunAnim()
	{
		mCurrentRunTime = 0f;
		if ((bool)mAnimator)
		{
			mAnimator.gameObject.SetActive(true);
			mAnimator.Play(ANIMSHOW, 0, 0f);
		}
	}

	private string RangeRunAnim()
	{
		int num = new System.Random().Next(0, ANIMRUNS.Length);
		return ANIMRUNS[num];
	}

	private void AnimEventListen(GameObject obj, string info)
	{
		if (info == "fire")
		{
			OnFireEvent();
		}
	}
}
