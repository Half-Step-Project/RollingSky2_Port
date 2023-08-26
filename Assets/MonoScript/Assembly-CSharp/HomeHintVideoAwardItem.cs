using DG.Tweening;
using UnityEngine;

public class HomeHintVideoAwardItem : MonoBehaviour
{
	public float MoveSpeed = 50f;

	public Transform moveAreaMin;

	public Transform moveAreaMax;

	public Transform moveRoot;

	[HideInInspector]
	public int delayShowCloseButtonTime = 1;

	private void OnDestroy()
	{
		moveRoot.DOKill();
	}

	private void OnDisable()
	{
	}

	public void StartWork()
	{
		moveRoot.gameObject.SetActive(true);
		StartMove();
	}

	private void StartMove()
	{
		moveRoot.DOKill();
		moveRoot.localPosition = GetRandomPosInArea();
		LoopMove();
	}

	private void LoopMove()
	{
		moveRoot.DOLocalMove(GetRandomPosInArea(), MoveSpeed).SetSpeedBased(true).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				LoopMove();
			});
	}

	private Vector3 GetRandomPosInArea()
	{
		if (MonoSingleton<GameTools>.Instacne.IsNullAndLogError(moveAreaMin, "GetRandomPosInArea moveAreaMin == null"))
		{
			return Vector3.zero;
		}
		if (MonoSingleton<GameTools>.Instacne.IsNullAndLogError(moveAreaMax, "GetRandomPosInArea moveAreaMax == null"))
		{
			return Vector3.zero;
		}
		float x = Random.Range(moveAreaMin.localPosition.x, moveAreaMax.localPosition.x);
		float y = Random.Range(moveAreaMin.localPosition.y, moveAreaMax.localPosition.y);
		return new Vector3(x, y, 0f);
	}

	public void OnClick()
	{
	}
}
