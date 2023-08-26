using UnityEngine;
using User.TileMap;

namespace RS2
{
	public abstract class BaseGroup : BaseElement, IGroupElement
	{
		public override TileObjectType GetTileObjectType
		{
			get
			{
				return TileObjectType.Group;
			}
		}

		public override bool CanRecycle
		{
			get
			{
				return false;
			}
		}

		public abstract Bounds GetGroupBounds(byte[] byteData);

		protected virtual void OnDrawGizmos()
		{
		}
	}
}
