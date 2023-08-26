using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class TimeLineTools
{
	public static Dictionary<string, PlayableBinding> CollectAllPlayableBinding(PlayableDirector director)
	{
		Dictionary<string, PlayableBinding> result = new Dictionary<string, PlayableBinding>();
		if (director != null)
		{
			result = CollectAllPlayableBinding(director.playableAsset);
		}
		return result;
	}

	public static Dictionary<string, PlayableBinding> CollectAllPlayableBinding(PlayableAsset asset)
	{
		Dictionary<string, PlayableBinding> dictionary = new Dictionary<string, PlayableBinding>();
		if (asset != null)
		{
			IEnumerator<PlayableBinding> enumerator = asset.outputs.GetEnumerator();
			while (enumerator.MoveNext())
			{
				dictionary.Add(enumerator.Current.streamName, enumerator.Current);
			}
		}
		return dictionary;
	}

	public static void SetGenericBinding(PlayableDirector director, PlayableBinding binding, Object valueObject)
	{
		director.SetGenericBinding(binding.sourceObject, valueObject);
	}
}
