using System.Globalization;
using UnityEngine;

namespace Foundation
{
	internal sealed class QualityInformationWindow : ScrollableDebuggerWindowBase
	{
		private bool _applyExpensiveChanges;

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Quality Level</b>");
			GUILayout.BeginVertical("box");
			int qualityLevel = QualitySettings.GetQualityLevel();
			DrawItem("Current Quality Level:", QualitySettings.names[qualityLevel]);
			_applyExpensiveChanges = GUILayout.Toggle(_applyExpensiveChanges, "Apply expensive changes on quality level change.");
			int num = GUILayout.SelectionGrid(qualityLevel, QualitySettings.names, 3, "toggle");
			if (num != qualityLevel)
			{
				QualitySettings.SetQualityLevel(num, _applyExpensiveChanges);
			}
			GUILayout.EndVertical();
			GUILayout.Label("<b>Rendering Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Active Color Space:", QualitySettings.activeColorSpace.ToString());
			DrawItem("Desired Color Space:", QualitySettings.desiredColorSpace.ToString());
			DrawItem("Max Queued Frames:", QualitySettings.maxQueuedFrames.ToString());
			DrawItem("Pixel Light Count:", QualitySettings.pixelLightCount.ToString());
			DrawItem("Master Texture Limit:", QualitySettings.masterTextureLimit.ToString());
			DrawItem("Anisotropic Filtering:", QualitySettings.anisotropicFiltering.ToString());
			DrawItem("Anti Aliasing:", QualitySettings.antiAliasing.ToString());
			DrawItem("Realtime Reflection Probes:", QualitySettings.realtimeReflectionProbes.ToString());
			DrawItem("Billboards Face Camera Position:", QualitySettings.billboardsFaceCameraPosition.ToString());
			DrawItem("Resolution Scaling Fixed DPI Factor:", QualitySettings.resolutionScalingFixedDPIFactor.ToString(CultureInfo.InvariantCulture));
			GUILayout.EndVertical();
			GUILayout.Label("<b>Shadows Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Shadow Resolution:", QualitySettings.shadowResolution.ToString());
			DrawItem("Shadow Quality:", QualitySettings.shadows.ToString());
			DrawItem("Shadow Projection:", QualitySettings.shadowProjection.ToString());
			DrawItem("Shadow Distance:", QualitySettings.shadowDistance.ToString(CultureInfo.InvariantCulture));
			DrawItem("Shadowmask Mode:", QualitySettings.shadowmaskMode.ToString());
			DrawItem("Shadow Near Plane Offset:", QualitySettings.shadowNearPlaneOffset.ToString(CultureInfo.InvariantCulture));
			DrawItem("Shadow Cascades:", QualitySettings.shadowCascades.ToString());
			DrawItem("Shadow Cascade 2 Split:", QualitySettings.shadowCascade2Split.ToString(CultureInfo.InvariantCulture));
			DrawItem("Shadow Cascade 4 Split:", QualitySettings.shadowCascade4Split.ToString());
			GUILayout.EndVertical();
			GUILayout.Label("<b>Other Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Blend Weights:", QualitySettings.skinWeights.ToString());
			DrawItem("VSync Count:", QualitySettings.vSyncCount.ToString());
			DrawItem("LOD Bias:", QualitySettings.lodBias.ToString(CultureInfo.InvariantCulture));
			DrawItem("Maximum LOD Level:", QualitySettings.maximumLODLevel.ToString());
			DrawItem("Particle Raycast Budget:", QualitySettings.particleRaycastBudget.ToString());
			DrawItem("Async Upload Time Slice:", QualitySettings.asyncUploadTimeSlice + " ms");
			DrawItem("Async Upload Buffer Size:", QualitySettings.asyncUploadBufferSize + " MB");
			DrawItem("Soft Particles:", QualitySettings.softParticles.ToString());
			DrawItem("Soft Vegetation:", QualitySettings.softVegetation.ToString());
			GUILayout.EndVertical();
		}
	}
}
