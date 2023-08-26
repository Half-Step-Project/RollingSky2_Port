using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	[ExecuteInEditMode]
	public sealed class EntityGroup : MonoBehaviour
	{
		private readonly LinkedList<Entity> _entities = new LinkedList<Entity>();

		private ObjectPool<EntityObject> _entityPool;

		public string Name { get; private set; }

		public int Count
		{
			get
			{
				return _entities.Count;
			}
		}

		public Entity[] Entities
		{
			get
			{
				List<Entity> list = new List<Entity>();
				for (LinkedListNode<Entity> linkedListNode = _entities.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					list.Add(linkedListNode.Value);
				}
				return list.ToArray();
			}
		}

		public bool Contains(int entityId)
		{
			for (LinkedListNode<Entity> linkedListNode = _entities.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.Id == entityId)
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return false;
			}
			for (LinkedListNode<Entity> linkedListNode = _entities.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.AssetName == assetName)
				{
					return true;
				}
			}
			return false;
		}

		public Entity Get(int entityId)
		{
			for (LinkedListNode<Entity> linkedListNode = _entities.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.Id == entityId)
				{
					return linkedListNode.Value;
				}
			}
			return null;
		}

		public Entity Get(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return null;
			}
			for (LinkedListNode<Entity> linkedListNode = _entities.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.AssetName == assetName)
				{
					return linkedListNode.Value;
				}
			}
			return null;
		}

		public Entity[] GetAll(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return null;
			}
			List<Entity> list = new List<Entity>();
			for (LinkedListNode<Entity> linkedListNode = _entities.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value.AssetName == assetName)
				{
					list.Add(linkedListNode.Value);
				}
			}
			return list.ToArray();
		}

		public void SetLock(GameObject entityGo, bool locked)
		{
			if (entityGo == null)
			{
				Log.Error("Entity instance is invalid.");
			}
			else
			{
				_entityPool.SetLock(entityGo, locked);
			}
		}

		internal void Init(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.Error("Entity group name is invalid.");
				return;
			}
			Name = name;
			_entityPool = Mod.ObjectPool.Create<EntityObject>("Entity Pool (" + name + ")", false, true);
		}

		internal void Tick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("EntityMod.EntityGroup.Tick");
			LinkedListNode<Entity> linkedListNode = _entities.First;
			while (linkedListNode != null)
			{
				LinkedListNode<Entity> next = linkedListNode.Next;
				linkedListNode.Value.OnTick(elapseSeconds, realElapseSeconds);
				linkedListNode = next;
			}
			Profiler.EndSample();
		}

		internal void Add(Entity entity)
		{
			_entities.AddLast(entity);
		}

		internal void Remove(Entity entity)
		{
			_entities.Remove(entity);
		}

		internal void Register(EntityObject entityObject)
		{
			_entityPool.Register(entityObject);
		}

		internal EntityObject Spawn(string assetName)
		{
			return _entityPool.Spawn(assetName);
		}

		internal EntityObject Spawn(GameObject entityGo)
		{
			return _entityPool.Spawn(entityGo);
		}

		internal void Recycle(Entity entity)
		{
			_entityPool.Recycle(entity.gameObject);
		}
	}
}
