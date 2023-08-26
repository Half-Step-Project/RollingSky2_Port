using Foundation;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ImageShineMaterialAnimation : MonoBehaviour
{
	[Range(-5f, 5f)]
	public float Percent = 1f;

	[Range(0f, 360f)]
	public float Angle = 30f;

	public float Width = 30f;

	public Color ShineColor = Color.white;

	private Material m_Material;

	private int m_PercentPropertyID = -1;

	private int m_AnglePropertyID = -1;

	private int m_WidthPropertyID = -1;

	private int m_ShineColorPropertyID = -1;

	protected void Start()
	{
		m_PercentPropertyID = Shader.PropertyToID("_Percent");
		m_AnglePropertyID = Shader.PropertyToID("_Angle");
		m_WidthPropertyID = Shader.PropertyToID("_Width");
		m_ShineColorPropertyID = Shader.PropertyToID("_ShineColor");
		Graphic component = GetComponent<Graphic>();
		if ((bool)component)
		{
			m_Material = component.materialForRendering;
			if (m_Material == null)
			{
				Log.Warning("ImageShineMaterialAnimation graphic.materialForRendering == null.");
			}
			else
			{
				m_Material = new Material(m_Material);
				component.material = m_Material;
			}
		}
		else
		{
			Renderer component2 = GetComponent<Renderer>();
			if ((bool)component2)
			{
				m_Material = component2.sharedMaterial;
				if (m_Material == null)
				{
					Log.Warning("ImageShineMaterialAnimation renderer.sharedMaterial == null.");
				}
				else
				{
					m_Material = new Material(m_Material);
					component.material = m_Material;
				}
			}
		}
		if (m_Material == null)
		{
			Log.Error("Missing materials for ImageShineMaterialAnimation.");
		}
	}

	protected void LateUpdate()
	{
		if ((bool)m_Material)
		{
			m_Material.SetFloat(m_PercentPropertyID, Percent);
			m_Material.SetFloat(m_AnglePropertyID, Angle);
			m_Material.SetFloat(m_WidthPropertyID, Width);
			m_Material.SetColor(m_ShineColorPropertyID, ShineColor);
		}
	}
}
