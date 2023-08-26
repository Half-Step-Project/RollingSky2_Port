using System;

namespace Foundation
{
	public interface IObjectPool
	{
		string Name { get; }

		Type ObjectType { get; }

		int Count { get; }

		int CanUnloadCount { get; }

		PoolObjectInfo[] ObjectInfos { get; }

		bool CanSpawn(string name);

		bool CanSpawn(object target);

		void Recycle(object target);

		void SetLock(object target, bool locked);

		void Unload();

		void Destroy();
	}
}
