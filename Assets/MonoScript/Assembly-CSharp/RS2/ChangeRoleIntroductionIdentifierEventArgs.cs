using Foundation;

namespace RS2
{
	public sealed class ChangeRoleIntroductionIdentifierEventArgs : EventArgs<ChangeRoleIntroductionIdentifierEventArgs>
	{
		public bool m_ifShow;

		public int m_identifierIndex;

		public int mCollectCount;

		public static ChangeRoleIntroductionIdentifierEventArgs Make(bool ifShow, int identifierIndex, int collectCount = 0)
		{
			ChangeRoleIntroductionIdentifierEventArgs changeRoleIntroductionIdentifierEventArgs = Mod.Reference.Acquire<ChangeRoleIntroductionIdentifierEventArgs>();
			changeRoleIntroductionIdentifierEventArgs.m_ifShow = ifShow;
			changeRoleIntroductionIdentifierEventArgs.m_identifierIndex = identifierIndex;
			changeRoleIntroductionIdentifierEventArgs.mCollectCount = collectCount;
			return changeRoleIntroductionIdentifierEventArgs;
		}

		protected override void OnRecycle()
		{
			m_ifShow = false;
			m_identifierIndex = -1;
		}
	}
}
