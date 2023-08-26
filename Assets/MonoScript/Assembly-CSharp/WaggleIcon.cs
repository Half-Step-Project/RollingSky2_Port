using DG.Tweening;
using UnityEngine;

public class WaggleIcon : MonoBehaviour
{
	public Transform target;

	public Transform basePos;

	public float range;

	public float speed;

	private void OnEnable()
	{
		StartWaggle();
	}

	private void StartWaggle()
	{
		target.DOKill();
		target.localPosition = basePos.localPosition;
		LoopMove();
	}

	private void LoopMove()
	{
		target.DOLocalMove(GetRandomPosInRange(), speed).SetSpeedBased(true).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				LoopMove();
			});
	}

	private Vector3 GetRandomPosInRange()
	{
		float x = Random.Range(basePos.localPosition.x - range, basePos.localPosition.x + range);
		float y = Random.Range(basePos.localPosition.y - range, basePos.localPosition.y + range);
		return new Vector3(x, y, 0f);
	}
}
