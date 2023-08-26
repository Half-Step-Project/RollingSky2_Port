using System;

public class CoupleData
{
	public int CoupleId;

	public string CouplePrefabPath;

	public Type CoupleClassType;

	public object[] CoupleAttributes;

	public CoupleData(int id, string prefabPath, Type classType, params object[] atts)
	{
		CoupleId = id;
		CouplePrefabPath = prefabPath;
		CoupleClassType = classType;
		CoupleAttributes = atts;
	}
}
