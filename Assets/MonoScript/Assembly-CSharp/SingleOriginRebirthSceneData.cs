using System;
using UnityEngine;

[Serializable]
public class SingleOriginRebirthSceneData
{
	[Obsolete("this is Obsolete,please  please use RailwayData !")]
	public string m_railwayData = string.Empty;

	[Obsolete("this is Obsolete,please  please use RoleData !")]
	public string m_roleData = string.Empty;

	[Obsolete("this is Obsolete,please  please use CoupleData !")]
	public string m_coupleData = string.Empty;

	[Obsolete("this is Obsolete,please  please use MapData !")]
	public string m_mapData = string.Empty;

	[HideInInspector]
	[Obsolete("this is Obsolete,please  please use BrushData !")]
	public string m_brushData = string.Empty;

	[Obsolete("this is Obsolete,please  please use WorldConfigureData !")]
	public string m_worldConfigureData = string.Empty;

	[Obsolete("this is Obsolete,please  please use CameraData !")]
	public string m_cameraData = string.Empty;

	[Obsolete("this is Obsolete,please  please use AudioData !")]
	public string m_audioData = string.Empty;

	[Obsolete("this is Obsolete,please  please use BackgroundData !")]
	public string m_backgroundData = string.Empty;

	[Obsolete("this is Obsolete,please  please use ProgressData !")]
	public string m_progressData = string.Empty;

	[Obsolete("this is Obsolete,please  please use InputData !")]
	public string m_inputData = string.Empty;

	[Obsolete("this is Obsolete,please  please use PetData !")]
	public string m_petData = string.Empty;

	[Obsolete("this is Obsolete,please  please use PropData !")]
	public string m_propData = string.Empty;

	[Obsolete("this is Obsolete,please  please use GridsData !")]
	public string m_gridsData = string.Empty;

	public byte[] RailwayData;

	public byte[] RoleData;

	public byte[] CoupleData;

	public byte[] MapData;

	[HideInInspector]
	public byte[] BrushData;

	public byte[] WorldConfigureData;

	public byte[] CameraData;

	public byte[] AudioData;

	public byte[] BackgroundData;

	public byte[] ProgressData;

	public byte[] InputData;

	public byte[] PetData;

	public byte[] PropData;

	public byte[] GridsData;
}
