using UnityEngine;

namespace RS2
{
	public class BaseCouple : MonoBehaviour, IOriginRebirth
	{
		public static BaseCouple theCouple;

		public GameObject treasureChest;

		public CoupleData CurrentCoupleData;

		public float DizzinessTime = 2f;

		public float NewRoundTime = 2f;

		public bool IfHaveTreasureChest { get; set; }

		public bool IfDizziness { get; set; }

		public bool IfNewRound { get; set; }

		public Vector3 StartPos { get; private set; }

		public Vector3 StartDir { get; private set; }

		public virtual bool IsRecordOriginRebirth
		{
			get
			{
				return false;
			}
		}

		public virtual void PreInitialize()
		{
		}

		public virtual void Initialize(Vector3 startPos)
		{
			theCouple = this;
			StartPos = startPos;
			StartDir = base.transform.forward;
			base.transform.position = StartPos;
			IfDizziness = false;
			IfNewRound = false;
		}

		public virtual void ChangeToStartAnim()
		{
		}

		public virtual void ChangeToCutScene()
		{
		}

		public virtual void StartCouple()
		{
		}

		public virtual void UpdateCouple()
		{
		}

		public virtual void ResetCouple()
		{
			base.transform.position = StartPos;
			base.transform.forward = StartDir;
		}

		public virtual void DestoryLocal()
		{
		}

		public virtual void ResetBySavePointInfo(RebirthBoxData savePoint)
		{
		}

		protected virtual void OnTriggerEnter(Collider collider)
		{
		}

		protected virtual void OnTriggerExit(Collider collider)
		{
		}

		public virtual void SetPathMoveData(CouplePathMoveData pathData)
		{
		}

		public virtual void CouplePlayAnim(int animType)
		{
		}

		public virtual void GainTreasureChest(bool ifGain)
		{
			IfHaveTreasureChest = ifGain;
		}

		public virtual object GetOriginRebirthData(object obj = null)
		{
			return null;
		}

		public virtual void SetOriginRebirthData(object dataInfo)
		{
		}

		public virtual void StartRunByOriginRebirthData(object dataInfo)
		{
		}

		public virtual byte[] GetOriginRebirthBsonData(object obj = null)
		{
			return null;
		}

		public virtual void SetOriginRebirthBsonData(byte[] dataInfo)
		{
		}

		public virtual void StartRunByOriginRebirthBsonData(byte[] dataInfo)
		{
		}
	}
}
