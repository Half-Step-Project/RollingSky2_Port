using System.Collections.Generic;

namespace Foundation
{
	public sealed class TaskPool<T> where T : Task
	{
		private readonly List<ITaskAgent<T>> _freeAgents = new List<ITaskAgent<T>>();

		private readonly LinkedList<ITaskAgent<T>> _workingAgents = new LinkedList<ITaskAgent<T>>();

		private readonly LinkedList<T> _waitingTasks = new LinkedList<T>();

		public int TotalAgentCount
		{
			get
			{
				return FreeAgentCount + WorkingAgentCount;
			}
		}

		public int FreeAgentCount
		{
			get
			{
				return _freeAgents.Count;
			}
		}

		public int WorkingAgentCount
		{
			get
			{
				return _workingAgents.Count;
			}
		}

		public int WaitingTaskCount
		{
			get
			{
				return _waitingTasks.Count;
			}
		}

		public bool AddTask(T task)
		{
			if (task == null)
			{
				Log.Warning("Task is invalid.");
				return false;
			}
			if (_waitingTasks.Contains(task))
			{
				Log.Warning("Task was contain.");
				return false;
			}
			for (LinkedListNode<ITaskAgent<T>> linkedListNode = _workingAgents.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				ITaskAgent<T> value = linkedListNode.Value;
				if (value.Task == task)
				{
					Log.Warning("Task is executing.");
					return false;
				}
			}
			task.Status = TaskStatus.Todo;
			_waitingTasks.AddLast(task);
			return true;
		}

		public bool RemoveTask(int taskId)
		{
			for (LinkedListNode<T> linkedListNode = _waitingTasks.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				T value = linkedListNode.Value;
				if (value.Id == taskId)
				{
					_waitingTasks.Remove(linkedListNode);
					return true;
				}
			}
			for (LinkedListNode<ITaskAgent<T>> linkedListNode2 = _workingAgents.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
			{
				ITaskAgent<T> value2 = linkedListNode2.Value;
				if (value2.Task.Id == taskId)
				{
					_workingAgents.Remove(linkedListNode2);
					value2.Recycle();
					_freeAgents.Add(value2);
					return true;
				}
			}
			return false;
		}

		public void RemoveTasks()
		{
			_waitingTasks.Clear();
			for (LinkedListNode<ITaskAgent<T>> linkedListNode = _workingAgents.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				ITaskAgent<T> value = linkedListNode.Value;
				value.Recycle();
				_freeAgents.Add(value);
			}
			_workingAgents.Clear();
		}

		public bool AddAgent(ITaskAgent<T> agent)
		{
			if (agent == null)
			{
				Log.Warning("Task agent is invalid.");
				return false;
			}
			if (_freeAgents.Contains(agent))
			{
				Log.Warning("Agent was contain.");
				return false;
			}
			for (LinkedListNode<ITaskAgent<T>> linkedListNode = _workingAgents.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				if (linkedListNode.Value == agent)
				{
					Log.Warning("Agent was contain.");
					return false;
				}
			}
			agent.Init();
			_freeAgents.Add(agent);
			return true;
		}

		public void Destroy()
		{
			_waitingTasks.Clear();
			for (int i = 0; i < _freeAgents.Count; i++)
			{
				ITaskAgent<T> taskAgent = _freeAgents[i];
				taskAgent.Exit();
			}
			_freeAgents.Clear();
			for (LinkedListNode<ITaskAgent<T>> linkedListNode = _workingAgents.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				ITaskAgent<T> value = linkedListNode.Value;
				value.Recycle();
				value.Exit();
			}
			_workingAgents.Clear();
		}

		public void Tick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("TaskPool.Tick");
			LinkedListNode<ITaskAgent<T>> linkedListNode = _workingAgents.First;
			while (linkedListNode != null)
			{
				ITaskAgent<T> value = linkedListNode.Value;
				if (value.Task.IsDone)
				{
					LinkedListNode<ITaskAgent<T>> next = linkedListNode.Next;
					_workingAgents.Remove(linkedListNode);
					value.Recycle();
					_freeAgents.Add(value);
					linkedListNode = next;
				}
				else
				{
					value.Tick(elapseSeconds, realElapseSeconds);
					linkedListNode = linkedListNode.Next;
				}
			}
			while (WaitingTaskCount > 0 && FreeAgentCount > 0)
			{
				ITaskAgent<T> taskAgent = _freeAgents.RemoveLast();
				T value2 = _waitingTasks.First.Value;
				_waitingTasks.RemoveFirst();
				value2.Status = TaskStatus.Doing;
				taskAgent.Boot(value2);
				if (value2.IsDone)
				{
					taskAgent.Recycle();
					_freeAgents.Add(taskAgent);
				}
				else
				{
					_workingAgents.AddLast(taskAgent);
				}
			}
			Profiler.EndSample();
		}
	}
}
