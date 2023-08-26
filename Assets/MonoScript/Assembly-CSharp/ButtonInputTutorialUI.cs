using UnityEngine;

public class ButtonInputTutorialUI : MonoBehaviour
{
	public GameObject resumeBtn;

	private void Start()
	{
		base.gameObject.transform.Find("window/anim").gameObject.SetActive(false);
		base.gameObject.transform.Find("window/anim (1)").gameObject.SetActive(false);
		resumeBtn.transform.Find("Joystick").gameObject.SetActive(false);
	}

	private void Update()
	{
	}
}
