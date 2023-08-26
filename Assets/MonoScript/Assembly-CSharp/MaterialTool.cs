using UnityEngine;

public class MaterialTool
{
	private static MaterialPropertyBlock prop = new MaterialPropertyBlock();

	public static void SetMaterialColor(Renderer renderer, string name, Color color)
	{
		renderer.GetPropertyBlock(prop);
		prop.SetColor(name, color);
		renderer.SetPropertyBlock(prop);
		prop.Clear();
	}

	public static void SetMaterialFloat(Renderer renderer, string name, float value)
	{
		renderer.GetPropertyBlock(prop);
		prop.SetFloat(name, value);
		renderer.SetPropertyBlock(prop);
		prop.Clear();
	}

	public static Color GetMaterialColor(Renderer renderer, string name)
	{
		return renderer.sharedMaterial.GetColor(name);
	}

	public static float GetMaterialFloat(Renderer renderer, string name)
	{
		return renderer.sharedMaterial.GetFloat(name);
	}

	public static string GetMaterialTag(Renderer renderer, string name, bool searchFallbacks = false)
	{
		return renderer.sharedMaterial.GetTag(name, searchFallbacks);
	}

	public static void SetMaterialTag(Renderer renderer, string name, string value)
	{
		renderer.sharedMaterial.SetOverrideTag(name, value);
	}

	public static bool IsTransparentMaterial(Renderer renderer)
	{
		return GetMaterialTag(renderer, "RenderType") == "Transparent";
	}

	public static void SetMaterialKeyWord(Renderer renderer, string name, bool value)
	{
		if (value)
		{
			renderer.sharedMaterial.EnableKeyword(name);
		}
		else
		{
			renderer.sharedMaterial.DisableKeyword(name);
		}
	}

	public static bool GetMaterialKeyWord(Renderer renderer, string name)
	{
		return renderer.sharedMaterial.IsKeywordEnabled(name);
	}

	public static void SetSkyboxMaterial(Material material)
	{
		RenderSettings.skybox = material;
	}

	public static Material GetSkyboxMaterial()
	{
		return RenderSettings.skybox;
	}
}
