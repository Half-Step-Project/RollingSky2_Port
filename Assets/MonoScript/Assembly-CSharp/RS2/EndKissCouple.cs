using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class EndKissCouple : BaseTriggerBox
	{
		private Animation anim;

		private RD_EndKissCouple__DATA rebirthData;

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
			anim = base.transform.Find("model").GetComponent<Animation>();
			Log.Info("BC ====== EndKissCouple Initialize");
			bool flag = (bool)anim;
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			bool flag = (bool)anim;
			PlayAnim(anim, false);
			commonState = CommonState.None;
		}

		public override void UpdateElement()
		{
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
			if (thiefEndEventArgs == null || thiefEndEventArgs.ObjType != ThiefEndEventArgs.eObjCouple)
			{
				return;
			}
			Log.Info("BC ====== OnThiefEnd " + commonState);
			if (commonState == CommonState.None)
			{
				if (anim == null)
				{
					anim = base.transform.Find("model").GetComponent<Animation>();
					Log.Error("Can not find model in " + base.name);
				}
				PlayAnim(anim, true);
				commonState = CommonState.Begin;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_EndKissCouple__DATA rD_EndKissCouple__DATA = (rebirthData = JsonUtility.FromJson<RD_EndKissCouple__DATA>(rd_data as string));
			commonState = (CommonState)rD_EndKissCouple__DATA.commonState;
			anim.SetAnimData(rD_EndKissCouple__DATA.anim, ProcessState.Pause);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_EndKissCouple__DATA
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
			RD_EndKissCouple__DATA rD_EndKissCouple__DATA = (rebirthData = Bson.ToObject<RD_EndKissCouple__DATA>(rd_data));
			commonState = (CommonState)rD_EndKissCouple__DATA.commonState;
			anim.SetAnimData(rD_EndKissCouple__DATA.anim, ProcessState.Pause);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_EndKissCouple__DATA
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
