using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Entity")]
	public sealed class EntityMod : ModBase
	{
		[Serializable]
		private sealed class EntityGroupInfo
		{
			[SerializeField]
			private string _name;

			public string Name
			{
				get
				{
					return _name;
				}
			}
		}

		private sealed class UserDataEx
		{
			[CompilerGenerated]
			private readonly int _003CEntityId_003Ek__BackingField;

			[CompilerGenerated]
			private readonly EntityGroup _003CEntityGroup_003Ek__BackingField;

			[CompilerGenerated]
			private readonly object _003CUserData_003Ek__BackingField;

			public int EntityId
			{
				[CompilerGenerated]
				get
				{
					return _003CEntityId_003Ek__BackingField;
				}
			}

			public EntityGroup EntityGroup
			{
				[CompilerGenerated]
				get
				{
					return _003CEntityGroup_003Ek__BackingField;
				}
			}

			public object UserData
			{
				[CompilerGenerated]
				get
				{
					return _003CUserData_003Ek__BackingField;
				}
			}

			public UserDataEx(int entityId, EntityGroup group, object userData)
			{
				_003CEntityId_003Ek__BackingField = entityId;
				_003CEntityGroup_003Ek__BackingField = group;
				_003CUserData_003Ek__BackingField = userData;
			}
		}

		private sealed class EntityNode
		{
			private static readonly Entity[] _empty = new Entity[0];

			private List<Entity> _children;

			[CompilerGenerated]
			private readonly Entity _003CEntity_003Ek__BackingField;

			public Entity Entity
			{
				[CompilerGenerated]
				get
				{
					return _003CEntity_003Ek__BackingField;
				}
			}

			public EntityStatus Status { get; set; }

			public Entity Parent { get; set; }

			public Entity[] Children
			{
				get
				{
					List<Entity> children = _children;
					return ((children != null) ? children.ToArray() : null) ?? _empty;
				}
			}

			public EntityNode(Entity entity)
			{
				if (entity == null)
				{
					Log.Error("Entity is invalid.");
				}
				else
				{
					_003CEntity_003Ek__BackingField = entity;
				}
			}

			public void AddChild(Entity child)
			{
				if (_children == null)
				{
					_children = new List<Entity>();
				}
				if (_children.Contains(child))
				{
					Log.Warning("Can not add child entity which is already exist.");
				}
				else
				{
					_children.Add(child);
				}
			}

			public void RemoveChild(Entity child)
			{
				List<Entity> children = _children;
				if (children == null || !children.Remove(child))
				{
					Log.Warning("Can not remove child entity which is not exist.");
				}
			}
		}

		private enum EntityStatus
		{
			WillInit = 0,
			Inited = 1,
			WillShow = 2,
			Showed = 3,
			WillHide = 4,
			Hidden = 5,
			WillRecycle = 6,
			Recycled = 7
		}

		public sealed class HideCompleteEventArgs : EventArgs<HideCompleteEventArgs>
		{
			public int EntityId { get; private set; }

			public string AssetName { get; private set; }

			public EntityGroup EntityGroup { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				EntityId = -1;
				AssetName = null;
				EntityGroup = null;
				UserData = null;
			}

			public static HideCompleteEventArgs Make(int entityId, string assetName, EntityGroup entityGroup, object userData)
			{
				HideCompleteEventArgs hideCompleteEventArgs = Mod.Reference.Acquire<HideCompleteEventArgs>();
				hideCompleteEventArgs.EntityId = entityId;
				hideCompleteEventArgs.AssetName = assetName;
				hideCompleteEventArgs.EntityGroup = entityGroup;
				hideCompleteEventArgs.UserData = userData;
				return hideCompleteEventArgs;
			}
		}

		public sealed class ShowDependencyEventArgs : EventArgs<ShowDependencyEventArgs>
		{
			public int InstanceId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public string DependencyAssetName { get; private set; }

			public int LoadedCount { get; private set; }

			public int TotalCount { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				InstanceId = -1;
				AssetName = null;
				GroupName = null;
				DependencyAssetName = null;
				LoadedCount = 0;
				TotalCount = 0;
				UserData = null;
			}

			public static ShowDependencyEventArgs Make(int entityId, string assetName, string groupName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
			{
				ShowDependencyEventArgs showDependencyEventArgs = Mod.Reference.Acquire<ShowDependencyEventArgs>();
				showDependencyEventArgs.InstanceId = entityId;
				showDependencyEventArgs.AssetName = assetName;
				showDependencyEventArgs.GroupName = groupName;
				showDependencyEventArgs.DependencyAssetName = dependencyAssetName;
				showDependencyEventArgs.LoadedCount = loadedCount;
				showDependencyEventArgs.TotalCount = totalCount;
				showDependencyEventArgs.UserData = userData;
				return showDependencyEventArgs;
			}
		}

		public sealed class ShowFailureEventArgs : EventArgs<ShowFailureEventArgs>
		{
			public int EntityId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public string Message { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				EntityId = -1;
				AssetName = null;
				GroupName = null;
				Message = null;
				UserData = null;
			}

			public static ShowFailureEventArgs Make(int entityId, string assetName, string groupName, string message, object userData)
			{
				ShowFailureEventArgs showFailureEventArgs = Mod.Reference.Acquire<ShowFailureEventArgs>();
				showFailureEventArgs.EntityId = entityId;
				showFailureEventArgs.AssetName = assetName;
				showFailureEventArgs.GroupName = groupName;
				showFailureEventArgs.Message = message;
				showFailureEventArgs.UserData = userData;
				return showFailureEventArgs;
			}
		}

		public sealed class ShowSuccessEventArgs : EventArgs<ShowSuccessEventArgs>
		{
			public Entity Entity { get; private set; }

			public float Duration { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				Entity = null;
				Duration = 0f;
				UserData = null;
			}

			public static ShowSuccessEventArgs Make(Entity entity, float duration, object userData)
			{
				ShowSuccessEventArgs showSuccessEventArgs = Mod.Reference.Acquire<ShowSuccessEventArgs>();
				showSuccessEventArgs.Entity = entity;
				showSuccessEventArgs.Duration = duration;
				showSuccessEventArgs.UserData = userData;
				return showSuccessEventArgs;
			}
		}

		public sealed class ShowUpdateEventArgs : EventArgs<ShowUpdateEventArgs>
		{
			public int EntityId { get; private set; }

			public string AssetName { get; private set; }

			public string GroupName { get; private set; }

			public float Progress { get; private set; }

			public object UserData { get; private set; }

			protected override void OnRecycle()
			{
				EntityId = -1;
				AssetName = null;
				GroupName = null;
				Progress = 0f;
				UserData = null;
			}

			public static ShowUpdateEventArgs Make(int entityId, string assetName, string groupName, float progress, object userData)
			{
				ShowUpdateEventArgs showUpdateEventArgs = Mod.Reference.Acquire<ShowUpdateEventArgs>();
				showUpdateEventArgs.EntityId = entityId;
				showUpdateEventArgs.AssetName = assetName;
				showUpdateEventArgs.GroupName = groupName;
				showUpdateEventArgs.Progress = progress;
				showUpdateEventArgs.UserData = userData;
				return showUpdateEventArgs;
			}
		}

		[SerializeField]
		private EntityGroupInfo[] _groupInfos = new EntityGroupInfo[0];

		private readonly Dictionary<int, EntityNode> _entityNodes = new Dictionary<int, EntityNode>();

		private readonly Dictionary<string, EntityGroup> _entityGroups = new Dictionary<string, EntityGroup>();

		private readonly HashSet<int> _loadingEntities = new HashSet<int>();

		private readonly HashSet<int> _unloadingEntities = new HashSet<int>();

		private readonly LinkedList<EntityNode> _recycleQueue = new LinkedList<EntityNode>();

		private AssetLoadCallbacks _loadCallbacks;

		private int _nextEntityId;

		public const int InvalidEntityId = -1;

		public int EntityCount
		{
			get
			{
				return _entityNodes.Count;
			}
		}

		public int EntityGroupCount
		{
			get
			{
				return _entityGroups.Count;
			}
		}

		public EntityGroup[] EntityGroups
		{
			get
			{
				int num = 0;
				EntityGroup[] array = new EntityGroup[_entityGroups.Count];
				foreach (KeyValuePair<string, EntityGroup> entityGroup in _entityGroups)
				{
					array[num++] = entityGroup.Value;
				}
				return array;
			}
		}

		private Entity[] LoadedEntities
		{
			get
			{
				int num = 0;
				Entity[] array = new Entity[_entityNodes.Count];
				foreach (KeyValuePair<int, EntityNode> entityNode in _entityNodes)
				{
					array[num++] = entityNode.Value.Entity;
				}
				return array;
			}
		}

		public int[] LoadingEntityIds
		{
			get
			{
				int num = 0;
				int[] array = new int[_loadingEntities.Count];
				foreach (int loadingEntity in _loadingEntities)
				{
					array[num++] = loadingEntity;
				}
				return array;
			}
		}

		public bool HasEntityGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Error("Entity group name is invalid.");
				return false;
			}
			return _entityGroups.ContainsKey(groupName);
		}

		public EntityGroup GetEntityGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Error("Entity group name is invalid.");
				return null;
			}
			EntityGroup value;
			if (!_entityGroups.TryGetValue(groupName, out value))
			{
				return null;
			}
			return value;
		}

		public bool HasEntity(int entityId)
		{
			return _entityNodes.ContainsKey(entityId);
		}

		public bool HasEntity(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return false;
			}
			foreach (KeyValuePair<int, EntityNode> entityNode in _entityNodes)
			{
				if (entityNode.Value.Entity.AssetName == assetName)
				{
					return true;
				}
			}
			return false;
		}

		public Entity GetEntity(int entityId)
		{
			EntityNode value;
			if (!_entityNodes.TryGetValue(entityId, out value))
			{
				return null;
			}
			if (value == null)
			{
				return null;
			}
			return value.Entity;
		}

		public Entity GetEntity(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return null;
			}
			foreach (KeyValuePair<int, EntityNode> entityNode in _entityNodes)
			{
				if (entityNode.Value.Entity.AssetName == assetName)
				{
					return entityNode.Value.Entity;
				}
			}
			return null;
		}

		public Entity[] GetEntities(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return null;
			}
			List<Entity> list = new List<Entity>();
			foreach (KeyValuePair<int, EntityNode> entityNode in _entityNodes)
			{
				if (entityNode.Value.Entity.AssetName == assetName)
				{
					list.Add(entityNode.Value.Entity);
				}
			}
			return list.ToArray();
		}

		public bool IsLoadingEntity(int entityId)
		{
			return _loadingEntities.Contains(entityId);
		}

		public bool IsValidEntity(Entity entity)
		{
			if (entity != null)
			{
				return HasEntity(entity.Id);
			}
			return false;
		}

		public int ShowEntity(string assetName, string groupName, object userData = null)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				Log.Error("Entity asset name is invalid.");
				return -1;
			}
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Error("Entity group name is invalid.");
				return -1;
			}
			EntityGroup entityGroup = GetEntityGroup(groupName);
			if (entityGroup == null)
			{
				Log.Error("Entity group '" + groupName + "' is not exist.");
				return -1;
			}
			int num = _nextEntityId++;
			EntityObject entityObject = entityGroup.Spawn(assetName);
			if (entityObject == null)
			{
				_loadingEntities.Add(num);
				Mod.Resource.LoadAsset(assetName, _loadCallbacks, new UserDataEx(num, entityGroup, userData));
			}
			else
			{
				InternalShowEntity(num, assetName, entityGroup, (GameObject)entityObject.Target, false, 0f, userData);
			}
			return num;
		}

		public void HideEntity(int entityId, object userData = null)
		{
			EntityNode value;
			if (IsLoadingEntity(entityId))
			{
				_unloadingEntities.Add(entityId);
				_loadingEntities.Remove(entityId);
			}
			else if (!_entityNodes.TryGetValue(entityId, out value) || value == null)
			{
				Log.Error("Can not find entity '" + entityId + "'.");
			}
			else
			{
				InternalHideEntity(value, userData);
			}
		}

		public void HideEntity(Entity entity, object userData = null)
		{
			if (entity == null)
			{
				Log.Warning("Entity is invalid.");
			}
			else
			{
				HideEntity(entity.Id, userData);
			}
		}

		public void HideLoadedEntities(object userData = null)
		{
			using (Dictionary<int, EntityNode>.Enumerator enumerator = _entityNodes.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					InternalHideEntity(enumerator.Current.Value, userData);
				}
			}
		}

		public void HideLoadingEntities()
		{
			foreach (int loadingEntity in _loadingEntities)
			{
				_unloadingEntities.Add(loadingEntity);
			}
			_loadingEntities.Clear();
		}

		public Entity GetParentEntity(int entityId)
		{
			EntityNode value;
			if (!_entityNodes.TryGetValue(entityId, out value) || value == null)
			{
				Log.Error("Can not find entity '" + entityId + "'.");
				return null;
			}
			return value.Parent;
		}

		public Entity GetParentEntity(Entity entity)
		{
			if (entity == null)
			{
				Log.Error("Child entity is invalid.");
				return null;
			}
			return GetParentEntity(entity.Id);
		}

		public Entity[] GetChildEntities(int entityId)
		{
			EntityNode value;
			if (!_entityNodes.TryGetValue(entityId, out value) || value == null)
			{
				Log.Error("Can not find entity '" + entityId + "'.");
				return null;
			}
			return value.Children;
		}

		public Entity[] GetChildEntities(Entity entity)
		{
			if (entity == null)
			{
				Log.Error("Parent entity is invalid.");
				return null;
			}
			return GetChildEntities(entity.Id);
		}

		public void AttachEntity(int entityId, int targetEntityId, object userData = null)
		{
			EntityNode value;
			if (!_entityNodes.TryGetValue(entityId, out value) || value == null)
			{
				Log.Error("Can not find child entity '" + entityId + "'.");
				return;
			}
			if (value.Status >= EntityStatus.WillHide)
			{
				Log.Error("Can not attach entity when entity status is '" + value.Status.ToString() + "'.");
				return;
			}
			EntityNode value2;
			if (!_entityNodes.TryGetValue(targetEntityId, out value2) || value2 == null)
			{
				Log.Error("Can not find target entity '" + targetEntityId + "'.");
				return;
			}
			if (value2.Status >= EntityStatus.WillHide)
			{
				Log.Error("Can not attach entity when target entity status is '" + value2.Status.ToString() + "'.");
				return;
			}
			DetachEntity(value.Entity.Id, userData);
			value.Parent = value2.Entity;
			value2.AddChild(value.Entity);
			value2.Entity.OnAttached(value.Entity, userData);
			value.Entity.OnAttachTo(value2.Entity, userData);
		}

		public void AttachEntity(int entityId, Entity targetEntity, object userData = null)
		{
			if (targetEntity == null)
			{
				Log.Error("Parent entity is invalid.");
			}
			else
			{
				AttachEntity(entityId, targetEntity.Id, userData);
			}
		}

		public void AttachEntity(Entity entity, int targetEntityId, object userData = null)
		{
			if (entity == null)
			{
				Log.Error("Child entity is invalid.");
			}
			else
			{
				AttachEntity(entity.Id, targetEntityId, userData);
			}
		}

		public void AttachEntity(Entity entity, Entity targetEntity, object userData = null)
		{
			if (entity == null)
			{
				Log.Error("Child entity is invalid.");
			}
			else if (targetEntity == null)
			{
				Log.Error("Parent entity is invalid.");
			}
			else
			{
				AttachEntity(entity.Id, targetEntity.Id, userData);
			}
		}

		public void DetachEntity(int entityId, object userData = null)
		{
			EntityNode value;
			if (!_entityNodes.TryGetValue(entityId, out value) || value == null)
			{
				Log.Error("Can not find entity '" + entityId + "'.");
			}
			else if (!(value.Parent == null))
			{
				EntityNode value2;
				if (!_entityNodes.TryGetValue(value.Parent.Id, out value2) || value2 == null)
				{
					Log.Error("Can not find parent entity '" + value.Parent.Id + "'.");
					return;
				}
				value.Parent = null;
				value2.RemoveChild(value.Entity);
				value2.Entity.OnDetached(value.Entity, userData);
				value.Entity.OnDetachFrom(value2.Entity, userData);
			}
		}

		public void DetachEntity(Entity entity, object userData = null)
		{
			if (entity == null)
			{
				Log.Error("Child entity is invalid.");
			}
			else
			{
				DetachEntity(entity.Id, userData);
			}
		}

		public void DetachChildEntities(int entityId, object userData = null)
		{
			EntityNode value;
			if (!_entityNodes.TryGetValue(entityId, out value) || value == null)
			{
				Log.Error("Can not find entity '" + entityId + "'.");
				return;
			}
			Entity[] children = value.Children;
			foreach (Entity entity in children)
			{
				DetachEntity(entity.Id, userData);
			}
		}

		public void DetachChildEntities(Entity entity, object userData = null)
		{
			if (entity == null)
			{
				Log.Error("Parent entity is invalid.");
			}
			else
			{
				DetachChildEntities(entity.Id, userData);
			}
		}

		protected override void Awake()
		{
			Mod.Entity = this;
		}

		internal override void OnInit()
		{
			base.OnInit();
			_loadCallbacks = new AssetLoadCallbacks(OnLoadSuccess, OnLoadFailure, OnLoadUpdate, OnLoadDependencyAsset);
			for (int i = 0; i < _groupInfos.Length; i++)
			{
				EntityGroupInfo entityGroupInfo = _groupInfos[i];
				if (!string.IsNullOrEmpty(entityGroupInfo.Name))
				{
					GameObject gameObject = new GameObject("Entity Group - " + entityGroupInfo.Name);
					gameObject.transform.parent = base.transform;
					gameObject.hideFlags |= HideFlags.DontSave;
					gameObject.hideFlags |= HideFlags.NotEditable;
					EntityGroup group = gameObject.AddComponent<EntityGroup>();
					if (!AddEntityGroup(group, entityGroupInfo.Name))
					{
						Log.Warning("Add entity group '" + entityGroupInfo.Name + "' failure.");
					}
				}
			}
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("EntityMod.OnTick");
			while (_recycleQueue.Count > 0)
			{
				EntityNode value = _recycleQueue.First.Value;
				_recycleQueue.RemoveFirst();
				if (value.Entity.Group == null)
				{
					Log.Error("Entity group is invalid.");
					continue;
				}
				value.Status = EntityStatus.WillRecycle;
				value.Entity.OnRecycle();
				value.Status = EntityStatus.Recycled;
				value.Entity.Group.Recycle(value.Entity);
			}
			foreach (KeyValuePair<string, EntityGroup> entityGroup in _entityGroups)
			{
				entityGroup.Value.Tick(elapseSeconds, realElapseSeconds);
			}
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			HideLoadingEntities();
			HideLoadedEntities();
			_loadingEntities.Clear();
			_unloadingEntities.Clear();
			_recycleQueue.Clear();
			_entityGroups.Clear();
			_entityNodes.Clear();
			int childCount = base.transform.childCount;
			GameObject[] array = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				array[i] = base.transform.GetChild(i).gameObject;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (Application.isEditor)
				{
					UnityEngine.Object.DestroyImmediate(array[j]);
				}
				else
				{
					UnityEngine.Object.Destroy(array[j]);
				}
			}
		}

		private void InternalShowEntity(int entityId, string assetName, EntityGroup group, GameObject entityGo, bool isNew, float duration, object userData)
		{
			try
			{
				Entity entity = CreateEntity(entityGo, group);
				EntityNode entityNode = new EntityNode(entity);
				_entityNodes.Add(entityId, entityNode);
				entityNode.Status = EntityStatus.WillInit;
				entity.OnInit(entityId, assetName, group, isNew);
				entityNode.Status = EntityStatus.Inited;
				group.Add(entity);
				entityNode.Status = EntityStatus.WillShow;
				entity.OnShow(userData);
				entityNode.Status = EntityStatus.Showed;
				ShowSuccessEventArgs args = ShowSuccessEventArgs.Make(entity, duration, userData);
				Mod.Event.Fire(this, args);
			}
			catch (Exception ex)
			{
				ShowFailureEventArgs args2 = ShowFailureEventArgs.Make(entityId, assetName, group.Name, ex.ToString(), userData);
				Mod.Event.Fire(this, args2);
			}
		}

		private void InternalHideEntity(EntityNode entityNode, object userData)
		{
			Entity entity = entityNode.Entity;
			Entity[] children = entityNode.Children;
			foreach (Entity entity2 in children)
			{
				HideEntity(entity2.Id, userData);
			}
			DetachEntity(entity.Id, userData);
			entityNode.Status = EntityStatus.WillHide;
			entity.OnHide(userData);
			entityNode.Status = EntityStatus.Hidden;
			EntityGroup group = entity.Group;
			if (group == null)
			{
				Log.Error("Entity group is invalid.");
				return;
			}
			group.Remove(entity);
			if (!_entityNodes.Remove(entity.Id))
			{
				Log.Error("Entity info is unmanaged.");
				return;
			}
			_recycleQueue.AddLast(entityNode);
			HideCompleteEventArgs args = HideCompleteEventArgs.Make(entity.Id, entity.AssetName, group, userData);
			Mod.Event.Fire(this, args);
		}

		private bool AddEntityGroup(EntityGroup group, string groupName)
		{
			if (group == null)
			{
				Log.Error("Entity group handler is invalid.");
				return false;
			}
			if (string.IsNullOrEmpty(groupName))
			{
				Log.Error("Entity group name is invalid.");
				return false;
			}
			if (HasEntityGroup(groupName))
			{
				return false;
			}
			group.Init(groupName);
			_entityGroups[groupName] = group;
			return true;
		}

		private Entity CreateEntity(GameObject entityGo, EntityGroup group)
		{
			if (entityGo == null)
			{
				return null;
			}
			Transform transform = entityGo.transform;
			transform.SetParent(group.transform);
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			return entityGo.GetOrAddComponent<Entity>();
		}

		private void OnLoadSuccess(string assetName, object asset, float duration, object userData)
		{
			UserDataEx userDataEx = (UserDataEx)userData;
			_loadingEntities.Remove(userDataEx.EntityId);
			if (_unloadingEntities.Remove(userDataEx.EntityId))
			{
				Log.Info("Unload entity '" + userDataEx.EntityId + "' on loading success.");
				Mod.Resource.UnloadAsset((UnityEngine.Object)asset);
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate((UnityEngine.Object)asset);
			gameObject.hideFlags |= HideFlags.DontSave;
			EntityObject entityObject = new EntityObject(assetName, (GameObject)asset, gameObject);
			entityObject.OnSpawn();
			userDataEx.EntityGroup.Register(entityObject);
			InternalShowEntity(userDataEx.EntityId, assetName, userDataEx.EntityGroup, (GameObject)entityObject.Target, true, duration, userDataEx.UserData);
		}

		private void OnLoadFailure(string assetName, string message, object userData)
		{
			UserDataEx userDataEx = (UserDataEx)userData;
			_loadingEntities.Remove(userDataEx.EntityId);
			_unloadingEntities.Remove(userDataEx.EntityId);
			ShowFailureEventArgs args = ShowFailureEventArgs.Make(userDataEx.EntityId, assetName, userDataEx.EntityGroup.Name, message, userDataEx.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadUpdate(string assetName, float progress, object userData)
		{
			UserDataEx userDataEx = (UserDataEx)userData;
			ShowUpdateEventArgs args = ShowUpdateEventArgs.Make(userDataEx.EntityId, assetName, userDataEx.EntityGroup.Name, progress, userDataEx.UserData);
			Mod.Event.Fire(this, args);
		}

		private void OnLoadDependencyAsset(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
		{
			UserDataEx userDataEx = (UserDataEx)userData;
			ShowDependencyEventArgs args = ShowDependencyEventArgs.Make(userDataEx.EntityId, assetName, userDataEx.EntityGroup.Name, dependencyAssetName, loadedCount, totalCount, userDataEx.UserData);
			Mod.Event.Fire(this, args);
		}
	}
}
