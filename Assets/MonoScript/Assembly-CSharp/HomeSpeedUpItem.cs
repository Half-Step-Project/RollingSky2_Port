using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class HomeSpeedUpItem : MonoBehaviour
{
	private enum State
	{
		Idle,
		SpeedUp
	}

	public Text speedUpText;

	public GameObject freeText;

	public Animator anim;

	private State currentState;

	private void Awake()
	{
		speedUpText.text = "";
		freeText.SetActive(false);
	}

	private void Start()
	{
		InvokeRepeating("UpdateSecond", 0f, 1f);
	}

	private void OnDestroy()
	{
		CancelInvoke();
	}

	private void UpdateSecond()
	{
		RefreshSpeedUpTime();
	}

	private void RefreshSpeedUpTime()
	{
		int num = PlayerDataModule.Instance.ProductSpeedUpLeftTime();
		if (num > 0)
		{
			if (currentState == State.Idle)
			{
				anim.SetTrigger("up");
				currentState = State.SpeedUp;
			}
			speedUpText.text = MonoSingleton<GameTools>.Instacne.CommonTimeFormat(num, true);
			freeText.SetActive(false);
		}
		else
		{
			if (currentState == State.SpeedUp)
			{
				anim.SetTrigger("idle");
				currentState = State.Idle;
			}
			speedUpText.text = "";
			freeText.SetActive(true);
		}
	}

	public void OnClick()
	{
		Mod.UI.OpenUIForm(UIFormId.InstrumentSpeedUpForm);
	}
}
