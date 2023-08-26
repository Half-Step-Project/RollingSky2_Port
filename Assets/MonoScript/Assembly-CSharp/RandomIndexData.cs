using UnityEngine;

public class RandomIndexData
{
	public int[] ItemArray = new int[0];

	public RandomIndexData()
	{
	}

	public RandomIndexData(int[] orderArray, bool ifOrder)
	{
		ItemArray = orderArray;
		if (!ifOrder)
		{
			ResetRandomData();
		}
	}

	public int GetSelectIndexByCycleIndex(int cycleIndex)
	{
		return ItemArray[cycleIndex % ItemArray.Length];
	}

	public void ResetRandomData()
	{
		if (ItemArray.Length <= 1)
		{
			return;
		}
		int num = -1;
		int[] array = new int[ItemArray.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = num;
		}
		for (int j = 0; j < ItemArray.Length; j++)
		{
			int num2 = ItemArray[j];
			int num3 = Random.Range(0, array.Length);
			if (array[num3] == num)
			{
				array[num3] = num2;
				continue;
			}
			for (int k = num3; k < num3 + array.Length; k++)
			{
				int num4 = k % array.Length;
				if (array[num4] == num)
				{
					array[num4] = num2;
					break;
				}
			}
		}
		ItemArray = array;
	}
}
