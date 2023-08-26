using System;

[Serializable]
internal class RD_TwoEffectTriggerPro_DATA
{
	public RD_ElementTransform_DATA model;

	public RD_ElementTransform_DATA distanceEff;

	public RD_ElementTransform_DATA triggerEff;

	public RD_ElementParticle_DATA[] distanceParticles;

	public RD_ElementParticle_DATA[] triggerParticles;

	public TwoEffectTriggerPro.TwoEffState currentState;

	public RD_ElementAnim_DATA anim;
}
