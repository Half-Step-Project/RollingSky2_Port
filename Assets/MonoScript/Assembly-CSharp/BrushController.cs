public class BrushController : IOriginRebirth
{
	private MapController m_mapController;

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public BrushController(MapController mapController)
	{
		m_mapController = mapController;
	}

	public void Shutdown()
	{
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return m_mapController.GetElementData();
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		m_mapController.SetElementData((string)dataInfo);
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
		m_mapController.StartRunByElementData((string)dataInfo);
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return m_mapController.GetElementBsonData();
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		m_mapController.SetElementBsonData(dataInfo, true);
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
		m_mapController.StartRunByElementBsonData(dataInfo, true);
	}
}
