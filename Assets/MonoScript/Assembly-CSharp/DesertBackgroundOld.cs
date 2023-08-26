using UnityEngine;

public class DesertBackgroundOld : BaseBackgroundElement
{
	public Renderer m_renderer;

	public Vector2 m_scale = new Vector2(0.5f, 0.5f);

	public float m_leftAngle = -90f;

	public float m_rightAngle = 90f;

	public float m_upAngle = 90f;

	public float m_downAngle = -90f;

	private float _eulerX;

	private float _eulerY;

	private float _offsetX;

	private float _offsetY;

	private Vector3 backBeginPos;

	public float UpDownScaler = 0.01f;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	private void Awake()
	{
		Initialize(Camera.main.transform);
		backBeginPos = base.transform.position;
	}

	public override void Initialize(Transform parent)
	{
		base.transform.parent = parent;
		m_renderer = GetComponent<Renderer>();
		m_renderer.material.mainTextureScale = m_scale;
		m_renderer.material.mainTextureOffset = new Vector2((1f - m_scale.x) / 2f, (1f - m_scale.y) / 2f);
		m_leftAngle = 0f - Mathf.Abs(m_leftAngle);
		m_rightAngle = Mathf.Abs(m_rightAngle);
		m_upAngle = 0f - Mathf.Abs(m_upAngle);
		m_downAngle = Mathf.Abs(m_downAngle);
	}

	public override void ResetElement()
	{
		m_renderer.material.mainTextureScale = m_scale;
		m_renderer.material.mainTextureOffset = new Vector2((1f - m_scale.x) / 2f, (1f - m_scale.y) / 2f);
	}

	private void Update()
	{
		float num = (base.transform.position - backBeginPos).y * UpDownScaler;
		_eulerX = Vector3.SignedAngle(Vector3.right, base.transform.right, Vector3.up);
		_eulerY = Vector3.SignedAngle(Vector3.up, base.transform.up, Vector3.left);
		if (_eulerX >= 90f || _eulerX <= -90f)
		{
			_eulerY = 0f - _eulerY;
		}
		_eulerX = Mathf.Clamp(_eulerX, m_leftAngle, m_rightAngle);
		_eulerY = Mathf.Clamp(_eulerY, m_upAngle, m_downAngle);
		float num2 = m_rightAngle - m_leftAngle;
		float num3 = m_downAngle - m_upAngle;
		_offsetX = (_eulerX - m_leftAngle) / num2 * (1f - m_scale.x) / 2f;
		_offsetY = (_eulerY - m_upAngle) / num3 * (1f - m_scale.y) / 2f + num;
		m_renderer.material.mainTextureOffset = new Vector2(_offsetX, _offsetY);
	}
}
