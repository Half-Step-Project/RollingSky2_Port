using System;
using UnityEngine;
using User.TileMap;

[Serializable]
public class RebirthBoxData
{
	public int m_gridId;

	public Point m_point;

	public Vector3 WorldOrigin;

	public Vector3 WorldRight;

	public Vector3 RailwayPos;

	public RebirthCameraData m_cameraData;

	public RebirthRoleData m_roleData;

	public RebirthMapData m_mapData;

	public float m_musicTime;

	public int m_themeIndex;

	public int m_worldProgress;

	public int m_ballRowNum;

	public int m_backIndex;

	public RebirthWindData m_rebirthWindData;

	public RebirthVehicleData m_rebirthVehicleData;

	public RebirthProgressData m_rebirthProgressData;

	public override string ToString()
	{
		return base.ToString() + ",m_gridId = " + m_gridId + ",m_point = " + m_point.ToString() + ",m_musicTime = " + m_musicTime;
	}
}
