using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;

public class ResultMoveEnergyEffect : MonoBehaviour
{
	private const float MaxDelay = 1f;

	private const float MinDelay = 0f;

	private const float MaxInternal = 0.2f;

	private const float MinInternal = 0.2f;

	public const float EffectMoveTime = 0.5f;

	public const int MaxEffectCount = 3;

	public List<Transform> moveStarts = new List<Transform>();

	private List<Transform> effects = new List<Transform>();

	public GameObject effectTemplate;

	public GameObject rareEffectTemplate;

	public float delayTime;

	public float internalTime;

	public Transform energyMoveTarget;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		DestroyEffects();
		StopAllCoroutines();
	}

	private void DestroyEffects()
	{
		foreach (Transform effect in effects)
		{
			Object.Destroy(effect);
		}
		effects.Clear();
	}

	public void StartMove(int count, float delayTime, float internalTime, bool isRare)
	{
		if (count != 0)
		{
			this.delayTime = delayTime;
			this.internalTime = internalTime;
			DestroyEffects();
			for (int i = 0; i < count; i++)
			{
				Transform transform = null;
				transform = ((!isRare) ? Object.Instantiate(effectTemplate).transform : Object.Instantiate(rareEffectTemplate).transform);
				transform.SetParent(base.transform);
				transform.gameObject.SetActive(false);
				effects.Add(transform);
				transform.position = moveStarts[Random.Range(0, moveStarts.Count)].position;
			}
			StartCoroutine(ToStartMove(count));
		}
	}

	private IEnumerator ToStartMove(int count)
	{
		yield return new WaitForSeconds(delayTime);
		int index = 0;
		while (index < count)
		{
			Transform effect = effects[index];
			effect.gameObject.SetActive(true);
			effect.DOMove(energyMoveTarget.position, 0.5f).OnComplete(delegate
			{
				effect.gameObject.SetActive(false);
				Mod.Event.Fire(this, Mod.Reference.Acquire<EnergyEffectOneFinishedEventArgs>().Initialize());
			});
			index++;
			yield return new WaitForSeconds(internalTime);
		}
	}
}
