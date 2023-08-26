using UnityEngine;
using UnityEngine.UI;

public class AutoReleaseImage : MonoBehaviour
{
	public Image image;

	private LoadReleaseIcon loadReleaseIcon;

	private void Awake()
	{
		image = GetComponent<Image>();
		loadReleaseIcon = new LoadReleaseIcon();
	}

	private void OnDestroy()
	{
		loadReleaseIcon.Release();
	}

	public void SetImageByGoodsId(int goodsId)
	{
		if (!(image == null))
		{
			int goodsIconIdByGoodsId = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(goodsId);
			if (goodsIconIdByGoodsId != -1)
			{
				image.gameObject.SetActive(false);
				loadReleaseIcon.Load(image, goodsIconIdByGoodsId);
			}
		}
	}
}
