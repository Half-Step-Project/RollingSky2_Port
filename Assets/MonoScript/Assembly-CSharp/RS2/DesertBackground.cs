using Foundation;
using UnityEngine;

namespace RS2
{
	public class DesertBackground : BaseBackgroundElement
	{
		public Renderer m_renderer;

		public Vector2 m_scale = new Vector2(0.8f, 0.8f);

		public Vector2 m_offset = new Vector2(0.1f, 0f);

		private Vector3 backBeginPos;

		public float BeginEulerY = -20f;

		public float UpDownScaler = 0.005f;

		public float RotateYScaler = 0.8f;

		public float RealDistance = 320f;

		public float BackDistance = 92f;

		public float TotalEuler = 128f;

		public float ViewEuler = 85f;

		private float eularToUVScaler;

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<HideBackgroundEventArgs>.EventId, OnHideBackground);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<HideBackgroundEventArgs>.EventId, OnHideBackground);
		}

		public override void Initialize(Transform parent)
		{
			base.transform.parent = parent;
			base.transform.localPosition = new Vector3(0f, 0f, 92f);
			base.transform.localEulerAngles = Vector3.zero;
			m_renderer = GetComponent<Renderer>();
			m_renderer.material.mainTextureScale = m_scale;
			m_renderer.material.mainTextureOffset = m_offset;
			backBeginPos = base.transform.position;
			eularToUVScaler = (1f - m_scale.y) / (TotalEuler - ViewEuler) * RotateYScaler;
			Update();
		}

		public override void ResetElement()
		{
			if (!m_renderer.enabled)
			{
				m_renderer.enabled = true;
			}
			m_renderer.material.mainTextureScale = m_scale;
			m_renderer.material.mainTextureOffset = m_offset;
		}

		private void Update()
		{
			if (m_renderer != null && m_renderer.enabled)
			{
				float value = Vector3.SignedAngle(Vector3.up, base.transform.up, Vector3.left) - BeginEulerY;
				value = Mathf.Clamp(value, (0f - (TotalEuler - ViewEuler)) / 2f, (TotalEuler - ViewEuler) / 2f);
				float y = (base.transform.position - backBeginPos).y * UpDownScaler - value * eularToUVScaler;
				m_renderer.material.mainTextureOffset = new Vector2(0.1f, y);
			}
			float farClipPlane = Camera.main.farClipPlane;
			BackDistance = farClipPlane - 8f;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, BackDistance);
		}

		private void OnHideBackground(object sender, EventArgs e)
		{
			if (e is HideBackgroundEventArgs && m_renderer.enabled)
			{
				m_renderer.enabled = false;
				base.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
	}
}
