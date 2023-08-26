namespace RS2
{
	public abstract class BaseTutorialStep
	{
		private BaseTutorialStage m_currneStage;

		private object m_userData;

		private int m_stepId = -1;

		public int StepId
		{
			get
			{
				return m_stepId;
			}
			set
			{
				m_stepId = value;
			}
		}

		public void Init(BaseTutorialStage stage, int stepId = -1, object userData = null)
		{
			m_currneStage = stage;
			m_userData = userData;
			StepId = -1;
		}

		public abstract void Do();

		public virtual void Next()
		{
			if (m_currneStage != null)
			{
				m_currneStage.DoNextStep();
			}
		}
	}
}
