namespace RS2
{
	public sealed class LevelUnLock_table : DataRowBase
	{
		public const string DataTableName = "LevelUnLock_table";

		public int Id { get; private set; }

		public string Notes { get; private set; }

		public int LockType { get; private set; }

		public int LockTypeId { get; private set; }

		public int UnLockNum { get; private set; }

		public string LinkLevel { get; private set; }

		public int Relation { get; private set; }

		public int Desc { get; private set; }

		public override int GetId()
		{
			return Id;
		}

		protected override void ParseRow()
		{
			Id = ReadInt();
			Notes = ReadLocalString();
			LockType = ReadInt();
			LockTypeId = ReadInt();
			UnLockNum = ReadInt();
			LinkLevel = ReadLocalString();
			Relation = ReadInt();
			Desc = ReadInt();
		}
	}
}
