using Foundation;
using RS2;
using UnityEngine;

public class HomeHintLoginAwardItem : MonoBehaviour
{
	public void OnClick()
	{
		Mod.UI.OpenUIForm(UIFormId.SequenceLoginAwardForm);
		InfocUtils.Report_rollingsky2_games_pageshow(6, 10, 2);
	}
}
