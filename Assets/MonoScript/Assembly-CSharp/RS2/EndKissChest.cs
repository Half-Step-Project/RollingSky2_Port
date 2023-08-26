using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class EndKissChest : BaseTriggerBox
	{
		private Animation anim;

		private RD_EndKissChest__DATA rebirthData;

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
			anim = base.transform.Find("ThiefEndChest").GetComponent<Animation>();
			commonState = CommonState.None;
			bool flag = (bool)anim;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			PlayAnim(anim, false);
			bool flag = (bool)anim;
			commonState = CommonState.None;
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<ThiefEndEventArgs>.EventId, OnThiefEnd);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<ThiefEndEventArgs>.EventId, OnThiefEnd);
		}

		public override void TriggerEnter(BaseRole ball)
		{
		}

		private void OnThiefEnd(object sender, Foundation.EventArgs e)
		{
			ThiefEndEventArgs thiefEndEventArgs = e as ThiefEndEventArgs;
			if (thiefEndEventArgs != null && commonState == CommonState.None && thiefEndEventArgs.ObjType == ThiefEndEventArgs.eObjChest)
			{
				if (anim == null)
				{
					anim = base.transform.Find("ThiefEndChest").GetComponent<Animation>();
					Log.Error("Can not find model in " + base.name);
				}
				PlayAnim(anim, true);
				commonState = CommonState.Begin;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_EndKissChest__DATA rD_EndKissChest__DATA = (rebirthData = JsonUtility.FromJson<RD_EndKissChest__DATA>(rd_data as string));
			commonState = (CommonState)rD_EndKissChest__DATA.commonState;
			anim.SetAnimData(rD_EndKissChest__DATA.anim, ProcessState.Pause);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_EndKissChest__DATA
			{
				ifShowAnim = anim.gameObject.activeSelf,
				anim = anim.GetAnimData(),
				commonState = (int)commonState
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			anim.SetAnimData(rebirthData.anim, ProcessState.UnPause);
			rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_EndKissChest__DATA rD_EndKissChest__DATA = (rebirthData = Bson.ToObject<RD_EndKissChest__DATA>(rd_data));
			commonState = (CommonState)rD_EndKissChest__DATA.commonState;
			anim.SetAnimData(rD_EndKissChest__DATA.anim, ProcessState.Pause);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_EndKissChest__DATA
			{
				ifShowAnim = anim.gameObject.activeSelf,
				anim = anim.GetAnimData(),
				commonState = (int)commonState
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			anim.SetAnimData(rebirthData.anim, ProcessState.UnPause);
			rebirthData = null;
		}
	}
}
