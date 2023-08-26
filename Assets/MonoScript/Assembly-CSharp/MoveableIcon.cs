using System;
using DG.Tweening;
using Foundation;
using UnityEngine;

public class MoveableIcon : MonoBehaviour
{
	public Transform moveIcon;

	public Transform target;

	public System.Action moveFinished;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		moveIcon.DOKill();
		MonoSingleton<GameTools>.Instacne.EnableInput();
	}

	public void StartMove()
	{
		UIInputModule.UIInput.EnableInput = false;
		moveIcon.gameObject.SetActive(true);
		moveIcon.DOMove(target.position, 1f).OnComplete(delegate
		{
			MonoSingleton<GameTools>.Instacne.EnableInput();
			moveIcon.gameObject.SetActive(false);
			if (moveFinished != null)
			{
				moveFinished();
			}
		});
		moveIcon.DOScale(1.5f, 0.1f).OnComplete(delegate
		{
			moveIcon.DOScale(1f, 0.9f);
		});
		MonoSingleton<GameTools>.Instacne.TimerToEnableInput(1200u);
	}
}
