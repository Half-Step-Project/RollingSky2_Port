using RS2;
using UnityEngine;

public class ButtonInputSceneTutorialForm : MonoBehaviour
{
	public SceneTutorialForm sceneTutorialForm;

	private void Start()
	{
	}

	private void Update()
	{
		if (InputService.KeyDown_A && sceneTutorialForm.mStartWidget.activeSelf)
		{
			sceneTutorialForm.OnClickStartButton(null);
		}
	}
}
