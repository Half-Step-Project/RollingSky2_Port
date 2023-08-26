using System;

namespace User.TileMap
{
	[Serializable]
	public class RelatedAssetData
	{
		public const int Type_AnimClip = 1;

		public const int Type_AudioClip = 2;

		public int AssetType;

		public string Path;

		public RelatedAssetData(string path, int assetType = 1)
		{
			Path = path;
			AssetType = assetType;
		}
	}
}
