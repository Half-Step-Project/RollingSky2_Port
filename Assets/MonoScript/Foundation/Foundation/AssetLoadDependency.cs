namespace Foundation
{
	public delegate void AssetLoadDependency(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData);
}
