using System;
using UnityEngine;

namespace Foundation
{
	public static class UnityExtension
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T val = gameObject.GetComponent<T>();
			if ((UnityEngine.Object)val == (UnityEngine.Object)null)
			{
				val = gameObject.AddComponent<T>();
			}
			return val;
		}

		public static Component GetOrAddComponent(this GameObject gameObject, Type type)
		{
			Component component = gameObject.GetComponent(type);
			if (component == null)
			{
				component = gameObject.AddComponent(type);
			}
			return component;
		}

		public static bool InScene(this GameObject gameObject)
		{
			return gameObject.scene.name != null;
		}

		public static Vector2 ToVector2(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.z);
		}

		public static Vector3 ToVector3(this Vector2 vector)
		{
			return new Vector3(vector.x, 0f, vector.y);
		}

		public static Vector3 ToVector3(this Vector2 vector, float y)
		{
			return new Vector3(vector.x, y, vector.y);
		}

		public static void SetPositionX(this Transform transform, float x)
		{
			Vector3 position = transform.position;
			position.x = x;
			transform.position = position;
		}

		public static void SetPositionY(this Transform transform, float y)
		{
			Vector3 position = transform.position;
			position.y = y;
			transform.position = position;
		}

		public static void SetPositionZ(this Transform transform, float z)
		{
			Vector3 position = transform.position;
			position.z = z;
			transform.position = position;
		}

		public static void AddPositionX(this Transform transform, float x)
		{
			Vector3 position = transform.position;
			position.x += x;
			transform.position = position;
		}

		public static void AddPositionY(this Transform transform, float y)
		{
			Vector3 position = transform.position;
			position.y += y;
			transform.position = position;
		}

		public static void AddPositionZ(this Transform transform, float z)
		{
			Vector3 position = transform.position;
			position.z += z;
			transform.position = position;
		}

		public static void SetLocalPositionX(this Transform transform, float x)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.x = x;
			transform.localPosition = localPosition;
		}

		public static void SetLocalPositionY(this Transform transform, float y)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.y = y;
			transform.localPosition = localPosition;
		}

		public static void SetLocalPositionZ(this Transform transform, float z)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.z = z;
			transform.localPosition = localPosition;
		}

		public static void AddLocalPositionX(this Transform transform, float x)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.x += x;
			transform.localPosition = localPosition;
		}

		public static void AddLocalPositionY(this Transform transform, float y)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.y += y;
			transform.localPosition = localPosition;
		}

		public static void AddLocalPositionZ(this Transform transform, float z)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.z += z;
			transform.localPosition = localPosition;
		}

		public static void SetLocalScaleX(this Transform transform, float x)
		{
			Vector3 localScale = transform.localScale;
			localScale.x = x;
			transform.localScale = localScale;
		}

		public static void SetLocalScaleY(this Transform transform, float y)
		{
			Vector3 localScale = transform.localScale;
			localScale.y = y;
			transform.localScale = localScale;
		}

		public static void SetLocalScaleZ(this Transform transform, float z)
		{
			Vector3 localScale = transform.localScale;
			localScale.z = z;
			transform.localScale = localScale;
		}

		public static void AddLocalScaleX(this Transform transform, float x)
		{
			Vector3 localScale = transform.localScale;
			localScale.x += x;
			transform.localScale = localScale;
		}

		public static void AddLocalScaleY(this Transform transform, float y)
		{
			Vector3 localScale = transform.localScale;
			localScale.y += y;
			transform.localScale = localScale;
		}

		public static void AddLocalScaleZ(this Transform transform, float z)
		{
			Vector3 localScale = transform.localScale;
			localScale.z += z;
			transform.localScale = localScale;
		}

		public static void LookAt2D(this Transform transform, Vector2 lookAtPoint)
		{
			Vector3 vector = lookAtPoint.ToVector3() - transform.position;
			vector.y = 0f;
			if (vector.magnitude > 0f)
			{
				transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
			}
		}

		public static void SetLayerRecursively(this Transform transform, int layer)
		{
			Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layer;
			}
		}
	}
}
