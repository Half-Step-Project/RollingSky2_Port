using System;
using System.Collections;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class AnimEnemyPlayByEvnet : BaseEnemy
	{
		private static readonly string LinePart = "model/line";

		private static readonly string MatColorParam = "_TintColor";

		private Animation anim;

		private float animLength;

		private float changeTime;

		private float matAlpha;

		private Color defColor;

		private Renderer[] renderers;

		private float m_progress;

		private RD_AnimEnemyPlayByEvnet_DATA m_rebirthData;

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
			commonState = CommonState.None;
			anim = GetComponentInChildren<Animation>();
			Renderer[] array = (renderers = base.transform.Find(LinePart).GetComponentsInChildren<SkinnedMeshRenderer>());
			if (renderers.Length != 0)
			{
				defColor = MaterialTool.GetMaterialColor(renderers[0], MatColorParam);
				matAlpha = defColor.a;
			}
			AnimationState defaultAnimState = GetDefaultAnimState(anim);
			defaultAnimState.speed *= 1.2f;
			animLength = defaultAnimState.length;
			changeTime = 0f;
			Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnStartGame);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			ResetAnim();
			SetMaterialAlpha(1f);
			GetDefaultAnimState(anim).speed = 1f;
			changeTime = 0f;
			Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnStartGame);
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.None)
			{
				changeTime += Time.deltaTime;
				float num = 1f - changeTime / animLength;
				if (num <= 0f)
				{
					num = 0f;
					commonState = CommonState.End;
				}
				else
				{
					SetMaterialAlpha(num);
				}
			}
		}

		private void ResetAnim()
		{
			PlayAnim(anim, false);
		}

		private void OnStartGame(object sender, Foundation.EventArgs e)
		{
			GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
			if (gameStartEventArgs != null && gameStartEventArgs.StartType == GameStartEventArgs.GameStartType.Normal)
			{
				PlayAnim(anim, true);
			}
		}

		private void SetMaterialAlpha(float p)
		{
			m_progress = p;
			for (int i = 0; i < renderers.Length; i++)
			{
				MaterialTool.SetMaterialColor(renderers[i], MatColorParam, new Color(defColor.r, defColor.g, defColor.b, matAlpha * p));
			}
		}

		private AnimationState GetDefaultAnimState(Animation anim)
		{
			AnimationState result = null;
			if ((bool)anim)
			{
				IEnumerator enumerator = anim.GetEnumerator();
				if (enumerator.MoveNext())
				{
					result = enumerator.Current as AnimationState;
				}
			}
			return result;
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_rebirthData = JsonUtility.FromJson<RD_AnimEnemyPlayByEvnet_DATA>(rd_data as string);
			animLength = m_rebirthData.animLength;
			changeTime = m_rebirthData.changeTime;
			matAlpha = m_rebirthData.matAlpha;
			anim.SetAnimData(m_rebirthData.m_anim, ProcessState.Pause);
			commonState = m_rebirthData.m_commonState;
			m_progress = m_rebirthData.m_colorProgress;
			SetMaterialAlpha(m_progress);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			RD_AnimEnemyPlayByEvnet_DATA rD_AnimEnemyPlayByEvnet_DATA = new RD_AnimEnemyPlayByEvnet_DATA();
			rD_AnimEnemyPlayByEvnet_DATA.animLength = animLength;
			rD_AnimEnemyPlayByEvnet_DATA.changeTime = changeTime;
			rD_AnimEnemyPlayByEvnet_DATA.matAlpha = matAlpha;
			if (anim != null)
			{
				rD_AnimEnemyPlayByEvnet_DATA.m_anim = anim.GetAnimData();
			}
			rD_AnimEnemyPlayByEvnet_DATA.m_commonState = commonState;
			rD_AnimEnemyPlayByEvnet_DATA.m_colorProgress = m_progress;
			return JsonUtility.ToJson(rD_AnimEnemyPlayByEvnet_DATA);
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			anim.SetAnimData(m_rebirthData.m_anim, ProcessState.UnPause);
			m_rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_AnimEnemyPlayByEvnet_DATA>(rd_data);
			animLength = m_rebirthData.animLength;
			changeTime = m_rebirthData.changeTime;
			matAlpha = m_rebirthData.matAlpha;
			anim.SetAnimData(m_rebirthData.m_anim, ProcessState.Pause);
			commonState = m_rebirthData.m_commonState;
			m_progress = m_rebirthData.m_colorProgress;
			SetMaterialAlpha(m_progress);
		}

		public override byte[] RebirthWriteByteData()
		{
			RD_AnimEnemyPlayByEvnet_DATA rD_AnimEnemyPlayByEvnet_DATA = new RD_AnimEnemyPlayByEvnet_DATA();
			rD_AnimEnemyPlayByEvnet_DATA.animLength = animLength;
			rD_AnimEnemyPlayByEvnet_DATA.changeTime = changeTime;
			rD_AnimEnemyPlayByEvnet_DATA.matAlpha = matAlpha;
			if (anim != null)
			{
				rD_AnimEnemyPlayByEvnet_DATA.m_anim = anim.GetAnimData();
			}
			rD_AnimEnemyPlayByEvnet_DATA.m_commonState = commonState;
			rD_AnimEnemyPlayByEvnet_DATA.m_colorProgress = m_progress;
			return Bson.ToBson(rD_AnimEnemyPlayByEvnet_DATA);
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			anim.SetAnimData(m_rebirthData.m_anim, ProcessState.UnPause);
			m_rebirthData = null;
		}
	}
}
