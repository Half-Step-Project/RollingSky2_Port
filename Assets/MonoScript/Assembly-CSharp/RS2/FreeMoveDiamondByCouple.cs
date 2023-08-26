namespace RS2
{
	public class FreeMoveDiamondByCouple : FreeMoveCrownByCouple
	{
		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		protected override void OnCollideBall(BaseRole ball)
		{
			if (model.gameObject.activeSelf)
			{
				ball.GainDiamond(m_uuId, data.sortID);
				model.SetActive(false);
				PlayParticle(particle, true);
				PlaySoundEffect();
				commonState = CommonState.End;
			}
		}

		public override DropType GetDropType()
		{
			return DropType.DIAMOND;
		}
	}
}
