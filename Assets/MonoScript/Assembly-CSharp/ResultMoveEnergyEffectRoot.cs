using Foundation;
using RS2;
using UnityEngine;

public class ResultMoveEnergyEffectRoot : MonoBehaviour
{
	private const float PercentOneEffect = 0.3f;

	private const float EffectInternalTime = 0.2f;

	private const float DiamoundDelayTime = 0.1f;

	private const float CrownDelayTime = 0.2f;

	private static float allMoveTime;

	public ResultMoveEnergyEffect getEnergyByDiamound;

	public ResultMoveEnergyEffect getEnergyByProgress;

	public ResultMoveEnergyEffect getEnergyByCrown;

	public ParticleSystem getEffect;

	public ParticleSystem rareGetEffect;

	private bool isRare;

	private void Awake()
	{
		Mod.Event.Subscribe(EventArgs<EnergyEffectOneFinishedEventArgs>.EventId, OnEnergyEffectOneFinished);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<EnergyEffectOneFinishedEventArgs>.EventId, OnEnergyEffectOneFinished);
	}

	private void OnEnergyEffectOneFinished(object sender, EventArgs e)
	{
		if (isRare)
		{
			rareGetEffect.Play();
		}
		else
		{
			getEffect.Play();
		}
	}

	public float StartMove(bool isRare)
	{
		this.isRare = isRare;
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		GameDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		int diamonds = dataModule2.GetLevelTableById(dataModule2.CurLevelId).Diamonds;
		int value = 0;
		if (dataModule.GainedDiamonds == diamonds)
		{
			value = 3;
		}
		else if (dataModule.GainedDiamonds > 0)
		{
			value = (int)((float)dataModule.GainedDiamonds / ((float)diamonds * 0.3f)) + 1;
		}
		value = Mathf.Clamp(value, 0, 3);
		int value2 = 0;
		if (dataModule.GainedCrowns == ResultForm.maxCrownNum)
		{
			value2 = 3;
		}
		else if (dataModule.GainedCrowns > 0)
		{
			value2 = (int)((float)dataModule.GainedCrowns / ((float)ResultForm.maxCrownNum * 0.3f));
		}
		value2 = Mathf.Clamp(value2, 0, 3);
		int value3 = 0;
		if (dataModule.ProgressPercentage == 100)
		{
			value3 = 3;
		}
		else if (dataModule.ProgressPercentage > 0)
		{
			value3 = (int)((float)dataModule.ProgressPercentage / 30.0000019f) + 1;
		}
		value3 = Mathf.Clamp(value3, 0, 3);
		getEnergyByProgress.StartMove(value3, 0f, 0.2f, isRare);
		getEnergyByDiamound.StartMove(value, 0.1f, 0.2f, isRare);
		getEnergyByCrown.StartMove(value2, 0.2f, 0.2f, isRare);
		float a = 0f;
		float b = 0f;
		float b2 = 0f;
		if (value3 > 0)
		{
			a = (float)value3 * 0.5f - (float)(value3 - 1) * 0.2f;
		}
		if (value > 0)
		{
			b = (float)value * 0.5f - (float)(value - 1) * 0.2f + 0.1f;
		}
		if (value2 > 0)
		{
			b2 = (float)value2 * 0.5f - (float)(value2 - 1) * 0.2f + 0.2f;
		}
		allMoveTime = Mathf.Max(Mathf.Max(a, b), b2);
		return allMoveTime;
	}

	public static float GetAllEffectMoveTime()
	{
		return allMoveTime;
	}
}
