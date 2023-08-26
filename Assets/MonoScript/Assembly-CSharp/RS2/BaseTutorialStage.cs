using System.Collections.Generic;

namespace RS2
{
	public abstract class BaseTutorialStage
	{
		private int m_stageId = -1;

		private bool m_ExecuteOnStart = true;

		protected Queue<BaseTutorialStep> steps = new Queue<BaseTutorialStep>();

		public bool ExecuteOnStart
		{
			get
			{
				return m_ExecuteOnStart;
			}
			set
			{
				m_ExecuteOnStart = value;
			}
		}

		public int StageId
		{
			get
			{
				return m_stageId;
			}
			private set
			{
				m_stageId = value;
			}
		}

		public bool IsTutoiralGoing { get; set; }

		public virtual void Init(int stageId)
		{
			StageId = stageId;
			InitSteps();
		}

		public abstract void InitSteps();

		public virtual void AddStep(BaseTutorialStep step)
		{
			step.Init(this);
			steps.Enqueue(step);
		}

		public virtual void Start()
		{
			IsTutoiralGoing = true;
			if (m_ExecuteOnStart)
			{
				Execute();
			}
		}

		public virtual void Execute()
		{
			DoNextStep();
		}

		public virtual void End()
		{
			IsTutoiralGoing = false;
		}

		public virtual void DoNextStep()
		{
			if (steps.Count >= 1)
			{
				steps.Dequeue().Do();
			}
		}
	}
}
