using UnityEngine;

namespace RS2
{
	public class CharactorUIData
	{
		private string charactorPath = "";

		private string animation = "";

		private Vector3 pos = Vector3.zero;

		private Quaternion rotation = Quaternion.identity;

		private Vector3 scale = Vector3.one;

		public string CharactorPath
		{
			get
			{
				return charactorPath;
			}
			set
			{
				charactorPath = value;
			}
		}

		public string Animation
		{
			get
			{
				return animation;
			}
			set
			{
				animation = value;
			}
		}

		public Vector3 Pos
		{
			get
			{
				return pos;
			}
			set
			{
				pos = value;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
			}
		}

		public void Clear()
		{
			CharactorPath = "";
			Animation = "";
			Pos = Vector3.zero;
			Rotation = Quaternion.identity;
			Scale = Vector3.one;
		}
	}
}
