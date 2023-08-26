using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialPro : MonoBehaviour
{
	private string materialPath = "Brush/Level1/Materials/A";

	public MaterialTheme _materialTheme = MaterialTheme.Theme1;

	private List<Material> m_themeMaterialTheme = new List<Material>();

	public Dictionary<string, Material> m_oldMaterialDic = new Dictionary<string, Material>();

	private void Start()
	{
	}

	public void OpenTheme()
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
		m_themeMaterialTheme.Clear();
		Material[] array = null;
		string empty = string.Empty;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array = componentsInChildren[i].sharedMaterials;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					string arg = materialPath;
					int materialTheme = (int)_materialTheme;
					Material material = ResourcesManager.Load<Material>(string.Format("{0}{1}/{2}", arg, materialTheme.ToString(), array[j].name));
					m_themeMaterialTheme.Add(material);
					if (material != null)
					{
						array[j].CopyPropertiesFromMaterial(material);
					}
					m_oldMaterialDic[array[j].name] = array[j];
				}
			}
		}
	}

	public void SaveTheme()
	{
		for (int i = 0; i < m_themeMaterialTheme.Count; i++)
		{
			if (m_themeMaterialTheme[i] != null && m_oldMaterialDic.ContainsKey(m_themeMaterialTheme[i].name))
			{
				m_themeMaterialTheme[i].CopyPropertiesFromMaterial(m_oldMaterialDic[m_themeMaterialTheme[i].name]);
			}
		}
	}
}
