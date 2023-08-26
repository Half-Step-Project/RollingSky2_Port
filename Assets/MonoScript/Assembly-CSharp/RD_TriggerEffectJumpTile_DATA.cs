using System;

[Serializable]
internal class RD_TriggerEffectJumpTile_DATA
{
	public float collidePos;

	public RD_ElementTransform_DATA trans;

	public RD_ElementTransform_DATA state1;

	public RD_ElementTransform_DATA state2;

	public RD_ElementAnim_DATA anim;

	public float jumpMat;

	public bool ifTriggerActive;

	public BaseElement.CommonState commonState;
}
