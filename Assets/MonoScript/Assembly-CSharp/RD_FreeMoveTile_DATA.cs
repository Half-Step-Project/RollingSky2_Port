using System;

[Serializable]
internal class RD_FreeMoveTile_DATA
{
	public RD_ElementAnim_DATA anim;

	public bool ifPlayAnim;

	public RD_ElementParticle_DATA[] particles;

	public BaseElement.CommonState commonState;

	public float distance;
}
