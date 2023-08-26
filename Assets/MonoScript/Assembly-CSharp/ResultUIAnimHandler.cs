using RS2;
using UnityEngine;

public class ResultUIAnimHandler : MonoBehaviour
{
	private ResultForm resultForm;

	public void Init(ResultForm resultForm)
	{
		this.resultForm = resultForm;
	}

	public void OnCenterInFinished()
	{
		resultForm.OnCenterInFinished();
	}

	public void OnGetGoldStartIncrease()
	{
		resultForm.OnGetGoldStartIncrease();
	}

	public void OnGetGoldInFinished()
	{
		resultForm.OnGetGoldInFinished();
	}

	public void OnGetGoldMoveFinished()
	{
		resultForm.OnGetGoldMoveFinished();
	}

	public void OnGetGoldMoveBoxFinished()
	{
		resultForm.OnGetGoldMoveBoxFinished();
	}
}
