using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class RoleShowMaskEffect : BaseEnemy
	{
		private static readonly string NodeModel = "model";

		private GameObject modelPart;

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
			Transform transform = base.transform.Find(NodeModel);
			if ((bool)transform)
			{
				modelPart = transform.gameObject;
				modelPart.SetActive(true);
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if ((bool)modelPart)
			{
				modelPart.SetActive(true);
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if ((bool)modelPart)
			{
				modelPart.SetActive(false);
				ball.ForceShowCoverFace(true);
			}
			else
			{
				ball.ForceShowCoverFace(false);
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_RoleShowMaskEffect_DATA rD_RoleShowMaskEffect_DATA = JsonUtility.FromJson<RD_RoleShowMaskEffect_DATA>(rd_data as string);
			if ((bool)modelPart)
			{
				modelPart.SetActive(rD_RoleShowMaskEffect_DATA.ifShowModel);
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			RD_RoleShowMaskEffect_DATA rD_RoleShowMaskEffect_DATA = new RD_RoleShowMaskEffect_DATA();
			rD_RoleShowMaskEffect_DATA.ifShowModel = true;
			if ((bool)modelPart)
			{
				rD_RoleShowMaskEffect_DATA.ifShowModel = modelPart.activeSelf;
			}
			return JsonUtility.ToJson(rD_RoleShowMaskEffect_DATA);
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_RoleShowMaskEffect_DATA rD_RoleShowMaskEffect_DATA = Bson.ToObject<RD_RoleShowMaskEffect_DATA>(rd_data);
			if ((bool)modelPart)
			{
				modelPart.SetActive(rD_RoleShowMaskEffect_DATA.ifShowModel);
			}
		}

		public override byte[] RebirthWriteByteData()
		{
			RD_RoleShowMaskEffect_DATA rD_RoleShowMaskEffect_DATA = new RD_RoleShowMaskEffect_DATA();
			rD_RoleShowMaskEffect_DATA.ifShowModel = true;
			if ((bool)modelPart)
			{
				rD_RoleShowMaskEffect_DATA.ifShowModel = modelPart.activeSelf;
			}
			return Bson.ToBson(rD_RoleShowMaskEffect_DATA);
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
