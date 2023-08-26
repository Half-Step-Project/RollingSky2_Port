using Foundation;
using RS2;
using UnityEngine;

public class autoface : MonoBehaviour
{
	public string[] m_nodeName = new string[2] { "", "" };

	public TextMesh m_showText;

	private GameObject[] m_showElements = new GameObject[0];

	private InsideGameDataModule insideGameDataModule;

	private void OnEnable()
	{
		Mod.Event.Subscribe(EventArgs<ChangeRoleIntroductionIdentifierEventArgs>.EventId, OnShowItemChange);
		Mod.Event.Subscribe(EventArgs<GameOriginRebirthResetEventArgs>.EventId, OnShowItemChange);
		Mod.Event.Subscribe(EventArgs<GameResetEventArgs>.EventId, OnShowItemChange);
	}

	private void OnDisable()
	{
		Mod.Event.Unsubscribe(EventArgs<ChangeRoleIntroductionIdentifierEventArgs>.EventId, OnShowItemChange);
		Mod.Event.Unsubscribe(EventArgs<GameOriginRebirthResetEventArgs>.EventId, OnShowItemChange);
		Mod.Event.Unsubscribe(EventArgs<GameResetEventArgs>.EventId, OnShowItemChange);
	}

	private void Awake()
	{
		insideGameDataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
	}

	private void Start()
	{
		m_showElements = new GameObject[m_nodeName.Length];
		for (int i = 0; i < m_nodeName.Length; i++)
		{
			m_showElements[i] = base.transform.Find(m_nodeName[i]).gameObject;
			m_showElements[i].SetActive(false);
			m_showElements[i].transform.localScale = 0.3f * Vector3.one;
		}
		m_showText.gameObject.SetActive(false);
		m_showText.transform.localScale = 0.5f * Vector3.one;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = -0.16f;
		base.transform.localPosition = localPosition;
	}

	private void LateUpdate()
	{
		base.transform.LookAt(Camera.main.transform);
	}

	private void OnShowItemChange(object sender, EventArgs e)
	{
		ChangeRoleIntroductionIdentifierEventArgs changeRoleIntroductionIdentifierEventArgs = e as ChangeRoleIntroductionIdentifierEventArgs;
		if (changeRoleIntroductionIdentifierEventArgs == null)
		{
			GameObject[] showElements = m_showElements;
			for (int i = 0; i < showElements.Length; i++)
			{
				showElements[i].SetActive(false);
			}
			m_showText.gameObject.SetActive(false);
			return;
		}
		bool ifShow = changeRoleIntroductionIdentifierEventArgs.m_ifShow;
		int num = changeRoleIntroductionIdentifierEventArgs.m_identifierIndex / 16 - 1;
		int awardNumByType = insideGameDataModule.GetAwardNumByType((DropType)changeRoleIntroductionIdentifierEventArgs.m_identifierIndex);
		int mCollectCount = changeRoleIntroductionIdentifierEventArgs.mCollectCount;
		for (int j = 0; j < m_showElements.Length; j++)
		{
			m_showElements[j].SetActive(ifShow && num == j);
		}
		m_showText.gameObject.SetActive(ifShow);
		m_showText.text = string.Format("{0} / {1}", awardNumByType.ToString(), mCollectCount.ToString());
	}
}
