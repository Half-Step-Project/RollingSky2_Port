using DG.Tweening;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeGetCupRoot : MonoBehaviour
{
	public GameObject root;

	public Image iconAllStar;

	public Image iconPerfect;

	public Text tips;

	public Image moveIcon;

	public Transform moveTarget;

	public Image black;

	public UnityAction moveFinished;

	private float blackAlpha;

	private const float MOVE_TIME = 0.5f;

	private void Awake()
	{
		blackAlpha = black.color.a;
	}

	public void Show(bool isPerfect)
	{
		base.gameObject.SetActive(true);
		root.SetActive(true);
		iconAllStar.gameObject.SetActive(!isPerfect);
		iconPerfect.gameObject.SetActive(isPerfect);
		moveIcon.sprite = (isPerfect ? iconPerfect.sprite : iconAllStar.sprite);
		moveIcon.transform.localPosition = Vector3.zero;
		moveIcon.transform.localScale = Vector3.one;
		moveIcon.gameObject.SetActive(false);
		black.color = new Color(0f, 0f, 0f, blackAlpha);
		MenuForm.Form.SetState(MenuFormState.CupRoot);
		if (isPerfect)
		{
			tips.text = Mod.Localization.GetInfoById(285);
		}
		else
		{
			tips.text = Mod.Localization.GetInfoById(284);
		}
		if ((bool)MenuForm.Form)
		{
			MenuForm.Form.m_tweenAnimationBack.gameObject.SetActive(false);
		}
	}

	public void OnClickGet()
	{
		root.SetActive(false);
		moveIcon.gameObject.SetActive(true);
		moveIcon.transform.DOScale(0.15f, 0.5f);
		black.DOFade(0f, 0.5f);
		moveIcon.transform.DOMove(moveTarget.position, 0.5f).OnComplete(delegate
		{
			base.gameObject.SetActive(false);
			if (moveFinished != null)
			{
				moveFinished();
			}
			if ((bool)MenuForm.Form)
			{
				MenuForm.Form.m_tweenAnimationBack.gameObject.SetActive(true);
				MenuForm.Form.SetState(MenuFormState.SelectLevel);
			}
		});
	}

	private void Update()
	{
		if (MenuForm.State == MenuFormState.CupRoot && InputService.KeyDown_A)
		{
			OnClickGet();
		}
	}
}
