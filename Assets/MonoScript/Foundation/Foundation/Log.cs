using System;
using System.Diagnostics;
using UnityEngine;

namespace Foundation
{
	public static class Log
	{
		public static bool Enabled { get; set; } = true;


		public static LogLevel Level { get; set; } = LogLevel.Info;


		public static bool IsAllowed(LogLevel logLevel)
		{
			if (Enabled)
			{
				return logLevel >= Level;
			}
			return false;
		}

		[Conditional("DEBUG")]
		public static void Debug(object message)
		{
			if (Enabled)
			{
				Info(message);
			}
		}

		[Conditional("DEBUG")]
		public static void Debug(object message, object context)
		{
			if (Enabled)
			{
				Info(message, context);
			}
		}

		public static void Assert(bool condition)
		{
			bool enabled = Enabled;
		}

		public static void Assert(bool condition, object message)
		{
			bool enabled = Enabled;
		}

		public static void Assert(bool condition, object message, object context)
		{
			bool enabled = Enabled;
		}

		public static void Info(object message)
		{
			if (IsAllowed(LogLevel.Info))
			{
				UnityEngine.Debug.Log(message);
			}
		}

		public static void Info(object message, object context)
		{
			if (IsAllowed(LogLevel.Info))
			{
				UnityEngine.Debug.Log(message, (UnityEngine.Object)context);
			}
		}

		public static void Warning(object message)
		{
			if (IsAllowed(LogLevel.Warning))
			{
				UnityEngine.Debug.LogWarning(message);
			}
		}

		public static void Warning(object message, object context)
		{
			if (IsAllowed(LogLevel.Warning))
			{
				UnityEngine.Debug.LogWarning(message, (UnityEngine.Object)context);
			}
		}

		public static void Error(object message)
		{
			if (IsAllowed(LogLevel.Error))
			{
				UnityEngine.Debug.LogError(message);
			}
		}

		public static void Error(object message, object context)
		{
			if (IsAllowed(LogLevel.Error))
			{
				UnityEngine.Debug.LogError(message, (UnityEngine.Object)context);
			}
		}

		public static void Exception(Exception exception)
		{
			if (Enabled)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}

		public static void Exception(Exception exception, object context)
		{
			if (Enabled)
			{
				UnityEngine.Debug.LogException(exception, (UnityEngine.Object)context);
			}
		}
	}
}
