using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using User.TileMap;

public class CameraAnimTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public string TriggerPath;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TriggerPath = bytes.GetStringWithSize(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return TriggerPath.GetBytesWithSize();
		}
	}

	public static readonly string BASE_PATH = "Assets/_RS2Art/Res/Brush/Related/Animations/";

	[HideInInspector]
	public AnimationClip m_Animation;

	public TriggerData data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		if (m_Animation == null)
		{
			string path = string.Format("{0}{1}.anim", BASE_PATH, data.TriggerPath);
			m_Animation = MapController.Instance.GetRelatedAnimationClipByPath(path);
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (m_Animation != null)
		{
			CameraController.theCamera.TriggerPlayAnimClip(m_Animation, m_Animation.name);
		}
		else
		{
			Debug.LogError("wyj=CameraAnimTrigger has no anim!!!==" + base.name);
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if (m_Animation != null)
		{
			CameraController.theCamera.TriggerRemoveAnimClip(m_Animation, m_Animation.name);
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		data.TriggerPath = objs[0] as string;
	}

	public override void AppendRelatedInfo(ref List<RelatedAssetData> relatedAssetList)
	{
		RelatedAssetData item = new RelatedAssetData(string.Format("{0}{1}.anim", BASE_PATH, data.TriggerPath));
		relatedAssetList.Add(item);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return string.Empty;
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
