using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTools
{
	public static Dictionary<string, GameObject> CollectAllGameObjects(GameObject rootGameObject)
	{
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		CollectAllGameObject(dictionary, rootGameObject);
		return dictionary;
	}

	private static void CollectAllGameObject(Dictionary<string, GameObject> objectMap, GameObject gameObject)
	{
		objectMap[gameObject.name] = gameObject;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			CollectAllGameObject(objectMap, gameObject.transform.GetChild(i).gameObject);
		}
	}

	public static Sprite CreateNewSpriteFromSource(Sprite sourceSprite, Vector4 border)
	{
		return Sprite.Create(sourceSprite.texture, sourceSprite.rect, sourceSprite.pivot, sourceSprite.pixelsPerUnit, 0u, SpriteMeshType.FullRect, border);
	}

	public static void SwitchTextFont(Text sourceText, SystemLanguage type = SystemLanguage.English)
	{
		switch (type)
		{
		case SystemLanguage.Arabic:
		case SystemLanguage.Dutch:
		case SystemLanguage.French:
		case SystemLanguage.German:
		case SystemLanguage.Italian:
		case SystemLanguage.Japanese:
		case SystemLanguage.Korean:
		case SystemLanguage.Russian:
		case SystemLanguage.Spanish:
		case SystemLanguage.ChineseSimplified:
		case SystemLanguage.ChineseTraditional:
		case SystemLanguage.Unknown:
			sourceText.font = Resources.Load<Font>("Fonts/Papyrus");
			break;
		case SystemLanguage.English:
			sourceText.font = Resources.Load<Font>("Fonts/Luminari");
			break;
		}
	}
}
