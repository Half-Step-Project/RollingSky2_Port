using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation
{
	public static class ReflectionUtility
	{
		private static readonly Assembly[] _assemblies = AppDomain.CurrentDomain.GetAssemblies();

		private static readonly Dictionary<string, Type> _cachedTypes = new Dictionary<string, Type>();

		private static readonly Dictionary<KeyValuePair<object, string>, FieldInfo> _fieldInfoFromPaths = new Dictionary<KeyValuePair<object, string>, FieldInfo>(KeyValuePairComparer.Default);

		public static Assembly[] AllAssemblies
		{
			get
			{
				return _assemblies;
			}
		}

		public static Type[] AllTypes
		{
			get
			{
				List<Type> list = new List<Type>();
				for (int i = 0; i < _assemblies.Length; i++)
				{
					list.AddRange(_assemblies[i].GetTypes());
				}
				return list.ToArray();
			}
		}

		public static Type GetType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				Log.Warning("Type name is invalid.");
				return null;
			}
			Type value;
			if (_cachedTypes.TryGetValue(typeName, out value))
			{
				return value;
			}
			value = Type.GetType(typeName);
			if ((object)value != null)
			{
				_cachedTypes.Add(typeName, value);
				return value;
			}
			for (int i = 0; i < _assemblies.Length; i++)
			{
				Assembly assembly = _assemblies[i];
				value = Type.GetType(typeName + ", " + assembly.FullName);
				if ((object)value != null)
				{
					_cachedTypes.Add(typeName, value);
					return value;
				}
			}
			return null;
		}

		public static string GetFullName<T>(string name)
		{
			return GetFullName(typeof(T), name);
		}

		public static string GetFullName(Type type, string name)
		{
			if ((object)type == null)
			{
				Log.Warning("Type is invalid.");
				return null;
			}
			string fullName = type.FullName;
			if (!string.IsNullOrEmpty(name))
			{
				return fullName + "." + name;
			}
			return fullName;
		}

		public static string FieldNameForDisplay(string fieldName)
		{
			if (string.IsNullOrEmpty(fieldName))
			{
				return string.Empty;
			}
			string input = Regex.Replace(fieldName, "^(m_|_)", string.Empty);
			return Regex.Replace(input, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").TrimStart();
		}

		public static FieldInfo GetFieldInfoFromPath(object source, string path)
		{
			KeyValuePair<object, string> key = new KeyValuePair<object, string>(source, path);
			FieldInfo value;
			if (!_fieldInfoFromPaths.TryGetValue(key, out value))
			{
				string[] array = path.Split('.');
				Type type = source.GetType();
				foreach (string name in array)
				{
					value = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if ((object)value == null)
					{
						break;
					}
					type = value.FieldType;
				}
				_fieldInfoFromPaths.Add(key, value);
			}
			return value;
		}

		public static string GetFieldPath<T, TValue>(Expression<Func<T, TValue>> expr)
		{
			MemberExpression memberExpression = null;
			ExpressionType nodeType = expr.Body.NodeType;
			if ((uint)(nodeType - 10) <= 1u)
			{
				UnaryExpression unaryExpression = expr.Body as UnaryExpression;
				if (unaryExpression != null)
				{
					memberExpression = unaryExpression.Operand as MemberExpression;
				}
			}
			else
			{
				memberExpression = expr.Body as MemberExpression;
			}
			List<string> list = new List<string>();
			while (memberExpression != null)
			{
				list.Add(memberExpression.Member.Name);
				memberExpression = memberExpression.Expression as MemberExpression;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int num = list.Count - 1; num >= 0; num--)
			{
				stringBuilder.Append(list[num]);
				if (num > 0)
				{
					stringBuilder.Append('.');
				}
			}
			return stringBuilder.ToString();
		}

		public static object GetFieldValue(object source, string name)
		{
			Type type = source.GetType();
			while ((object)type != null)
			{
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if ((object)field != null)
				{
					return field.GetValue(source);
				}
				type = type.BaseType;
			}
			return null;
		}

		public static object GetFieldValueFromPath(object source, ref Type baseType, string path)
		{
			string[] array = path.Split('.');
			object obj = source;
			foreach (string name in array)
			{
				FieldInfo field = baseType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if ((object)field == null)
				{
					baseType = null;
					break;
				}
				baseType = field.FieldType;
				obj = GetFieldValue(obj, name);
			}
			if ((object)baseType != null)
			{
				return obj;
			}
			return null;
		}

		public static object GetParentObject(string path, object obj)
		{
			string[] array = path.Split('.');
			if (array.Length == 1)
			{
				return obj;
			}
			FieldInfo field = obj.GetType().GetField(array[0], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if ((object)field != null)
			{
				obj = field.GetValue(obj);
			}
			return GetParentObject(string.Join(".", array, 1, array.Length - 1), obj);
		}
	}
}
