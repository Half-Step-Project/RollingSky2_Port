using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;

public class PetController : MonoBehaviour, IOriginRebirth
{
	public static PetController m_current;

	public PetBase m_petBase;

	public Dictionary<string, GameObject> m_prefabs = new Dictionary<string, GameObject>();

	public static PetController Instance
	{
		get
		{
			if (m_current == null)
			{
				m_current = new GameObject("PetController").AddComponent<PetController>();
			}
			return m_current;
		}
	}

	public GameDataModule GetGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		}
	}

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public void DestroyLocal()
	{
		if (m_petBase != null)
		{
			m_petBase.DestroyLocal();
		}
		Object.Destroy(base.gameObject);
	}

	public void CreatePet(int petID)
	{
		PetData petDataById = PetManager.GetPetDataById(petID);
		PetBase petBase = null;
		if (petDataById != null)
		{
			string petPrefabAsset = AssetUtility.GetPetPrefabAsset(petDataById.m_path);
			GameObject gameObject = LevelResources.theResource.GetLevelResource(petPrefabAsset) as GameObject;
			if (gameObject != null)
			{
				petBase = Object.Instantiate(gameObject).AddComponent(petDataById.m_classType) as PetBase;
				petBase.m_petData = petDataById;
			}
			else
			{
				Log.Error("Pet CreatePet prefab  = null");
			}
		}
		m_petBase = petBase;
	}

	public object GetOriginRebirthData(object obj = null)
	{
		return string.Empty;
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		if (m_petBase != null)
		{
			m_petBase.SwitchPetState(PetState.OriginRebirthReady);
		}
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
		if (m_petBase != null)
		{
			m_petBase.SwitchPetState(PetState.FastAdmission);
		}
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		return new byte[0];
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		if (m_petBase != null)
		{
			m_petBase.SwitchPetState(PetState.OriginRebirthReady);
		}
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
		if (m_petBase != null)
		{
			m_petBase.SwitchPetState(PetState.FastAdmission);
		}
	}
}
