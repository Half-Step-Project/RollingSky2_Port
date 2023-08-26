using System.Collections.Generic;
using UnityEngine.Events;

namespace RS2
{
	public class CommonTutorialData
	{
		public UnityAction endAction;

		private Queue<CommonTutorialStepData> stepQueue = new Queue<CommonTutorialStepData>();

		private int m_tutorialId = -1;

		public int TutorialId
		{
			get
			{
				return m_tutorialId;
			}
		}

		public CommonTutorialData(int id)
		{
			m_tutorialId = id;
		}

		public void AddStep(CommonTutorialStepData stepData)
		{
			stepQueue.Enqueue(stepData);
		}

		public CommonTutorialStepData GetStep()
		{
			if (stepQueue.Count > 0)
			{
				return stepQueue.Dequeue();
			}
			return null;
		}

		public int SetCount()
		{
			return stepQueue.Count;
		}

		public void Clear()
		{
			stepQueue.Clear();
		}
	}
}
