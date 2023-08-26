using UnityEngine;

public sealed class NativeUtils
{
	private static NativeUtils instance = null;

	private static bool m_IsEnable = true;

	public static NativeUtils Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new NativeUtils();
			}
			return instance;
		}
	}

	public NativeUtils()
	{
		bool isEnable = m_IsEnable;
	}

	public bool Call_bool(string MethonName, params object[] args)
	{
		bool isEnable = m_IsEnable;
		return false;
	}

	public string Call_string(string MethonName, params object[] args)
	{
		bool isEnable = m_IsEnable;
		return "";
	}

	public long Call_long(string MethonName, params object[] args)
	{
		if (m_IsEnable)
		{
			return 314572800L;
		}
		return -1L;
	}

	public int Call_int(string MethonName, params object[] args)
	{
		bool isEnable = m_IsEnable;
		return 0;
	}

	public float Call_float(string MethonName, params object[] args)
	{
		bool isEnable = m_IsEnable;
		return 0f;
	}

	public void Call(string MethonName, params object[] args)
	{
		bool isEnable = m_IsEnable;
	}

	public void CallSpec(string MethonName, params object[] args)
	{
		bool isEnable = m_IsEnable;
	}

	public void OpenFeedbackView()
	{
		Debug.Log("OpenFeedbackView");
	}
}
