using System;
using UnityEngine;

public class CMPGDPRUtils : MonoBehaviour
{
	public class GDPREventArgs : EventArgs
	{
		private bool agreed;

		public bool Agreed
		{
			get
			{
				return agreed;
			}
		}

		public GDPREventArgs(bool _agreed)
		{
			agreed = _agreed;
		}
	}

	public static CMPGDPRUtils Instance;

	private static string gameObjectName = "CMPGDPRUtils";

	public event EventHandler<GDPREventArgs> OnGDPRCallback;

	public static void Initialize()
	{
		if (Instance == null)
		{
			Debug.Log("Initialize CMPGDPRUtils");
			GameObject gameObject = GameObject.Find(gameObjectName);
			if (gameObject == null)
			{
				gameObject = new GameObject(gameObjectName);
			}
			if (gameObject != null)
			{
				gameObject.AddComponent<CMPGDPRUtils>();
			}
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			Init();
		}
	}

	private void Init()
	{
		string msgReceiverName = gameObjectName;
		if (base.gameObject.name != null && base.gameObject.name != "")
		{
			msgReceiverName = base.gameObject.name;
		}
		showGDPRDialog(msgReceiverName);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public bool checkIsGDPREnforcedCountry()
	{
		return false;
	}

	public bool checkIfGDPRAgreedPolicyUpdates()
	{
		return false;
	}

	public bool checkIfGDPRAgreedAdStayInformed()
	{
		return false;
	}

	public void setGDPRAgreedAdStayInformed(bool isAgreed)
	{
	}

	public void showGDPRDialog(string msgReceiverName)
	{
	}

	public void onGDPRAgree(string isAgreed)
	{
		Debug.Log("CMPGDPRUtils onGDPRAgree  msg received@@@@@@   return value:" + isAgreed);
		if (this.OnGDPRCallback != null)
		{
			bool agreed = string.Equals(isAgreed, "true", StringComparison.CurrentCultureIgnoreCase);
			this.OnGDPRCallback(this, new GDPREventArgs(agreed));
		}
	}

	public void doReport(string jsonData)
	{
		Debug.Log("CMPGDPRUtils doReport  msg received@@@@@@   return jsonData:" + jsonData);
	}
}
