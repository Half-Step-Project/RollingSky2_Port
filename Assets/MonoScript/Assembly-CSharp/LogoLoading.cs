using UnityEngine;
using UnityEngine.UI;

public class LogoLoading : MonoBehaviour
{
	public Text t_load;

	public Text t_head;

	private string[] loadTextArray = new string[11]
	{
		"Loading", "Descarga en proceso", "加载中", "載入中", "ロード中", "En cours de chargement…", "Laden", "caricamento", "Belading", "Загружается",
		"로딩중"
	};

	private string[] headTextArray = new string[11]
	{
		"Please use headphones for best experience", "Por favor ponte los auriculares para la mejor experiencia", "请佩戴耳机，享受最佳游戏体验", "請佩戴耳機，享受最佳遊戲體驗", "イヤホンをしてお楽しみください", "Veuillez utiliser des écouteurs pour une meilleure expérience", "Bitte verwenden Sie Kopfhörer für ein besseres Erlebnis", "Si\u00a0prega\u00a0di\u00a0utilizzare\u00a0le\u00a0cuffie\u00a0per\u00a0la\u00a0migliore\u00a0esperienza", "Gebruik\u00a0een\u00a0hoofdtelefoon\u00a0voor\u00a0een\u00a0betere\u00a0ervaring", "Пожалуйста, используйте наушники для лучшего игрового опыта",
		"좋은 게임 경험을 위해 핸드폰을 쓰세요"
	};

	private SystemLanguage systemName;

	private void Start()
	{
		systemName = Application.systemLanguage;
		ShowInfo();
	}

	private void ShowInfo()
	{
		int num = 0;
		switch (systemName)
		{
		case SystemLanguage.English:
			num = 0;
			break;
		case SystemLanguage.Japanese:
			num = 1;
			break;
		case SystemLanguage.ChineseSimplified:
			num = 2;
			break;
		case SystemLanguage.ChineseTraditional:
			num = 3;
			break;
		case SystemLanguage.Spanish:
			num = 4;
			break;
		case SystemLanguage.French:
			num = 5;
			break;
		case SystemLanguage.German:
			num = 6;
			break;
		case SystemLanguage.Italian:
			num = 7;
			break;
		case SystemLanguage.Dutch:
			num = 8;
			break;
		case SystemLanguage.Russian:
			num = 9;
			break;
		case SystemLanguage.Korean:
			num = 10;
			break;
		default:
			num = 0;
			break;
		}
		t_load.text = loadTextArray[num];
		t_head.text = headTextArray[num];
	}
}
