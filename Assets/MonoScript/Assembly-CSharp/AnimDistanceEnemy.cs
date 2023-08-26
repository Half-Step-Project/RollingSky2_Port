using System;
using Foundation;
using UnityEngine;

public class AnimDistanceEnemy : BaseEnemy
{
	[Serializable]
	public struct Data
	{
		[Header("prefab 数据")]
		public string[] animObjNames;

		public string[] effectObjNames;

		[Header("距离检测数据")]
		public DistanceData[] distanceDatas;

		[Header("碰到时触发")]
		public PlayData triggerData;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.animObjNames = new string[1] { "model" };
				result.effectObjNames = new string[1] { "effect" };
				result.distanceDatas = new DistanceData[1] { DistanceData.DefaultValue };
				result.triggerData = PlayData.DefaultValue;
				return result;
			}
		}
	}

	[Serializable]
	public struct DistanceData
	{
		public float distance;

		public PlayData playData;

		public static DistanceData DefaultValue
		{
			get
			{
				DistanceData result = default(DistanceData);
				result.distance = -10f;
				result.playData = PlayData.DefaultValue;
				return result;
			}
		}
	}

	[Serializable]
	public struct PlayData
	{
		public int animObjIndex;

		public string animName;

		public int effectObjIndex;

		public static PlayData DefaultValue
		{
			get
			{
				PlayData result = default(PlayData);
				result.animObjIndex = 0;
				result.animName = "anim01";
				result.effectObjIndex = 0;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA trans;

		public RD_ElementAnim_DATA[] anims;

		public RD_ElementParticle_DATA[] effects;

		public bool[] sensores;
	}

	public Animation[] mAnimations;

	public ParticleSystem[] mEffects;

	public Data mData = Data.DefaultValue;

	[Label]
	public float m_currentDistance;

	private DistancSensore[] _distancSensores;

	private RebirthData _rebirthData;

	public override void Initialize()
	{
		base.Initialize();
		FindObjects();
		StopAllPlayData();
		_distancSensores = new DistancSensore[mData.distanceDatas.Length];
		for (int i = 0; i < _distancSensores.Length; i++)
		{
			Animation anim = null;
			ParticleSystem effect = null;
			bool num = IsCanPlayAnim(mData.distanceDatas[i].playData, out anim);
			bool flag = IsCanPlayEffect(mData.distanceDatas[i].playData, out effect);
			if (num || flag)
			{
				_distancSensores[i] = new DistancSensore(mData.distanceDatas[i].distance);
			}
		}
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		if (Application.isPlaying)
		{
			m_currentDistance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		}
		for (int i = 0; i < _distancSensores.Length; i++)
		{
			if (_distancSensores[i] != null && _distancSensores[i].Run(m_currentDistance))
			{
				PlayThePlayData(mData.distanceDatas[i].playData);
			}
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		StopAllPlayData();
	}

	public override void TriggerEnter(BaseRole ball)
	{
		PlayThePlayData(mData.triggerData);
		base.TriggerEnter(ball);
	}

	private void FindObjects()
	{
		if (mData.animObjNames != null)
		{
			mAnimations = new Animation[mData.animObjNames.Length];
			for (int i = 0; i < mData.animObjNames.Length; i++)
			{
				if (!string.IsNullOrEmpty(mData.animObjNames[i]))
				{
					Transform transform = base.gameObject.transform.Find(mData.animObjNames[i]);
					if (transform != null)
					{
						Animation component = transform.gameObject.GetComponent<Animation>();
						mAnimations[i] = component;
					}
				}
			}
		}
		if (mData.effectObjNames == null)
		{
			return;
		}
		mEffects = new ParticleSystem[mData.effectObjNames.Length];
		for (int j = 0; j < mData.effectObjNames.Length; j++)
		{
			if (!string.IsNullOrEmpty(mData.effectObjNames[j]))
			{
				Transform transform2 = base.gameObject.transform.Find(mData.effectObjNames[j]);
				if (transform2 != null)
				{
					ParticleSystem component2 = transform2.gameObject.GetComponent<ParticleSystem>();
					mEffects[j] = component2;
				}
			}
		}
	}

	private void PlayThePlayData(PlayData playData)
	{
		Animation anim = null;
		ParticleSystem effect = null;
		bool num = IsCanPlayAnim(playData, out anim);
		bool flag = IsCanPlayEffect(playData, out effect);
		if (num && anim != null)
		{
			anim.Play(playData.animName);
		}
		if (flag && effect != null)
		{
			effect.Play(true);
		}
	}

	private bool IsCanPlayAnim(PlayData playData, out Animation anim)
	{
		int animObjIndex = playData.animObjIndex;
		if (animObjIndex >= 0 && animObjIndex < mAnimations.Length && mAnimations[animObjIndex] != null && !string.IsNullOrEmpty(playData.animName))
		{
			anim = mAnimations[animObjIndex];
			return true;
		}
		anim = null;
		return false;
	}

	private bool IsCanPlayEffect(PlayData playData, out ParticleSystem effect)
	{
		int effectObjIndex = playData.effectObjIndex;
		if (effectObjIndex >= 0 && effectObjIndex < mEffects.Length && mEffects[effectObjIndex] != null)
		{
			effect = mEffects[effectObjIndex];
			return true;
		}
		effect = null;
		return false;
	}

	private void StopAllPlayData()
	{
		for (int i = 0; i < mAnimations.Length; i++)
		{
			if (mAnimations[i] != null)
			{
				mAnimations[i].Stop();
			}
		}
		for (int j = 0; j < mEffects.Length; j++)
		{
			if (mEffects[j] != null)
			{
				mEffects[j].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			FindObjects();
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(mData);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(mData);
	}

	public override void Read(string info)
	{
		mData = JsonUtility.FromJson<Data>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		mData = Bson.ToObject<Data>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		mData = (Data)objs[0];
	}

	public override object RebirthWriteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.trans = base.gameObject.transform.GetTransData();
		rebirthData.anims = new RD_ElementAnim_DATA[mAnimations.Length];
		for (int i = 0; i < mAnimations.Length; i++)
		{
			rebirthData.anims[i] = mAnimations[i].GetAnimData();
		}
		rebirthData.effects = new RD_ElementParticle_DATA[mEffects.Length];
		for (int j = 0; j < mEffects.Length; j++)
		{
			rebirthData.effects[j] = mEffects[j].GetParticleData();
		}
		rebirthData.sensores = new bool[_distancSensores.Length];
		for (int k = 0; k < _distancSensores.Length; k++)
		{
			rebirthData.sensores[k] = _distancSensores[k] != null && _distancSensores[k].m_isTriggerStay;
		}
		return JsonUtility.ToJson(rebirthData);
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.trans = base.gameObject.transform.GetTransData();
		rebirthData.anims = new RD_ElementAnim_DATA[mAnimations.Length];
		for (int i = 0; i < mAnimations.Length; i++)
		{
			rebirthData.anims[i] = mAnimations[i].GetAnimData();
		}
		rebirthData.effects = new RD_ElementParticle_DATA[mEffects.Length];
		for (int j = 0; j < mEffects.Length; j++)
		{
			rebirthData.effects[j] = mEffects[j].GetParticleData();
		}
		rebirthData.sensores = new bool[_distancSensores.Length];
		for (int k = 0; k < _distancSensores.Length; k++)
		{
			rebirthData.sensores[k] = _distancSensores[k] != null && _distancSensores[k].m_isTriggerStay;
		}
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data == null)
		{
			return;
		}
		string text = (string)rd_data;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		_rebirthData = JsonUtility.FromJson<RebirthData>(text);
		base.gameObject.transform.SetTransData(_rebirthData.trans);
		for (int i = 0; i < _rebirthData.anims.Length; i++)
		{
			mAnimations[i].SetAnimData(_rebirthData.anims[i], ProcessState.Pause);
		}
		for (int j = 0; j < _rebirthData.effects.Length; j++)
		{
			mEffects[j].SetParticleData(_rebirthData.effects[j], ProcessState.Pause);
		}
		for (int k = 0; k < _rebirthData.sensores.Length; k++)
		{
			if (_distancSensores[k] != null)
			{
				_distancSensores[k].m_isTriggerStay = _rebirthData.sensores[k];
			}
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data == null)
		{
			return;
		}
		_rebirthData = Bson.ToObject<RebirthData>(rd_data);
		base.gameObject.transform.SetTransData(_rebirthData.trans);
		for (int i = 0; i < _rebirthData.anims.Length; i++)
		{
			mAnimations[i].SetAnimData(_rebirthData.anims[i], ProcessState.Pause);
		}
		for (int j = 0; j < _rebirthData.effects.Length; j++)
		{
			mEffects[j].SetParticleData(_rebirthData.effects[j], ProcessState.Pause);
		}
		for (int k = 0; k < _rebirthData.sensores.Length; k++)
		{
			if (_distancSensores[k] != null)
			{
				_distancSensores[k].m_isTriggerStay = _rebirthData.sensores[k];
			}
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		if (_rebirthData != null)
		{
			for (int i = 0; i < _rebirthData.anims.Length; i++)
			{
				mAnimations[i].SetAnimData(_rebirthData.anims[i], ProcessState.UnPause);
			}
			for (int j = 0; j < _rebirthData.effects.Length; j++)
			{
				mEffects[j].SetParticleData(_rebirthData.effects[j], ProcessState.UnPause);
			}
			_rebirthData = null;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (_rebirthData != null)
		{
			for (int i = 0; i < _rebirthData.anims.Length; i++)
			{
				mAnimations[i].SetAnimData(_rebirthData.anims[i], ProcessState.UnPause);
			}
			for (int j = 0; j < _rebirthData.effects.Length; j++)
			{
				mEffects[j].SetParticleData(_rebirthData.effects[j], ProcessState.UnPause);
			}
			_rebirthData = null;
		}
	}
}
