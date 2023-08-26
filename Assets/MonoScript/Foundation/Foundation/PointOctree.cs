using System.Collections.Generic;
using UnityEngine;

namespace Foundation
{
	public class PointOctree<T> where T : class
	{
		private sealed class Node
		{
			private struct InternalObject
			{
				private readonly T _object;

				private readonly Vector3 _position;

				public T Object
				{
					get
					{
						return _object;
					}
				}

				public Vector3 Position
				{
					get
					{
						return _position;
					}
				}

				public InternalObject(T obj, Vector3 position)
				{
					_object = obj;
					_position = position;
				}
			}

			private const int TotalObjectsAllowed = 8;

			private float _minSize;

			private Bounds _bounds;

			private Bounds[] _childBounds;

			private readonly List<InternalObject> _objects = new List<InternalObject>();

			private Node[] _children;

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
					return _bounds.size.x;
				}
			}

			public Node(float size, float minSize, Vector3 center)
			{
				Reset(size, minSize, center);
			}

			public static float DistanceToRay(Ray ray, Vector3 point)
			{
				return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
			}

			public bool Add(T obj, Vector3 position)
			{
				if (!Encapsulates(_bounds, position))
				{
					return false;
				}
				AddObject(new InternalObject(obj, position));
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

			public void GetNearby(ref Ray ray, ref float maxDistance, List<T> result)
			{
				Bounds bounds = _bounds;
				bounds.Expand(maxDistance * 2f);
				if (!bounds.IntersectRay(ray))
				{
					return;
				}
				for (int i = 0; i < _objects.Count; i++)
				{
					if (DistanceToRay(ray, _objects[i].Position) <= maxDistance)
					{
						result.Add(_objects[i].Object);
					}
				}
				if (_children != null)
				{
					for (int j = 0; j < _children.Length; j++)
					{
						_children[j].GetNearby(ref ray, ref maxDistance, result);
					}
				}
			}

			public void DrawAllBounds(float depth = 0f)
			{
				float num = depth / 7f;
				Color color = Gizmos.color;
				Gizmos.color = new Color(num, 0f, 1f - num);
				Gizmos.DrawWireCube(Center, _bounds.size);
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
				float num = Size / 20f;
				Color color = Gizmos.color;
				Gizmos.color = new Color(0f, 1f - num, num, 0.25f);
				for (int i = 0; i < _objects.Count; i++)
				{
					Gizmos.DrawIcon(_objects[i].Position, "marker.tif", true);
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
				if (Size < 2f * minSize)
				{
					return this;
				}
				if (_objects.Count == 0 && _children.Length == 0)
				{
					return this;
				}
				int num = -1;
				for (int i = 0; i < _objects.Count; i++)
				{
					int num2 = BestFitChild(_objects[i].Position);
					if (i == 0 || num2 == num)
					{
						if (num < 0)
						{
							num = num2;
						}
						continue;
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
					Reset(Size / 2f, minSize, _childBounds[num].center);
					return this;
				}
				return _children[num];
			}

			public void SetChildren(Node[] children)
			{
				if (children == null)
				{
					Log.Warning("children is invalid");
				}
				else if (children.Length != 8)
				{
					Log.Error("children amount is invalid");
				}
				else
				{
					_children = children;
				}
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

			private static bool Encapsulates(Bounds outerBounds, Vector3 point)
			{
				return outerBounds.Contains(point);
			}

			private void Reset(float size, float minSize, Vector3 center)
			{
				_minSize = minSize;
				_bounds = new Bounds(center, new Vector3(size, size, size));
				float num = size / 4f;
				float num2 = size / 2f;
				Vector3 size2 = new Vector3(num2, num2, num2);
				_childBounds = new Bounds[8]
				{
					new Bounds(center + new Vector3(0f - num, num, 0f - num), size2),
					new Bounds(center + new Vector3(num, num, 0f - num), size2),
					new Bounds(center + new Vector3(0f - num, num, num), size2),
					new Bounds(center + new Vector3(num, num, num), size2),
					new Bounds(center + new Vector3(0f - num, 0f - num, 0f - num), size2),
					new Bounds(center + new Vector3(num, 0f - num, 0f - num), size2),
					new Bounds(center + new Vector3(0f - num, 0f - num, num), size2),
					new Bounds(center + new Vector3(num, 0f - num, num), size2)
				};
			}

			private void AddObject(InternalObject internalObject)
			{
				if (_objects.Count < 8 || Size / 2f < _minSize)
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
						_objects.RemoveAt(num);
						num2 = BestFitChild(internalObject2.Position);
						_children[num2].AddObject(internalObject2);
					}
				}
				num2 = BestFitChild(internalObject.Position);
				_children[num2].AddObject(internalObject);
			}

			private void MakeChildNode()
			{
				float num = Size / 4f;
				float size = Size / 2f;
				_children = new Node[8]
				{
					new Node(size, _minSize, Center + new Vector3(0f - num, num, 0f - num)),
					new Node(size, _minSize, Center + new Vector3(num, num, 0f - num)),
					new Node(size, _minSize, Center + new Vector3(0f - num, num, num)),
					new Node(size, _minSize, Center + new Vector3(num, num, num)),
					new Node(size, _minSize, Center + new Vector3(0f - num, 0f - num, 0f - num)),
					new Node(size, _minSize, Center + new Vector3(num, 0f - num, 0f - num)),
					new Node(size, _minSize, Center + new Vector3(0f - num, 0f - num, num)),
					new Node(size, _minSize, Center + new Vector3(num, 0f - num, num))
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
					if (node._children != null)
					{
						break;
					}
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

			private int BestFitChild(Vector3 position)
			{
				return ((!(position.x <= _bounds.center.x)) ? 1 : 0) + ((!(position.y >= _bounds.center.y)) ? 4 : 0) + ((!(position.z <= _bounds.center.z)) ? 2 : 0);
			}

			private bool HasAnyObjects()
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
		}

		private const int MaxGlowCount = 20;

		private const int TotalChildNode = 8;

		private readonly float _initialSize;

		private readonly float _minSize;

		private int _count;

		private Node _root;

		public int Count
		{
			get
			{
				return _count;
			}
		}

		public PointOctree(float initialSize, Vector3 initialPosition, float minNodeSize)
		{
			if (minNodeSize > initialSize)
			{
				Log.Warning(string.Format("Minimum node size must be at least as big as the initial world size. Was: {0} Adjusted to: {1}.", minNodeSize, initialSize));
				minNodeSize = initialSize;
			}
			_count = 0;
			_initialSize = initialSize;
			_minSize = minNodeSize;
			_root = new Node(_initialSize, _minSize, initialPosition);
		}

		public void Add(T obj, Vector3 position)
		{
			int num = 0;
			while (!_root.Add(obj, position))
			{
				Grow(position - _root.Center);
				if (++num > 20)
				{
					Log.Error(string.Format("Aborted Add operation as it seemed to be going on forever ({0}) attempts at growing the octree.", num - 1));
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

		public T[] GetNearby(Ray ray, float maxDistance)
		{
			List<T> list = new List<T>();
			_root.GetNearby(ref ray, ref maxDistance, list);
			return list.ToArray();
		}

		public void DrawAllBounds()
		{
			_root.DrawAllBounds();
		}

		public void DrawAllObjects()
		{
			_root.DrawAllObjects();
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
			_root = new Node(size, _minSize, vector);
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
				array[i] = new Node(_root.Size, _minSize, vector + new Vector3((float)num * num4, (float)num2 * num4, (float)num3 * num4));
			}
			_root.SetChildren(array);
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
	}
}
