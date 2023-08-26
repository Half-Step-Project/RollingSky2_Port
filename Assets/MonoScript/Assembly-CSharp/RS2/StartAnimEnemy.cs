using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class StartAnimEnemy : BaseEnemy
	{
		private Animation anim;

		private GameObject effectNode;

		private RD_StartAnimEnemy_DATA rebirthData;

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
			Mod.Event.Subscribe(EventArgs<ThiefGrabTreasureEventArgs>.EventId, OnGrabTreasure);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
			Mod.Event.Unsubscribe(EventArgs<ThiefGrabTreasureEventArgs>.EventId, OnGrabTreasure);
		}

		public override void Initialize()
		{
			base.Initialize();
			anim = base.transform.GetComponentInChildren<Animation>();
			if ((bool)effectChild)
			{
				effectNode = effectChild.gameObject;
				effectNode.SetActive(true);
			}
			PlayAnim(anim, false);
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			PlayAnim(anim, false);
			effectNode.SetActive(false);
			commonState = CommonState.None;
		}

		private void OnGameStart(object sender, Foundation.EventArgs e)
		{
			if (commonState == CommonState.None)
			{
				if (anim != null)
				{
					anim["anim01"].speed = Railway.theRailway.SpeedForward / 6f;
					PlayAnim(anim, true);
					effectNode.SetActive(true);
				}
				commonState = CommonState.Active;
			}
		}

		private void OnGrabTreasure(object sender, Foundation.EventArgs e)
		{
			if (commonState == CommonState.Active && e is ThiefGrabTreasureEventArgs)
			{
				effectNode.SetActive(false);
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_StartAnimEnemy_DATA rD_StartAnimEnemy_DATA = (rebirthData = JsonUtility.FromJson<RD_StartAnimEnemy_DATA>(rd_data as string));
			anim.SetAnimData(rD_StartAnimEnemy_DATA.animData, ProcessState.Pause);
			if ((bool)effectNode)
			{
				effectNode.SetActive(rD_StartAnimEnemy_DATA.ifEffectShow);
			}
			commonState = (CommonState)rD_StartAnimEnemy_DATA.commonState;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_StartAnimEnemy_DATA
			{
				animData = anim.GetAnimData(),
				ifEffectShow = (effectNode != null && effectNode.activeSelf),
				commonState = (int)commonState
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			anim.SetAnimData(rebirthData.animData, ProcessState.UnPause);
			rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_StartAnimEnemy_DATA rD_StartAnimEnemy_DATA = (rebirthData = Bson.ToObject<RD_StartAnimEnemy_DATA>(rd_data));
			anim.SetAnimData(rD_StartAnimEnemy_DATA.animData, ProcessState.Pause);
			if ((bool)effectNode)
			{
				effectNode.SetActive(rD_StartAnimEnemy_DATA.ifEffectShow);
			}
			commonState = (CommonState)rD_StartAnimEnemy_DATA.commonState;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_StartAnimEnemy_DATA
			{
				animData = anim.GetAnimData(),
				ifEffectShow = (effectNode != null && effectNode.activeSelf),
				commonState = (int)commonState
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			anim.SetAnimData(rebirthData.animData, ProcessState.UnPause);
			rebirthData = null;
		}
	}
}
