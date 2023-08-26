using UnityEngine;

namespace Foundation
{
	internal sealed class EntityObject : SharedObject
	{
		private GameObject _asset;

		public EntityObject(string name, GameObject asset, GameObject entityGo)
			: base(name, entityGo)
		{
			if (asset == null)
			{
				Log.Error("Entity asset is invalid.");
			}
			else
			{
				_asset = asset;
			}
		}

		protected internal override void OnUnload(bool force = false)
		{
			GameObject gameObject = base.Target as GameObject;
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
			base.OnUnload(force);
			Mod.Resource.UnloadAsset(_asset);
			_asset = null;
		}
	}
}
