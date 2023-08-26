using Foundation;

public class ChangeTempGoodsEvent : EventArgs<ChangeTempGoodsEvent>
{
	public bool isAdd;

	public ChangeTempGoodsEvent Init(bool isAdd)
	{
		this.isAdd = isAdd;
		return this;
	}

	public static ChangeTempGoodsEvent Make(bool isAdd)
	{
		return Mod.Reference.Acquire<ChangeTempGoodsEvent>().Init(isAdd);
	}

	protected override void OnRecycle()
	{
	}
}
