using DG.Tweening;
using UnityEngine;

public class LevelItemMaterialData
{
	private Color m_orinalColor;

	private Color m_currentColor;

	private Material m_material;

	public Color CurrentColor
	{
		get
		{
			return m_currentColor;
		}
		private set
		{
			m_currentColor = value;
		}
	}

	public LevelItemMaterialData(Material material)
	{
		m_material = material;
		string colorProperty = GetColorProperty();
		m_orinalColor = m_material.GetColor(colorProperty);
		m_currentColor = m_orinalColor;
	}

	public void SetMaterialColor(Color targetColor)
	{
		CurrentColor = targetColor;
		string colorProperty = GetColorProperty();
		m_material.SetColor(colorProperty, CurrentColor);
	}

	private string GetColorProperty()
	{
		if (m_material.HasProperty("_Color"))
		{
			return "_Color";
		}
		if (m_material.HasProperty("_TintColor"))
		{
			return "_TintColor";
		}
		return "_Color";
	}

	private string GetAlphaProperty()
	{
		if (m_material.HasProperty("_Alpha"))
		{
			return "_Alpha";
		}
		return string.Empty;
	}

	public void SetMaterialAlpha(float alpha)
	{
		string alphaProperty = GetAlphaProperty();
		if (!string.IsNullOrEmpty(alphaProperty))
		{
			m_material.SetFloat(alphaProperty, alpha);
			return;
		}
		Color currentColor = CurrentColor;
		currentColor.a = alpha;
		SetMaterialColor(currentColor);
	}

	public void DoFadeAlpha(float alpha, float duration)
	{
		string alphaProperty = GetAlphaProperty();
		if (!string.IsNullOrEmpty(alphaProperty))
		{
			m_material.DOFloat(alpha, alphaProperty, duration);
			return;
		}
		Color currentColor = CurrentColor;
		currentColor.a = alpha;
		DoFadeTargetColor(currentColor, duration);
	}

	public void DoFadeTargetColor(Color targetColor, float duration)
	{
		CurrentColor = targetColor;
		string colorProperty = GetColorProperty();
		m_material.DOColor(targetColor, colorProperty, duration);
	}

	public void Recover()
	{
		string colorProperty = GetColorProperty();
		m_material.SetColor(colorProperty, m_orinalColor);
	}

	public void Release()
	{
		m_material = null;
	}

	public override string ToString()
	{
		if (!(m_material == null))
		{
			return m_material.name;
		}
		return "Null";
	}
}
