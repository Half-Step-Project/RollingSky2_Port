using Foundation;

namespace RS2
{
	public static class DataTableModExtension
	{
		public static IDataTable<T> LoadOrGet<T>(this DataTableMod dataTable, object userData = null) where T : IRecord
		{
			string dataTableAsset = AssetUtility.GetDataTableAsset(typeof(T).Name);
			dataTable.Load<T>(dataTableAsset, userData);
			return dataTable.Get<T>();
		}
	}
}
