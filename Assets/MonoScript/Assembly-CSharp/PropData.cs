using System;

[Serializable]
public sealed class PropData
{
	public PropsName m_propsName;

	public string m_path;

	public Type m_classType;

	public PropData(PropsName propsName, string path, Type classType)
	{
		m_propsName = propsName;
		m_classType = classType;
		m_path = path;
	}
}
