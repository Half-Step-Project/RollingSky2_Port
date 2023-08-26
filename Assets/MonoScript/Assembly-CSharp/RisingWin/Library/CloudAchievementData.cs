namespace RisingWin.Library
{
	public class CloudAchievementData : CloudProcess
	{
		public bool achieved;

		public string id { get; private set; }

		public string name { get; private set; }

		public string description { get; private set; }

		public CloudAchievementData(string id)
		{
			this.id = id;
		}

		public CloudAchievementData(string id, string name, string description, bool achieved)
		{
			this.id = id;
			this.name = name;
			this.description = description;
			this.achieved = achieved;
		}

		public virtual void SetAttribute(string name, string description, bool achieved)
		{
			this.name = name;
			this.description = description;
			this.achieved = achieved;
		}

		public virtual string Sprintf()
		{
			return string.Format("id : {0}, name : {1}, description : {2}, achieved : {3}", id, name, description, achieved);
		}
	}
}
