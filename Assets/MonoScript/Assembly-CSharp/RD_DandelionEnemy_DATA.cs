using System;
using RS2;

[Serializable]
internal class RD_DandelionEnemy_DATA
{
	public RD_ElementTransform_DATA modelPart;

	public RD_ElementAnim_DATA defaultAnim;

	public RD_ElementParticle_DATA[] defaultParticles;

	public RD_ElementParticle_DATA[] triggerParticles;

	public DandelionEnemy.State currentState;
}
