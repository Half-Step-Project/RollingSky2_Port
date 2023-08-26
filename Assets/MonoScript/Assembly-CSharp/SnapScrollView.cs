using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class SnapScrollView : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler
{
	public Transform center;

	public float snapSpeed = 1f;

	private ScrollRect scrollRect;

	private bool isLerping;

	private float snapToPosX;

	private float snapFromPosX;

	private float lerpTimer;

	private Dictionary<int, float> pageAndContentPosX = new Dictionary<int, float>();

	public Action<int> PageChangeFinished;

	public Action PageChangeStart;

	public int CurrentPage { get; set; }

	public int TotalPage { get; set; }

	private void Awake()
	{
		pageAndContentPosX.Clear();
		scrollRect = GetComponent<ScrollRect>();
	}

	private void Update()
	{
		if (!isLerping)
		{
			return;
		}
		lerpTimer += Time.deltaTime * snapSpeed;
		float x = Mathf.Lerp(snapFromPosX, snapToPosX, lerpTimer);
		Vector2 anchoredPosition = scrollRect.content.anchoredPosition;
		scrollRect.content.anchoredPosition = new Vector2(x, anchoredPosition.y);
		if (lerpTimer >= 1f)
		{
			isLerping = false;
			lerpTimer = 0f;
			if (PageChangeFinished != null)
			{
				PageChangeFinished(CurrentPage);
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isLerping = false;
		if (PageChangeStart != null)
		{
			PageChangeStart();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float num = float.MaxValue;
		int num2 = 0;
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			float num3 = Mathf.Abs(center.position.x - scrollRect.content.GetChild(i).position.x);
			if (num3 < num)
			{
				num2 = i;
				num = num3;
			}
		}
		CurrentPage = num2 - 1;
		StartLerp(CurrentPage);
	}

	private void StartLerp(int page)
	{
		snapFromPosX = scrollRect.content.anchoredPosition.x;
		snapToPosX = pageAndContentPosX[page];
		lerpTimer = 0f;
		isLerping = true;
	}

	public void Init()
	{
		TotalPage = Mathf.Max(0, scrollRect.content.childCount - 3);
		float itemWidth = GetItemWidth();
		pageAndContentPosX.Clear();
		for (int i = 0; i <= TotalPage; i++)
		{
			float value = ((float)TotalPage / 2f - (float)i) * itemWidth;
			pageAndContentPosX.Add(i, value);
		}
		CurrentPage = 0;
	}

	public void SetPage(int page)
	{
		if (PageValid(page))
		{
			CurrentPage = page;
			float x = pageAndContentPosX[page];
			scrollRect.content.anchoredPosition = new Vector2(x, scrollRect.content.anchoredPosition.y);
		}
	}

	public void GotoPage(int page)
	{
		if (PageValid(page))
		{
			CurrentPage = page;
			StartLerp(page);
			if (PageChangeStart != null)
			{
				PageChangeStart();
			}
		}
	}

	public void NextPage()
	{
		if (!IsLastPage())
		{
			GotoPage(CurrentPage + 1);
		}
	}

	public void PrePage()
	{
		if (!IsFirstPage())
		{
			GotoPage(CurrentPage - 1);
		}
	}

	public bool IsFirstPage()
	{
		return CurrentPage == 0;
	}

	public bool IsLastPage()
	{
		return CurrentPage == TotalPage;
	}

	private float GetItemWidth()
	{
		if (scrollRect.content.childCount == 0)
		{
			return 0f;
		}
		return scrollRect.content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
	}

	private bool PageValid(int page)
	{
		if (page < 0 || page > TotalPage)
		{
			return false;
		}
		return true;
	}
}
