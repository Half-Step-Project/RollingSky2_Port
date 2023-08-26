using UnityEngine;

namespace Foundation
{
	public abstract class EditorLoaderBase : MonoBehaviour
	{
		protected internal abstract void LoadAsset(string assetName, AssetLoadCallbacks loadCallbacks, object userData);

		protected internal abstract void LoadScene(string assetName, SceneLoadCallbacks loadCallbacks, object userData);

		protected internal abstract void UnloadScene(string assetName, SceneUnloadCallbacks unloadCallbacks, object userData);

		protected internal abstract void Tick(float elapseSeconds, float realElapseSeconds);

		protected virtual void Awake()
		{
			base.hideFlags |= HideFlags.DontSaveInBuild;
		}
	}
}
