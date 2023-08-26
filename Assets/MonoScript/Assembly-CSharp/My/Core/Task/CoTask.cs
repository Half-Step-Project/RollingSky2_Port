using System.Collections;
using UnityEngine;

namespace My.Core.Task
{
	public sealed class CoTask : ITask
	{
		private IEnumerator routine;

		private float keepTime;

		private float durationTime;

		internal CoTaskState State { get; private set; }

		public bool Living
		{
			get
			{
				if (State != CoTaskState.Running)
				{
					return State == CoTaskState.Paused;
				}
				return true;
			}
		}

		public bool Running
		{
			get
			{
				return State == CoTaskState.Running;
			}
		}

		public bool Paused
		{
			get
			{
				return State == CoTaskState.Paused;
			}
		}

		public event TaskFinishedHandler Finished;

		public bool StartAndWait(float waitTime)
		{
			Start();
			return Wait(waitTime);
		}

		public bool Start()
		{
			if (State == CoTaskState.Initialized)
			{
				State = CoTaskState.Running;
				MonoSingleton<TaskManager>.Instance.StartCoroutine(CallWrapper());
				return true;
			}
			return false;
		}

		public bool Pause(bool pause)
		{
			if (pause && State == CoTaskState.Running)
			{
				State = CoTaskState.Paused;
				return true;
			}
			if (!pause && State == CoTaskState.Paused)
			{
				State = CoTaskState.Running;
				keepTime = 0f;
				durationTime = 0f;
				return true;
			}
			return false;
		}

		public bool Wait(float keepTime)
		{
			if (State == CoTaskState.Running || State == CoTaskState.Paused)
			{
				if (State == CoTaskState.Running)
				{
					State = CoTaskState.Paused;
				}
				this.keepTime = keepTime;
				durationTime = 0f;
				return true;
			}
			return false;
		}

		public void Cancel()
		{
			if (State == CoTaskState.Running || State == CoTaskState.Paused)
			{
				State = CoTaskState.Canceled;
			}
		}

		public CoTask(IEnumerator routine, bool start = true)
		{
			State = CoTaskState.Initialized;
			TaskManager.AddTask(this);
			this.routine = routine;
			keepTime = 0f;
			durationTime = 0f;
			if (start)
			{
				Start();
			}
		}

		private IEnumerator CallWrapper()
		{
			if (routine == null)
			{
				Debug.LogError("The routine must be not null ... ");
				State = CoTaskState.Exception;
				yield break;
			}
			while (State == CoTaskState.Running || State == CoTaskState.Paused)
			{
				if (State == CoTaskState.Paused)
				{
					if (keepTime > 0f)
					{
						durationTime += Time.deltaTime;
						if (durationTime >= keepTime)
						{
							State = CoTaskState.Running;
							keepTime = 0f;
							durationTime = 0f;
						}
						else
						{
							yield return null;
						}
					}
					else
					{
						yield return null;
					}
				}
				if (State == CoTaskState.Running)
				{
					if (routine.MoveNext())
					{
						yield return routine.Current;
					}
					else if (State != CoTaskState.Canceled)
					{
						State = CoTaskState.Finished;
					}
				}
			}
			if (State == CoTaskState.Canceled || State == CoTaskState.Finished)
			{
				if (State == CoTaskState.Canceled)
				{
					MonoSingleton<TaskManager>.Instance.StopCoroutine(routine);
				}
				if (this.Finished != null)
				{
					this.Finished(new TaskFinishedEventArgs(this, State == CoTaskState.Canceled));
				}
			}
		}
	}
}
