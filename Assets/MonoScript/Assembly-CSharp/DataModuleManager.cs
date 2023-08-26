using System.Collections.Generic;

public sealed class DataModuleManager : Singleton<DataModuleManager>
{
	private Dictionary<int, IDataModule> m_dataModules = new Dictionary<int, IDataModule>();

	private void Register(int key, IDataModule dataModule)
	{
		if (dataModule != null)
		{
			m_dataModules[key] = dataModule;
		}
	}

	private void UnRegister(int key)
	{
		m_dataModules.Remove(key);
	}

	private IDataModule GetByKey(int key)
	{
		IDataModule value = null;
		m_dataModules.TryGetValue(key, out value);
		return value;
	}

	private Dictionary<int, IDataModule>.KeyCollection GetAllKeys()
	{
		return m_dataModules.Keys;
	}

	public void RegisterDataModule(IDataModule dataModule)
	{
		Register((int)dataModule.GetName(), dataModule);
	}

	public void UnRegisterDataModule(IDataModule dataModule)
	{
		UnRegister((int)dataModule.GetName());
	}

	public T GetDataModule<T>(DataNames name) where T : IDataModule
	{
		return (T)GetByKey((int)name);
	}
}
