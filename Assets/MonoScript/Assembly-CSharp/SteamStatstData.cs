public class SteamStatstData
{
	public enum Type
	{
		INT,
		FLOAT,
		AVGRATE
	}

	public Type valueType;

	public object value;

	public string id { get; private set; }

	public SteamStatstData(string id, Type valueType, object value)
	{
		this.id = id;
		this.valueType = valueType;
		this.value = value;
	}

	public bool IsIntValueType()
	{
		return valueType == Type.INT;
	}

	public bool IsFloatValueType()
	{
		return Type.FLOAT == valueType;
	}

	public bool IsAvgrateValueType()
	{
		return Type.AVGRATE == valueType;
	}

	public string Printf()
	{
		return string.Format("id : {0}, valueType : {1}, value : {2}", id, valueType.ToString(), value);
	}
}
