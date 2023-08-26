using Foundation;

namespace RS2
{
	public sealed class GetLoginAwardEvent : EventArgs<GetLoginAwardEvent>
	{
		public int AwardId { get; set; }

		public GetLoginAwardEvent Initialize(int awardId)
		{
			AwardId = awardId;
			return this;
		}

		protected override void OnRecycle()
		{
		}
	}
}
