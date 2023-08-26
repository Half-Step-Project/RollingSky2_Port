using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public sealed class CommandBufferBlur : MonoBehaviour
{
	public Camera m_Camera;

	public Shader m_BlurShader;

	private CameraEvent m_CameraEvent = CameraEvent.AfterImageEffects;

	private Material m_Material;

	private CommandBuffer m_CommandBuffer;

	private AssetLoadCallbacks assetLoadCallBack;

	private void Awake()
	{
		assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			m_BlurShader = asset as Shader;
			if (m_Material == null)
			{
				m_Material = new Material(m_BlurShader);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
		}, delegate(string assetName, string status, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' failed.", assetName));
		});
	}

	private void OnEnable()
	{
		if (m_Camera == null)
		{
			m_Camera = base.gameObject.GetComponent<Camera>();
		}
		if (m_BlurShader == null)
		{
			string assetName = "CommandBufferBlur";
			Mod.Resource.LoadAsset(AssetUtility.GetCustomShaderAsset(assetName), assetLoadCallBack);
		}
	}

	private void OnDisable()
	{
		if (m_Camera != null && m_CommandBuffer != null)
		{
			m_Camera.forceIntoRenderTexture = false;
			m_Camera.RemoveCommandBuffer(m_CameraEvent, m_CommandBuffer);
			m_CommandBuffer = null;
		}
	}

	private void OnDestroy()
	{
		if (m_BlurShader != null)
		{
			Mod.Resource.UnloadAsset(m_BlurShader);
			m_BlurShader = null;
		}
		if (m_Material != null)
		{
			Object.Destroy(m_Material);
			m_Material = null;
		}
	}

	public void OnPostRender()
	{
		if (!(m_Camera == null) && !(m_Material == null) && m_CommandBuffer == null)
		{
			m_CommandBuffer = new CommandBuffer();
			m_CommandBuffer.name = "Screen Blur";
			ConfigCommandBuffer();
			m_Camera.AddCommandBuffer(m_CameraEvent, m_CommandBuffer);
			m_Camera.forceIntoRenderTexture = true;
		}
	}

	private void ConfigCommandBuffer()
	{
		int num = Shader.PropertyToID("_ScreenCopyTexture");
		m_CommandBuffer.GetTemporaryRT(num, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf);
		m_CommandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, num);
		int num2 = Shader.PropertyToID("_BlurredTexture");
		int num3 = Shader.PropertyToID("_BlurredTexture2");
		m_CommandBuffer.GetTemporaryRT(num2, -2, -2, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf);
		m_CommandBuffer.GetTemporaryRT(num3, -2, -2, 0, FilterMode.Bilinear, RenderTextureFormat.ARGBHalf);
		m_CommandBuffer.Blit(num, num2);
		m_CommandBuffer.ReleaseTemporaryRT(num);
		m_CommandBuffer.SetGlobalVector("offsets", new Vector4(6f / (float)Screen.width, 0f, 0f, 0f));
		m_CommandBuffer.Blit(num2, num3, m_Material, 0);
		m_CommandBuffer.SetGlobalVector("offsets", new Vector4(0f, 6f / (float)Screen.height, 0f, 0f));
		m_CommandBuffer.Blit(num3, num2, m_Material, 0);
		m_CommandBuffer.SetGlobalVector("offsets", new Vector4(8f / (float)Screen.width, 0f, 0f, 0f));
		m_CommandBuffer.Blit(num2, num3, m_Material, 0);
		m_CommandBuffer.SetGlobalVector("offsets", new Vector4(0f, 8f / (float)Screen.height, 0f, 0f));
		m_CommandBuffer.Blit(num3, num2, m_Material, 0);
		m_CommandBuffer.ReleaseTemporaryRT(num3);
		m_CommandBuffer.Blit(num2, BuiltinRenderTextureType.CameraTarget);
		m_CommandBuffer.ReleaseTemporaryRT(num2);
	}
}
