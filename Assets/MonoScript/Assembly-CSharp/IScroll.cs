public interface IScroll
{
	void OnInit();

	void OnOpen();

	void OnTick(float elapseSeconds, float realElapseSeconds);

	void OnClose();

	void OnRefresh();

	void OnRelease();
}
