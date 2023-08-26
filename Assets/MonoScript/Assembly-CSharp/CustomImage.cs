using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/CustomImage", 20)]
public class CustomImage : Image
{
	public enum MaterialTypeName
	{
		NONE,
		Common,
		Menu,
		LevelUI
	}

	[SerializeField]
	private MaterialTypeName m_MaterialName = MaterialTypeName.Menu;

	public MaterialTypeName MaterialName
	{
		get
		{
			return m_MaterialName;
		}
		set
		{
			m_MaterialName = value;
		}
	}
}
