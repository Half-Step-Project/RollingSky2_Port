using Foundation;
using UnityEngine;

namespace RS2
{
	public class ButtonInputButton : MonoBehaviour
	{
		public GameObject[] backInfo;

		private void OnEnable()
		{
			ChangeButtonImage(InputSystem.Instance.inputState);
			AddEvent();
		}

		private void OnDisable()
		{
			RemoveEvent();
		}

		private void AddEvent()
		{
			Mod.Event.Subscribe(EventArgs<ChangeInputArgs>.EventId, ChangeInputInfo);
		}

		private void RemoveEvent()
		{
			Mod.Event.Unsubscribe(EventArgs<ChangeInputArgs>.EventId, ChangeInputInfo);
		}

		private void Start()
		{
			backInfo[0].SetActive(false);
			backInfo[1].SetActive(false);
		}

		private void ChangeInputInfo(object sender, EventArgs e)
		{
			ChangeInputArgs changeInputArgs = e as ChangeInputArgs;
			ChangeButtonImage(changeInputArgs.inputState);
		}

		private void ChangeButtonImage(InputSystem.InputState inputState)
		{
			for (int i = 0; i < backInfo.Length; i++)
			{
				if (inputState == (InputSystem.InputState)i)
				{
					backInfo[i].SetActive(true);
				}
				else
				{
					backInfo[i].SetActive(false);
				}
			}
		}
	}
}
