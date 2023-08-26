using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DropDownButton : MonoBehaviour
{
	public Animator m_Animatior;

	public DOTweenAnimation m_dropAnimation;

	public Image m_normal;

	private bool m_isOpen;

	private void Start()
	{
		m_isOpen = false;
		m_normal.gameObject.SetActive(true);
		m_dropAnimation.DORewind();
	}

	public void OnClick()
	{
		if (m_isOpen)
		{
			m_dropAnimation.DOPlayBackwards();
			m_Animatior.SetTrigger("Close");
		}
		else
		{
			m_dropAnimation.DOPlayForward();
			m_Animatior.SetTrigger("Open");
		}
		m_isOpen = !m_isOpen;
	}
}
