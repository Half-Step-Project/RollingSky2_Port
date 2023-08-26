using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class PropRebirth : BaseProp
{
	private ParticleSystem m_bornEffect;

	private float m_blinkLifeTime = 2f;

	public override void Add()
	{
		Log.Info("add PropRebirth");
		m_update = null;
		base.gameObject.transform.position = BaseRole.theBall.transform.position;
		Mod.Event.Subscribe(EventArgs<SelectSkillFireEventArgs>.EventId, OnSelectSkillFinished);
	}

	public override void OnInstance()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		m_bornEffect = dictionary["glow_001"].GetComponent<ParticleSystem>();
	}

	public override void OnTrigger()
	{
		BaseRole.theBall.IsInvincible = true;
		float _time = 0f;
		ShowInvincibleForRole(true);
		m_update = delegate
		{
			base.transform.Rotate(0f, 1f, 0f);
			_time += Time.deltaTime;
			if (_time >= m_blinkLifeTime)
			{
				BaseRole.theBall.IsInvincible = false;
				if (GameController.Instance.m_propsController.IsContains(PropsName.SHIELD))
				{
					if (BaseRole.theBall != null)
					{
						BaseRole.theBall.ShowInnerGlowData(true, InnerGlowType.Shield);
					}
				}
				else
				{
					ShowInvincibleForRole(false);
				}
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsRemoveEventArgs>().Initialize(PropsName.REBIRTH));
				m_update = null;
			}
		};
	}

	public override void Remove()
	{
		if (BaseRole.theBall != null)
		{
			BaseRole.theBall.IsInvincible = false;
		}
		m_update = null;
		Mod.Event.Unsubscribe(EventArgs<SelectSkillFireEventArgs>.EventId, OnSelectSkillFinished);
	}

	private void ShowInvincibleForRole(bool enable)
	{
		if (BaseRole.theBall != null)
		{
			BaseRole.theBall.ShowInnerGlowData(enable);
		}
	}

	private void OnSelectSkillFinished(object sender, EventArgs e)
	{
		m_bornEffect.Play(true);
	}
}
