namespace RS2
{
	public struct LevelUpdateData
	{
		public int Level { get; private set; }

		public bool NeedUpdate { get; private set; }

		public int UpdateCount { get; private set; }

		public int UpdateTotalLength { get; private set; }

		public LevelUpdateData(int level, bool needUpdate, int updateCount, int updateTotalLength)
		{
			this = default(LevelUpdateData);
			Level = level;
			NeedUpdate = needUpdate;
			UpdateCount = updateCount;
			UpdateTotalLength = updateTotalLength;
		}
	}
}
