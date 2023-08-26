using UnityEngine;

namespace Foundation
{
	[ExecuteInEditMode]
	[ExecuteOrder(-10000)]
	public abstract class ModBase : MonoBehaviour
	{
		protected abstract void Awake();

		internal virtual void OnInit()
		{
		}

		internal abstract void OnTick(float elapseSeconds, float realElapseSeconds);

		internal virtual void OnExit()
		{
		}
	}
}
