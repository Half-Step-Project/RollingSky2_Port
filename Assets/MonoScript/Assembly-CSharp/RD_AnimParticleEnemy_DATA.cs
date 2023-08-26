using System;

[Serializable]
internal class RD_AnimParticleEnemy_DATA
{
	public string baseData;

	public RD_ElementAnim_DATA anim;

	public RD_ElementParticle_DATA[] particles;

	public AnimParticleEnemy.AnimState animState;

	public AnimParticleEnemy.EffState effState;
}
