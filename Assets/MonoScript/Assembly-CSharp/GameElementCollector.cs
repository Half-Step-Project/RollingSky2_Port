using System.Collections.Generic;
using Foundation;
using UnityEngine;

public static class GameElementCollector
{
	public static Dictionary<int, BaseElement> GameElementDictionary = new Dictionary<int, BaseElement>();

	public static void RegisterAllElement()
	{
		BaseElement[] array = Resources.FindObjectsOfTypeAll(typeof(BaseElement)) as BaseElement[];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.RegisterElement(array[i]);
		}
	}

	public static void RegisterElement(this GameObject gameObj, BaseElement elementScript)
	{
		Transform[] componentsInChildren = gameObj.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!GameElementDictionary.ContainsKey(componentsInChildren[i].gameObject.GetInstanceID()))
			{
				GameElementDictionary.Add(componentsInChildren[i].gameObject.GetInstanceID(), elementScript);
			}
		}
	}

	public static void ResetAllElement()
	{
		GameElementDictionary.Clear();
	}

	public static T GetGameComponent<T>(this GameObject gameObj) where T : BaseElement
	{
		BaseElement value = null;
		if (GameElementDictionary.TryGetValue(gameObj.GetInstanceID(), out value) && value == null)
		{
			Log.Warning("BaseElement is null " + gameObj.name);
		}
		return value as T;
	}
}
