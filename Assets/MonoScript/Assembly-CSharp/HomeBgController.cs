using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class HomeBgController : MonoBehaviour
{
	private int lastLevelId = 1;

	private int tempLevelId = -1;

	private float time = 1f;

	private MeshRenderer bgMeshRender;

	private Material bgMaterial;

	private Image upback;

	private Image downback_0;

	private Image downback_1;

	private List<GameObject> animationBackList = new List<GameObject>();

	private Color lasUpColor;

	private Color currnetUpColor;

	private Color lastDownColor_0;

	private Color currentDownColor_0;

	private Color lastDownColor_1;

	private Color currentDownColor_1;

	private int totalLevelNum;

	private Dictionary<int, Texture2D> bgTextureDic = new Dictionary<int, Texture2D>();

	private bool isInit;

	private List<object> loadedAsserts = new List<object>();

	public void Init(Dictionary<string, GameObject> _dic)
	{
		totalLevelNum = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).GetLevelNum();
		upback = _dic["upback"].GetComponent<Image>();
		downback_0 = _dic["downback_0"].GetComponent<Image>();
		downback_1 = _dic["downback_1"].GetComponent<Image>();
		bgMeshRender = _dic["BG"].GetComponent<MeshRenderer>();
		for (int i = 1; i < totalLevelNum + 1; i++)
		{
			animationBackList.Add(_dic["animation_bg_" + i]);
		}
	}

	public void SetData(LevelOrder_levelOrderTable[] levelsList)
	{
		Texture2D mat = null;
		LevelOrder_levelOrderTable levelOrder_levelOrderTable = null;
		int count = levelsList.Length;
		for (int i = 0; i < levelsList.Length; i++)
		{
			levelOrder_levelOrderTable = levelsList[i];
			string spriteName = "bg_levelback_" + levelOrder_levelOrderTable.LevelSeriesId;
			Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data)
			{
				mat = asset as Texture2D;
				LevelOrder_levelOrderTable levelOrder_levelOrderTable2 = (LevelOrder_levelOrderTable)data;
				if (bgTextureDic.ContainsKey(levelOrder_levelOrderTable2.LevelSeriesId))
				{
					bgTextureDic[levelOrder_levelOrderTable2.LevelSeriesId] = mat;
				}
				else
				{
					bgTextureDic.Add(levelOrder_levelOrderTable2.LevelSeriesId, mat);
				}
				loadedAsserts.Add(asset);
				count--;
				if (count <= 0)
				{
					isInit = true;
					if (tempLevelId > 0)
					{
						ChangeBgByLevelId(tempLevelId);
					}
				}
			}, delegate(string assetName, string errorMessage, object data)
			{
				int num = count;
				count = num - 1;
				if (count <= 0)
				{
					isInit = true;
					if (tempLevelId > 0)
					{
						ChangeBgByLevelId(tempLevelId);
					}
				}
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", spriteName, assetName, errorMessage));
			}), levelOrder_levelOrderTable);
		}
	}

	public void Release()
	{
		isInit = false;
		lastLevelId = 1;
		bgTextureDic.Clear();
		animationBackList.Clear();
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	private void Update()
	{
		if (isInit && !(time >= 1f))
		{
			time += Time.smoothDeltaTime * 3f;
			if ((double)time > 1.0)
			{
				time = 1f;
			}
			bgMaterial.SetFloat("_Ammount", time);
			if (upback != null)
			{
				upback.color = Color.Lerp(lasUpColor, currnetUpColor, time);
			}
			if (downback_0 != null)
			{
				downback_0.color = Color.Lerp(lastDownColor_0, currentDownColor_0, time);
			}
			if (downback_1 != null)
			{
				downback_1.color = Color.Lerp(lastDownColor_1, currentDownColor_1, time);
			}
		}
	}

	public void ChangeBgByLevelId(int levelId)
	{
		if (!isInit)
		{
			tempLevelId = levelId;
		}
		else
		{
			if (lastLevelId == levelId)
			{
				return;
			}
			if (bgMeshRender != null)
			{
				Texture2D value = bgTextureDic[lastLevelId];
				Texture2D value2 = bgTextureDic[levelId];
				bgMaterial.SetTexture("_MainTex", value);
				bgMaterial.SetTexture("_AltTex", value2);
				bgMaterial.SetFloat("_Ammount", 0f);
			}
			int num = lastLevelId - 1;
			time = 0f;
			for (int i = 0; i < animationBackList.Count; i++)
			{
				if (i == num)
				{
					animationBackList[i].SetActive(true);
				}
				else
				{
					animationBackList[i].SetActive(false);
				}
			}
			lastLevelId = levelId;
		}
	}

	public void InitBackColor(int leveId)
	{
	}
}
