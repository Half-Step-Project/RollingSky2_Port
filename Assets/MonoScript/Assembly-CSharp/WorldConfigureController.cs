using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class WorldConfigureController : IOriginRebirth
{
	public WorldConfigureScriptTable m_currentLevelConfigureTable;

	public Dictionary<int, Theme> m_currentLevelThemes = new Dictionary<int, Theme>();

	public Theme m_currentTheme;

	public Theme m_targetTheme;

	public Theme m_tempTheme;

	public float m_timeTakenDuringLerp = 2f;

	private bool m_isLerping;

	private float m_lerpPercentage;

	private float m_timeStartedLerping;

	private Light DirectionalLight;

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public void Initialize()
	{
		Mod.Event.Subscribe(EventArgs<GameWorldThemeChangeEventArgs>.EventId, OnWorldThemeChange);
		DirectionalLight = GameObject.Find("LightParent").transform.Find("Directional Light").GetComponent<Light>();
		m_tempTheme = new Theme();
		m_isLerping = false;
		m_lerpPercentage = 0f;
	}

	public void ResetController()
	{
		Mod.Event.Unsubscribe(EventArgs<GameWorldThemeChangeEventArgs>.EventId, OnWorldThemeChange);
	}

	public void InitThemes(int _level)
	{
		m_isLerping = false;
		m_lerpPercentage = 0f;
		m_currentLevelThemes = GetThemesByLevel(_level);
		m_currentTheme = m_currentLevelThemes[m_currentLevelConfigureTable.m_defaultThemeIndex];
		ApplyThemeInfo(m_currentTheme);
	}

	public void InitThemes(RebirthBoxData data, int _level)
	{
		m_isLerping = false;
		m_lerpPercentage = 0f;
		m_currentLevelThemes = GetThemesByLevel(_level);
		m_currentTheme = m_currentLevelThemes[data.m_themeIndex];
		ApplyThemeInfo(m_currentTheme);
	}

	public void UpdateTheme()
	{
		if (m_isLerping)
		{
			float num = (Time.time - m_timeStartedLerping) / m_timeTakenDuringLerp;
			LerpThemeInfo(m_targetTheme, num);
			if (num >= 1f)
			{
				m_isLerping = false;
				m_lerpPercentage = 0f;
				m_currentTheme = m_targetTheme;
			}
		}
	}

	private void OnWorldThemeChange(object sender, EventArgs e)
	{
		GameWorldThemeChangeEventArgs gameWorldThemeChangeEventArgs = e as GameWorldThemeChangeEventArgs;
		if (gameWorldThemeChangeEventArgs != null)
		{
			int themeIndex = gameWorldThemeChangeEventArgs.ThemeIndex;
			if (!m_currentLevelThemes.ContainsKey(themeIndex))
			{
				Debug.LogError("Theme " + themeIndex + " non-existent!");
			}
			else
			{
				ChangeThemeTo(themeIndex);
			}
		}
	}

	public void ChangeThemeTo(int _index)
	{
		m_isLerping = true;
		m_lerpPercentage = 0f;
		m_timeStartedLerping = Time.time;
		m_targetTheme = m_currentLevelThemes[_index];
		m_timeTakenDuringLerp = 2f;
		if (m_targetTheme.m_timeTaken > 0f)
		{
			m_timeTakenDuringLerp *= m_targetTheme.m_timeTaken;
		}
		ChangeTempTheme();
	}

	private void ChangeTempTheme()
	{
		m_tempTheme.m_themeCamera.m_isCameraColor = m_targetTheme.m_themeCamera.m_isCameraColor;
		m_tempTheme.m_themeLight.m_isLight = m_targetTheme.m_themeLight.m_isLight;
		m_tempTheme.m_themeFog.m_isFog = m_targetTheme.m_themeFog.m_isFog;
		m_tempTheme.m_themeSkyBox.m_isSkyBox = m_targetTheme.m_themeSkyBox.m_isSkyBox;
		if (m_targetTheme.m_themeSkyBox.m_isSkyBox)
		{
			m_tempTheme.m_themeSkyBox.m_BlendLerp = m_targetTheme.m_themeSkyBox.m_BlendLerp;
			MaterialTool.SetSkyboxMaterial(m_targetTheme.m_themeSkyBox.m_SkyBoxMaterial);
			if (m_targetTheme.m_themeSkyBox.m_BlendLerp)
			{
				MaterialTool.GetSkyboxMaterial().SetFloat("_Blend", m_lerpPercentage);
			}
		}
		m_tempTheme.m_themeLight.m_ifLightOn = m_targetTheme.m_themeLight.m_ifLightOn;
	}

	private void LerpThemeInfo(Theme _target, float _percentageComplete)
	{
		m_lerpPercentage = _percentageComplete;
		if (_target.m_themeCamera.m_isCameraColor)
		{
			m_tempTheme.m_themeCamera.m_backGround = Color.Lerp(m_currentTheme.m_themeCamera.m_backGround, _target.m_themeCamera.m_backGround, _percentageComplete);
		}
		if (_target.m_themeFog.m_isFog)
		{
			m_tempTheme.m_themeFog.m_fogColor = Color.Lerp(m_currentTheme.m_themeFog.m_fogColor, _target.m_themeFog.m_fogColor, _percentageComplete);
			m_tempTheme.m_themeFog.m_fogStartDistance = Mathf.Lerp(m_currentTheme.m_themeFog.m_fogStartDistance, _target.m_themeFog.m_fogStartDistance, _percentageComplete);
			m_tempTheme.m_themeFog.m_fogEndDistance = Mathf.Lerp(m_currentTheme.m_themeFog.m_fogEndDistance, _target.m_themeFog.m_fogEndDistance, _percentageComplete);
		}
		if (_target.m_themeLight.m_isLight)
		{
			m_tempTheme.m_themeLight.m_lightColor = Color.Lerp(m_currentTheme.m_themeLight.m_lightColor, _target.m_themeLight.m_lightColor, _percentageComplete);
			m_tempTheme.m_themeLight.m_intensity = Mathf.Lerp(m_currentTheme.m_themeLight.m_intensity, _target.m_themeLight.m_intensity, _percentageComplete);
			m_tempTheme.m_themeLight.m_shadowStrength = Mathf.Lerp(m_currentTheme.m_themeLight.m_shadowStrength, _target.m_themeLight.m_shadowStrength, _percentageComplete);
			m_tempTheme.m_themeLight.m_rotation = Vector3.Lerp(m_currentTheme.m_themeLight.m_rotation, _target.m_themeLight.m_rotation, _percentageComplete);
		}
		if (_target.m_themeSkyBox.m_isSkyBox)
		{
			m_tempTheme.m_themeSkyBox.m_SkyBoxMaterial = _target.m_themeSkyBox.m_SkyBoxMaterial;
			m_tempTheme.m_themeSkyBox.m_skyColor = Color.Lerp(m_currentTheme.m_themeSkyBox.m_skyColor, _target.m_themeSkyBox.m_skyColor, _percentageComplete);
			m_tempTheme.m_themeSkyBox.m_EquatorColor = Color.Lerp(m_currentTheme.m_themeSkyBox.m_EquatorColor, _target.m_themeSkyBox.m_EquatorColor, _percentageComplete);
			m_tempTheme.m_themeSkyBox.m_GroundColor = Color.Lerp(m_currentTheme.m_themeSkyBox.m_GroundColor, _target.m_themeSkyBox.m_GroundColor, _percentageComplete);
			m_tempTheme.m_themeSkyBox.m_ReflectionIntensity = Mathf.Lerp(m_currentTheme.m_themeSkyBox.m_ReflectionIntensity, _target.m_themeSkyBox.m_ReflectionIntensity, _percentageComplete);
		}
		ApplyThemeInfo(m_tempTheme);
	}

	private void ApplyThemeInfo(Theme _target)
	{
		if (_target.m_themeCamera.m_isCameraColor)
		{
			Camera.main.backgroundColor = _target.m_themeCamera.m_backGround;
		}
		if (_target.m_themeFog.m_isFog)
		{
			RenderSettings.fog = true;
			RenderSettings.fogColor = _target.m_themeFog.m_fogColor;
			RenderSettings.fogStartDistance = _target.m_themeFog.m_fogStartDistance;
			RenderSettings.fogEndDistance = _target.m_themeFog.m_fogEndDistance;
		}
		else
		{
			RenderSettings.fog = true;
			RenderSettings.fogStartDistance = 5000f;
			RenderSettings.fogEndDistance = 5000.1f;
		}
		if (_target.m_themeLight.m_isLight)
		{
			DirectionalLight.color = _target.m_themeLight.m_lightColor;
			DirectionalLight.intensity = _target.m_themeLight.m_intensity;
			DirectionalLight.shadowStrength = _target.m_themeLight.m_shadowStrength;
			DirectionalLight.transform.localEulerAngles = _target.m_themeLight.m_rotation;
			DirectionalLight.gameObject.SetActive(_target.m_themeLight.m_ifLightOn);
		}
		if (_target.m_themeSkyBox.m_isSkyBox)
		{
			MaterialTool.SetSkyboxMaterial(_target.m_themeSkyBox.m_SkyBoxMaterial);
			if (_target.m_themeSkyBox.m_BlendLerp)
			{
				MaterialTool.GetSkyboxMaterial().SetFloat("_Blend", m_lerpPercentage);
			}
			RenderSettings.ambientSkyColor = _target.m_themeSkyBox.m_skyColor;
			RenderSettings.ambientEquatorColor = _target.m_themeSkyBox.m_EquatorColor;
			RenderSettings.ambientGroundColor = _target.m_themeSkyBox.m_GroundColor;
			RenderSettings.reflectionIntensity = _target.m_themeSkyBox.m_ReflectionIntensity;
			RenderSettings.customReflection = _target.m_themeSkyBox.m_Cubemap;
		}
	}

	public Dictionary<int, Theme> GetThemesByLevel(int _level)
	{
		m_currentLevelConfigureTable = LevelResources.theResource.WorldConfigureTable;
		return List2Dic(m_currentLevelConfigureTable.m_themes);
	}

	public Theme GetDefaultThemeByLevel()
	{
		return m_currentLevelThemes[m_currentLevelConfigureTable.m_defaultThemeIndex];
	}

	public Dictionary<int, Theme> List2Dic(List<Theme> _list)
	{
		Dictionary<int, Theme> dictionary = new Dictionary<int, Theme>();
		for (int i = 0; i < _list.Count; i++)
		{
			if (!dictionary.ContainsKey(_list[i].m_index))
			{
				dictionary[_list[i].m_index] = _list[i];
			}
			else
			{
				Debug.LogError("Theme Index 重复");
			}
		}
		return dictionary;
	}

	public WorldStartInfo GetStartInfoByLevel()
	{
		return LevelResources.theResource.WorldConfigureTable.m_worldStartInfo;
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return JsonUtility.ToJson(new RD_WorldConfigureController_Data
		{
			m_currentIndex = m_currentTheme.m_index,
			m_targetIndex = ((m_targetTheme != null) ? m_targetTheme.m_index : 0),
			m_isLerping = m_isLerping,
			m_timeTakenDuringLerp = m_timeTakenDuringLerp,
			m_lerpPercentage = m_lerpPercentage
		});
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		RD_WorldConfigureController_Data rD_WorldConfigureController_Data = JsonUtility.FromJson<RD_WorldConfigureController_Data>(dataInfo as string);
		m_isLerping = rD_WorldConfigureController_Data.m_isLerping;
		m_currentTheme = m_currentLevelThemes[rD_WorldConfigureController_Data.m_currentIndex];
		m_targetTheme = m_currentLevelThemes[rD_WorldConfigureController_Data.m_targetIndex];
		m_timeTakenDuringLerp = rD_WorldConfigureController_Data.m_timeTakenDuringLerp;
		ChangeTempTheme();
		if (m_isLerping)
		{
			LerpThemeInfo(m_targetTheme, rD_WorldConfigureController_Data.m_lerpPercentage);
		}
		else
		{
			LerpThemeInfo(m_targetTheme, 1f);
		}
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return Bson.ToBson(new RD_WorldConfigureController_Data
		{
			m_currentIndex = m_currentTheme.m_index,
			m_targetIndex = ((m_targetTheme != null) ? m_targetTheme.m_index : 0),
			m_isLerping = m_isLerping,
			m_timeTakenDuringLerp = m_timeTakenDuringLerp,
			m_lerpPercentage = m_lerpPercentage
		});
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		RD_WorldConfigureController_Data rD_WorldConfigureController_Data = Bson.ToObject<RD_WorldConfigureController_Data>(dataInfo);
		m_isLerping = rD_WorldConfigureController_Data.m_isLerping;
		m_currentTheme = m_currentLevelThemes[rD_WorldConfigureController_Data.m_currentIndex];
		m_targetTheme = m_currentLevelThemes[rD_WorldConfigureController_Data.m_targetIndex];
		m_timeTakenDuringLerp = rD_WorldConfigureController_Data.m_timeTakenDuringLerp;
		ChangeTempTheme();
		if (m_isLerping)
		{
			LerpThemeInfo(m_targetTheme, rD_WorldConfigureController_Data.m_lerpPercentage);
		}
		else
		{
			LerpThemeInfo(m_targetTheme, 1f);
		}
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}
}
