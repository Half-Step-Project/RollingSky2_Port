using System;
using Foundation;
using UnityEngine;

public class BackGroundTrigger : BaseTriggerBox
{
	private GameObject model;

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
		base.transform.localScale = new Vector3(6.7f, 3.22f, 3.22f);
		base.transform.localPosition = new Vector3(5.5f, 0f, 30.6f);
		model = base.transform.Find("model").gameObject;
		if (model != null)
		{
			model.SetActive(true);
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (model != null)
		{
			model.SetActive(false);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_BackGroundTrigger_DATA rD_BackGroundTrigger_DATA = JsonUtility.FromJson<RD_BackGroundTrigger_DATA>(rd_data as string);
		if (model != null)
		{
			model.SetActive(rD_BackGroundTrigger_DATA.modelActive);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_BackGroundTrigger_DATA rD_BackGroundTrigger_DATA = new RD_BackGroundTrigger_DATA();
		if (model != null)
		{
			rD_BackGroundTrigger_DATA.modelActive = model.activeSelf;
		}
		return JsonUtility.ToJson(rD_BackGroundTrigger_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_BackGroundTrigger_DATA rD_BackGroundTrigger_DATA = Bson.ToObject<RD_BackGroundTrigger_DATA>(rd_data);
		if (model != null)
		{
			model.SetActive(rD_BackGroundTrigger_DATA.modelActive);
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_BackGroundTrigger_DATA rD_BackGroundTrigger_DATA = new RD_BackGroundTrigger_DATA();
		if (model != null)
		{
			rD_BackGroundTrigger_DATA.modelActive = model.activeSelf;
		}
		return Bson.ToBson(rD_BackGroundTrigger_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
