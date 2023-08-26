using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsSwitcher : MonoBehaviour
{
	[Serializable]
	public struct MaterialsReplacementArray
	{
		public Material DefaultMaterial;

		public Material ReplacementMaterial;
	}

	[SerializeField]
	private MaterialsReplacementArray[] m_MaterialReplacementArray = new MaterialsReplacementArray[0];

	private const int MAX_MATERIALS_IN_ARRAY = 2;

	private int m_CurrentMaterialState;

	private MeshRenderer[] m_MeshRenderers;

	private Dictionary<MeshRenderer, Dictionary<int, int>> m_MaterialsMapping = new Dictionary<MeshRenderer, Dictionary<int, int>>();

	protected void Awake()
	{
		RefreshMaterialMapping();
	}

	protected void OnDestroy()
	{
		m_MeshRenderers = null;
		m_MaterialReplacementArray = null;
		m_MaterialsMapping.Clear();
		m_MaterialsMapping = null;
	}

	public bool Switch()
	{
		int targetState = ++m_CurrentMaterialState % 2;
		return SwitchTo(targetState);
	}

	public bool SwitchTo(int targetState)
	{
		if (m_MeshRenderers == null || m_MeshRenderers.Length == 0)
		{
			Debug.LogError("Switch failed. m_MeshRenderers == 0 || m_MeshRenderers.Length == 0");
			return false;
		}
		bool flag = targetState % 2 == 0;
		for (int i = 0; i < m_MeshRenderers.Length; i++)
		{
			MeshRenderer meshRenderer = m_MeshRenderers[i];
			Dictionary<int, int> value;
			if (!m_MaterialsMapping.TryGetValue(meshRenderer, out value))
			{
				continue;
			}
			Material[] sharedMaterials = meshRenderer.sharedMaterials;
			foreach (int key in value.Keys)
			{
				MaterialsReplacementArray materialsReplacementArray = m_MaterialReplacementArray[value[key]];
				sharedMaterials[key] = (flag ? materialsReplacementArray.DefaultMaterial : materialsReplacementArray.ReplacementMaterial);
			}
			meshRenderer.sharedMaterials = sharedMaterials;
		}
		return true;
	}

	private void RefreshMaterialMapping()
	{
		m_MaterialsMapping.Clear();
		m_MeshRenderers = GetComponentsInChildren<MeshRenderer>(true);
		for (int i = 0; i < m_MeshRenderers.Length; i++)
		{
			MeshRenderer meshRenderer = m_MeshRenderers[i];
			if (m_MaterialsMapping.ContainsKey(meshRenderer))
			{
				continue;
			}
			bool flag = false;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Material[] sharedMaterials = meshRenderer.sharedMaterials;
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				Material material = sharedMaterials[j];
				for (int k = 0; k < m_MaterialReplacementArray.Length; k++)
				{
					if (m_MaterialReplacementArray[k].DefaultMaterial == material)
					{
						dictionary.Add(j, k);
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				m_MaterialsMapping.Add(meshRenderer, dictionary);
			}
		}
	}
}
