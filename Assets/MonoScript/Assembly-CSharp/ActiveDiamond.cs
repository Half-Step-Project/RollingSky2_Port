using System;
using System.IO;
using Foundation;
using UnityEngine;

public class ActiveDiamond : BaseTriggerBox, IBrushTrigger, IRebirth, IAwardComplete, IAward
{
	[Serializable]
	public struct Data : IReadWriteBytes
	{
		public Vector3[] positions;

		public int smooth;

		public float speed;

		public float beginDistance;

		[HideInInspector]
		public Vector3 beginPos;

		[Label]
		public int sortID;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			positions = bytes.GetVector3Array(ref startIndex);
			smooth = bytes.GetInt32(ref startIndex);
			speed = bytes.GetSingle(ref startIndex);
			beginDistance = bytes.GetSingle(ref startIndex);
			beginPos = bytes.GetVector3(ref startIndex);
			sortID = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(positions.GetBytes(), ref offset);
				memoryStream.WriteByteArray(smooth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(speed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(beginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(beginPos.GetBytes(), ref offset);
				memoryStream.WriteByteArray(sortID.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public const string NodeShowModelPath = "model/showNode";

	public const string NodeHideModelPath = "model/hideNode";

	protected GameObject modelShowNode;

	protected GameObject modelHideNode;

	protected Animation animShowNode;

	protected Animation animHideNode;

	public Data data;

	private Collider getCollider;

	private Transform modle;

	private Transform triggerPoint;

	private Transform cacheParent;

	private bool startMove;

	private bool isFinished;

	private BezierMover bezierMover;

	private Vector3[] paths;

	private ParticleSystem particle;

	private Animation anim;

	private RD_ActiveDiamond_DATA m_rebirthData;

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
		modle = base.transform.Find("model");
		modle.gameObject.SetActive(false);
		getCollider = base.transform.Find("model/getTrigger").GetComponent<Collider>();
		Transform transform = base.gameObject.transform.Find("effect");
		if (transform != null)
		{
			particle = transform.GetComponentInChildren<ParticleSystem>();
		}
		startMove = false;
		isFinished = false;
		ResetBezier();
		Transform transform2 = base.transform.Find("model/showNode");
		Transform transform3 = base.transform.Find("model/hideNode");
		modelShowNode = (transform2 ? transform2.gameObject : null);
		modelHideNode = (transform3 ? transform3.gameObject : null);
		animShowNode = (modelShowNode ? modelShowNode.GetComponent<Animation>() : null);
		animHideNode = (modelHideNode ? modelHideNode.GetComponent<Animation>() : null);
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType dropType = GetDropType();
		bool flag = dataModule.IsShowForAwardByDropType(dropType, data.sortID);
		anim = (flag ? animShowNode : animHideNode);
		if ((bool)modelShowNode)
		{
			modelShowNode.SetActive(flag);
		}
		if ((bool)modelHideNode)
		{
			modelHideNode.SetActive(!flag);
		}
		if ((bool)anim)
		{
			anim.wrapMode = WrapMode.Loop;
			anim["anim01"].normalizedTime = 0f;
			anim.Play();
		}
	}

	public void ResetBezier()
	{
		if (data.positions != null)
		{
			Vector3[] array = new Vector3[data.positions.Length];
			for (int i = 0; i < data.positions.Length; i++)
			{
				array[i] = base.transform.TransformPoint(data.positions[i]);
			}
			paths = ThreeBezier.GetPathByPositions(array, data.smooth);
			bezierMover = new BezierMover(paths);
			modle.gameObject.SetActive(false);
		}
	}

	private Transform GetTriggerPoint()
	{
		if (triggerPoint == null)
		{
			triggerPoint = base.transform.Find("triggerPoint");
		}
		return triggerPoint;
	}

	private Transform GetParent()
	{
		if (cacheParent == null)
		{
			cacheParent = base.transform.parent;
		}
		return cacheParent;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		startMove = false;
		isFinished = false;
		ResetBezier();
	}

	public override void UpdateElement()
	{
		float num = GetParent().InverseTransformPoint(BaseRole.BallPosition).z - GetParent().InverseTransformPoint(base.transform.position).z;
		if (num >= data.beginDistance)
		{
			startMove = true;
		}
		if (startMove && !isFinished)
		{
			if (!modle.gameObject.activeInHierarchy)
			{
				modle.gameObject.SetActive(true);
			}
			num = Railway.theRailway.SpeedForward * Time.deltaTime * data.speed;
			Vector3 targetPos = Vector3.zero;
			Vector3 moveDir = Vector3.forward;
			Vector3 position = modle.position;
			isFinished = bezierMover.MoveForwardByDis(num, position, ref targetPos, ref moveDir);
			modle.position = targetPos;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<Data>(info);
		GetTriggerPoint().position = data.beginPos;
	}

	public override string Write()
	{
		data.beginPos = GetTriggerPoint().position;
		data.beginDistance = base.transform.parent.InverseTransformPoint(GetTriggerPoint().position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<Data>(bytes);
		GetTriggerPoint().position = data.beginPos;
	}

	public override byte[] WriteBytes()
	{
		data.beginPos = GetTriggerPoint().position;
		data.beginDistance = base.transform.parent.InverseTransformPoint(GetTriggerPoint().position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		data = (Data)objs[0];
	}

	public bool IsRecordRebirth()
	{
		return true;
	}

	public object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		return null;
	}

	public void ResetBySavePointData(object obj)
	{
		ResetBezier();
	}

	public void StartRunningForRebirthData(object obj)
	{
	}

	public void TriggerEnter(BaseRole ball, Collider collider)
	{
		if (collider == getCollider)
		{
			ball.GainDiamond(m_uuId, data.sortID);
			PlayParticle();
			PlaySoundEffect();
			modle.gameObject.SetActive(false);
		}
	}

	private void PlayParticle()
	{
		if (particle != null)
		{
			particle.Play();
		}
	}

	public override void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(base.transform.position, GetTriggerPoint().position);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(GetTriggerPoint().position, new Vector3(1f, 0.1f, 0.1f));
	}

	public int GetAwardSortID()
	{
		return data.sortID;
	}

	public void SetAwardSortID(int id)
	{
		data.sortID = id;
	}

	public virtual DropType GetDropType()
	{
		return DropType.DIAMOND;
	}

	public bool IsHaveFragment()
	{
		return false;
	}

	public int GetHaveFragmentCount()
	{
		return 0;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_ActiveDiamond_DATA
		{
			modle = modle.GetTransData(),
			triggerPoint = triggerPoint.GetTransData(),
			cacheParent = cacheParent.GetTransData(),
			startMove = (startMove ? 1 : 0),
			isFinished = (isFinished ? 1 : 0),
			particle = particle.GetParticleData(),
			m_bezierMover = bezierMover.GetBezierData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_ActiveDiamond_DATA>(rd_data as string);
		modle.SetTransData(m_rebirthData.modle);
		triggerPoint.SetTransData(m_rebirthData.triggerPoint);
		cacheParent.SetTransData(m_rebirthData.cacheParent);
		startMove = m_rebirthData.startMove == 1;
		isFinished = m_rebirthData.isFinished == 1;
		bezierMover.SetBezierData(m_rebirthData.m_bezierMover);
		particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		m_rebirthData = null;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthReadDataForDrop(object rd_data)
	{
		modle.gameObject.SetActive(false);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthStartGameForDrop(object rd_data)
	{
		modle.gameObject.SetActive(false);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_ActiveDiamond_DATA
		{
			modle = modle.GetTransData(),
			triggerPoint = triggerPoint.GetTransData(),
			cacheParent = cacheParent.GetTransData(),
			startMove = (startMove ? 1 : 0),
			isFinished = (isFinished ? 1 : 0),
			particle = particle.GetParticleData(),
			m_bezierMover = bezierMover.GetBezierData()
		});
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_ActiveDiamond_DATA>(rd_data);
		modle.SetTransData(m_rebirthData.modle);
		triggerPoint.SetTransData(m_rebirthData.triggerPoint);
		cacheParent.SetTransData(m_rebirthData.cacheParent);
		startMove = m_rebirthData.startMove == 1;
		isFinished = m_rebirthData.isFinished == 1;
		bezierMover.SetBezierData(m_rebirthData.m_bezierMover);
		particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		m_rebirthData = null;
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		modle.gameObject.SetActive(false);
	}

	public override void RebirthStartGameByteDataForDrop(byte[] rd_data)
	{
		modle.gameObject.SetActive(false);
	}
}
