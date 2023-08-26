using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RisingWin.Library
{
	public class ButtonTool
	{
		public static Button curButton;

		private static Button[] AryButton;

		public static Button CurrentSelectedButton
		{
			get
			{
				if (EventSystem.current.currentSelectedGameObject == null)
				{
					return null;
				}
				return EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
			}
		}

		public static void Select(Button button)
		{
			if (CurrentSelectedButton != null)
			{
				CurrentSelectedButton.OnDeselect(new BaseEventData(EventSystem.current));
			}
			button.Select();
			button.OnSelect(new BaseEventData(EventSystem.current));
			curButton = button;
		}

		public static void ChangeButtonNavigation(Transform transOpen)
		{
			AryButton = Object.FindObjectsOfType<Button>();
			Navigation navigation = default(Navigation);
			navigation.mode = Navigation.Mode.None;
			for (int i = 0; i < AryButton.Length; i++)
			{
				AryButton[i].navigation = navigation;
			}
			Button[] componentsInChildren = transOpen.GetComponentsInChildren<Button>();
			navigation.mode = Navigation.Mode.Automatic;
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].navigation = navigation;
			}
		}

		public static void RevertButtonNavigation()
		{
			Navigation navigation = default(Navigation);
			navigation.mode = Navigation.Mode.Automatic;
			for (int i = 0; i < AryButton.Length; i++)
			{
				AryButton[i].navigation = navigation;
			}
		}
	}
}
