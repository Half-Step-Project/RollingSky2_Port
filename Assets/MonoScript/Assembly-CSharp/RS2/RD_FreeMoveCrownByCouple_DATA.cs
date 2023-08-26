using System;

namespace RS2
{
	[Serializable]
	internal class RD_FreeMoveCrownByCouple_DATA
	{
		public RD_BezierMove_Data bezierMover;

		public int model;

		public RD_ElementTransform_DATA award;

		public RD_ElementTransform_DATA transform;

		public RD_ElementTransform_DATA trigger;

		public RD_ElementAnim_DATA anim;

		public RD_ElementParticle_DATA[] particle;

		public BaseElement.CommonState commonState;
	}
}
