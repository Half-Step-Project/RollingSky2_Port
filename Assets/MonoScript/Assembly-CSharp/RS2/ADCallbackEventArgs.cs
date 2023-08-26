using System;

namespace RS2
{
	public class ADCallbackEventArgs : EventArgs
	{
		public ADScene Scene;

		public ADStatus Status;

		public int ID;

		public ADCallbackEventArgs(ADScene scene, ADStatus status)
		{
			Scene = scene;
			Status = status;
		}

		public override string ToString()
		{
			return "ID:" + ID + "ADScen:" + Scene.ToString() + "ADStatus:" + Status;
		}
	}
}
