using System;
using UnityEngine;

public class EmissionTileTrigger : BaseTriggerBox
{
	private Material[] m_modelMat;

	private Transform m_player;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void UpdateElement()
	{
		m_player = BaseRole.theBall.transform;
		SetMaterialParameter(m_player.position.x, m_player.position.y, m_player.position.z);
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		m_modelMat = base.transform.Find("model").GetComponent<MeshRenderer>().sharedMaterials;
		Vector3 roleStartPos = Railway.theRailway.GetRoleStartPos();
		SetMaterialParameter(roleStartPos.x, roleStartPos.y, roleStartPos.z);
	}

	private void SetMaterialParameter(float x, float y, float z)
	{
		for (int i = 0; i < m_modelMat.Length; i++)
		{
			m_modelMat[i].SetVector("_PlayerLocation", new Vector4(x, y, z, 0f));
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		UpdateElement();
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return string.Empty;
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
