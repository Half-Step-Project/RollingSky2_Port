public interface IAwardFragement : IAward
{
	int GetNeedFragementCount();

	void SetNeedFragementCount(int count);

	void SetCompleteSortID(int sortID);

	int GetCompleteSortID();

	DropType GetCompleteDropType();
}
