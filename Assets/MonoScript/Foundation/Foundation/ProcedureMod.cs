using System;
using System.Collections;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Procedure")]
	public sealed class ProcedureMod : ModBase
	{
		[SerializeField]
		private string[] _availableTypeNames;

		[SerializeField]
		private string _entranceTypeName;

		private IFsm<ProcedureMod> _procedureFsm;

		private FsmState<ProcedureMod> _entrance;

		public Procedure Current
		{
			get
			{
				if (_procedureFsm == null)
				{
					Log.Warning("You must initialize procedure first.");
					return null;
				}
				return (Procedure)_procedureFsm.CurrentState;
			}
		}

		public float CurrentDuration
		{
			get
			{
				if (_procedureFsm == null)
				{
					Log.Warning("You must initialize procedure first.");
					return 0f;
				}
				return _procedureFsm.CurrentStateDuration;
			}
		}

		public bool Contains(Type type)
		{
			if (_procedureFsm == null)
			{
				Log.Warning("You must initialize procedure first.");
				return false;
			}
			return _procedureFsm.Contains(type);
		}

		public bool Contains<T>() where T : Procedure
		{
			if (_procedureFsm == null)
			{
				Log.Warning("You must initialize procedure first.");
				return false;
			}
			return _procedureFsm.Contains<T>();
		}

		public Procedure Get<T>() where T : Procedure
		{
			if (_procedureFsm == null)
			{
				Log.Warning("You must initialize procedure first.");
				return null;
			}
			return _procedureFsm.GetState<T>();
		}

		public Procedure Get(Type type)
		{
			if (_procedureFsm == null)
			{
				Log.Warning("You must initialize procedure first.");
				return null;
			}
			return (Procedure)_procedureFsm.GetState(type);
		}

		private IEnumerator Start()
		{
			if ((Application.isEditor && !Application.isPlaying) || base.gameObject.scene.name != "Launch")
			{
				yield break;
			}
			FsmState<ProcedureMod>[] array = new FsmState<ProcedureMod>[_availableTypeNames.Length];
			for (int i = 0; i < _availableTypeNames.Length; i++)
			{
				Type type = ReflectionUtility.GetType(_availableTypeNames[i]);
				if ((object)type == null)
				{
					Log.Error("Can not find procedure type '" + _availableTypeNames[i] + "'.");
					yield break;
				}
				array[i] = (Procedure)Activator.CreateInstance(type);
				if (array[i] == null)
				{
					Log.Error("Can not create procedure instance '" + _availableTypeNames[i] + "'.");
					yield break;
				}
				if (_entranceTypeName == _availableTypeNames[i])
				{
					_entrance = array[i];
				}
			}
			if (_entrance == null)
			{
				Log.Warning("Entrance procedure is invalid.");
				yield break;
			}
			_procedureFsm = Mod.Fsm.Create(this, array);
			yield return new WaitForEndOfFrame();
			_procedureFsm.Start(_entrance.GetType());
		}

		protected override void Awake()
		{
			Mod.Procedure = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
		}

		internal override void OnExit()
		{
			base.OnExit();
			Mod.Fsm.Destroy(_procedureFsm);
		}
	}
}
