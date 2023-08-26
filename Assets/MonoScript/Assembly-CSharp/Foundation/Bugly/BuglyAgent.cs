using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RS2;
using UnityEngine;

namespace Foundation.Bugly
{
	public static class BuglyAgent
	{
		private const string Version = "1.5.3";

		private const int TypeU3DCrash = 4;

		private const int CrashReporterType = 1;

		private const int ExceptionTypeUncaught = 1;

		private const int ExceptionTypeCaught = 2;

		private static bool _isInitialized;

		private static bool _debugMode;

		private static LogSeverity _autoReportLogLevel;

		private static int _crashReproterCustomizedLogLevel;

		private static bool _uncaughtAutoReportOnce;

		public static void InitSDK()
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			switch (Mod.Core.LogLevel)
			{
			case LogLevel.Info:
				_autoReportLogLevel = LogSeverity.LogDebug;
				_crashReproterCustomizedLogLevel = 3;
				break;
			case LogLevel.Warning:
				_autoReportLogLevel = LogSeverity.LogWarning;
				_crashReproterCustomizedLogLevel = 2;
				break;
			case LogLevel.Error:
				_autoReportLogLevel = LogSeverity.LogAssert;
				_crashReproterCustomizedLogLevel = 1;
				break;
			}
			if (_isInitialized)
			{
				_DebugLog(null, "BuglyAgent has already been initialized.");
				return;
			}
			if (string.IsNullOrEmpty(empty))
			{
				_DebugLog("Error", "AppID is null or empty.");
				return;
			}
			_debugMode = false;
			_EnableDebugMode(_debugMode);
			_InitBuglyAgent(empty, empty2, Application.version, SystemInfo.deviceUniqueIdentifier, 100L);
			_RegisterExceptionHandler();
			_SetUnityVersion();
			_isInitialized = true;
		}

		public static void ReportException(Exception e, string message)
		{
			if (_isInitialized)
			{
				_HandleException(e, message, false);
			}
		}

		public static void ReportException(string name, string message, string stackTrace)
		{
			if (_isInitialized)
			{
				_HandleException(LogSeverity.LogException, name, message, stackTrace, false);
			}
		}

		public static void SetUserId(string userId)
		{
			if (_isInitialized)
			{
				_SetUserInfo(userId);
			}
		}

		public static void SetScene(int sceneId)
		{
			if (_isInitialized)
			{
				_SetCurrentScene(sceneId);
			}
		}

		public static void AddSceneData(string key, string value)
		{
			if (_isInitialized)
			{
				_AddPairInScene(key, value);
			}
		}

		public static void _ReportExInfo()
		{
			if (_isInitialized && Mod.Procedure.Current is LevelProcedure)
			{
				GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
				InsideGameDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
				if (dataModule != null)
				{
					_AddPairInScene("levelID", dataModule.CurLevelId.ToString());
				}
				if (dataModule2 != null)
				{
					_AddPairInScene("progress", dataModule2.ProgressPercentage.ToString());
				}
			}
		}

		private static void _DebugLog(string tag, string format, params object[] args)
		{
			if (_debugMode && !string.IsNullOrEmpty(format))
			{
				Console.WriteLine("[BuglyAgent] <Debug> - {0} : {1}", tag, string.Format(format, args));
			}
		}

		private static void _PrintLog(LogSeverity level, string format, params object[] args)
		{
			if (!string.IsNullOrEmpty(format))
			{
				_LogRecord(level, string.Format(format, args));
			}
		}

		private static int _LogSeverityToInt(LogSeverity logLevel)
		{
			switch (logLevel)
			{
			case LogSeverity.Log:
				return 5;
			case LogSeverity.LogDebug:
				return 4;
			case LogSeverity.LogInfo:
				return 3;
			case LogSeverity.LogWarning:
			case LogSeverity.LogAssert:
				return 2;
			case LogSeverity.LogError:
			case LogSeverity.LogException:
				return 1;
			default:
				return 0;
			}
		}

		private static void _RegisterExceptionHandler()
		{
			try
			{
				Application.logMessageReceived -= _OnLogCallbackHandler;
				Application.logMessageReceived += _OnLogCallbackHandler;
				AppDomain.CurrentDomain.UnhandledException -= _OnUncaughtExceptionHandler;
				AppDomain.CurrentDomain.UnhandledException += _OnUncaughtExceptionHandler;
			}
			catch
			{
			}
		}

