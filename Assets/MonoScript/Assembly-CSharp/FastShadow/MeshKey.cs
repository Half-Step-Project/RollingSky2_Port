using UnityEngine;

namespace FastShadow
{
	public sealed class MeshKey
	{
		public bool isStatic;

		public Material material;

		public MeshKey(Material material, bool isStatic)
		{
			this.isStatic = isStatic;
			this.material = material;
		}

		public override bool Equals(object obj)
		{
			MeshKey meshKey = obj as MeshKey;
			if (meshKey == null)
			{
				return false;
			}
			if (meshKey.isStatic == isStatic)
			{
				return meshKey.material == material;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return isStatic.GetHashCode() ^ material.GetHashCode();
		}
	}
}
