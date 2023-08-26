using System.IO;

namespace Foundation
{
	public static class PathUtility
	{
		public static string GetRegularPath(string path)
		{
			if (path == null)
			{
				Log.Warning("The path is invalid.");
				return null;
			}
			return path.Replace('\\', '/');
		}

		public static string GetCombinePath(string path, string path2)
		{
			if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(path2))
			{
				Log.Warning("The path is invalid.");
				return null;
			}
			return GetRegularPath(Path.Combine(path, path2));
		}

		public static string GetCombinePath(params string[] path)
		{
			if (path == null || path.Length < 1)
			{
				Log.Warning("The path is invalid.");
				return null;
			}
			string text = path[0];
			for (int i = 1; i < path.Length; i++)
			{
				text = Path.Combine(text, path[i]);
			}
			return GetRegularPath(text);
		}

		public static string GetRemotePath(string path)
		{
			if (!path.Contains("://"))
			{
				return ("file:///" + path).Replace("file:////", "file:///");
			}
			return path;
		}

		public static string GetRemotePath(params string[] path)
		{
			string combinePath = GetCombinePath(path);
			if (combinePath == null)
			{
				return null;
			}
			if (!combinePath.Contains("://"))
			{
				return ("file:///" + combinePath).Replace("file:////", "file:///");
			}
			return combinePath;
		}

		public static string GetRemoteResourcePath(string resourceUrlPrefix, string resourcePath)
		{
			return resourceUrlPrefix + resourcePath;
		}

		public static string GetResourceNameWithSuffix(string resourceName)
		{
			if (string.IsNullOrEmpty(resourceName))
			{
				Log.Warning("Resource name is invalid.");
				return null;
			}
			return resourceName + ".dat";
		}

		public static string GetResourceNameWithCrcAndSuffix(string resourceName, uint hashCode)
		{
			if (string.IsNullOrEmpty(resourceName))
			{
				Log.Warning("Resource name is invalid.");
				return null;
			}
			return string.Format("{0}.{1:x8}.dat", resourceName, hashCode);
		}

		public static bool RemoveEmptyDirectory(string directory)
		{
			if (string.IsNullOrEmpty(directory))
			{
				Log.Warning("Directory name is invalid.");
				return false;
			}
			try
			{
				if (!Directory.Exists(directory))
				{
					return false;
				}
				string[] directories = Directory.GetDirectories(directory, "*");
				int num = directories.Length;
				foreach (string directory2 in directories)
				{
					if (RemoveEmptyDirectory(directory2))
					{
						num--;
					}
				}
				if (num > 0)
				{
					return false;
				}
				if (Directory.GetFiles(directory, "*").Length != 0)
				{
					return false;
				}
				Directory.Delete(directory);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
