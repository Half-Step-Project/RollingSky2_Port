using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;

public class BuyOutRebirthBoxTrigger : BaseRebirthBoxTrigger
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		[Header("check point的位置")]
		public Vector3 m_move1LocalPosition;

		[Header("光幕的位置")]
		public Vector3 m_move2LocalPosition;

		public float m_beginDistance;

		public float m_resetDistance;

		public int progress;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_move1LocalPosition = bytes.GetVector3(ref startIndex);
			m_move2LocalPosition = bytes.GetVector3(ref startIndex);
			m_beginDistance = bytes.GetSingle(ref startIndex);
			m_resetDistance = bytes.GetSingle(ref startIndex);
			try
			{
				progress = bytes.GetInt32(ref startIndex);
			}
			catch (Exception)
			{
				Log.Warning("Can not find progress");
			}
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_move1LocalPosition.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_move2LocalPosition.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_beginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_resetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(progress.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}

		public string WriteJson()
		{
			return JsonUtility.ToJson(this);
		}

		public void ReadJson(string json)
		{
			this = JsonUtility.FromJson<TileData>(json);
		}
	}

	private Transform model;

	private bool m_isTrigger;

	private bool m_distanceDetection = true;

	private GameObject m_canMoveGameObject1;

	private GameObject m_canMoveGameObject2;

	private GameObject m_effect_fuhuodian02;

	private ParticleSystem m_effect_fuhuodian02ByEffect;

	private TextMesh m_progressText;

	public TileData m_data;

	[Header("editor模式下，运行一帧")]
	public bool m_runPlay;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		m_canMoveGameObject1 = dictionary["canmove1"];
		m_canMoveGameObject2 = dictionary["canmove2"];
		m_effect_fuhuodian02 = dictionary["Effect_fuhuodian02"];
		m_effect_fuhuodian02ByEffect = dictionary["glow_024"].GetComponent<ParticleSystem>();
		m_progressText = dictionary["ProgressText"].GetComponent<TextMesh>();
		m_canMoveGameObject2.SetActive(false);
		m_canMoveGameObject1.SetActive(false);
		m_isTrigger = true;
		m_distanceDetection = true;
		commonState = CommonState.None;
		model = base.transform.Find("model");
		if (Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).TriggeredBuyOutRebirth.ContainsKey(m_uuId))
		{
			model.gameObject.SetActive(false);
		}
		else
		{
			model.gameObject.SetActive(true);
		}
		if (m_progressText != null)
		{
			m_progressText.text = m_data.progress + "%";
		}
	}

	public override void LateInitialize()
	{
		base.LateInitialize();
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		if (dataModule.CurrentRebirthBoxData != null && dataModule.CurrentRebirthBoxData.m_gridId == m_gridId && dataModule.CurrentRebirthBoxData.m_point == point && m_isTrigger)
		{
			m_isTrigger = false;
			m_distanceDetection = false;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		base.gameObject.SetActive(true);
		OnDefault();
		commonState = CommonState.None;
		model.gameObject.SetActive(true);
	}

	public override void UpdateElement()
	{
		if (!m_distanceDetection)
		{
			return;
		}
		float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (m_progressText != null)
		{
			Quaternion rotation = Quaternion.LookRotation(CameraController.theCamera.m_Camera.transform.position - m_progressText.transform.position) * Quaternion.Euler(0f, 180f, 0f);
			m_progressText.transform.rotation = rotation;
		}
		if (commonState == CommonState.None)
		{
			if (num >= m_data.m_beginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active && num >= m_data.m_resetDistance)
		{
			OnTriggerStop();
			commonState = CommonState.End;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		if (m_isTrigger)
		{
			base.TriggerEnter(ball);
			m_canMoveGameObject1.SetActive(false);
			m_canMoveGameObject2.SetActive(false);
			m_effect_fuhuodian02.transform.position = ball.transform.TransformPoint(0f, 0f, 0.5f);
			m_effect_fuhuodian02.SetActive(true);
			m_effect_fuhuodian02ByEffect.Play(true);
			if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(4) && !GameController.Instance.m_propsController.IsContains(PropsName.SHIELD))
			{
				Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.SHIELD));
			}
			m_isTrigger = false;
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		if (!dataModule.TriggeredBuyOutRebirth.ContainsKey(m_uuId))
		{
			dataModule.BuyOutRebirthIndex++;
			dataModule.TriggeredBuyOutRebirth.Add(m_uuId, m_uuId);
		}
		dataModule.BuyOutRebirthPoint = base.transform.position;
		dataModule.BuyOutRebirthProgress = m_data.progress;
		if (dataModule.BuyOutRebirthIndex == 1)
		{
			dataModule.FirstBuyOutRebirthPoint = dataModule.BuyOutRebirthPoint;
			dataModule.FirstBuyOutRebirthProgress = dataModule.BuyOutRebirthProgress;
			dataModule.FirstBuyOutRebirthUUID = m_uuId;
		}
		if (dataModule.ProgressPercentage > dataModule.FurthestBuyOutRebirthProgress)
		{
			if (dataModule.FurthestBuyOutRebirthProgress >= 0)
			{
				dataModule.DieCountForShowPromote = 0;
			}
			dataModule.FurthestBuyOutRebirthProgress = dataModule.ProgressPercentage;
			dataModule.BuyOutRebirthGainDiamounds = dataModule.GainedDiamonds;
			dataModule.BuyOutRebirthGainCrowns = dataModule.GainedCrowns;
			dataModule.BuyOutRebirthGainCrownFragments = dataModule.GainedCrownFragments;
			dataModule.BuyOutRebirthGainDiamoundFragments = dataModule.GainedDiamondFragments;
			dataModule.BuyOutRebirthPreDiamounds = dataModule.PreDiamonds;
			dataModule.BuyOutRebirthPreCrowns = dataModule.PreCrowns;
		}
	}

	public override void OnTriggerPlay()
	{
		m_isTrigger = true;
		m_canMoveGameObject1.SetActive(true);
		m_canMoveGameObject2.SetActive(true);
		base.OnTriggerPlay();
	}

	public override void OnTriggerStop()
	{
		m_isTrigger = false;
		m_distanceDetection = false;
		base.OnTriggerStop();
	}

	private void OnDefault()
	{
		m_canMoveGameObject2.SetActive(true);
		m_canMoveGameObject1.SetActive(true);
		m_effect_fuhuodian02.transform.localPosition = Vector3.zero;
		m_effect_fuhuodian02.SetActive(false);
	}

	public void OnValidate()
	{
		RefreshTileData(m_data);
		if (m_runPlay)
		{
			ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Simulate(1f, true);
			}
			m_runPlay = false;
		}
	}

	public override void Read(string info)
	{
		if (!string.IsNullOrEmpty(info))
		{
			m_data = JsonUtility.FromJson<TileData>(info);
		}
		RefreshTileData(m_data);
	}

	public override string Write()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		GameObject gameObject = dictionary["canmove1"];
		GameObject gameObject2 = dictionary["canmove2"];
		m_data.m_move1LocalPosition = gameObject.transform.localPosition;
		m_data.m_move2LocalPosition = gameObject2.transform.localPosition;
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		RefreshTileData(m_data);
	}

	public override byte[] WriteBytes()
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		GameObject gameObject = dictionary["canmove1"];
		GameObject gameObject2 = dictionary["canmove2"];
		m_data.m_move1LocalPosition = gameObject.transform.localPosition;
		m_data.m_move2LocalPosition = gameObject2.transform.localPosition;
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (TileData)objs[0];
		RefreshTileData(m_data);
	}

	private void RefreshTileData(TileData data)
	{
		Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
		GameObject gameObject = dictionary["canmove1"];
		GameObject gameObject2 = dictionary["canmove2"];
		if (gameObject != null)
		{
			gameObject.transform.localPosition = m_data.m_move1LocalPosition;
		}
		if (gameObject2 != null)
		{
			gameObject2.transform.localPosition = m_data.m_move2LocalPosition;
		}
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
}
