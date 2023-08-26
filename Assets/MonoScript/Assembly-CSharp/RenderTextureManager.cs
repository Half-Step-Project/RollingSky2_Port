using UnityEngine;

public class RenderTextureManager
{
	public static RenderTexture Get(int width, int height, int depthBuffer = 24, RenderTextureFormat format = RenderTextureFormat.ARGB32, RenderTextureReadWrite rw = RenderTextureReadWrite.Default, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, string name = "defaultRenderTextureName")
	{
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format);
		temporary.filterMode = filterMode;
		temporary.wrapMode = wrapMode;
		temporary.name = name;
		return temporary;
	}

	public static void Release(ref RenderTexture renderTexture)
	{
		if (!(renderTexture == null))
		{
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = null;
		}
	}
}
