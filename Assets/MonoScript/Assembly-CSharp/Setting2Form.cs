using RisingWin.Library;
using UnityEngine;
using UnityEngine.UI;

public class Setting2Form : MonoBehaviour
{
	private float controller;

	[HideInInspector]
	public float value;

	public Scrollbar scrollbar;

	public Text sensitivityTex;

	private void Start()
	{
		value = PlayerPrefsAdapter.GetFloat(PlayerLocalDatakey.SETTING_MOUSE_SENSITY, 0.25f);
		scrollbar.value = value;
	}

	public void Init()
	{
		value = PlayerPrefsAdapter.GetFloat(PlayerLocalDatakey.SETTING_MOUSE_SENSITY, 0.25f);
		scrollbar.value = value;
	}

	public void ChangeSensititivity(Scrollbar bar)
	{
		value = bar.value;
		PlayerPrefsAdapter.SetFloat(PlayerLocalDatakey.SETTING_MOUSE_SENSITY, value);
		float num = (InputController.mouseSensitivity = 1f + bar.value * 4f);
		sensitivityTex.text = num.ToString("#0.00");
	}
}
