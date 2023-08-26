using System;

public class RoleData
{
	public int RoleId;

	public string RolePrefabPath;

	public string RoleDataPath;

	public Type RoleClassType;

	public object[] RoleAttributes;

	public RoleData(int id, string prefabPath, string dataPath, Type classType, params object[] attrs)
	{
		RoleId = id;
		RolePrefabPath = prefabPath;
		RoleDataPath = dataPath;
		RoleClassType = classType;
		RoleAttributes = attrs;
	}
}
