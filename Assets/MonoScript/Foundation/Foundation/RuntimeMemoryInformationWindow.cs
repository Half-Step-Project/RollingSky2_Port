using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace Foundation
{
	internal sealed class RuntimeMemoryInformationWindow<T> : ScrollableDebuggerWindowBase where T : UnityEngine.Object
	{
		private sealed class Sample
		{
			[CompilerGenerated]
			private readonly string _003CName_003Ek__BackingField;

			[CompilerGenerated]
			private readonly string _003CType_003Ek__BackingField;

			[CompilerGenerated]
			private readonly long _003CSize_003Ek__BackingField;

			public string Name
			{
				[CompilerGenerated]
				get
				{
					return _003CName_003Ek__BackingField;
				}
			}

			public string Type
			{
				[CompilerGenerated]
				get
				{
					return _003CType_003Ek__BackingField;
				}
			}

			public long Size
			{
				[CompilerGenerated]
				get
				{
					return _003CSize_003Ek__BackingField;
				}
			}

			public bool Highlight { get; set; }

			public Sample(string name, string type, long size)
			{
				_003CName_003Ek__BackingField = name;
				_003CType_003Ek__BackingField = type;
				_003CSize_003Ek__BackingField = size;
			}
		}

		private const int ShowSampleCount = 300;

		private DateTime _sampleTime = DateTime.MinValue;

		private long _sampleSize;

		private long _duplicateSampleSize;

		private int _duplicateSimpleCount;

		private readonly List<Sample> _samples = new List<Sample>();

		protected override void OnDrawScrollableWindow()
		{
			string name = typeof(T).Name;
			GUILayout.Label("<b>" + name + " Runtime Memory Information</b>");
			GUILayout.BeginVertical("box");
			if (GUILayout.Button("Take Sample for " + name, GUILayout.Height(30f)))
			{
				TakeSample();
			}
			if (_sampleTime <= DateTime.MinValue)
			{
				GUILayout.Label("<b>Please take sample for " + name + " first.</b>");
			}
			else
			{
				if (_duplicateSimpleCount > 0)
				{
					GUILayout.Label(string.Format("<b>{0} {1}s ({2}) obtained at {3:yyyy-MM-dd HH:mm:ss}, while {4} {1}s ({5}) might be duplicated.</b>", _samples.Count.ToString(), name, GetSizeString(_sampleSize), _sampleTime, _duplicateSimpleCount.ToString(), GetSizeString(_duplicateSampleSize)));
				}
				else
				{
					GUILayout.Label(string.Format("<b>{0} {1}s ({2}) obtained at {3:yyyy-MM-dd HH:mm:ss}.</b>", _samples.Count.ToString(), name, GetSizeString(_sampleSize), _sampleTime));
				}
				if (_samples.Count > 0)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label("<b>" + name + " Name</b>");
					GUILayout.Label("<b>Type</b>", GUILayout.Width(240f));
					GUILayout.Label("<b>Size</b>", GUILayout.Width(80f));
					GUILayout.EndHorizontal();
				}
				int num = 0;
				for (int i = 0; i < _samples.Count; i++)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(_samples[i].Highlight ? ("<color=yellow>" + _samples[i].Name + "</color>") : _samples[i].Name);
					GUILayout.Label(_samples[i].Highlight ? ("<color=yellow>" + _samples[i].Type + "</color>") : _samples[i].Type, GUILayout.Width(240f));
					GUILayout.Label(_samples[i].Highlight ? ("<color=yellow>" + GetSizeString(_samples[i].Size) + "</color>") : GetSizeString(_samples[i].Size), GUILayout.Width(80f));
					GUILayout.EndHorizontal();
					num++;
					if (num >= 300)
					{
						break;
					}
				}
			}
			GUILayout.EndVertical();
		}

		private void TakeSample()
		{
			_sampleTime = DateTime.Now;
			_sampleSize = 0L;
			_duplicateSampleSize = 0L;
			_duplicateSimpleCount = 0;
			_samples.Clear();
			T[] array = Resources.FindObjectsOfTypeAll<T>();
			for (int i = 0; i < array.Length; i++)
			{
				long runtimeMemorySizeLong = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(array[i]);
				_sampleSize += runtimeMemorySizeLong;
				_samples.Add(new Sample(array[i].name, array[i].GetType().Name, runtimeMemorySizeLong));
			}
			_samples.Sort(SampleComparer);
			for (int j = 1; j < _samples.Count; j++)
			{
				if (_samples[j].Name == _samples[j - 1].Name && _samples[j].Type == _samples[j - 1].Type && _samples[j].Size == _samples[j - 1].Size)
				{
					_samples[j].Highlight = true;
					_duplicateSampleSize += _samples[j].Size;
					_duplicateSimpleCount++;
				}
			}
		}

		private string GetSizeString(long size)
		{
			if (size < 1024)
			{
				return size + " Bytes";
			}
			if (size < 1048576)
			{
				return string.Format("{0:F2} KB", (float)size / 1024f);
			}
			if (size < 1073741824)
			{
				return string.Format("{0:F2} MB", (float)size / 1024f / 1024f);
			}
			if (size < 1099511627776L)
			{
				return string.Format("{0:F2} GB", (float)size / 1024f / 1024f / 1024f);
			}
			return string.Format("{0:F2} TB", (float)size / 1024f / 1024f / 1024f / 1024f);
		}

		private int SampleComparer(Sample a, Sample b)
		{
			int num = b.Size.CompareTo(a.Size);
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(a.Type, b.Type, StringComparison.Ordinal);
			if (num == 0)
			{
				return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
			}
			return num;
		}
	}
}
