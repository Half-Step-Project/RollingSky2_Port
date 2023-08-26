using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class PropShield : BaseProp
{
	private GameObject m_moveGameObject;

	private Animator m_aniamtor;

	private float m_moveLifeTime = 0.8f;

	private float m_blinkLifeTime = 1f;

	private Vector3 m_fromOffest = new Vector3(0f, 0f, 3f);

	private string m_scaleAnimationEventForShiedName = "ShowShied";

	private string m_scaleAnimationEventForFinishedName = "Finished";

	private int mCanTriggerCount = 1;

	private int mCurrentTriggerCount;

	public UsingAssertForm GetUsingAssertViewModule
	{
		get
		{
			return Mod.UI.GetUIForm(UIFormId.UsingAssertForm) as UsingAssertForm;
		}
	}

	public override void OnInstance()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		m_moveGameObject = dictionary["Effect_lizi"];
		m_aniamtor = dictionary["effect_hudun001"].GetComponent<Animator>();
		OnDefault();
	}

	public override void Add()
	{
		m_update = null;
		mCurrentTriggerCount = 0;
		mCanTriggerCount = 1;
		if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(8))
		{
			int num = (mCanTriggerCount = PlayerDataModule.Instance.GetStarLevelAbilityNum(8));
		}
		OnDefault();
		base.gameObject.transform.position = BaseRole.theBall.BodyPart.RoleWaist.position;
		float _time = 0f;
		Vector3 _fromPosition = Vector3.zero;
		Vector3 _toPosition = Vector3.zero;
		if (Mod.UI.UIFormIsOpen(UIFormId.UsingAssertForm))
		{
			Camera uICamera = Mod.UI.UICamera;
			Vector3 zero = Vector3.zero;
			zero = ((!(GetUsingAssertViewModule.m_personalAssetsList.m_bufferContainer != null) || !(GetUsingAssertViewModule.m_personalAssetsList.m_bufferContainer.shieldBuffItem != null)) ? uICamera.WorldToScreenPoint(GetUsingAssertViewModule.transform.position) : uICamera.WorldToScreenPoint(GetUsingAssertViewModule.m_personalAssetsList.m_bufferContainer.shieldBuffItem.transform.position));
			Camera main = Camera.main;
			zero = new Vector3(z: main.WorldToScreenPoint(base.transform.position - m_fromOffest).z, x: zero.x, y: zero.y);
			zero = MonoSingleton<GameTools>.Instacne.GetValidScreenPos(zero);
			_fromPosition = main.ScreenToWorldPoint(zero);
			m_update = delegate
			{
				_time += Time.deltaTime;
				float num2 = _time / m_moveLifeTime;
				if (num2 <= 1f)
				{
					_toPosition = BaseRole.theBall.BodyPart.RoleLeftShoulder.transform.position;
					m_moveGameObject.transform.position = Vector3.Lerp(_fromPosition, _toPosition, num2);
				}
				else
				{
					m_moveGameObject.SetActive(false);
					_time = 0f;
					ShowShieldForRole(true);
					m_aniamtor.Play("shangdun", 0);
					m_update = OnLerp;
				}
			};
		}
		else
		{
			m_moveGameObject.SetActive(false);
			ShowShieldForRole(true);
			m_aniamtor.Play("shangdun", 0);
			m_update = OnLerp;
		}
	}

	private void OnLerp()
	{
		base.gameObject.transform.position = Vector3.Lerp(base.gameObject.transform.position, BaseRole.theBall.BodyPart.RoleWaist.position, Time.deltaTime * 30f);
	}

	private void ShowShieldForRole(bool enable)
	{
		if (BaseRole.theBall != null)
		{
			BaseRole.theBall.ShowInnerGlowData(enable, InnerGlowType.Shield);
		}
	}

	private void ShowInvincibleForRole(bool enable)
	{
		if (BaseRole.theBall != null)
		{
			BaseRole.theBall.ShowInnerGlowData(enable);
		}
	}

	private void OnScaleEventHandler(GameObject obj, string stringParameter)
	{
		if (stringParameter == m_scaleAnimationEventForShiedName)
		{
			ShowShieldForRole(true);
		}
		else
		{
			bool flag = stringParameter == m_scaleAnimationEventForFinishedName;
		}
	}

	private void OnDefault()
	{
		m_moveGameObject.SetActive(true);
		ShowShieldForRole(false);
	}

	public override void OnTrigger()
	{
		float _time = 0f;
		ShowInvincibleForRole(true);
		int num = mCurrentTriggerCount + 1;
		if (num >= mCanTriggerCount)
		{
			Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(4, -1.0);
			m_aniamtor.Play("xiaoshi", 0);
			m_update = delegate
			{
				OnLerp();
				_time += Time.deltaTime;
				if (_time >= m_blinkLifeTime)
				{
					ShowInvincibleForRole(false);
					Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsRemoveEventArgs>().Initialize(PropsName.SHIELD));
					m_update = null;
				}
			};
		}
		else
		{
			m_aniamtor.Play("xiaoshi", 0);
			m_update = delegate
			{
				OnLerp();
				_time += Time.deltaTime;
				if (_time >= m_blinkLifeTime)
				{
					base.IsTriggering = false;
					ShowInvincibleForRole(false);
					m_moveGameObject.SetActive(false);
					m_aniamtor.Play("shangdun", 0);
					ShowShieldForRole(true);
					m_update = OnLerp;
				}
			};
		}
		mCurrentTriggerCount = num;
	}

	public override void Remove()
	{
		ShowShieldForRole(false);
		m_update = null;
		if (m_aniamtor != null)
		{
			m_aniamtor.StopPlayback();
		}
	}
}
