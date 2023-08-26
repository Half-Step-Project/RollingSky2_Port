using Foundation;
using UnityEngine;

public sealed class RiskPerceptionAgent : MonoBehaviour
{
	private const string _pirate = "Pirate";

	private const string _genuine = "Genuine";

	private static RiskPerceptionAgent _instance;

	public static RiskPerceptionAgent Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Director.Ins.gameObject.GetOrAddComponent<RiskPerceptionAgent>();
				if (_instance == null)
				{
					Log.Error("The risk perception instance is null.");
				}
			}
			return _instance;
		}
	}

	private event Action _reportRisk;

	public event Action ReportRisk
	{
		add
		{
			_reportRisk += value;
			Verify();
		}
		remove
		{
			_reportRisk -= value;
		}
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	public static void Init()
	{
	}

	public void SetRole()
	{
	}

	public void Verify()
	{
	}

	private void InvokeRisk(string message)
	{
		Log.Warning("Risk Perception: " + message);
		if (this._reportRisk != null)
		{
			this._reportRisk();
		}
	}
}
