using UnityEngine;

namespace Foundation
{
	internal sealed class GraphicsInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Graphics Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Device ID:", SystemInfo.graphicsDeviceID.ToString());
			DrawItem("Device Name:", SystemInfo.graphicsDeviceName);
			DrawItem("Device Vendor ID:", SystemInfo.graphicsDeviceVendorID.ToString());
			DrawItem("Device Vendor:", SystemInfo.graphicsDeviceVendor);
			DrawItem("Device Type:", SystemInfo.graphicsDeviceType.ToString());
			DrawItem("Device Version:", SystemInfo.graphicsDeviceVersion);
			DrawItem("Memory Size:", SystemInfo.graphicsMemorySize + " MB");
			DrawItem("Multi Threaded:", SystemInfo.graphicsMultiThreaded.ToString());
			DrawItem("Shader Level:", GetShaderLevelString(SystemInfo.graphicsShaderLevel));
			DrawItem("NPOT Support:", SystemInfo.npotSupport.ToString());
			DrawItem("Max Texture Size:", SystemInfo.maxTextureSize.ToString());
			DrawItem("Max Cubemap Size:", SystemInfo.maxCubemapSize.ToString());
			DrawItem("Copy Texture Support:", SystemInfo.copyTextureSupport.ToString());
			DrawItem("Supported Render Target Count:", SystemInfo.supportedRenderTargetCount.ToString());
			DrawItem("Supports Sparse Textures:", SystemInfo.supportsSparseTextures.ToString());
			DrawItem("Supports 3D Textures:", SystemInfo.supports3DTextures.ToString());
			DrawItem("Supports 3D Render Textures:", SystemInfo.supports3DRenderTextures.ToString());
			DrawItem("Supports 2D Array Textures:", SystemInfo.supports2DArrayTextures.ToString());
			DrawItem("Supports Shadows:", SystemInfo.supportsShadows.ToString());
			DrawItem("Supports Raw Shadow Depth Sampling:", SystemInfo.supportsRawShadowDepthSampling.ToString());
			DrawItem("Supports Render To Cubemap:", SystemInfo.supportsRenderToCubemap.ToString());
			DrawItem("Supports Compute Shader:", SystemInfo.supportsComputeShaders.ToString());
			DrawItem("Supports Instancing:", SystemInfo.supportsInstancing.ToString());
			DrawItem("Supports Image Effects:", SystemInfo.supportsImageEffects.ToString());
			DrawItem("Supports Cubemap Array Textures:", SystemInfo.supportsCubemapArrayTextures.ToString());
			DrawItem("Supports Motion Vectors:", SystemInfo.supportsMotionVectors.ToString());
			DrawItem("Graphics UV Starts At Top:", SystemInfo.graphicsUVStartsAtTop.ToString());
			DrawItem("Uses Reversed ZBuffer:", SystemInfo.usesReversedZBuffer.ToString());
			GUILayout.EndVertical();
		}

		private string GetShaderLevelString(int shaderLevel)
		{
			return "Shader Model " + shaderLevel / 10 + "." + shaderLevel % 10;
		}
	}
}
