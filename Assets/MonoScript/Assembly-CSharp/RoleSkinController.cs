using System;
using UnityEngine;

public class RoleSkinController : MonoBehaviour
{
	[Serializable]
	public struct SkinDatas
	{
		public ActiveData[] m_actives;

		public MaterialData[] m_materials;

		public AnimationData[] m_animations;
	}

	[Serializable]
	public struct ActiveData
	{
		public GameObject m_gameObject;

		public bool m_active;
	}

	[Serializable]
	public struct MaterialData
	{
		public Renderer m_renderer;

		public bool m_isCanLerp;

		public Material[] m_materials;
	}

	[Serializable]
	public struct AnimationData
	{
		public Animation m_animation;

		public bool m_play;

		public string m_playAnimationName;
	}

	public SkinDatas[] m_skinDatas;

	public bool SetSkinByIndex(int index)
	{
		if (m_skinDatas != null && m_skinDatas.Length > index)
		{
			if (m_skinDatas[index].m_actives != null)
			{
				ActiveData[] actives = m_skinDatas[index].m_actives;
				for (int i = 0; i < actives.Length; i++)
				{
					if (actives[i].m_gameObject != null)
					{
						actives[i].m_gameObject.SetActive(actives[i].m_active);
					}
				}
			}
			if (m_skinDatas[index].m_materials != null)
			{
				MaterialData[] materials = m_skinDatas[index].m_materials;
				for (int j = 0; j < materials.Length; j++)
				{
					if (materials[j].m_renderer != null)
					{
						materials[j].m_renderer.materials = materials[j].m_materials;
					}
				}
			}
			if (m_skinDatas[index].m_animations != null)
			{
				AnimationData[] animations = m_skinDatas[index].m_animations;
				for (int k = 0; k < animations.Length; k++)
				{
					if (animations[k].m_animation != null)
					{
						if (animations[k].m_play)
						{
							animations[k].m_animation.Play(animations[k].m_playAnimationName);
						}
						else
						{
							animations[k].m_animation.Stop();
						}
					}
				}
			}
			return true;
		}
		return false;
	}

	public void LerpRangeFloatForShaderFieldName(int index, string blendFieldName, float progress, string yFieldName, float y)
	{
		bool flag = ((!string.IsNullOrEmpty(blendFieldName)) ? true : false);
		bool flag2 = ((!string.IsNullOrEmpty(yFieldName)) ? true : false);
		if (m_skinDatas == null || m_skinDatas.Length <= index || m_skinDatas[index].m_materials == null)
		{
			return;
		}
		MaterialData[] materials = m_skinDatas[index].m_materials;
		for (int i = 0; i < materials.Length; i++)
		{
			if (materials[i].m_renderer != null && materials[i].m_isCanLerp)
			{
				if (flag)
				{
					MaterialTool.SetMaterialFloat(materials[i].m_renderer, blendFieldName, progress);
				}
				if (flag2)
				{
					MaterialTool.SetMaterialFloat(materials[i].m_renderer, yFieldName, y);
				}
			}
		}
	}
}
