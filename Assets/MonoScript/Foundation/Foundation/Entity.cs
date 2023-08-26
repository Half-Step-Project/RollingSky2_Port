using UnityEngine;

namespace Foundation
{
	public abstract class Entity : MonoBehaviour, IHearable
	{
		public Vector3 Position
		{
			get
			{
				return base.transform.position;
			}
		}

		public bool ActiveAndEnabled
		{
			get
			{
				return base.isActiveAndEnabled;
			}
		}

		public int Id { get; private set; }

		public string AssetName { get; private set; }

		public EntityGroup Group { get; private set; }

		protected internal virtual void OnInit(int id, string assetName, EntityGroup group, bool isNew)
		{
			Id = id;
			AssetName = assetName;
			if (isNew)
			{
				Group = group;
			}
			else if (Group != group)
			{
				Log.Error("Entity group is inconsistent for non-new-instance entity.");
			}
		}

		protected internal virtual void OnRecycle()
		{
			Id = -1;
		}

		protected internal virtual void OnShow(object userData)
		{
			base.enabled = true;
		}

		protected internal virtual void OnHide(object userData)
		{
			base.enabled = false;
		}

		protected internal abstract void OnAttached(Entity child, object userData);

		protected internal abstract void OnAttachTo(Entity parent, object userData);

		protected internal abstract void OnDetached(Entity child, object userData);

		protected internal abstract void OnDetachFrom(Entity parent, object userData);

		protected internal abstract void OnTick(float elapseSeconds, float realElapseSeconds);
	}
}
