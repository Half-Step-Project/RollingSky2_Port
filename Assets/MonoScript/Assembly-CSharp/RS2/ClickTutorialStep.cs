using Foundation;
using UnityEngine;

namespace RS2
{
	public class ClickTutorialStep : BaseTutorialStep
	{
		private Transform target;

		private Vector2 posOffset;

		public ClickTutorialStep(Transform target, Vector2 posOffset)
		{
			this.target = target;
			this.posOffset = posOffset;
		}

		public override void Do()
		{
			CommonTutorialData commonTutorialData = new CommonTutorialData(3);
			CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
			RectTransform rectTransform = target.transform as RectTransform;
			commonTutorialStepData.showContent = false;
			commonTutorialStepData.needBlock = true;
			commonTutorialStepData.position = new Rect(rectTransform.position.x, rectTransform.position.y, rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
			commonTutorialStepData.posOffset = posOffset;
			commonTutorialStepData.changeRect = true;
			commonTutorialStepData.target = target;
			commonTutorialStepData.finishTargetActive = false;
			commonTutorialStepData.stepAction = delegate
			{
				Mod.UI.OpenUIForm(UIFormId.BufferShowForm);
				Next();
			};
			commonTutorialData.AddStep(commonTutorialStepData);
			Mod.UI.OpenUIForm(UIFormId.CommonTutorialForm, commonTutorialData);
		}
	}
}
