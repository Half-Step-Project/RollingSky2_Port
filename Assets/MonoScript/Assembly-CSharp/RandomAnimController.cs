using System.Collections.Generic;
using Foundation;
using UnityEngine;

public sealed class RandomAnimController
{
	public static readonly RandomAnimController Instance = new RandomAnimController();

	private Dictionary<int, RandomIndexData> randomDic = new Dictionary<int, RandomIndexData>();

	public int CycleIndex { get; private set; }

	private RandomAnimController()
	{
	}

	public void OnGameEnter()
	{
		CycleIndex = 0;
		Log.Info("RandomController OnGameEnter:" + CycleIndex);
	}

	public void OnGameFinish()
	{
		CycleIndex++;
		Log.Info("RandomController OnGameFinish:" + CycleIndex);
	}

	public void OnGameRebirth()
	{
		Log.Info("RandomController OnGameRebirth:" + CycleIndex);
	}

	public void OnGameExit()
	{
		CycleIndex = 0;
		Log.Info("RandomController OnGameExit:" + CycleIndex);
	}

	public void ReqisterTrigger(int triggerId, IRandomTrigger triggerElement)
	{
		if (!randomDic.ContainsKey(triggerId))
		{
			RandomIndexData value = new RandomIndexData(triggerElement.OrderArray, triggerElement.IfOrder);
			randomDic.Add(triggerId, value);
			Log.Info("RandomController ReqisterTrigger New:" + triggerId);
		}
		else
		{
			Log.Info("RandomController ReqisterTrigger Old:" + triggerId);
		}
	}

	public int GetSelectIndexByTriggerId(int triggerId)
	{
		return randomDic[triggerId].GetSelectIndexByCycleIndex(CycleIndex);
	}

	private void TestRandom()
	{
		RandomIndexData randomIndexData = new RandomIndexData();
		randomIndexData.ItemArray = new int[11]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10
		};
		Debug.Log("NormalOrder:");
		int[] itemArray = randomIndexData.ItemArray;
		foreach (int num in itemArray)
		{
			Debug.Log("item:" + num);
		}
		randomIndexData.ResetRandomData();
		Debug.Log("RandomOrder:");
		itemArray = randomIndexData.ItemArray;
		foreach (int num2 in itemArray)
		{
			Debug.Log("item:" + num2);
		}
	}
}
