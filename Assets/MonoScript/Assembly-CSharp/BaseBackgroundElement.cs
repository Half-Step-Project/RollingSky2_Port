using System;
using Foundation;
using UnityEngine;

public class BaseBackgroundElement : MonoBehaviour, IOriginRebirth, IElementRebirth
{
	public BackgroundData CurrentBackData;

	public virtual bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public virtual bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public virtual void StartPlayAnim()
	{
	}

	public virtual void Initialize(Transform parent)
	{
	}

	public virtual void ResetElement()
	{
	}

	public virtual int GetBackgrondIndex()
	{
		return 0;
	}

	public virtual void ResetBySavePointInfo(RebirthBoxData savePoint)
	{
	}

	[Obsolete("this is Obsolete,please  please use GetOriginRebirthBsonData !")]
	public virtual object GetOriginRebirthData(object obj = null)
	{
		return string.Empty;
	}

	[Obsolete("this is Obsolete,please  please use SetOriginRebirthBsonData !")]
	public virtual void SetOriginRebirthData(object dataInfo)
	{
	}

	[Obsolete("this is Obsolete,please  please use StartRunByOriginRebirthBsonData !")]
	public virtual void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public virtual byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return new byte[0];
	}

	public virtual void SetOriginRebirthBsonData(byte[] dataInfo)
	{
	}

	public virtual void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}

	public virtual void RebirthReadData(object rd_data)
	{
	}

	public virtual object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_BaseBackgroundElement_DATA());
	}

	public virtual void RebirthStartGame(object rd_data)
	{
	}

	public virtual void RebirthReadByteData(byte[] rd_data)
	{
	}

	public virtual byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_BaseBackgroundElement_DATA());
	}

	public virtual void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
