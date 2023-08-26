using System;

public class BackgroundData
{
	public int BackId;

	public string BackPrefabPath;

	public string BackDataPath;

	public Type BackClassType;

	public object[] BackAttributes;

	public BackgroundData(int id, string prefabPath, string dataPath, Type classType, params object[] attrs)
	{
		BackId = id;
		BackPrefabPath = prefabPath;
		BackDataPath = dataPath;
		BackClassType = classType;
		BackAttributes = attrs;
	}
}