		private static void _OnLogCallbackHandler(string condition, string stackTrace, LogType type)
		{
			if (_isInitialized && !_uncaughtAutoReportOnce && (string.IsNullOrEmpty(condition) || !condition.Contains("[BuglyAgent] <Log>")))
			{
				LogSeverity logLevel = LogSeverity.Log;
				switch (type)
				{
				case LogType.Exception:
					logLevel = LogSeverity.LogException;
					break;
				case LogType.Error:
					logLevel = LogSeverity.LogError;
					break;
				case LogType.Assert:
					logLevel = LogSeverity.LogAssert;
					break;
				case LogType.Warning:
					logLevel = LogSeverity.LogWarning;
					break;
				case LogType.Log:
					logLevel = LogSeverity.LogDebug;
					break;
				}
				_HandleException(logLevel, null, condition, stackTrace, true);
			}
		}

		private static void _OnUncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			if (args == null || args.ExceptionObject == null)
			{
				return;
			}
			try
			{
				if (args.ExceptionObject.GetType() != typeof(Exception))
				{
					return;
				}
			}
			catch
			{
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.LogError("BuglyAgent: Failed to report uncaught exception");
				}
				return;
			}
			if (_isInitialized && !_uncaughtAutoReportOnce)
			{
				_HandleException((Exception)args.ExceptionObject, null, true);
			}
		}

		private static void _HandleException(Exception e, string message, bool uncaught)
		{
			if (!_isInitialized || e == null)
			{
				return;
			}
			string name = e.GetType().Name;
			string text = e.Message;
			if (!string.IsNullOrEmpty(message))
			{
				text = string.Format("{0}{1}***{2}", text, Environment.NewLine, message);
			}
			StringBuilder stringBuilder = new StringBuilder("");
			StackTrace stackTrace = new StackTrace(e, true);
			int frameCount = stackTrace.FrameCount;
			for (int i = 0; i < frameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				Type declaringType = frame.GetMethod().DeclaringType;
				if (declaringType != null)
				{
					stringBuilder.AppendFormat("{0}.{1}", declaringType.Name, frame.GetMethod().Name);
				}
				ParameterInfo[] parameters = frame.GetMethod().GetParameters();
				if (parameters.Length == 0)
				{
					stringBuilder.Append(" () ");
				}
				else
				{
					stringBuilder.Append(" (");
					int num = parameters.Length;
					for (int j = 0; j < num; j++)
					{
						ParameterInfo parameterInfo = parameters[j];
						stringBuilder.AppendFormat("{0} {1}", parameterInfo.ParameterType.Name, parameterInfo.Name);
						if (j != num - 1)
						{
							stringBuilder.Append(", ");
						}
					}
					stringBuilder.Append(") ");
				}
				string fileName = frame.GetFileName();
				if (!string.IsNullOrEmpty(fileName) && !fileName.ToLower().Equals("unknown"))
				{
					fileName = fileName.Replace("\\", "/");
					int num2 = fileName.ToLower().IndexOf("/assets/", StringComparison.Ordinal);
					if (num2 < 0)
					{
						num2 = fileName.ToLower().IndexOf("assets/", StringComparison.Ordinal);
					}
					if (num2 > 0)
					{
						fileName = fileName.Substring(num2);
					}
					stringBuilder.AppendFormat("(at {0}:{1})", fileName, frame.GetFileLineNumber());
				}
				stringBuilder.AppendLine();
			}
			_ReportException(uncaught, name, text, stringBuilder.ToString());
		}

		private static void _HandleException(LogSeverity logLevel, string name, string message, string stackTrace, bool uncaught)
		{
			if (!_isInitialized || logLevel == LogSeverity.Log || (uncaught && logLevel < _autoReportLogLevel))
			{
				return;
			}
			string text = null;
			string text2 = null;
			if (!string.IsNullOrEmpty(message))
			{
				try
				{
					if (LogSeverity.LogException == logLevel && message.Contains("Exception"))
					{
						Match match = new Regex("^(?<errorType>\\S+):\\s*(?<errorMessage>.*)", RegexOptions.Singleline).Match(message);
						if (match.Success)
						{
							text = match.Groups["errorType"].Value.Trim();
							text2 = match.Groups["errorMessage"].Value.Trim();
						}
					}
					else if (LogSeverity.LogError == logLevel && message.StartsWith("Unhandled Exception:", StringComparison.Ordinal))
					{
						Match match2 = new Regex("^Unhandled\\s+Exception:\\s*(?<exceptionName>\\S+):\\s*(?<exceptionDetail>.*)", RegexOptions.Singleline).Match(message);
						if (match2.Success)
						{
							string text3 = match2.Groups["exceptionName"].Value.Trim();
							string text4 = match2.Groups["exceptionDetail"].Value.Trim();
							int num = text3.LastIndexOf(".", StringComparison.Ordinal);
							text = ((num <= 0 || num == text3.Length) ? text3 : text3.Substring(num + 1));
							int num2 = text4.IndexOf(" at ", StringComparison.Ordinal);
							if (num2 > 0)
							{
								text2 = text4.Substring(0, num2);
								string text5 = text4.Substring(num2 + 3).Replace(" at ", "\n").Replace("in <filename unknown>:0", "")
									.Replace("[0x00000]", "");
								stackTrace = string.Format("{0}\n{1}", stackTrace, text5.Trim());
							}
							else
							{
								text2 = text4;
							}
							if (text.Equals("LuaScriptException") && text4.Contains(".lua") && text4.Contains("Stack TraceBack:"))
							{
								num2 = text4.IndexOf("Stack TraceBack:", StringComparison.Ordinal);
								if (num2 > 0)
								{
									text2 = text4.Substring(0, num2);
									string text6 = text4.Substring(num2 + 16).Replace(" [", " \n[");
									stackTrace = string.Format("{0}\n{1}", stackTrace, text6.Trim());
								}
							}
						}
					}
				}
				catch
				{
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = message;
				}
			}
			if (string.IsNullOrEmpty(name))
			{
				if (string.IsNullOrEmpty(text))
				{
					text = string.Format("Unity{0}", logLevel.ToString());
				}
			}
			else
			{
				text = name;
			}
			_ReportException(uncaught, text, text2, stackTrace);
		}

		private static void _ReportException(bool uncaught, string name, string reason, string stackTrace)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			if (string.IsNullOrEmpty(stackTrace))
			{
				stackTrace = StackTraceUtility.ExtractStackTrace();
			}
			_ReportExInfo();
			if (string.IsNullOrEmpty(stackTrace))
			{
				stackTrace = "Empty";
			}
			else
			{
				string[] array = stackTrace.Split('\n');
				try
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (array.Length != 0)
					{
						int num = array.Length;
						for (int i = 0; i < num; i++)
						{
							string text = array[i];
							if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text.Trim()))
							{
								continue;
							}
							text = text.Trim();
							if (!text.StartsWith("System.Collections.Generic.", StringComparison.Ordinal) && !text.StartsWith("ShimEnumerator", StringComparison.Ordinal) && !text.StartsWith("Bugly", StringComparison.Ordinal) && !text.Contains("..ctor"))
							{
								int num2 = text.ToLower().IndexOf("(at", StringComparison.Ordinal);
								int num3 = text.ToLower().IndexOf("/assets/", StringComparison.Ordinal);
								if (num2 > 0 && num3 > 0)
								{
									stringBuilder.AppendFormat("{0}(at {1}", text.Substring(0, num2).Replace(":", "."), text.Substring(num3));
								}
								else
								{
									stringBuilder.Append(text.Replace(":", "."));
								}
								stringBuilder.AppendLine();
							}
						}
						stackTrace = stringBuilder.ToString();
					}
				}
				catch
				{
					_PrintLog(LogSeverity.LogError, "{0}", "Error to parse the stack trace");
				}
			}
			_PrintLog(LogSeverity.LogError, "Report Exception: {0} {1}\n*********\n{2}\n*********", name, reason, stackTrace);
			_uncaughtAutoReportOnce = false;
			_LogTrace(uncaught ? 1 : 2, name, reason, stackTrace, _uncaughtAutoReportOnce);
		}

		private static void _EnableDebugMode(bool enable)
		{
		}

		private static void _InitBuglyAgent(string appId, string channel, string version, string user, long delay)
		{
		}

		private static void _SetUserInfo(string userInfo)
		{
		}

		private static void _LogTrace(int type, string name, string message, string stackTrace, bool quitProgram)
		{
		}

		private static void _SetCurrentScene(int sceneId)
		{
		}

		private static void _AddPairInScene(string key, string value)
		{
		}

		private static void _AddExtraDataWithException(string key, string value)
		{
		}

		private static void _LogRecord(LogSeverity level, string message)
		{
		}

		private static void _SetUnityVersion()
		{
		}
	}
}
