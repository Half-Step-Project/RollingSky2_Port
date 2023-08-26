using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class HomelandBack : BaseBackgroundElement
	{
		public static readonly string[] backNames = new string[3] { "back0", "back1", "back2" };

		public Vector2 m_scale = new Vector2(0.8f, 0.8f);

		public Vector2 m_offset = new Vector2(0.1f, 0f);

		public float BeginEulerY = -20f;

		public float UpDownScaler = 0.005f;

		public float RotateYScaler = 0.8f;

		public float TotalEuler = 128f;

		public float ViewEuler = 85f;

		public float MainTextureOffsetX = 0.1f;

		public int m_HidProgressNum;

		public List<GameObject> m_HidObject;

		private Renderer[] m_renderers;

		private Transform[] m_backParts;

		private Vector3 backBeginPos;

		private float eularToUVScaler;

		private int currentIndex;

		public override bool IsRecordOriginRebirth
		{
			get
			{
				return true;
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<HideBackgroundEventArgs>.EventId, OnHideBackground);
			Mod.Event.Subscribe(EventArgs<ShowBackgroundEventArgs>.EventId, OnShowBackground);
		}

		private void OnDisable()
		{
			if (Mod.Event != null)
			{
				Mod.Event.Unsubscribe(EventArgs<HideBackgroundEventArgs>.EventId, OnHideBackground);
				Mod.Event.Unsubscribe(EventArgs<ShowBackgroundEventArgs>.EventId, OnShowBackground);
			}
		}

		public override void Initialize(Transform parent)
		{
			base.transform.parent = parent;
			if (base.name.Contains("Back_Tutorial_Sky"))
			{
				base.transform.localPosition = new Vector3(0f, 35f, 92f);
			}
			else
			{
				base.transform.localPosition = new Vector3(0f, 0f, 92f);
			}
			base.transform.localEulerAngles = Vector3.zero;
			int num = backNames.Length;
			m_backParts = new Transform[num];
			m_renderers = new Renderer[num];
			currentIndex = 0;
			for (int i = 0; i < num; i++)
			{
				m_backParts[i] = base.transform.Find(backNames[i]);
				m_renderers[i] = m_backParts[i].GetComponent<Renderer>();
			}
			ShowBackByIndex(currentIndex);
			backBeginPos = base.transform.position;
			eularToUVScaler = (1f - m_scale.y) / (TotalEuler - ViewEuler) * RotateYScaler;
			Update();
		}

		public override void ResetElement()
		{
			currentIndex = 0;
			ShowBackByIndex(currentIndex);
		}

		public override int GetBackgrondIndex()
		{
			return currentIndex;
		}

		public override void ResetBySavePointInfo(RebirthBoxData savePoint)
		{
			currentIndex = savePoint.m_backIndex;
			ShowBackByIndex(currentIndex);
		}

		private void Update()
		{
			Renderer renderer = m_renderers[currentIndex];
			if (renderer.enabled)
			{
				float value = Vector3.SignedAngle(Vector3.up, base.transform.up, Vector3.left) - BeginEulerY;
				value = Mathf.Clamp(value, (0f - (TotalEuler - ViewEuler)) / 2f, (TotalEuler - ViewEuler) / 2f);
				float y = (base.transform.position - backBeginPos).y * UpDownScaler - value * eularToUVScaler;
				renderer.material.mainTextureOffset = new Vector2(MainTextureOffsetX, y);
			}
			if (m_HidObject.Count <= 0 || Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).ProgressPercentage < m_HidProgressNum)
			{
				return;
			}
			foreach (GameObject item in m_HidObject)
			{
				item.SetActive(false);
			}
		}

		private void OnHideBackground(object sender, Foundation.EventArgs e)
		{
			if (e is HideBackgroundEventArgs)
			{
				m_renderers[currentIndex].enabled = false;
			}
		}

		private void OnShowBackground(object sender, Foundation.EventArgs e)
		{
			ShowBackgroundEventArgs showBackgroundEventArgs = e as ShowBackgroundEventArgs;
			if (showBackgroundEventArgs != null)
			{
				int backgroundIndex = showBackgroundEventArgs.BackgroundIndex;
				if (backgroundIndex != -1)
				{
					ShowBackByIndex(backgroundIndex);
					currentIndex = backgroundIndex;
				}
			}
		}

		private void ShowBackByIndex(int index)
		{
			int num = backNames.Length;
			for (int i = 0; i < num; i++)
			{
				m_renderers[i].enabled = i == index;
				Material material = m_renderers[i].material;
				material.mainTextureScale = m_scale;
				material.mainTextureOffset = m_offset;
			}
		}

		[Obsolete("this is Obsolete,please  please use SetOriginRebirthBsonData !")]
		public override void SetOriginRebirthData(object dataInfo)
		{
			RD_HomelandBack_DATA rD_HomelandBack_DATA = JsonUtility.FromJson<RD_HomelandBack_DATA>(dataInfo as string);
			if (rD_HomelandBack_DATA != null)
			{
				currentIndex = rD_HomelandBack_DATA.CurrentIndex;
				base.transform.SetTransData(rD_HomelandBack_DATA.TransData);
				ShowBackByIndex(currentIndex);
			}
		}

		[Obsolete("this is Obsolete,please  please use GetOriginRebirthBsonData !")]
		public override object GetOriginRebirthData(object obj = null)
		{
			return JsonUtility.ToJson(new RD_HomelandBack_DATA
			{
				CurrentIndex = currentIndex,
				TransData = base.transform.GetTransData()
			});
		}

		[Obsolete("this is Obsolete,please  please use StartRunByOriginRebirthBsonData !")]
		public override void StartRunByOriginRebirthData(object dataInfo)
		{
		}

		public override void SetOriginRebirthBsonData(byte[] dataInfo)
		{
			RD_HomelandBack_DATA rD_HomelandBack_DATA = Bson.ToObject<RD_HomelandBack_DATA>(dataInfo);
			if (rD_HomelandBack_DATA != null)
			{
				currentIndex = rD_HomelandBack_DATA.CurrentIndex;
				ShowBackByIndex(currentIndex);
			}
		}

		public override byte[] GetOriginRebirthBsonData(object obj = null)
		{
			return Bson.ToBson(new RD_HomelandBack_DATA
			{
				CurrentIndex = currentIndex,
				TransData = base.transform.GetTransData()
			});
		}

		public override void StartRunByOriginRebirthBsonData(byte[] dataInfo)
		{
		}
	}
}
