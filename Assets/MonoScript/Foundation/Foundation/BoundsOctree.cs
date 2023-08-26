using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	public sealed class BoundsOctree<T> where T : class
	{
		private sealed class Node
		{
			private struct InternalObject
			{
				private readonly T _object;

				private readonly Bounds _bounds;

				public T Object
				{
					get
					{
						return _object;
					}
				}

				public Bounds Bounds
				{
					get
					{
						return _bounds;
					}
				}

				public InternalObject(T obj, Bounds bounds)
				{
					_object = obj;
					_bounds = bounds;
				}
			}

			private const int TotalObjectsAllowed = 8;

			private float _minSize;

			private float _size;

			private float _looseness;

			private Bounds _bounds;

			private readonly List<InternalObject> _objects = new List<InternalObject>();

			private Node[] _children;

			private Bounds[] _childBounds;

			public Vector3 Center
			{
				get
				{
					return _bounds.center;
				}
			}

			public float Size
			{
				get
				{
					return _size;
				}
			}

			public float ActualSize
			{
				get
				{
					return _bounds.size.x;
				}
			}

			public Bounds Bounds
			{
				get
				{
					return _bounds;
				}
			}

			public Node(float size, float minSize, float looseness, Vector3 center)
			{
				Reset(size, minSize, looseness, center);
			}

			public bool Add(T obj, Bounds bounds)
			{
				if (!Encapsulates(_bounds, bounds))
				{
					return false;
				}
				AddObject(new InternalObject(obj, bounds));
				return true;
			}

			public bool Remove(T obj)
			{
				bool flag = false;
				for (int i = 0; i < _objects.Count; i++)
				{
					if (_objects[i].Object.Equals(obj))
					{
						_objects.RemoveAt(i);
						flag = true;
						break;
					}
				}
				if (!flag && _children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						flag = _children[j].Remove(obj);
						if (flag)
						{
							break;
						}
					}
				}
				if (flag && _children != null)
				{
					MergeIfPossible();
				}
				return flag;
			}

			public bool IsColliding(ref Bounds bounds)
			{
				if (!_bounds.Intersects(bounds))
				{
					return false;
				}
				for (int i = 0; i < _objects.Count; i++)
				{
					if (_objects[i].Bounds.Intersects(bounds))
					{
						return true;
					}
				}
				if (_children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						if (_children[j].IsColliding(ref bounds))
						{
							return true;
						}
					}
				}
				return false;
			}

			public bool IsColliding(ref Ray ray, float maxDistance = float.PositiveInfinity)
			{
				float distance;
				if (!_bounds.IntersectRay(ray, out distance) || distance > maxDistance)
				{
					return false;
				}
				for (int i = 0; i < _objects.Count; i++)
				{
					if (_objects[i].Bounds.IntersectRay(ray, out distance) && distance <= maxDistance)
					{
						return true;
					}
				}
				if (_children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						if (_children[j].IsColliding(ref ray, maxDistance))
						{
							return true;
						}
					}
				}
				return false;
			}

			public void GetColliding(ref Bounds bounds, List<T> result)
			{
				if (!_bounds.Intersects(bounds))
				{
					return;
				}
				for (int i = 0; i < _objects.Count; i++)
				{
					if (_objects[i].Bounds.Intersects(bounds))
					{
						result.Add(_objects[i].Object);
					}
				}
				if (_children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						_children[j].GetColliding(ref bounds, result);
					}
				}
			}

			public void GetColliding(ref Ray ray, List<T> result, float maxDistance = float.PositiveInfinity)
			{
				float distance;
				if (!_bounds.IntersectRay(ray, out distance) || distance > maxDistance)
				{
					return;
				}
				for (int i = 0; i < _objects.Count; i++)
				{
					if (_objects[i].Bounds.IntersectRay(ray, out distance) && distance <= maxDistance)
					{
						result.Add(_objects[i].Object);
					}
				}
				if (_children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						_children[j].GetColliding(ref ray, result, maxDistance);
					}
				}
			}

			public void SetChildren(Node[] children)
			{
				if (children == null)
				{
					Log.Warning("children is invalid");
				}
				else if (children.Length != 8)
				{
					Log.Error(string.Format("Child octree array must be length {0}. Was length: {1}", 8, children.Length));
				}
				else
				{
					_children = children;
				}
			}

			public void DrawAllBounds(float depth = 0f)
			{
				float num = depth / 7f;
				Color color = Gizmos.color;
				Gizmos.color = new Color(num, 0f, 1f - num);
				Gizmos.DrawWireCube(_bounds.center, _bounds.size);
				if (_children != null)
				{
					depth += 1f;
					for (int i = 0; i < _children.Length; i++)
					{
						_children[i].DrawAllBounds(depth);
					}
				}
				Gizmos.color = color;
			}

			public void DrawAllObjects()
			{
				float num = _size / 20f;
				Color color = Gizmos.color;
				Gizmos.color = new Color(0f, 1f - num, num, 0.25f);
				for (int i = 0; i < _objects.Count; i++)
				{
					Bounds bounds = _objects[i].Bounds;
					Gizmos.DrawCube(bounds.center, bounds.size);
				}
				if (_children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						_children[j].DrawAllObjects();
					}
				}
				Gizmos.color = color;
			}

			public Node ShrinkIfPossible(float minSize)
			{
				if (_size < 2f * minSize)
				{
					return this;
				}
				if (_objects.Count == 0 && (_children == null || _children.Length == 0))
				{
					return this;
				}
				int num = -1;
				for (int i = 0; i < _objects.Count; i++)
				{
					InternalObject internalObject = _objects[i];
					int num2 = BestFitChild(internalObject.Bounds);
					if (i == 0 || num2 == num)
					{
						if (Encapsulates(_childBounds[num2], internalObject.Bounds))
						{
							if (num < 0)
							{
								num = num2;
							}
							continue;
						}
						return this;
					}
					return this;
				}
				if (_children != null)
				{
					bool flag = false;
					for (int j = 0; j < _children.Length; j++)
					{
						if (_children[j].HasAnyObjects())
						{
							if (flag)
							{
								return this;
							}
							if (num >= 0 && num != j)
							{
								return this;
							}
							flag = true;
							num = j;
						}
					}
				}
				if (_children == null)
				{
					Reset(_size / 2f, minSize, _looseness, _childBounds[num].center);
					return this;
				}
				if (num == -1)
				{
					return this;
				}
				return _children[num];
			}

			public int GetTotalObjects(int startingNum = 0)
			{
				int num = startingNum + _objects.Count;
				if (_children != null)
				{
					for (int i = 0; i < _children.Length; i++)
					{
						num += _children[i].GetTotalObjects();
					}
				}
				return num;
			}

			public bool HasAnyObjects()
			{
				if (_objects.Count > 0)
				{
					return true;
				}
				if (_children != null)
				{
					for (int i = 0; i < _children.Length; i++)
					{
						if (_children[i].HasAnyObjects())
						{
							return true;
						}
					}
				}
				return false;
			}

			private static bool Encapsulates(Bounds outerBounds, Bounds innerBounds)
			{
				if (outerBounds.Contains(innerBounds.min))
				{
					return outerBounds.Contains(innerBounds.max);
				}
				return false;
			}

			private void Reset(float size, float minSize, float looseness, Vector3 center)
			{
				float num = looseness * size;
				_minSize = minSize;
				_size = size;
				_looseness = looseness;
				_bounds = new Bounds(center, new Vector3(num, num, num));
				float num2 = size / 4f;
				float num3 = size / 2f * _looseness;
				Vector3 size2 = new Vector3(num3, num3, num3);
				_childBounds = new Bounds[8]
				{
					new Bounds(center + new Vector3(0f - num2, num2, 0f - num2), size2),
					new Bounds(center + new Vector3(num2, num2, 0f - num2), size2),
					new Bounds(center + new Vector3(0f - num2, num2, num2), size2),
					new Bounds(center + new Vector3(num2, num2, num2), size2),
					new Bounds(center + new Vector3(0f - num2, 0f - num2, 0f - num2), size2),
					new Bounds(center + new Vector3(num2, 0f - num2, 0f - num2), size2),
					new Bounds(center + new Vector3(0f - num2, 0f - num2, num2), size2),
					new Bounds(center + new Vector3(num2, 0f - num2, num2), size2)
				};
			}

			private void AddObject(InternalObject internalObject)
			{
				if (_objects.Count < 8 || _size / 2f < _minSize)
				{
					_objects.Add(internalObject);
					return;
				}
				int num2;
				if (_children == null)
				{
					MakeChildNode();
					if (_children == null)
					{
						Log.Error("Child creation failed for an unknown reason. Early exit.");
						return;
					}
					for (int num = _objects.Count - 1; num >= 0; num--)
					{
						InternalObject internalObject2 = _objects[num];
						num2 = BestFitChild(internalObject2.Bounds);
						if (Encapsulates(_children[num2]._bounds, internalObject2.Bounds))
						{
							_objects.RemoveAt(num);
							_children[num2].AddObject(internalObject2);
						}
					}
				}
				num2 = BestFitChild(internalObject.Bounds);
				if (Encapsulates(_children[num2]._bounds, internalObject.Bounds))
				{
					_children[num2].AddObject(internalObject);
				}
				else
				{
					_objects.Add(internalObject);
				}
			}

			private void MakeChildNode()
			{
				float num = _size / 4f;
				float size = _size / 2f;
				_children = new Node[8]
				{
					new Node(size, _minSize, _looseness, Center + new Vector3(0f - num, num, 0f - num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(num, num, 0f - num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(0f - num, num, num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(num, num, num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(0f - num, 0f - num, 0f - num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(num, 0f - num, 0f - num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(0f - num, 0f - num, num)),
					new Node(size, _minSize, _looseness, Center + new Vector3(num, 0f - num, num))
				};
			}

			private void MergeIfPossible()
			{
				if (_children == null)
				{
					return;
				}
				int num = _objects.Count;
				for (int i = 0; i < _children.Length; i++)
				{
					Node node = _children[i];
					Node[] child = node._children;
					num += node._objects.Count;
				}
				if (num > 8)
				{
					return;
				}
				for (int j = 0; j < _children.Length; j++)
				{
					Node node2 = _children[j];
					int count = node2._objects.Count;
					for (int num2 = count - 1; num2 >= 0; num2--)
					{
						_objects.Add(node2._objects[num2]);
					}
				}
				_children = null;
			}

			private int BestFitChild(Bounds objBounds)
			{
				return ((!(objBounds.center.x <= Center.x)) ? 1 : 0) + ((!(objBounds.center.y >= Center.y)) ? 4 : 0) + ((!(objBounds.center.z <= Center.z)) ? 2 : 0);
			}
		}

		private const int MaxGlowCount = 20;

		private const int TotalChildNode = 8;

		private Node _root;

		private int _count;

		private readonly float _looseness;

		private readonly float _initialSize;

		private readonly float _minSize;

		public int Count
		{
			get
			{
				return _count;
			}
		}

		public Bounds MaxBounds
		{
			get
			{
				return _root.Bounds;
			}
		}

		public BoundsOctree(float initialSize, Vector3 initialPosition, float minSize, float looseness)
		{
			if (minSize > initialSize)
			{
				Log.Warning(string.Format("Minimum node size must be at least as big as the initial world size. Was: " + minSize + " Adjusted to: " + initialSize));
				minSize = initialSize;
			}
			_count = 0;
			_initialSize = initialSize;
			_minSize = minSize;
			_looseness = Mathf.Clamp(looseness, 1f, 2f);
			_root = new Node(_initialSize, _minSize, _looseness, initialPosition);
		}

		public void Add(T obj, Bounds bounds)
		{
			int num = 0;
			while (!_root.Add(obj, bounds))
			{
				Grow(bounds.center - _root.Center);
				if (++num > 20)
				{
					Log.Error(string.Format("Aborted Add operation as it seemed to be going on forever (" + (num - 1) + ") attempts at growing the octree."));
					return;
				}
			}
			_count++;
		}

		public bool Remove(T obj)
		{
			bool flag = _root.Remove(obj);
			if (flag)
			{
				_count--;
				_root = _root.ShrinkIfPossible(_initialSize);
			}
			return flag;
		}

		public bool IsColliding(Bounds bounds)
		{
			AddCollisionCheck(bounds);
			return _root.IsColliding(ref bounds);
		}

		public bool IsColliding(Ray ray, float maxDistance)
		{
			AddCollisionCheck(ray);
			return _root.IsColliding(ref ray, maxDistance);
		}

		public void GetColliding(List<T> colliding, Bounds bounds)
		{
			AddCollisionCheck(bounds);
			_root.GetColliding(ref bounds, colliding);
		}

		public void GetColliding(List<T> colliding, Ray ray, float maxDistance = float.PositiveInfinity)
		{
			AddCollisionCheck(ray);
			_root.GetColliding(ref ray, colliding, maxDistance);
		}

		public void DrawAllBounds()
		{
			_root.DrawAllBounds();
		}

		public void DrawAllObjects()
		{
			_root.DrawAllObjects();
		}

		public void DrawCollisionChecks()
		{
		}

		private static int GetRootPosIndex(int xdir, int ydir, int zdir)
		{
			int num = ((xdir > 0) ? 1 : 0);
			if (ydir < 0)
			{
				num += 4;
			}
			if (zdir > 0)
			{
				num += 2;
			}
			return num;
		}

		private void AddCollisionCheck(Bounds bounds)
		{
		}

		private void AddCollisionCheck(Ray ray)
		{
		}

		private void Grow(Vector3 direction)
		{
			int num = ((direction.x >= 0f) ? 1 : (-1));
			int num2 = ((direction.y >= 0f) ? 1 : (-1));
			int num3 = ((direction.z >= 0f) ? 1 : (-1));
			Node root = _root;
			float num4 = _root.Size / 2f;
			float size = _root.Size * 2f;
			Vector3 vector = _root.Center + new Vector3((float)num * num4, (float)num2 * num4, (float)num3 * num4);
			_root = new Node(size, _minSize, _looseness, vector);
			if (!root.HasAnyObjects())
			{
				return;
			}
			int rootPosIndex = GetRootPosIndex(num, num2, num3);
			Node[] array = new Node[8];
			for (int i = 0; i < array.Length; i++)
			{
				if (i == rootPosIndex)
				{
					array[i] = root;
					continue;
				}
				num = ((i % 2 != 0) ? 1 : (-1));
				num2 = ((i <= 3) ? 1 : (-1));
				num3 = ((i >= 2 && (i <= 3 || i >= 6)) ? 1 : (-1));
				array[i] = new Node(_root.Size, _minSize, _looseness, vector + new Vector3((float)num * num4, (float)num2 * num4, (float)num3 * num4));
			}
			_root.SetChildren(array);
		}
	}
}
