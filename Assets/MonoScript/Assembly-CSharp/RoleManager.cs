using System.Collections.Generic;
using RS2;
using UnityEngine;

public class RoleManager
{
	public static Dictionary<int, RoleData> RoleDataDic = new Dictionary<int, RoleData>
	{
		{
			1,
			new RoleData(1, "RoleDream", "RoleDream", typeof(RolePrince), null)
		},
		{
			2,
			new RoleData(2, "RolePharaohsOld", "RolePharaohsOld", typeof(RolePrince), null)
		},
		{
			3,
			new RoleData(3, "RolePharaohs", "RolePharaohs", typeof(RolePrince), null)
		},
		{
			4,
			new RoleData(4, "RolePharaohsSleep", "RolePharaohsSleep", typeof(RolePrince), null)
		},
		{
			5,
			new RoleData(5, "RoleDestiny", "RoleDestiny", typeof(RolePrince), null)
		},
		{
			6,
			new RoleData(6, "RoleHome", "RoleHome", typeof(RolePrince), null)
		},
		{
			7,
			new RoleData(7, "RoleWaltz1", "RoleWaltz1", typeof(RolePrince), null)
		},
		{
			9,
			new RoleData(9, "RoleThief01", "RoleThief01", typeof(RolePrince), null)
		},
		{
			10,
			new RoleData(10, "RoleJazz", "RoleJazz", typeof(RolePrince), null)
		},
		{
			11,
			new RoleData(11, "RoleWeird", "RoleWeird", typeof(RolePrince), null)
		},
		{
			12,
			new RoleData(12, "RoleRainbow", "RoleRainbow", typeof(RolePrince), null)
		},
		{
			16,
			new RoleData(16, "RoleTutorial", "RoleTutorial", typeof(RolePrince), null)
		},
		{
			17,
			new RoleData(17, "RoleSamurai", "RoleSamurai", typeof(RolePrince), null)
		},
		{
			18,
			new RoleData(18, "RoleDestinyTutorial", "RoleDestinyTutorial", typeof(RolePrince), null)
		},
		{
			19,
			new RoleData(19, "RoleThief2_01", "RoleThief2_01", typeof(RolePrince), null)
		}
	};

	private static Dictionary<int, BaseRole> PreLoadedRoleDic = new Dictionary<int, BaseRole>();

	public static BaseRole CreateRole(int id)
	{
		RoleData value = null;
		BaseRole value2 = null;
		if (RoleDataDic.TryGetValue(id, out value))
		{
			if (!PreLoadedRoleDic.TryGetValue(id, out value2))
			{
				string rolePrefabAsset = AssetUtility.GetRolePrefabAsset(value.RolePrefabPath);
				GameObject gameObject = LevelResources.theResource.GetLevelResource(rolePrefabAsset) as GameObject;
				if (gameObject != null)
				{
					value2 = Object.Instantiate(gameObject).AddComponent(value.RoleClassType) as BaseRole;
					value2.CurrentRoleData = value;
					string roleConfigAsset = AssetUtility.GetRoleConfigAsset(value.RoleDataPath);
					RoleProperty roleProperty = LevelResources.theResource.GetLevelResource(roleConfigAsset) as RoleProperty;
					if (roleProperty != null)
					{
						value2.m_roleProperty = Object.Instantiate(roleProperty);
					}
				}
				value2.PreInitialize();
				PreLoadedRoleDic[id] = value2;
			}
		}
		else
		{
			Debug.LogError("the RoleId " + id + " can't be found!");
		}
		value2.gameObject.SetActive(false);
		return value2;
	}

	public static void DestroyAllRoles()
	{
		foreach (KeyValuePair<int, BaseRole> item in PreLoadedRoleDic)
		{
			BaseRole value = item.Value;
			value.UnloadLocal();
			value.DestroyLocal();
		}
		PreLoadedRoleDic.Clear();
	}
}
