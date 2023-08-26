public class RandomNormalCrown : RandomNormalAward
{
	protected override void OnCollideBall(BaseRole ball)
	{
		if (modelPart.gameObject.activeSelf)
		{
			ball.GainCrown(m_uuId, data.sortID);
			modelPart.gameObject.SetActive(false);
			PlayParticle();
			PlaySoundEffect();
			commonState = CommonState.End;
		}
	}

	public override DropType GetDropType()
	{
		return DropType.CROWN;
	}
}
