public interface IActiveForm
{
	bool IsShowForm { get; }

	void NormalForm();

	void ShowForm(float posY = 0f);

	void HideForm();
}
