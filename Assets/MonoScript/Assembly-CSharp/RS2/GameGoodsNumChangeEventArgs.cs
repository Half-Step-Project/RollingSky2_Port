using Foundation;

namespace RS2
{
	public sealed class GameGoodsNumChangeEventArgs : EventArgs<GameGoodsNumChangeEventArgs>
	{
		public int GoodsId { get; private set; }

		public double ChangeNum { get; private set; }

		public AssertChangeType ChangeType { get; private set; }

		public GameGoodsNumChangeEventArgs Initialize(int goodsId, double changeNum, AssertChangeType type)
		{
			GoodsId = goodsId;
			ChangeNum = changeNum;
			ChangeType = type;
			return this;
		}

		protected override void OnRecycle()
		{
			GoodsId = 0;
			ChangeNum = 0.0;
		}
	}
}
