using System;
using Foundation;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;
public class BaseEnemy : BaseElement
{
	public float TestDistance;

	public BaseTile RefTile;

	protected Transform effectChild;

	protected ParticleSystem[] particles;

	private RD_BaseEnemy_DATA m_theRebirthData;

	public override TileObjectType GetTileObjectType
	{
		get
		{
			return TileObjectType.Enemy;
		}
	}

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
		if ((bool)RefTile)
		{
			base.transform.parent = RefTile.transform;
		}
		if (effectChild == null)
		{
			effectChild = base.transform.Find("effect");
			if (effectChild == null)
			{
				effectChild = base.transform.Find("model/effect");
			}
			if ((bool)effectChild)
			{
				particles = effectChild.GetComponentsInChildren<ParticleSystem>();
				StopParticle();
			}
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
		}
		if ((bool)audioSource)
		{
			audioSource.Stop();
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball && !GameController.IfNotDeath)
		{
			ball.CrashBall();
		}
	}

	public virtual void OnDrawGizmos()
	{
		if (Grid.m_isShowCollider)
		{
			BoxCollider[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoxCollider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 center = componentsInChildren[i].center;
				Vector3 size = componentsInChildren[i].size;
				Vector3[] array = new Vector3[8]
				{
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, center.z + size.z / 2f)),
					componentsInChildren[i].gameObject.transform.TransformPoint(new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, center.z - size.z / 2f))
				};
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(array[0], array[1]);
				Gizmos.DrawLine(array[1], array[2]);
				Gizmos.DrawLine(array[2], array[3]);
				Gizmos.DrawLine(array[3], array[0]);
				Gizmos.DrawLine(array[4], array[5]);
				Gizmos.DrawLine(array[5], array[6]);
				Gizmos.DrawLine(array[6], array[7]);
				Gizmos.DrawLine(array[7], array[4]);
				Gizmos.DrawLine(array[0], array[4]);
				Gizmos.DrawLine(array[1], array[5]);
				Gizmos.DrawLine(array[2], array[6]);
				Gizmos.DrawLine(array[3], array[7]);
				Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
				Gizmos.DrawLine(componentsInChildren[i].bounds.center, base.gameObject.transform.position);
				Gizmos.color = new Color(1f, 0f, 0f, 1f);
				Gizmos.DrawSphere(base.gameObject.transform.position, 0.5f);
			}
		}
	}

	protected void PlayParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Play();
			}
		}
	}

	protected void StopParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_theRebirthData = JsonUtility.FromJson<RD_BaseEnemy_DATA>(rd_data as string);
		base.RebirthReadData(m_theRebirthData.baseData);
		particles.SetParticlesData(m_theRebirthData.particlesData, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_BaseEnemy_DATA
		{
			baseData = (base.RebirthWriteData() as string),
			particlesData = particles.GetParticlesData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		particles.SetParticlesData(m_theRebirthData.particlesData, ProcessState.UnPause);
		m_theRebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_theRebirthData = Bson.ToObject<RD_BaseEnemy_DATA>(rd_data);
		base.RebirthReadByteData(m_theRebirthData.baseBytesData);
		particles.SetParticlesData(m_theRebirthData.particlesData, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_BaseEnemy_DATA
		{
			baseBytesData = base.RebirthWriteByteData(),
			particlesData = particles.GetParticlesData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		particles.SetParticlesData(m_theRebirthData.particlesData, ProcessState.UnPause);
		m_theRebirthData = null;
	}
}
