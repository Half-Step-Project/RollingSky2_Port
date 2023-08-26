using System;
using Foundation;
using RS2;
using UnityEngine;

public class SwitchListenTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		public string m_offAnimName;

		public int m_offGroupID;

		public string m_onAnimName;

		public int m_onGroupID;

		public SwitchState m_defaultState;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_offAnimName = "anim01";
				result.m_offGroupID = 1;
				result.m_onAnimName = "anim02";
				result.m_onGroupID = 1;
				result.m_defaultState = SwitchState.On;
				return result;
			}
		}
	}

	public struct RebirthData
	{
		public RD_ElementTransform_DATA tran;

		public RD_ElementAnim_DATA anim;

		public SwitchState state;
	}

	public Data m_data;

	public Animation m_animation;

	[Range(-1f, 1f)]
	public float m_animProgress;

	[Label]
	public SwitchState m_currentSwitchState;

	private RebirthData m_rebirthData;

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
		m_animation = base.gameObject.GetComponentInChildren<Animation>();
		if ((bool)m_animation)
		{
			string empty = string.Empty;
			if (m_data.m_defaultState == SwitchState.Off)
			{
				Data datum = m_data;
			}
			else if (m_data.m_defaultState == SwitchState.On)
			{
				Data datum2 = m_data;
			}
			m_animation.Play(m_data.m_offAnimName);
			m_animation[m_data.m_offAnimName].normalizedTime = 0f;
			m_animation.Sample();
			m_animation.Stop();
		}
		m_currentSwitchState = m_data.m_defaultState;
		Mod.Event.Subscribe(EventArgs<SwitchEventArgs>.EventId, OnListenEvent);
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if (m_animation != null)
		{
			m_animation.Stop();
		}
		Mod.Event.Unsubscribe(EventArgs<SwitchEventArgs>.EventId, OnListenEvent);
	}

	private void OnValidate()
	{
		m_animation = base.gameObject.GetComponentInChildren<Animation>();
		if (m_animation == null)
		{
			return;
		}
		if (m_animProgress >= 0f)
		{
			if (m_animation.GetClip(m_data.m_onAnimName) != null)
			{
				m_animation.Play(m_data.m_onAnimName);
				m_animation[m_data.m_onAnimName].normalizedTime = m_animProgress;
				m_animation.Sample();
			}
		}
		else if (m_animation.GetClip(m_data.m_offAnimName) != null)
		{
			m_animation.Play(m_data.m_offAnimName);
			m_animation[m_data.m_offAnimName].normalizedTime = 0f - m_animProgress;
			m_animation.Sample();
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (Data)objs[0];
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		if (m_currentSwitchState == SwitchState.Off)
		{
			CrashBall(ball);
		}
	}

	protected void CrashBall(BaseRole ball)
	{
		if ((bool)ball && !GameController.IfNotDeath)
		{
			ball.CrashBall();
		}
	}

	private void OnListenEvent(object sender, Foundation.EventArgs e)
	{
		SwitchEventArgs switchEventArgs = e as SwitchEventArgs;
		if (switchEventArgs == null)
		{
			return;
		}
		SwitchSendData[] sendDatas = switchEventArgs.m_data.m_sendDatas;
		for (int i = 0; i < sendDatas.Length; i++)
		{
			if (sendDatas[i].m_switchState == SwitchState.Off && sendDatas[i].m_onGroupID == m_data.m_offGroupID && !string.IsNullOrEmpty(m_data.m_offAnimName))
			{
				m_currentSwitchState = SwitchState.Off;
				m_animation.Play(m_data.m_offAnimName);
				break;
			}
			if (sendDatas[i].m_switchState == SwitchState.On && sendDatas[i].m_onGroupID == m_data.m_onGroupID && !string.IsNullOrEmpty(m_data.m_onAnimName))
			{
				m_currentSwitchState = SwitchState.On;
				m_animation.Play(m_data.m_onAnimName);
				break;
			}
		}
	}

	public override object RebirthWriteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.tran = base.gameObject.transform.GetTransData();
		rebirthData.anim = m_animation.GetAnimData();
		rebirthData.state = m_currentSwitchState;
		return JsonUtility.ToJson(rebirthData);
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data != null)
		{
			string text = (string)rd_data;
			if (!string.IsNullOrEmpty(text))
			{
				RebirthData rebirthData = JsonUtility.FromJson<RebirthData>(text);
				base.gameObject.transform.SetTransData(rebirthData.tran);
				m_animation.SetAnimData(rebirthData.anim, ProcessState.Pause);
				m_currentSwitchState = rebirthData.state;
				m_rebirthData = rebirthData;
			}
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		m_animation.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.tran = base.gameObject.transform.GetTransData();
		rebirthData.anim = m_animation.GetAnimData();
		rebirthData.state = m_currentSwitchState;
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null && rd_data.Length != 0)
		{
			RebirthData rebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(rebirthData.tran);
			m_animation.SetAnimData(rebirthData.anim, ProcessState.Pause);
			m_currentSwitchState = rebirthData.state;
			m_rebirthData = rebirthData;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		m_animation.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
	}
}
