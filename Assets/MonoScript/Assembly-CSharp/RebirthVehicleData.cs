using System;
using User.TileMap;

[Serializable]
public class RebirthVehicleData
{
	public int m_vehicleGridId = -1;

	public Point m_vehiclePoint;

	public TileObjectType m_tileType;

	public int m_vehicleID;

	public string m_data = string.Empty;

	public byte[] m_dataBytes;
}
