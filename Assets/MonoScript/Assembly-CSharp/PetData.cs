using System;

[Serializable]
public class PetData
{
	public int m_id;

	public string m_path;

	public Type m_classType;

	public object[] m_attributes;

	public PetData(int id, string path, Type classType, params object[] attributes)
	{
		m_id = id;
		m_path = path;
		m_classType = classType;
		m_attributes = attributes;
	}
}
