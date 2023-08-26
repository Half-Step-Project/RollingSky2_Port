using System;
using Foundation;
using UnityEngine;

public class DropDieTriggerPro : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		[Label]
		public Vector3 Position;

		[Label]
		public Quaternion Rotation;

		[Label]
		public Vector3 Scale;

		[Label]
		public Vector3 Center;

		[Label]
		public Vector3 Size;

		[Header("忽略无敌")]
		public bool IgnoreInvincible;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.Position = Vector3.zero;
				result.Rotation = Quaternion.identity;
				result.Scale = Vector3.zero;
				result.Center = Vector3.zero;
				result.Size = Vector3.one;
				result.IgnoreInvincible = true;
				return result;
			}
		}

		public static Data DefaultValue1
		{
			get
			{
				Data result = default(Data);
				result.Position = Vector3.zero;
				result.Rotation = Quaternion.identity;
				result.Scale = Vector3.zero;
				result.Center = Vector3.zero;
				result.Size = Vector3.one;
				result.IgnoreInvincible = false;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA trans;
	}

	public Data mData;

	private BoxCollider mCollider;

	public override void Initialize()
	{
		base.Initialize();
		mCollider = base.transform.Find("collider").GetComponent<BoxCollider>();
		mCollider.transform.localPosition = mData.Position;
		mCollider.transform.localRotation = mData.Rotation;
		mCollider.transform.localScale = mData.Scale;
		mCollider.center = mData.Center;
		mCollider.size = mData.Size;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!GameController.IfNotDeath)
		{
			bool flag = true;
			if (BaseRole.theBall.IsInvincible && !mData.IgnoreInvincible)
			{
				flag = false;
			}
			if (flag)
			{
				ball.BeginDropDie();
			}
		}
	}

	public override void Read(string info)
	{
		mData = JsonUtility.FromJson<Data>(info);
		if (mCollider == null)
		{
			mCollider = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		mCollider.transform.localPosition = mData.Position;
		mCollider.transform.localRotation = mData.Rotation;
		mCollider.transform.localScale = mData.Scale;
		mCollider.center = mData.Center;
		mCollider.size = mData.Size;
	}

	public override string Write()
	{
		if (mCollider == null)
		{
			mCollider = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		mData.Position = mCollider.transform.localPosition;
		mData.Rotation = mCollider.transform.localRotation;
		mData.Scale = mCollider.transform.localScale;
		mData.Center = mCollider.center;
		mData.Size = mCollider.size;
		return JsonUtility.ToJson(mData);
	}

	public override void SetDefaultValue(object[] objs)
	{
		mData = (Data)objs[0];
	}

	public override void ReadBytes(byte[] bytes)
	{
		mData = Bson.ToObject<Data>(bytes);
		if (mCollider == null)
		{
			mCollider = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		mCollider.transform.localPosition = mData.Position;
		mCollider.transform.localRotation = mData.Rotation;
		mCollider.transform.localScale = mData.Scale;
		mCollider.center = mData.Center;
		mCollider.size = mData.Size;
	}

	public override byte[] WriteBytes()
	{
		if (mCollider == null)
		{
			mCollider = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		mData.Position = mCollider.transform.localPosition;
		mData.Rotation = mCollider.transform.localRotation;
		mData.Scale = mCollider.transform.localScale;
		mData.Center = mCollider.center;
		mData.Size = mCollider.size;
		return Bson.ToBson(mData);
	}

	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData()
		});
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RebirthData
		{
			trans = base.gameObject.transform.GetTransData()
		});
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data != null)
		{
			string text = (string)rd_data;
			if (!string.IsNullOrEmpty(text))
			{
				RebirthData rebirthData = JsonUtility.FromJson<RebirthData>(text);
				base.gameObject.transform.SetTransData(rebirthData.trans);
			}
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null)
		{
			RebirthData rebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(rebirthData.trans);
		}
	}
}
