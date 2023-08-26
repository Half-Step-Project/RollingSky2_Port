using UnityEngine;

public class DebugMode : MonoBehaviour
{
	public static bool isDebugInvincible;

	private string invincibleStatText;

	private string moveStatText;

	private float width;

	private float height;

	private string resurrectionText;

	private string autoRunText;

	private void Update()
	{
	}

	private void OnGUI()
	{
		new GUIStyle(GUI.skin.button).fontSize = Mathf.RoundToInt((float)Screen.height * 0.025f);
		new GUIStyle(GUI.skin.label)
		{
			fontSize = Mathf.RoundToInt((float)Screen.height * 0.025f),
			normal = 
			{
				textColor = Color.yellow
			}
		};
		width = (float)Screen.width * 0.075f;
		height = (float)Screen.height * 0.05f;
	}

	private void ResetInput()
	{
		if (InputController.instance != null)
		{
			InputController.instance.Reset();
		}
	}
}
