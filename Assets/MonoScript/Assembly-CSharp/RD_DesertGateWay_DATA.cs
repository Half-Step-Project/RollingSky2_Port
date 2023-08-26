using System;

[Serializable]
internal class RD_DesertGateWay_DATA
{
	public RD_ElementAnim_DATA waitingAnim;

	public RD_ElementAnim_DATA openModelAnim;

	public RD_ElementAnim_DATA openEffectAnim;

	public RD_ElementParticle_DATA[] waitingEffect;

	public RD_ElementParticle_DATA[] openEffect;
}
