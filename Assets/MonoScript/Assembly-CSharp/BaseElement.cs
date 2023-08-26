using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public abstract class BaseElement : MonoBehaviour, IHearable, IDistancePlayer, ITriggerPlayer, ReadOrWrite, IElementSerializable, IElementRebirth
{
	public enum CommonState
	{
		None,
		Begin,
		Active,
		ActiveReverse,
		InActive,
		End
	}

	protected Vector3 StartPos;

	protected Vector3 StartLocalEuler;

	protected Vector3 StartLocalPos;

	protected bool IfInitialize;

	public int m_id;

	[Label]
	public int m_gridId;

	[HideInInspector]
	public int worldRow;

	[HideInInspector]
	public int ParentKey = -1;

	[HideInInspector]
	public int TileChildCount;

	[HideInInspector]
	public int EnemyChildCount;

	[Label]
	public int m_uuId;

	private Transform _groupTrans;

	private Grid _groundGrid;

	public GameObject colider;

	public AudioSource audioSource;

	public Point point;

	[Label]
	public CommonState commonState;

	[HideInInspector]
	public abstract TileObjectType GetTileObjectType { get; }

	public virtual bool CanRecycle
	{
		get
		{
			Transform transform = groupTransform;
			return transform.InverseTransformPoint(BaseRole.BallPosition).z - transform.InverseTransformPoint(base.transform.position).z > 20f;
		}
	}

	public Grid CurrentGrid
	{
		get
		{
			return _groundGrid;
		}
	}

	public Transform groupTransform
	{
		get
		{
			if (_groundGrid == null)
			{
				_groundGrid = MapController.Instance.GetGridById(m_gridId);
				_groupTrans = _groundGrid.transform;
			}
			return _groupTrans;
		}
		set
		{
			_groupTrans = value;
			if (value == null)
			{
				_groundGrid = null;
			}
			else
			{
				_groundGrid = _groupTrans.GetComponent<Grid>();
			}
		}
	}

	public Vector3 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	public bool ActiveAndEnabled
	{
		get
		{
			return base.isActiveAndEnabled;
		}
	}

	public virtual bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public void SetGrid(Grid grid)
	{
		_groundGrid = grid;
		if (grid != null)
		{
			_groupTrans = _groundGrid.transform;
		}
	}

	public virtual float GetPercent(float distance)
	{
		return 0f;
	}

	public virtual void PlayByPercent(float percent)
	{
	}

	public virtual void DebugPlay(float distance)
	{
	}

	public virtual void AddTriggerListener()
	{
	}

	public virtual void RemoveTriggerListener()
	{
	}

	public virtual void TriggerEnter(BaseRole ball)
	{
	}

	public virtual void CoupleTriggerEnter(BaseCouple couple, Collider collider)
	{
	}

	public virtual void OnTriggerPlay()
	{
	}

	public virtual void OnTriggerStop()
	{
	}

	public virtual void Initialize()
	{
		StartPos = base.transform.position;
		StartLocalPos = base.transform.localPosition;
		StartLocalEuler = base.transform.localEulerAngles;
		audioSource = GetComponentInChildren<AudioSource>();
		IfInitialize = true;
	}

	public virtual void LateInitialize()
	{
	}

	public virtual void InitElement()
	{
	}

	public virtual void UpdateElement()
	{
	}

	public virtual void ResetElement()
	{
		if (IfInitialize)
		{
			base.transform.position = StartPos;
			IfInitialize = false;
		}
	}

	public virtual void Read(string info)
	{
	}

	public virtual string Write()
	{
		return "";
	}

	public virtual void ReadBytes(byte[] bytes)
	{
	}

	public virtual byte[] WriteBytes()
	{
		return null;
	}

	public virtual void SetDefaultValue(object[] objs)
	{
	}

	public virtual void PlaySoundEffect()
	{
		if ((bool)audioSource && GameController.Instance.GetPlayerDataModule.IsSoundPlayOn())
		{
			audioSource.Play();
		}
	}

	protected void PlayAnim(Animation anim, bool ifPlay)
	{
		if (!anim)
		{
			return;
		}
		if (anim.clip == null)
		{
			Log.Error("bc === with no anim clip " + base.name);
		}
		anim.Play();
		if (ifPlay)
		{
			return;
		}
		foreach (AnimationState item in anim)
		{
			item.normalizedTime = 0f;
		}
		IEnumerator enumerator2 = anim.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			((AnimationState)enumerator2.Current).normalizedTime = 0f;
		}
		anim.Sample();
		anim.Stop();
	}

	protected void PlayAnim(Animation anim, string animName, bool ifPlay)
	{
		if (!anim)
		{
			return;
		}
		if (anim.GetClip(animName) != null)
		{
			anim[animName].normalizedTime = 0f;
			anim.Play(animName);
		}
		if (!ifPlay)
		{
			IEnumerator enumerator = anim.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((AnimationState)enumerator.Current).normalizedTime = 0f;
			}
			anim.Sample();
			anim.Stop();
		}
	}

	protected void SetAnimPercent(Animation anim, float percent)
	{
		if ((bool)anim)
		{
			anim.Play();
			IEnumerator enumerator = anim.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((AnimationState)enumerator.Current).normalizedTime = percent;
			}
			anim.Sample();
			anim.Stop();
		}
	}

	protected void PlayParticle(ParticleSystem[] particles, bool ifPlay)
	{
		if (particles == null)
		{
			return;
		}
		for (int i = 0; i < particles.Length; i++)
		{
			if (ifPlay)
			{
				particles[i].Play();
			}
			else
			{
				particles[i].Stop();
			}
		}
	}

	public static Vector3[] GetWorldPointsByLocal(Vector3[] localPoints, Transform rootTrans)
	{
		Vector3[] array = new Vector3[localPoints.Length];
		for (int i = 0; i < localPoints.Length; i++)
		{
			array[i] = rootTrans.TransformPoint(localPoints[i]);
		}
		return array;
	}

	protected static void DrawWorldPath(Vector3[] points, Color lineColor)
	{
	}

	protected static void DrawLocalPath(Vector3[] points, Transform rootTrans, Color lineColor)
	{
	}

	protected void ShowMeshRenders(MeshRenderer[] meshRenderers, bool ifShow)
	{
		if (meshRenderers != null)
		{
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].enabled = ifShow;
			}
		}
	}

	public virtual void SetBakeState()
	{
	}

	public virtual void SetBaseState()
	{
	}

	public virtual void AppendRelatedInfo(ref List<RelatedAssetData> relatedAssetList)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public virtual void RebirthReadData(object rd_data)
	{
		RD_BaseElement_DATA rD_BaseElement_DATA = JsonUtility.FromJson<RD_BaseElement_DATA>(rd_data as string);
		commonState = rD_BaseElement_DATA.commonState;
		base.transform.SetTransData(rD_BaseElement_DATA.transData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public virtual object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_BaseElement_DATA
		{
			commonState = commonState,
			transData = base.transform.GetTransData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public virtual void RebirthStartGame(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public virtual void RebirthReadDataForDrop(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public virtual void RebirthStartGameForDrop(object rd_data)
	{
	}

	public virtual void RebirthReadByteData(byte[] rd_data)
	{
		RD_BaseElement_DATA rD_BaseElement_DATA = Bson.ToObject<RD_BaseElement_DATA>(rd_data);
		commonState = rD_BaseElement_DATA.commonState;
		base.transform.SetTransData(rD_BaseElement_DATA.transData);
	}

	public virtual byte[] RebirthWriteByteData()
	{
		byte[] array = new byte[0];
		return Bson.ToBson(new RD_BaseElement_DATA
		{
			commonState = commonState,
			transData = base.transform.GetTransData()
		});
	}

	public virtual void RebirthStartByteGame(byte[] rd_data)
	{
	}

	public virtual void RebirthReadByteDataForDrop(byte[] rd_data)
	{
	}

	public virtual void RebirthStartGameByteDataForDrop(byte[] rd_data)
	{
	}
}
