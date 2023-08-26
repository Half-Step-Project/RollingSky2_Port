using System;
using Foundation;
using RS2;
using UnityEngine;

public class PathGuideTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		[Header("    动画默认名称          - Default")]
		[Header("    动画触发名称          - TriggerEnter")]
		[Header("    动画保持名称          - Stay")]
		[Header("    动画触发退出名称       - TriggerExit")]
		[Header("    move的物体          - root/move")]
		[Header("    model          - root/move/model")]
		[Header("    碰到的特效          - root/move/enter")]
		[Header("    离开的特效          - root/move/exit")]
		public float stayDistance;

		[Range(0f, 10f)]
		public float triggerExitDistance;

		public float resetDistance;

		public float speed;

		public bool lookAt;

		public Vector3[] positons;

		public Vector3[] bezierPositons;

		public Vector3[] runPositons;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.stayDistance = -10f;
				result.triggerExitDistance = 10f;
				result.resetDistance = 80f;
				result.speed = 1f;
				result.lookAt = true;
				result.positons = null;
				result.bezierPositons = null;
				result.runPositons = null;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public CommonState state;

		public bool isTirggerExit;

		public RD_ElementTransform_DATA trans;

		public RD_ElementTransform_DATA moveTrans;

		public RD_ElementTransform_DATA modelTrans;

		public RD_ElementParticle_DATA enterPs;

		public RD_ElementParticle_DATA exitPs;

		public RD_ElementAnimator_DATA animatorData;

		public RD_BezierMove_Data bezierData;
	}

	private const string AnimationNameDefault = "Default";

	private const string AnimationNameTriggerEnter = "TriggerEnter";

	private const string AnimationNameStay = "Stay";

	private const string AnimationNameTriggerExit = "TriggerExit";

	private const string MoveName = "root/move";

	private const string ModelName = "root/move/model";

	private const string TriggerEnterPsName = "root/move/enter";

	private const string TriggerExitPsName = "root/move/exit";

	public GameObject mMoveObject;

	public GameObject mModel;

	public ParticleSystem mTriggerEnterPs;

	public ParticleSystem mTriggerExitPs;

	public Animator mAnimator;

	[Label]
	public bool mIsFinished;

	[Label]
	public float mDistance;

	[Label]
	public bool mIsTirggerExit;

	public Data mData;

	private InsideGameDataModule _insideGameDataModule;

	private BezierMover _bezierMover;

	private float _deltaLocZ;

	private Vector3 _beginPos = Vector3.zero;

	private Vector3 _targetPos = Vector3.zero;

	private Vector3 _moveLocDir = Vector3.forward;

	private Vector3 _startMoveObjectEular = Vector3.zero;

	private Vector3 _startMoveObjectPos = Vector3.zero;

	private Vector3 _startPosition;

	private Vector3 _endPosition;

	private RebirthData _rebirthData;

	private string levelname;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		levelname = dataModule.CurLevelId.ToString();
		if (dataModule.CurLevelId.ToString() != "10000")
		{
			base.gameObject.SetActive(false);
		}
		_insideGameDataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		LoadData();
		if (mData.runPositons != null && mData.runPositons.Length != 0)
		{
			_startPosition = mData.runPositons[0];
			_endPosition = mData.runPositons[mData.runPositons.Length - 1];
		}
		if (mMoveObject != null)
		{
			mMoveObject.transform.position = _startPosition;
			_startMoveObjectPos = mMoveObject.transform.localPosition;
			_startMoveObjectEular = mMoveObject.transform.localEulerAngles;
		}
		if (mModel != null)
		{
			mModel.SetActive(false);
		}
		if (mTriggerEnterPs != null)
		{
			mTriggerEnterPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		if (mTriggerExitPs != null)
		{
			mTriggerExitPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		if (mAnimator != null)
		{
			mAnimator.Play("Default", 0, 0f);
		}
		mIsFinished = false;
		_bezierMover = new BezierMover(mData.runPositons);
		mIsTirggerExit = false;
		commonState = CommonState.None;
		Mod.Event.Subscribe(EventArgs<PathGuideHideEventArgs>.EventId, OnPlayExitHandler);
	}

	public override void UpdateElement()
	{
		if (!_insideGameDataModule.GuideLine)
		{
			return;
		}
		mDistance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (mDistance >= mData.stayDistance)
			{
				if (mModel != null)
				{
					mModel.SetActive(true);
				}
				if (mTriggerEnterPs != null)
				{
					mTriggerEnterPs.Play(true);
				}
				if (mAnimator != null)
				{
					mAnimator.Play("TriggerEnter", 0, 0f);
				}
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active)
		{
			OnTriggerPlay();
			if (mDistance >= mData.resetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}
	}

	public override void OnTriggerPlay()
	{
		if (_bezierMover == null || !(mMoveObject != null) || mIsFinished)
		{
			return;
		}
		_deltaLocZ = Railway.theRailway.SpeedForward * Time.deltaTime * mData.speed;
		if (_deltaLocZ == 0f)
		{
			return;
		}
		_beginPos = mMoveObject.transform.position;
		mIsFinished = _bezierMover.MoveForwardByZ(_deltaLocZ, null, _beginPos, ref _targetPos, ref _moveLocDir);
		mMoveObject.transform.position = _targetPos;
		if (mData.lookAt)
		{
			mMoveObject.transform.forward = Vector3.Lerp(mMoveObject.transform.forward, _moveLocDir, 0.4f);
		}
		if (mIsFinished)
		{
			commonState = CommonState.End;
		}
		if (_endPosition.z - mMoveObject.transform.position.z <= mData.triggerExitDistance && !mIsTirggerExit)
		{
			if (mTriggerExitPs != null)
			{
				mTriggerExitPs.Play(true);
			}
			if (mAnimator != null)
			{
				mAnimator.Play("TriggerExit", 0, 0f);
			}
			mIsTirggerExit = true;
		}
	}

	private void OnPlayExitHandler(object sender, Foundation.EventArgs e)
	{
		if (e is PathGuideHideEventArgs)
		{
			if (mTriggerExitPs != null)
			{
				mTriggerExitPs.Play(true);
			}
			if (mAnimator != null)
			{
				mAnimator.Play("TriggerExit", 0, 0f);
			}
			mIsTirggerExit = true;
			commonState = CommonState.End;
		}
	}

	public override void OnTriggerStop()
	{
		if (mModel != null)
		{
			mModel.SetActive(false);
		}
		if (mTriggerEnterPs != null)
		{
			mTriggerEnterPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		if (mTriggerExitPs != null)
		{
			mTriggerExitPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		if (mAnimator != null)
		{
			mAnimator.StopRecording();
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		Mod.Event.Unsubscribe(EventArgs<PathGuideHideEventArgs>.EventId, OnPlayExitHandler);
		_bezierMover = null;
		commonState = CommonState.None;
		OnTriggerStop();
		if ((bool)mMoveObject)
		{
			mMoveObject.transform.localPosition = _startMoveObjectPos;
			mMoveObject.transform.localEulerAngles = _startMoveObjectEular;
		}
		mIsFinished = false;
		mIsTirggerExit = false;
	}

	public override string Write()
	{
		Vector3[] array = null;
		if (mData.positons != null && mData.positons.Length != 0)
		{
			array = Bezier.GetPathByPositions(mData.positons);
		}
		else if (mData.bezierPositons != null && mData.bezierPositons.Length != 0)
		{
			array = mData.bezierPositons;
		}
		if (array != null)
		{
			mData.runPositons = new Vector3[array.Length];
			for (int i = 0; i < mData.runPositons.Length; i++)
			{
				Vector3 vector = base.transform.TransformPoint(array[i]);
				mData.runPositons[i] = vector;
			}
		}
		return JsonUtility.ToJson(mData);
	}

	public override byte[] WriteBytes()
	{
		Vector3[] array = null;
		if (mData.positons != null && mData.positons.Length != 0)
		{
			array = Bezier.GetPathByPositions(mData.positons);
		}
		else if (mData.bezierPositons != null && mData.bezierPositons.Length != 0)
		{
			array = mData.bezierPositons;
		}
		if (array != null)
		{
			mData.runPositons = new Vector3[array.Length];
			for (int i = 0; i < mData.runPositons.Length; i++)
			{
				Vector3 vector = base.transform.TransformPoint(array[i]);
				mData.runPositons[i] = vector;
			}
		}
		return Bson.ToBson(mData);
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			mData = JsonUtility.FromJson<Data>(info);
		}
	}

	public override void ReadBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			mData = Bson.ToObject<Data>(bytes);
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null && objs.Length != 0)
		{
			mData = (Data)objs[0];
		}
	}

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			mMoveObject = null;
			mAnimator = null;
			mModel = null;
			mTriggerEnterPs = null;
			mTriggerExitPs = null;
			LoadData();
		}
	}

	private void LoadData()
	{
		if (mMoveObject == null)
		{
			Transform transform = base.gameObject.transform.Find("root/move");
			if (transform != null)
			{
				mMoveObject = transform.gameObject;
			}
		}
		if (mMoveObject != null)
		{
			if (mModel == null)
			{
				Transform transform2 = base.gameObject.transform.Find("root/move/model");
				if (transform2 != null)
				{
					mModel = transform2.gameObject;
				}
			}
			if (mTriggerEnterPs == null)
			{
				Transform transform3 = base.gameObject.transform.Find("root/move/enter");
				if (transform3 != null)
				{
					mTriggerEnterPs = transform3.gameObject.GetComponent<ParticleSystem>();
				}
			}
			if (mTriggerExitPs == null)
			{
				Transform transform4 = base.gameObject.transform.Find("root/move/exit");
				if (transform4 != null)
				{
					mTriggerExitPs = transform4.gameObject.GetComponent<ParticleSystem>();
				}
			}
		}
		if (mAnimator == null)
		{
			mAnimator = base.gameObject.GetComponentInChildren<Animator>();
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RebirthData rebirthData = new RebirthData
		{
			state = commonState,
			trans = base.transform.GetTransData()
		};
		if (mMoveObject != null)
		{
			rebirthData.moveTrans = mMoveObject.transform.GetTransData();
		}
		if (mModel != null)
		{
			rebirthData.modelTrans = mModel.transform.GetTransData();
		}
		rebirthData.animatorData = mAnimator.GetAnimData();
		rebirthData.enterPs = mTriggerEnterPs.GetParticleData();
		rebirthData.exitPs = mTriggerExitPs.GetParticleData();
		rebirthData.isTirggerExit = mIsTirggerExit;
		rebirthData.bezierData = _bezierMover.GetBezierData();
		return JsonUtility.ToJson(rebirthData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rdData)
	{
		string text = (string)rdData;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		_rebirthData = JsonUtility.FromJson<RebirthData>(text);
		if (_rebirthData != null)
		{
			commonState = _rebirthData.state;
			base.gameObject.transform.SetTransData(_rebirthData.trans);
			if (mMoveObject != null)
			{
				mMoveObject.transform.SetTransData(_rebirthData.moveTrans);
			}
			if (mModel != null)
			{
				mModel.SetActive(false);
			}
			mIsTirggerExit = _rebirthData.isTirggerExit;
			_bezierMover.SetBezierData(_rebirthData.bezierData);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rdData)
	{
		if (!_insideGameDataModule.GuideLine)
		{
			return;
		}
		if (_rebirthData != null)
		{
			commonState = _rebirthData.state;
			base.gameObject.transform.SetTransData(_rebirthData.trans);
			if (mMoveObject != null)
			{
				mMoveObject.transform.SetTransData(_rebirthData.moveTrans);
			}
			if (mModel != null)
			{
				mModel.transform.SetTransData(_rebirthData.modelTrans);
			}
			mIsTirggerExit = _rebirthData.isTirggerExit;
			_bezierMover.SetBezierData(_rebirthData.bezierData);
			mTriggerEnterPs.SetParticleData(_rebirthData.enterPs, ProcessState.Pause);
			mTriggerExitPs.SetParticleData(_rebirthData.exitPs, ProcessState.Pause);
			mAnimator.SetAnimData(_rebirthData.animatorData, ProcessState.Pause);
			mAnimator.SetAnimData(_rebirthData.animatorData, ProcessState.UnPause);
			mTriggerEnterPs.SetParticleData(_rebirthData.enterPs, ProcessState.UnPause);
			mTriggerExitPs.SetParticleData(_rebirthData.exitPs, ProcessState.UnPause);
		}
		_rebirthData = null;
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = new RebirthData
		{
			state = commonState,
			trans = base.transform.GetTransData()
		};
		if (mMoveObject != null)
		{
			rebirthData.moveTrans = mMoveObject.transform.GetTransData();
		}
		if (mModel != null)
		{
			rebirthData.modelTrans = mModel.transform.GetTransData();
		}
		rebirthData.animatorData = mAnimator.GetAnimData();
		rebirthData.enterPs = mTriggerEnterPs.GetParticleData();
		rebirthData.exitPs = mTriggerExitPs.GetParticleData();
		rebirthData.isTirggerExit = mIsTirggerExit;
		rebirthData.bezierData = _bezierMover.GetBezierData();
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthReadByteData(byte[] rdData)
	{
		if (levelname != "10000")
		{
			base.gameObject.SetActive(false);
			return;
		}
		_rebirthData = Bson.ToObject<RebirthData>(rdData);
		if (_rebirthData != null)
		{
			commonState = _rebirthData.state;
			base.gameObject.transform.SetTransData(_rebirthData.trans);
			if (mMoveObject != null)
			{
				mMoveObject.transform.SetTransData(_rebirthData.moveTrans);
			}
			if (mModel != null)
			{
				mModel.SetActive(false);
			}
			mIsTirggerExit = _rebirthData.isTirggerExit;
			_bezierMover.SetBezierData(_rebirthData.bezierData);
		}
	}

	public override void RebirthStartByteGame(byte[] rdData)
	{
		if (levelname != "10000")
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			if (!_insideGameDataModule.GuideLine)
			{
				return;
			}
			if (_rebirthData != null)
			{
				commonState = _rebirthData.state;
				base.gameObject.transform.SetTransData(_rebirthData.trans);
				if (mMoveObject != null)
				{
					mMoveObject.transform.SetTransData(_rebirthData.moveTrans);
				}
				if (mModel != null)
				{
					mModel.transform.SetTransData(_rebirthData.modelTrans);
				}
				mIsTirggerExit = _rebirthData.isTirggerExit;
				_bezierMover.SetBezierData(_rebirthData.bezierData);
				mAnimator.SetAnimData(_rebirthData.animatorData, ProcessState.Pause);
				mTriggerEnterPs.SetParticleData(_rebirthData.enterPs, ProcessState.Pause);
				mTriggerExitPs.SetParticleData(_rebirthData.exitPs, ProcessState.Pause);
				mAnimator.SetAnimData(_rebirthData.animatorData, ProcessState.UnPause);
				mTriggerEnterPs.SetParticleData(_rebirthData.enterPs, ProcessState.UnPause);
				mTriggerExitPs.SetParticleData(_rebirthData.exitPs, ProcessState.UnPause);
			}
			_rebirthData = null;
		}
	}

	public override void OnDrawGizmos()
	{
		Vector3[] array = null;
		if (mData.positons != null && mData.positons.Length > 1)
		{
			for (int i = 0; i < mData.positons.Length - 1; i++)
			{
				Vector3 from = base.transform.TransformPoint(mData.positons[i]);
				Vector3 to = base.transform.TransformPoint(mData.positons[i + 1]);
				Gizmos.DrawLine(from, to);
			}
			array = Bezier.GetPathByPositions(mData.positons);
		}
		else if (mData.bezierPositons != null && mData.bezierPositons.Length != 0)
		{
			array = mData.bezierPositons;
		}
		if (array != null)
		{
			for (int j = 0; j < array.Length - 1; j++)
			{
				Vector3 from2 = base.transform.TransformPoint(array[j]);
				Vector3 to2 = base.transform.TransformPoint(array[j + 1]);
				Gizmos.DrawLine(from2, to2);
			}
		}
	}
}
