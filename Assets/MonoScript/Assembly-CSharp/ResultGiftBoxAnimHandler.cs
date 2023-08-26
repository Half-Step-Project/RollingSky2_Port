using UnityEngine;

public class ResultGiftBoxAnimHandler : MonoBehaviour
{
	public ResultMotivateRoot resultMotivateRoot;

	public void OpenFinished()
	{
		if (resultMotivateRoot != null)
		{
			resultMotivateRoot.OpenFinished();
		}
	}
}
