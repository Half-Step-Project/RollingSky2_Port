using UnityEngine;

public class MaterialController : MonoBehaviour
{
	public enum ControllerState
	{
		RoleMove
	}

	public ControllerState m_state;

	public Material[] m_targetMaterials;

	public static MaterialController m_target;

	private Transform m_player;

	public void Awake()
	{
		m_target = this;
	}

	public void Update()
	{
		if (BaseRole.theBall != null)
		{
			m_player = BaseRole.theBall.transform;
			if (m_state == ControllerState.RoleMove)
			{
				SetMaterialParameter(m_player.position.x, m_player.position.y, m_player.position.z);
			}
		}
	}

	private void SetMaterialParameter(float x, float y, float z)
	{
		for (int i = 0; i < m_targetMaterials.Length; i++)
		{
			if (m_targetMaterials[i] != null)
			{
				m_targetMaterials[i].SetVector("_PlayerLocation", new Vector4(x, y, z, 0f));
			}
		}
	}

	public void DestroyLocal()
	{
		if (m_targetMaterials != null)
		{
			for (int i = 0; i < m_targetMaterials.Length; i++)
			{
				m_targetMaterials[i] = null;
			}
		}
		m_target = null;
		Object.Destroy(base.gameObject);
	}
}
