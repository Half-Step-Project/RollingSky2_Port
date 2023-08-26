using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
	[Tooltip("播放序列帧的起始帧（0 ～ n-1)，每次Enable组件TextureAnimation，都会将CurrentIndex重置为StartIndex")]
	public int StartIndex;

	[Tooltip("序列帧纹理的总列数，会覆盖Material上的参数")]
	[Range(0f, 10f)]
	public int m_ColumnCount = 4;

	[Tooltip("序列帧纹理的总行数，会覆盖Material上的参数")]
	[Range(0f, 10f)]
	public int m_RowCount = 4;

	[Tooltip("序列帧播放速度，单位（帧/每秒）,会覆盖Material上的参数")]
	[Range(0f, 100f)]
	public float Speed;

	[Tooltip("不实例化SharedMaterial，默认关闭")]
	public bool UseSharedMaterial;

	private Material m_AnimationMaterial;

	private int m_CurrentIndex;

	private int m_NextIndex;

	private float m_AccumulateTime;

	private int m_IDAnimationParameters;

	private int m_IDSpeed;

	private int m_IDRow;

	private int m_IDColumn;

	private bool m_IsInitialized;

	protected void Start()
	{
		m_IDRow = Shader.PropertyToID("_Row");
		m_IDColumn = Shader.PropertyToID("_Col");
		m_IDSpeed = Shader.PropertyToID("_Speed");
		m_IDAnimationParameters = Shader.PropertyToID("_AnimationParameters");
		m_IsInitialized = true;
	}

	protected void OnEnable()
	{
		int num = m_ColumnCount * m_RowCount;
		m_CurrentIndex = StartIndex % num;
		m_NextIndex = m_CurrentIndex;
		m_AnimationMaterial = InitializeAnimationMaterial();
		m_AnimationMaterial.EnableKeyword("SCRIPT_CONTROL_ANIMATION");
	}

	protected void LateUpdate()
	{
		if (m_IsInitialized)
		{
			int num = m_ColumnCount * m_RowCount;
			m_CurrentIndex = m_NextIndex;
			m_AccumulateTime += Time.deltaTime;
			if (Speed <= float.Epsilon)
			{
				m_AccumulateTime = 0f;
			}
			else if (m_AccumulateTime * Speed > 1f)
			{
				m_NextIndex++;
				m_NextIndex %= num;
				m_AccumulateTime = 0f;
			}
			ApplyAnimationParameters();
		}
	}

	private void ApplyAnimationParameters()
	{
		float num = 1f / (float)m_RowCount;
		float num2 = 1f / (float)m_ColumnCount;
		Vector4 zero = Vector4.zero;
		zero.x = (float)(m_CurrentIndex % m_ColumnCount) * num2;
		zero.y = 1f - Mathf.Floor((float)m_CurrentIndex * num2) * num - num;
		zero.z = num2;
		zero.w = num;
		m_AnimationMaterial.SetVector(m_IDAnimationParameters, zero);
		m_AnimationMaterial.SetFloat(m_IDSpeed, Speed);
		m_AnimationMaterial.SetFloat(m_IDRow, m_RowCount);
		m_AnimationMaterial.SetFloat(m_IDColumn, m_ColumnCount);
	}

	private Material InitializeAnimationMaterial()
	{
		if (m_AnimationMaterial == null)
		{
			MeshRenderer component = GetComponent<MeshRenderer>();
			Material sharedMaterial = component.sharedMaterial;
			if (sharedMaterial == null)
			{
				Debug.LogError("Start:序列帧材质 == null.", base.gameObject);
			}
			string text = sharedMaterial.shader.name;
			if (!text.Equals("CustomShader/Unlit/TextureAnim"))
			{
				Debug.LogError(string.Format("序列帧材质使用了预期外的Shader({0}),预期使用Shader({1})", text, "CustomShader/Unlit/TextureAnim"), base.gameObject);
			}
			if (UseSharedMaterial)
			{
				m_AnimationMaterial = sharedMaterial;
			}
			else
			{
				m_AnimationMaterial = new Material(sharedMaterial);
				component.material = m_AnimationMaterial;
			}
		}
		return m_AnimationMaterial;
	}
}
