using UnityEngine;
using UnityEngine.Events;

namespace RS2
{
	public class CommonTutorialStepData
	{
		public string tutorialContent = "";

		public UnityAction stepAction;

		public Rect position = Rect.zero;

		public Vector2 posOffset = Vector2.zero;

		public bool changeRect;

		public bool showContent;

		public bool needBlock;

		public bool blockBtnEnable = true;

		public bool needBack = true;

		public UnityAction finishAction;

		public Transform target;

		public bool finishTargetActive = true;

		public float delayShowFinger;

		public TutorialStepType stepType;

		public bool useFingerLocalPos;

		public Vector3 fingerLocalPos = Vector3.zero;

		public bool useViewportAdjustPos;

		public bool disableBackClick;
	}
}
