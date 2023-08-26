using System;

[Serializable]
internal class RD_AnimEffectTrigger_DATA
{
	public RD_ElementAnim_DATA anim;

	public RD_ElementTransform_DATA model;

	public RD_ElementTransform_DATA effect;

	public RD_ElementParticle_DATA[] particles;
}
