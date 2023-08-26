public interface IScrollItem
{
	void SetScroll(IScroll scroll);

	void SetData(int itemIndex, int tableIndex, int tableID);

	void OnInit();

	void OnOpen();

	void OnTick(float elapseSeconds, float realElapseSeconds);

	void OnClose();

	void OnRefresh();

	void OnRelease();
}
