using System.Collections.Generic;
using UnityEngine;

public class DiscountController : MonoBehaviour
{
	public List<GameObject> tenList = new List<GameObject>();

	public List<GameObject> geList = new List<GameObject>();

	public void SetDiscount(int disCountNum)
	{
		if (disCountNum > 0)
		{
			disCountNum = 100 - disCountNum;
			base.gameObject.SetActive(true);
			int ten = disCountNum / 10;
			int ge = disCountNum % 10;
			ShowDiscountNum(ten, ge);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void ShowDiscountNum(int ten, int ge)
	{
		for (int i = 0; i < 10; i++)
		{
			if (i == ten)
			{
				tenList[i].SetActive(true);
			}
			else
			{
				tenList[i].SetActive(false);
			}
			if (i == ge)
			{
				geList[i].SetActive(true);
			}
			else
			{
				geList[i].SetActive(false);
			}
		}
	}
}
