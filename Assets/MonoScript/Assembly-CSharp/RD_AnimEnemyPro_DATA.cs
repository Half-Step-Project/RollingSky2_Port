using System;

[Serializable]
internal class RD_AnimEnemyPro_DATA
{
	public string m_currentAnimationName;

	public bool m_isShowMeshRenderer;

	public BaseElement.CommonState commonState;

	public RD_ElementAnim_DATA animData;

	public RD_ElementParticle_DATA[] additionParticlesData;

	public RD_ElementParticle_DATA[] particles;
}
