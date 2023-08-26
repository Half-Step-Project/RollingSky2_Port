using UnityEngine;
using User.TileMap;

public sealed class DevelopmentTool : MonoBehaviour
{
	public static DevelopmentTool Instance;

	public int DeveLevel = 1;

	public GridGroup DevGridGroup;

	public int BufferLength = 80;

	private GUIStyle Devstyle;

	private Rect DevLabelRect;

	private void OnEnable()
	{
		Instance = this;
	}

	private void Start()
	{
		InitGUI();
		InitDevlopment();
	}

	private void InitDevlopment()
	{
		if (DevGridGroup == null)
		{
			Debug.LogError("GridGroup is null !");
		}
	}

	private void InitGUI()
	{
		Devstyle = new GUIStyle();
		Devstyle.normal.background = null;
		Devstyle.normal.textColor = Color.yellow;
		Devstyle.fontSize = 40;
		DevLabelRect = new Rect(40f, 40f, 200f, 200f);
	}

	private void OnGUI()
	{
	}
}
